using System;
using Helion.Geometry;
using Helion.Geometry.Boxes;
using Helion.Geometry.Segments;
using Helion.Geometry.Vectors;
using Helion.Render.OpenGL.Context;
using Helion.Render.OpenGL.Framebuffer;
using Helion.Render.OpenGL.Renderers.Legacy.World.Data;
using Helion.Render.OpenGL.Renderers.Legacy.World.Entities;
using Helion.Render.OpenGL.Renderers.Legacy.World.Geometry;
using Helion.Render.OpenGL.Renderers.Legacy.World.Primitives;
using Helion.Render.OpenGL.Shared;
using Helion.Render.OpenGL.Texture.Legacy;
using Helion.Resources.Archives.Collection;
using Helion.Util;
using Helion.Util.Configs;
using Helion.World;
using Helion.World.Blockmap;
using Helion.World.Entities;
using Helion.World.Geometry.Sectors;
using OpenTK.Graphics.OpenGL;

namespace Helion.Render.OpenGL.Renderers.Legacy.World;

public class LegacyWorldRenderer : WorldRenderer
{
    private readonly IConfig m_config;
    private readonly GeometryRenderer m_geometryRenderer;
    private readonly EntityRenderer m_entityRenderer;
    private readonly PrimitiveWorldRenderer m_primitiveRenderer;
    private readonly InterpolationShader m_interpolationProgram = new();
    private readonly InterpolationTransparentShader m_interpolationTransparentProgram = new();
    private readonly InterpolationCompositeShader m_interpolationCompositeProgram = new();
    private readonly StaticShader m_staticProgram = new();
    private readonly RenderWorldDataManager m_worldDataManager = new();
    private readonly ArchiveCollection m_archiveCollection;
    private readonly LegacyGLTextureManager m_textureManager;
    private Vec2D m_occludeViewPos;
    private bool m_occlude;
    private bool m_spriteTransparency;
    private bool m_vanillaRender;
    private bool m_renderStatic;
    private int m_lastTicker = -1;
    private Entity? m_viewerEntity;
    private IWorld? m_previousWorld;
    private RenderBlockMapData m_renderData;

    private readonly OitFrameBuffer m_oitFrameBuffer = new();

    public LegacyWorldRenderer(IConfig config, ArchiveCollection archiveCollection, LegacyGLTextureManager textureManager)
    {
        m_config = config;
        m_entityRenderer = new(config, textureManager);
        m_primitiveRenderer = new();
        m_geometryRenderer = new(config, archiveCollection, textureManager, m_interpolationProgram, m_staticProgram, m_worldDataManager);
        m_archiveCollection = archiveCollection;
        m_textureManager = textureManager;
    }

    ~LegacyWorldRenderer()
    {
        ReleaseUnmanagedResources();
    }

    public override void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    public override void UpdateToNewWorld(IWorld world)
    {
        m_vanillaRender = m_config.Render.VanillaRender;
        TransferHeights.FlushSectorReferences();
        m_lastRenderedWorld.SetTarget(world);

        if (m_previousWorld != null)
            m_previousWorld.OnResetInterpolation -= World_OnResetInterpolation;

        var spriteDefinitions = m_archiveCollection.TextureManager.SpriteDefinitions;
        for (int i = 0; i < spriteDefinitions.Length; i++)
        {
            var spriteDefinition = spriteDefinitions[i];
            if (spriteDefinition == null)
                continue;

            m_textureManager.CacheSpriteRotations(spriteDefinition);
        }

        m_geometryRenderer.UpdateTo(world);
        m_entityRenderer.UpdateTo(world);
        world.OnResetInterpolation += World_OnResetInterpolation;
        m_previousWorld = world;
        m_lastTicker = -1;
    }

    private void World_OnResetInterpolation(object? sender, EventArgs e)
    {
        m_lastTicker = -1;
        ResetInterpolation((IWorld)sender!);
    }

    private void IterateBlockmap(IWorld world, RenderInfo renderInfo)
    {
        bool shouldRender = m_lastTicker != world.GameTicker;
        if (!shouldRender)
            return;

        m_geometryRenderer.SetRenderMode(m_renderStatic ? GeometryRenderMode.Dynamic : GeometryRenderMode.All, renderInfo.TransferHeightView);

        m_renderData.ViewerEntity = renderInfo.ViewerEntity;
        m_renderData.ViewPosInterpolated = renderInfo.Camera.PositionInterpolated.XY.Double;
        m_renderData.ViewPosInterpolated3D = renderInfo.Camera.PositionInterpolated.Double;
        m_renderData.ViewPos3D = renderInfo.Camera.Position.Double;
        m_renderData.ViewDirection = renderInfo.Camera.Direction.XY.Double;
        m_renderData.ViewIsland = world.Geometry.IslandGeometry.Islands[world.Geometry.BspTree.Subsectors[renderInfo.ViewerEntity.SubsectorId].IslandId];

        m_viewerEntity = renderInfo.ViewerEntity;
        m_geometryRenderer.Clear(renderInfo.TickFraction, true);
        m_renderData.CheckCount = ++WorldStatic.CheckCounter;

        m_renderData.MaxDistance = renderInfo.Uniforms.MaxDistance;

        m_renderData.MaxDistanceSquared = m_renderData.MaxDistance * m_renderData.MaxDistance;
        m_renderData.OccludePos = m_occlude ? m_occludeViewPos : null;
        Box2D box = new(m_renderData.ViewPosInterpolated.X, m_renderData.ViewPosInterpolated.Y, m_renderData.MaxDistance);

        Vec2D occluder = m_renderData.OccludePos ?? Vec2D.Zero;
        bool occlude = m_renderData.OccludePos.HasValue;

        var renderBlocks = world.RenderBlockmap.Blocks;
        var it = renderBlocks.CreateBoxIteration(box);
        for (int by = it.BlockStart.Y; by <= it.BlockEnd.Y; by++)
        {
            for (int bx = it.BlockStart.X; bx <= it.BlockEnd.X; bx++)
            {
                Block block = renderBlocks[by * it.Width + bx];
                if (occlude && !block.Box.InView(occluder, m_renderData.ViewDirection))
                    continue;

                RenderSectors(world, block);

                if (m_renderStatic)
                    RenderSides(block);

                for (var entity = block.HeadEntity; entity != null; entity = entity.RenderBlockNext)
                    RenderEntity(world, entity);
            }
        }

        m_lastTicker = world.GameTicker;
    }

    private void RenderSectors(IWorld world, Block block)
    {
        var sectorList = m_renderStatic ? block.DynamicSectors : block.Sectors;
        for (var islandNode = sectorList.Head; islandNode != null; islandNode = islandNode.Next)
        {
            var sectorIsland = islandNode.Value;
            if (sectorIsland.BlockmapCount == m_renderData.CheckCount)
                continue;

            sectorIsland.BlockmapCount = m_renderData.CheckCount;
            if (sectorIsland.ParentIsland != null && sectorIsland.ParentIsland != m_renderData.ViewIsland)
                continue;

            var sector = world.Sectors[sectorIsland.SectorId];
            if (sector.CheckCount == m_renderData.CheckCount)
                continue;

            double dx1 = Math.Max(sectorIsland.Box.Min.X - m_renderData.ViewPosInterpolated.X, Math.Max(0, m_renderData.ViewPosInterpolated.X - sectorIsland.Box.Max.X));
            double dy1 = Math.Max(sectorIsland.Box.Min.Y - m_renderData.ViewPosInterpolated.Y, Math.Max(0, m_renderData.ViewPosInterpolated.Y - sectorIsland.Box.Max.Y));
            if (dx1 * dx1 + dy1 * dy1 <= m_renderData.MaxDistanceSquared)
            {
                m_geometryRenderer.RenderSector(sector, m_renderData.ViewPos3D, m_renderData.ViewPosInterpolated3D);
                sector.CheckCount = m_renderData.CheckCount;
            }
        }
    }

    private void RenderSides(Block block)
    {
        // DynamicSides are either scrolling textures or alpha, neither should setup cover walls.
        m_geometryRenderer.SetBufferCoverWall(false);
        for (int i = 0; i < block.DynamicSides.Length; i++)
        {
            var side = block.DynamicSides.Data[i];
            if (side.BlockmapCount == m_renderData.CheckCount)
                continue;
            if (side.Sector.IsMoving || (side.PartnerSide != null && side.PartnerSide.Sector.IsMoving))
                continue;

            side.BlockmapCount = m_renderData.CheckCount;
            m_geometryRenderer.RenderSectorWall(side.Sector, side.Line, m_renderData.ViewPos3D, m_renderData.ViewPosInterpolated3D);
        }
        m_geometryRenderer.SetBufferCoverWall(true);
    }

    void RenderEntity(IWorld world, Entity entity)
    {
        if (entity.Frame.IsInvisible || entity.Flags.Invisible || entity.Flags.NoSector || entity == m_viewerEntity)
            return;

        // Not in front 180 FOV
        if (m_renderData.OccludePos.HasValue)
        {
            Vec2D entityToTarget = new(entity.Position.X - m_renderData.OccludePos.Value.X, entity.Position.Y - m_renderData.OccludePos.Value.Y);
            if (entityToTarget.Dot(m_renderData.ViewDirection) < 0)
                return;
        }

        double dx = Math.Max(entity.Position.X - m_renderData.ViewPosInterpolated.X, Math.Max(0, m_renderData.ViewPosInterpolated.X - entity.Position.X));
        double dy = Math.Max(entity.Position.Y - m_renderData.ViewPosInterpolated.Y, Math.Max(0, m_renderData.ViewPosInterpolated.Y - entity.Position.Y));
        entity.RenderDistanceSquared = dx * dx + dy * dy;
        if (entity.RenderDistanceSquared > m_renderData.MaxDistanceSquared)
            return;

        entity.LastRenderGametick = world.Gametick;
        m_entityRenderer.RenderEntity(entity, m_renderData.ViewPosInterpolated);     
    }

    protected override void PerformRender(IWorld world, RenderInfo renderInfo, GLFramebuffer framebuffer)
    {   
        // If the transfer height view is not the middle then the cached static geometry cannot be used.
        // Render all sectors dynamically instead.
        m_renderStatic = renderInfo.TransferHeightView == TransferHeightView.Middle;
        m_spriteTransparency = m_config.Render.SpriteTransparency;
        Clear(world, renderInfo);

        m_oitFrameBuffer.CreateOrUpdate((renderInfo.Viewport.Width, renderInfo.Viewport.Height));

        if (m_lastTicker != world.GameTicker)
            m_entityRenderer.Start(renderInfo);

        SetOccludePosition(renderInfo.Camera.PositionInterpolated.Double, renderInfo.Camera.YawRadians, renderInfo.Camera.PitchRadians,
            ref m_occlude, ref m_occludeViewPos);
        IterateBlockmap(world, renderInfo);
        PopulatePrimitives(world);

        m_geometryRenderer.RenderSkies(renderInfo);
        m_geometryRenderer.RenderPortals(renderInfo);

        if (m_renderStatic)
            m_geometryRenderer.RenderStaticSkies(renderInfo);

        if (!m_vanillaRender)
        {
            m_interpolationProgram.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            SetInterpolationUniforms(m_interpolationProgram, renderInfo);
            m_worldDataManager.RenderWalls();
            m_worldDataManager.RenderFlats();

            if (m_renderStatic)
            {
                m_staticProgram.Bind();
                GL.ActiveTexture(TextureUnit.Texture0);
                SetStaticUniforms(renderInfo);
                m_geometryRenderer.RenderStaticGeometryWalls();
                m_geometryRenderer.RenderStaticGeometryFlats();
            }

            RenderTwoSidedMiddleWalls(renderInfo);
            m_entityRenderer.RenderOpaque(renderInfo);
            RenderTransparent(renderInfo, framebuffer, false);
            m_primitiveRenderer.Render(renderInfo);
            return;
        }

        m_interpolationProgram.Bind();
        GL.ActiveTexture(TextureUnit.Texture0);
        SetInterpolationUniforms(m_interpolationProgram, renderInfo);
        m_worldDataManager.RenderWalls();
        m_worldDataManager.RenderFlats();

        if (m_renderStatic)
        {
            m_staticProgram.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            SetStaticUniforms(renderInfo);
            m_geometryRenderer.RenderStaticGeometryWalls();
            m_geometryRenderer.RenderStaticGeometryFlats();
        }

        RenderTwoSidedMiddleWalls(renderInfo);

        GL.Clear(ClearBufferMask.DepthBufferBit);
        GL.ColorMask(false, false, false, false);
        GL.Disable(EnableCap.CullFace);

        if (m_renderStatic)
        {
            m_staticProgram.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            m_geometryRenderer.RenderStaticCoverWalls();
        }

        m_interpolationProgram.Bind();
        m_worldDataManager.RenderCoverWalls();
        // Need to render flood fill again. Sprites need to be blocked by flood filling if visible.
        m_geometryRenderer.Portals.Render(renderInfo);
        GL.Enable(EnableCap.CullFace);

        if (m_renderStatic)
        {
            m_staticProgram.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            m_geometryRenderer.RenderStaticOneSidedCoverWalls();
        }

        RenderTwoSidedMiddleWalls(renderInfo);
        GL.ColorMask(true, true, true, true);

        m_entityRenderer.RenderOpaque(renderInfo);
        RenderTransparent(renderInfo, framebuffer, true);
        m_primitiveRenderer.Render(renderInfo);
    }

    private unsafe void RenderTransparent(RenderInfo renderInfo, GLFramebuffer framebuffer, bool vanillaRender)
    {
        bool fuzzData = m_entityRenderer.HasFuzz();
        GL.DepthMask(false);

        m_oitFrameBuffer.StartRender(framebuffer);
        m_entityRenderer.RenderOitTransparentPass(renderInfo);

        m_oitFrameBuffer.BindTextures(TextureUnit.Texture4, TextureUnit.Texture5, TextureUnit.Texture6, TextureUnit.Texture7, framebuffer);

        if (fuzzData)
        {
            if (GLInfo.MemoryBarrierSupported)
                GL.MemoryBarrier(MemoryBarrierFlags.FramebufferBarrierBit);

            ResetBlendEquations();
            framebuffer.Bind();
            // Refract pixels in the opaque framebuffer
            m_entityRenderer.RenderOitFuzzRefractionPass(renderInfo, false);

            m_oitFrameBuffer.SetBlendEquations();
            m_oitFrameBuffer.BindFrameBuffer();

            m_entityRenderer.RenderOitTransparentFuzzPass(renderInfo);
        }

        if (vanillaRender && m_worldDataManager.HasAlphaWalls())
            RenderFlatsToDepth(renderInfo);

        m_interpolationTransparentProgram.Bind();
        SetInterpolationUniforms(m_interpolationTransparentProgram, renderInfo);
        GL.ActiveTexture(TextureUnit.Texture0);
        m_worldDataManager.RenderAlphaWalls();

        ResetBlendEquations();
        framebuffer.Bind();

        m_entityRenderer.RenderOitCompositePass(renderInfo);

        if (m_worldDataManager.HasAlphaWalls())
        {
            if (vanillaRender)
                RenderFlatsToDepth(renderInfo);

            m_interpolationCompositeProgram.Bind();
            SetInterpolationUniforms(m_interpolationCompositeProgram, renderInfo);
            GL.ActiveTexture(TextureUnit.Texture0);
            m_worldDataManager.RenderAlphaWalls();
        }

        if (fuzzData)
            m_entityRenderer.RenderOitFuzzRefractionPass(renderInfo, true);

        GL.DepthMask(true);
    }

    private static void ResetBlendEquations()
    {
        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    private void RenderFlatsToDepth(RenderInfo renderInfo)
    {
        // Render flats so two-sided middle alpha walls are clipped to flats
        GL.DepthMask(true);
        GL.ColorMask(false, false, false, false);

        m_staticProgram.Bind();
        GL.ActiveTexture(TextureUnit.Texture0);
        SetStaticUniforms(renderInfo);
        m_geometryRenderer.RenderStaticGeometryFlats();

        m_interpolationProgram.Bind();
        GL.ActiveTexture(TextureUnit.Texture0);
        SetInterpolationUniforms(m_interpolationProgram, renderInfo);
        m_worldDataManager.RenderFlats();

        GL.DepthMask(false);
        GL.ColorMask(true, true, true, true);
    }

    private void RenderTwoSidedMiddleWalls(RenderInfo renderInfo)
    {
        m_interpolationProgram.Bind();
        GL.ActiveTexture(TextureUnit.Texture0);
        m_worldDataManager.RenderTwoSidedMiddleWalls();

        if (m_renderStatic)
        {
            m_staticProgram.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            SetStaticUniforms(renderInfo);
            m_geometryRenderer.RenderStaticTwoSidedWalls();
        }
    }

    public override void ResetInterpolation(IWorld world)
    {
        m_entityRenderer.ResetInterpolation(world);
    }

    public static void SetOccludePosition(in Vec3D position, double angleRadians, double pitchRadians, 
        ref bool occlude, ref Vec2D occludeViewPos)
    {
        // This is a hack until frustum culling exists.
        // Push the position back to stop occluding things that are straight up/down
        if (Math.Abs(pitchRadians) > MathHelper.QuarterPi)
        {
            occlude = false;
            return;
        }

        occlude = true;
        Vec2D unit = Vec2D.UnitCircle(angleRadians + MathHelper.Pi);
        occludeViewPos = position.XY + (unit * 32);
    }

    private void Clear(IWorld world, RenderInfo renderInfo)
    {
        bool newTick = world.GameTicker != m_lastTicker;
        m_geometryRenderer.Clear(renderInfo.TickFraction, newTick);

        if (newTick)
        {
            m_entityRenderer.Clear(world);
            m_worldDataManager.Clear();
        }
    }

    private void PopulatePrimitives(IWorld world)
    {
        var node = world.Player.Tracers.Tracers.First;
        while (node != null)
        {
            var info = node.Value;
            int ticks = info.Ticks <= 0 ? 0 : world.Gametick - info.Gametick;
            if (ticks > info.Ticks)
            {
                var removeNode = node;
                node = node.Next;
                world.Player.Tracers.Tracers.Remove(removeNode);
                continue;
            }
        
            float alpha = ticks == 0 ? 1 : (info.Ticks - ticks) / (float)ticks;
            for (var i = 0; i < info.Segs.Count; i++)
            {
                Seg3D tracer = info.Segs[i];
                AddSeg(tracer, node.Value.Color, alpha, info.Type);
            }

            node = node.Next;
        }
    }

    void AddSeg(Seg3D segment, Vec3F color, float alpha, PrimitiveRenderType type)
    {
        Seg3F seg = (segment.Start.Float, segment.End.Float);
        m_primitiveRenderer.AddSegment(seg, color, alpha, type);
    }

    private void SetInterpolationUniforms(InterpolationShader program, RenderInfo renderInfo)
    {
        program.BoundTexture(TextureUnit.Texture0);
        program.SectorLightTexture(TextureUnit.Texture1);
        program.ColormapTexture(TextureUnit.Texture2);
        program.SectorColormapTexture(TextureUnit.Texture3);
        program.HasInvulnerability(renderInfo.Uniforms.DrawInvulnerability);
        program.Mvp(renderInfo.Uniforms.Mvp);
        program.MvpNoPitch(renderInfo.Uniforms.MvpNoPitch);
        program.TimeFrac(renderInfo.TickFraction);
        program.LightLevelMix(renderInfo.Uniforms.Mix);
        program.ExtraLight(renderInfo.Uniforms.ExtraLight);
        program.DistanceOffset(renderInfo.Uniforms.DistanceOffset);
        program.ColorMix(renderInfo.Uniforms.ColorMix.Global);
        program.PaletteIndex((int)renderInfo.Uniforms.PaletteIndex);
        program.ColorMapIndex(renderInfo.Uniforms.ColorMapUniforms.GlobalIndex);
        program.LightMode(renderInfo.Uniforms.LightMode);
        program.GammaCorrection(renderInfo.Uniforms.GammaCorrection);

        if (program is InterpolationCompositeShader)
        {
            program.AccumTexture(TextureUnit.Texture4);
            program.AccumCountTextre(TextureUnit.Texture5);
        }
    }

    private void SetStaticUniforms(RenderInfo renderInfo)
    {
        m_staticProgram.BoundTexture(TextureUnit.Texture0);
        m_staticProgram.SectorLightTexture(TextureUnit.Texture1);
        m_staticProgram.ColormapTexture(TextureUnit.Texture2);
        m_staticProgram.SectorColormapTexture(TextureUnit.Texture3);
        m_staticProgram.HasInvulnerability(renderInfo.Uniforms.DrawInvulnerability);
        m_staticProgram.Mvp(renderInfo.Uniforms.Mvp);
        m_staticProgram.MvpNoPitch(renderInfo.Uniforms.MvpNoPitch);
        m_staticProgram.LightLevelMix(renderInfo.Uniforms.Mix);
        m_staticProgram.ExtraLight(renderInfo.Uniforms.ExtraLight);
        m_staticProgram.DistanceOffset(renderInfo.Uniforms.DistanceOffset);
        m_staticProgram.ColorMix(renderInfo.Uniforms.ColorMix.Global);
        m_staticProgram.PaletteIndex((int)renderInfo.Uniforms.PaletteIndex);
        m_staticProgram.ColorMapIndex(renderInfo.Uniforms.ColorMapUniforms.GlobalIndex);
        m_staticProgram.LightMode(renderInfo.Uniforms.LightMode);
        m_staticProgram.GammaCorrection(renderInfo.Uniforms.GammaCorrection);
    }

    private void ReleaseUnmanagedResources()
    {
        m_interpolationProgram.Dispose();
        m_staticProgram.Dispose();
        m_geometryRenderer.Dispose();
        m_worldDataManager.Dispose();
        m_primitiveRenderer.Dispose();
        m_entityRenderer.Dispose();
    }
}
