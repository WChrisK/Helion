﻿using Helion.Geometry.Vectors;
using Helion.Render.OpenGL.Shared;
using Helion.Render.OpenGL.Shared.World;
using Helion.Render.OpenGL.Texture.Legacy;
using Helion.Resources.Archives.Collection;
using Helion.World;
using Helion.World.Geometry.Sectors;
using Helion.World.Geometry.Sides;
using Helion.World.Static;
using System;
using System.Diagnostics;
using Helion.Render.OpenGL.Renderers.Legacy.World.Geometry.Portals.FloodFill;
using Helion.Util;
using Helion.World.Geometry.Lines;
using Helion.Render.OpenGL.Renderers.Legacy.World.Shader;

namespace Helion.Render.OpenGL.Renderers.Legacy.World.Geometry.Portals;

[Flags]
public enum FloodSet
{
    None = 0,
    Normal = 1,
    Alt = 2
}

public class PortalRenderer : IDisposable
{
    enum PushDir { Back, Forward }
    const int FakeWallHeight = Constants.MaxTextureHeight;

    private FloodFillRenderer m_floodFillRenderer;
    private readonly FloodFillRenderer m_floodFillStatic;
    private readonly FloodFillRenderer m_floodFillDynamic;
    private readonly ArchiveCollection m_archiveCollection;
    private readonly SectorPlane m_fakeFloor = new(SectorPlaneFace.Floor, 0, 0, 0);
    private readonly SectorPlane m_fakeCeiling = new(SectorPlaneFace.Floor, 0, 0, 0);
    private readonly double m_pushSegAmount;
    private TransferHeightView m_transferHeightView;
    private bool m_disposed;


    public PortalRenderer(ArchiveCollection archiveCollection, LegacyGLTextureManager glTextureManager)
    {
        m_archiveCollection = archiveCollection;
        m_floodFillStatic = new(glTextureManager, FloodFillRenderMode.Static);
        m_floodFillDynamic = new(glTextureManager, FloodFillRenderMode.Dynamic);
        m_floodFillRenderer = m_floodFillStatic;
        // ReversedZ allows for a much smaller push amount
        m_pushSegAmount = ShaderVars.ReversedZ ? 0.005 : 0.05;
        m_transferHeightView = TransferHeightView.Middle;
    }

    ~PortalRenderer()
    {
        Dispose(false);
    }

    public FloodFillRenderer GetStaticFloodFillRenderer() => m_floodFillStatic;

    public void SetTransferHeightView(TransferHeightView view)
    {
        m_transferHeightView = view;

        if (view == TransferHeightView.Middle)
        {
            m_floodFillRenderer = m_floodFillStatic;
            return;
        }

        m_floodFillRenderer = m_floodFillDynamic;
        m_floodFillDynamic.ClearVertices();
    }

    public void UpdateTo(IWorld world)
    {
        m_floodFillStatic.UpdateTo(world);
        m_floodFillDynamic.UpdateTo(world);
    }

    public void ClearStaticWall(int floodKey) =>
        m_floodFillRenderer.ClearStaticWall(floodKey);

    public FloodSet AddStaticFloodFillSide(Side facingSide, Side otherSide, Sector floodSector, SideTexture sideTexture, bool isFront, FloodFillRenderer? renderer = null) =>
        HandleStaticFloodFillSide(facingSide, otherSide, floodSector, sideTexture, isFront, false, renderer);

    public FloodSet UpdateStaticFloodFillSide(Side facingSide, Side otherSide, Sector floodSector, SideTexture sideTexture, bool isFront, FloodFillRenderer? renderer = null) =>
        HandleStaticFloodFillSide(facingSide, otherSide, floodSector, sideTexture, isFront, true, renderer);

    public void AddFloodFillPlane(Side facingSide, Sector floodSector, SectorPlanes planes, SectorPlaneFace face, bool isFront, FloodFillRenderer? renderer = null) =>
        HandleFloodFillPlane(facingSide, floodSector, planes, face, isFront, false, renderer);

    public void UpdateFloodFillPlane(Side facingSide, Sector floodSector, SectorPlanes planes, SectorPlaneFace face, bool isFront, FloodFillRenderer? renderer = null) =>
        HandleFloodFillPlane(facingSide, floodSector, planes,face, isFront, true, renderer);

    private void HandleFloodFillPlane(Side facingSide, Sector floodSector, SectorPlanes planes, SectorPlaneFace face, bool isFront, bool update, 
        FloodFillRenderer? useRenderer)
    {
        var renderer = useRenderer ?? m_floodFillRenderer;
        var line = facingSide.Line;
        var saveStart = line.Segment.Start;
        var saveEnd = line.Segment.End;
        WallVertices wall = default;

        PushSeg(line, facingSide, PushDir.Forward, m_pushSegAmount);

        if (face == SectorPlaneFace.Floor)
        {
            var top = floodSector.Floor;
            m_fakeFloor.TextureHandle = floodSector.Floor.TextureHandle;
            m_fakeFloor.Z = top.Z - FakeWallHeight;
            m_fakeFloor.PrevZ = floodSector.Floor.PrevZ - FakeWallHeight;
            m_fakeFloor.LightLevel = floodSector.LightLevel;

            WorldTriangulator.HandleTwoSidedLower(facingSide, top, m_fakeFloor, Vec2F.Zero, isFront, ref wall);

            if (update || m_transferHeightView != TransferHeightView.Middle)
                renderer.UpdateStaticWall(facingSide.FloorFloodKey, floodSector.Floor, wall, top.Z, double.MaxValue, isFloodFillPlane: true);
            else
                facingSide.FloorFloodKey = renderer.AddStaticWall(floodSector.Floor, wall, top.Z, double.MaxValue, isFloodFillPlane: true);
        }
        else
        {
            var bottom = floodSector.Ceiling;
            m_fakeCeiling.TextureHandle = floodSector.Ceiling.TextureHandle;
            m_fakeCeiling.Z = bottom.Z + FakeWallHeight;
            m_fakeCeiling.PrevZ = floodSector.Ceiling.PrevZ + FakeWallHeight;
            m_fakeCeiling.LightLevel = floodSector.LightLevel;

            WorldTriangulator.HandleTwoSidedUpper(facingSide, m_fakeCeiling, bottom, Vec2F.Zero, isFront, ref wall);

            if (update || m_transferHeightView != TransferHeightView.Middle)
                renderer.UpdateStaticWall(facingSide.CeilingFloodKey, floodSector.Ceiling, wall, double.MinValue, bottom.Z, isFloodFillPlane: true);
            else
                facingSide.CeilingFloodKey = renderer.AddStaticWall(floodSector.Ceiling, wall, double.MinValue, bottom.Z, isFloodFillPlane: true);
        }

        line.Segment.Start = saveStart;
        line.Segment.End = saveEnd;
    }

    private FloodSet HandleStaticFloodFillSide(Side facingSide, Side otherSide, Sector floodSector, SideTexture sideTexture, bool isFront, bool update,
        FloodFillRenderer? useRenderer)
    {
        var result = FloodSet.None;
        var renderer = useRenderer ?? m_floodFillRenderer;
        WallVertices wall = default;
        Sector facingSector = facingSide.Sector.GetRenderSector(m_transferHeightView);
        Sector otherSector = otherSide.Sector.GetRenderSector(m_transferHeightView);

        var line = facingSide.Line;
        var saveStart = line.Segment.Start;
        var saveEnd = line.Segment.End;

        // The middle texture renders over any potential flood textures. Push the flood texture slightly behind the line.
        PushSeg(facingSide.Line, facingSide, PushDir.Back, m_pushSegAmount);

        if (sideTexture == SideTexture.Upper)
        {
            SectorPlane top = facingSector.Ceiling;
            SectorPlane bottom = otherSector.Ceiling;
            WorldTriangulator.HandleTwoSidedUpper(facingSide, top, bottom, Vec2F.Zero, isFront, ref wall);
            double floodMaxZ = bottom.Z;
            if (!IsSky(floodSector.Ceiling))
            {
                result |= FloodSet.Normal;
                if (update || m_transferHeightView != TransferHeightView.Middle)
                    renderer.UpdateStaticWall(facingSide.UpperFloodKeys.Key1, floodSector.Ceiling, wall, double.MinValue, floodMaxZ);
                else
                    facingSide.UpperFloodKeys.Key1 = renderer.AddStaticWall(floodSector.Ceiling, wall, double.MinValue, floodMaxZ);
            }

            if (IgnoreAltFloodFill(facingSide, otherSide, SectorPlaneFace.Ceiling))
            {
                facingSide.Line.Segment.Start = saveStart;
                facingSide.Line.Segment.End = saveEnd;
                return result;
            }

            result |= FloodSet.Alt;

            bottom = facingSector.Ceiling;
            m_fakeCeiling.TextureHandle = floodSector.Ceiling.TextureHandle;
            m_fakeCeiling.Z = bottom.Z + FakeWallHeight;
            m_fakeCeiling.PrevZ = bottom.Z + FakeWallHeight;
            m_fakeCeiling.LightLevel = floodSector.LightLevel;
            WorldTriangulator.HandleTwoSidedLower(facingSide, m_fakeCeiling, bottom, Vec2F.Zero, !isFront, ref wall);

            var min = floodMaxZ;
            var max = double.MaxValue;

            if (update || m_transferHeightView != TransferHeightView.Middle)
                renderer.UpdateStaticWall(facingSide.UpperFloodKeys.Key2, facingSector.Ceiling, wall, min, max);
            else
                facingSide.UpperFloodKeys.Key2 = renderer.AddStaticWall(facingSector.Ceiling, wall, min, max);
        }
        else
        {
            Debug.Assert(sideTexture == SideTexture.Lower, $"Expected lower floor, got {sideTexture} instead");
            SectorPlane top = otherSector.Floor;
            SectorPlane bottom = facingSector.Floor;
            WorldTriangulator.HandleTwoSidedLower(facingSide, top, bottom, Vec2F.Zero, isFront, ref wall);
            double floodMinZ = top.Z;
            if (!IsSky(floodSector.Floor))
            {
                result |= FloodSet.Normal;
                if (update || m_transferHeightView != TransferHeightView.Middle)
                    renderer.UpdateStaticWall(facingSide.LowerFloodKeys.Key1, floodSector.Floor, wall, floodMinZ, double.MaxValue);
                else
                    facingSide.LowerFloodKeys.Key1 = renderer.AddStaticWall(floodSector.Floor, wall, floodMinZ, double.MaxValue);
            }

            if (IgnoreAltFloodFill(facingSide, otherSide, SectorPlaneFace.Floor))
            {
                facingSide.Line.Segment.Start = saveStart;
                facingSide.Line.Segment.End = saveEnd;
                return result;
            }

            // This is the alternate case where the floor will flood with the surrounding sector when the camera goes below the flood sector z.
            result |= FloodSet.Alt;
            top = facingSector.Floor;
            m_fakeFloor.TextureHandle = floodSector.Floor.TextureHandle;
            m_fakeFloor.Z = bottom.Z - FakeWallHeight;
            m_fakeFloor.PrevZ = bottom.Z - FakeWallHeight;
            m_fakeFloor.LightLevel = floodSector.LightLevel;
            WorldTriangulator.HandleTwoSidedLower(facingSide, top, m_fakeFloor, Vec2F.Zero, !isFront, ref wall);

            var min = double.MinValue;
            var max = floodMinZ;

            if (update || m_transferHeightView != TransferHeightView.Middle)
                renderer.UpdateStaticWall(facingSide.LowerFloodKeys.Key2, facingSector.Floor, wall, min, max);
            else
                facingSide.LowerFloodKeys.Key2 = renderer.AddStaticWall(facingSector.Floor, wall, min, max);
        }

        facingSide.Line.Segment.Start = saveStart;
        facingSide.Line.Segment.End = saveEnd;
        return result;
    }

    private static void PushSeg(Line line, Side facingSide, PushDir dir, double amount)
    {
        // Push it out to prevent potential z-fighting. Default pushes out from the sector.
        var angle = facingSide == line.Front ? line.Segment.Start.Angle(line.Segment.End) : line.Segment.End.Angle(line.Segment.Start);
        if (dir == PushDir.Forward)
            angle += MathHelper.Pi;

        var unit = Vec2D.UnitCircle(angle + MathHelper.HalfPi) * amount;
        line.Segment.Start += unit;
        line.Segment.End += unit;
    }

    private bool IgnoreAltFloodFill(Side facingSide, Side otherSide, SectorPlaneFace face)
    {
        return IsSky(facingSide.Sector.GetSectorPlane(face)) || IsSky(otherSide.Sector.GetSectorPlane(face));
    }

    public void Render(RenderInfo renderInfo)
    {
        m_floodFillRenderer.Render(renderInfo);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (m_disposed)
            return;

        m_floodFillStatic.Dispose();
        m_floodFillDynamic.Dispose();

        m_disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool IsSky(SectorPlane plane) => m_archiveCollection.TextureManager.IsSkyTexture(plane.TextureHandle);
}
