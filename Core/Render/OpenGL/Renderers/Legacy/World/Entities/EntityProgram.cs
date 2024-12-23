﻿using GlmSharp;
using Helion.Geometry.Vectors;
using Helion.Render.OpenGL.Renderers.Legacy.World.Shader;
using Helion.Render.OpenGL.Shader;
using Helion.Util.Configs.Components;
using OpenTK.Graphics.OpenGL;

namespace Helion.Render.OpenGL.Renderers.Legacy.World.Entities;

public class EntityProgram : RenderProgram
{
    private readonly int m_boundTextureLocation;
    private readonly int m_colormapTextureLocation;
    private readonly int m_sectorColormapTextureLocation;
    private readonly int m_mvpLocation;
    private readonly int m_timeFracLocation;
    private readonly int m_hasInvulnerabilityLocation;
    private readonly int m_mvpNoPitchLocation;
    private readonly int m_fuzzFracLocation;
    private readonly int m_lightLevelMixLocation;
    private readonly int m_extraLightLocation;
    private readonly int m_viewRightNormalLocation;
    private readonly int m_prevViewRightNormalLocation;
    private readonly int m_distanceOffsetLocation;
    private readonly int m_colorMixLocation;
    private readonly int m_fuzzDivLocation;
    private readonly int m_paletteIndexLocation;
    private readonly int m_colorMapIndexLocation;
    private readonly int m_lightModeLocation;
    private readonly int m_gammaCorrectionLocation;
    private readonly int m_maxDistanceLocation;
    private readonly int m_fadeDistanceLocation;
    private readonly int m_viewPosLocation;
    private readonly int m_accumTextureLocation;
    private readonly int m_accumCountTextureLocation;
    private readonly int m_fuzzTextureLocation;
    private readonly int m_opaqueTextureLocation;
    private readonly int m_renderFuzzLocation;
    private readonly int m_renderFuzzRefractionColorLocation;
    private readonly int m_screenBoundsLocation;

    public EntityProgram() : base("Entity")
    {
        m_boundTextureLocation = Uniforms.GetLocation("boundTexture");
        m_colormapTextureLocation = Uniforms.GetLocation("colormapTexture");
        m_sectorColormapTextureLocation = Uniforms.GetLocation("sectorColormapTexture");
        m_mvpLocation = Uniforms.GetLocation("mvp");
        m_timeFracLocation = Uniforms.GetLocation("timeFrac");
        m_hasInvulnerabilityLocation = Uniforms.GetLocation("hasInvulnerability");
        m_mvpNoPitchLocation = Uniforms.GetLocation("mvpNoPitch");
        m_fuzzFracLocation = Uniforms.GetLocation("fuzzFrac");
        m_lightLevelMixLocation = Uniforms.GetLocation("lightLevelMix");
        m_extraLightLocation = Uniforms.GetLocation("extraLight");
        m_viewRightNormalLocation = Uniforms.GetLocation("viewRightNormal");
        m_prevViewRightNormalLocation = Uniforms.GetLocation("prevViewRightNormal");
        m_distanceOffsetLocation = Uniforms.GetLocation("distanceOffset");
        m_colorMixLocation = Uniforms.GetLocation("colorMix");
        m_fuzzDivLocation = Uniforms.GetLocation("fuzzDiv");
        m_paletteIndexLocation = Uniforms.GetLocation("paletteIndex");
        m_colorMapIndexLocation = Uniforms.GetLocation("colormapIndex");
        m_lightModeLocation = Uniforms.GetLocation("lightMode");
        m_gammaCorrectionLocation = Uniforms.GetLocation("gammaCorrection");
        m_maxDistanceLocation = Uniforms.GetLocation("maxDistanceSquared");
        m_fadeDistanceLocation = Uniforms.GetLocation("fadeDistance");
        m_viewPosLocation = Uniforms.GetLocation("viewPos");
        m_accumTextureLocation = Uniforms.GetLocation("accum");
        m_accumCountTextureLocation = Uniforms.GetLocation("accumCount");
        m_fuzzTextureLocation = Uniforms.GetLocation("fuzzTexture");
        m_opaqueTextureLocation = Uniforms.GetLocation("opaqueTexture");
        m_renderFuzzLocation = Uniforms.GetLocation("renderFuzz");
        m_renderFuzzRefractionColorLocation = Uniforms.GetLocation("renderFuzzRefractionColor");
        m_screenBoundsLocation = Uniforms.GetLocation("screenBounds");
    }
    
    public void BoundTexture(TextureUnit unit) => Uniforms.Set(unit, m_boundTextureLocation);
    public void ColormapTexture(TextureUnit unit) => Uniforms.Set(unit, m_colormapTextureLocation);
    public void SectorColormapTexture(TextureUnit unit) => Uniforms.Set(unit, m_sectorColormapTextureLocation);
    public void AccumTexture(TextureUnit unit) => Uniforms.Set(unit, m_accumTextureLocation);
    public void AccumCountTextre(TextureUnit unit) => Uniforms.Set(unit, m_accumCountTextureLocation);
    public void FuzzTexture(TextureUnit unit) => Uniforms.Set(unit, m_fuzzTextureLocation);
    public void OpaqueTexture(TextureUnit unit) => Uniforms.Set(unit, m_opaqueTextureLocation);
    public void ExtraLight(int extraLight) => Uniforms.Set(extraLight, m_extraLightLocation);
    public void HasInvulnerability(bool invul) => Uniforms.Set(invul, m_hasInvulnerabilityLocation);
    public void LightLevelMix(float lightLevelMix) => Uniforms.Set(lightLevelMix, m_lightLevelMixLocation);
    public void Mvp(mat4 mvp) => Uniforms.Set(mvp, m_mvpLocation);
    public void MvpNoPitch(mat4 mvpNoPitch) => Uniforms.Set(mvpNoPitch, m_mvpNoPitchLocation);
    public void FuzzFrac(float frac) => Uniforms.Set(frac, m_fuzzFracLocation);
    public void TimeFrac(float frac) => Uniforms.Set(frac, m_timeFracLocation);
    public void ViewRightNormal(Vec2F viewRightNormal) => Uniforms.Set(viewRightNormal, m_viewRightNormalLocation);
    public void PrevViewRightNormal(Vec2F viewRightNormal) => Uniforms.Set(viewRightNormal, m_prevViewRightNormalLocation);
    public void DistanceOffset(float distance) => Uniforms.Set(distance, m_distanceOffsetLocation);
    public void ColorMix(Vec3F color) => Uniforms.Set(color, m_colorMixLocation);
    public void FuzzDiv(float div) => Uniforms.Set(div, m_fuzzDivLocation);
    public void PaletteIndex(int index) => Uniforms.Set(index, m_paletteIndexLocation);
    public void ColorMapIndex(int index) => Uniforms.Set(index, m_colorMapIndexLocation);
    public void LightMode(RenderLightMode mode) => Uniforms.Set((int)mode, m_lightModeLocation);
    public void GammaCorrection(float value) => Uniforms.Set(value, m_gammaCorrectionLocation);
    public void MaxDistanceSquared(float value) => Uniforms.Set(value, m_maxDistanceLocation);
    public void FadeDistance(float value) => Uniforms.Set(value, m_fadeDistanceLocation);
    public void ViewPos(Vec3F pos) => Uniforms.Set(pos, m_viewPosLocation);
    public void RenderFuzz(bool value) => Uniforms.Set(value, m_renderFuzzLocation);
    public void RenderFuzzRefractionColor(bool value) => Uniforms.Set(value, m_renderFuzzRefractionColorLocation);
    public void ScreenBounds(Vec2I value) => Uniforms.Set(value, m_screenBoundsLocation);

    protected override string VertexShader() => @"
        #version 330

        layout(location = 0) in vec3 pos;
        layout(location = 1) in float lightLevel;
        layout(location = 2) in float options;
        layout(location = 3) in vec3 prevPos;
        layout(location = 4) in vec3 sectorIndex;

        out float lightLevelOut;
        out float alphaOut;
        out float fuzzOut;
        out float flipUOut;
        out float colorMapTranslationOut;
        ${SectorColorMapVar}

        uniform float timeFrac;
        uniform samplerBuffer sectorColormapTexture;

        void main()
        {
            float splitOptions = options;
            float colorMapTranslation = trunc(splitOptions / 8);
            splitOptions -= (colorMapTranslation * 8);
            float flipU = trunc(splitOptions / 4);
            splitOptions -= (flipU * 4);
            float fuzz = trunc(splitOptions / 2);
            float alpha = splitOptions - (fuzz * 2);

            lightLevelOut = lightLevel;
            alphaOut = alpha;
            fuzzOut = fuzz;
            flipUOut = flipU;
            colorMapTranslationOut = colorMapTranslation;
            ${SectorColorMap}
            gl_Position = vec4(mix(prevPos, pos, timeFrac), 1.0);
        }
    "
    .Replace("${SectorColorMapVar}", ShaderVars.PaletteColorMode ? "out int sectorColorMapIndexOut;" : "out vec3 sectorColorMapIndexOut;")
    .Replace("${SectorColorMap}", ShaderVars.PaletteColorMode ?
        "sectorColorMapIndexOut = int(texelFetch(sectorColormapTexture, int(sectorIndex)).r);" :
        "sectorColorMapIndexOut = texelFetch(sectorColormapTexture, int(sectorIndex)).rgb;");

    protected override string? GeometryShader() => @"
        #version 330 core

        layout(points) in;
        layout(triangle_strip, max_vertices = 4) out;

        in float lightLevelOut[];
        in float alphaOut[];
        in float fuzzOut[];
        in float flipUOut[];
        in float colorMapTranslationOut[];
        ${SectorColorMapVar}

        out vec2 uvFrag;
        out float dist;
        out float fuzzDist;
        out float renderDistSquared;
        flat out float lightLevelFrag;
        flat out float alphaFrag;
        flat out float fuzzFrag;
        flat out float colorMapTranslationFrag;
        ${SectorColorMapFrag}

        uniform mat4 mvp;
        uniform mat4 mvpNoPitch;
        uniform vec2 viewRightNormal;
        uniform vec2 prevViewRightNormal;
        uniform sampler2D boundTexture;
        uniform float timeFrac;
        uniform vec3 viewPos;

        float distSquared(vec2 v1, vec2 v2) {
            vec2 length = v1.xy - v2.xy;
            return (length.x * length.x) + (length.y * length.y);
        }

        void main()
        {
            float leftU = clamp(flipUOut[0], 0, 1);
            float rightU = 1 - clamp(flipUOut[0], 0, 1);

            vec3 pos = gl_in[0].gl_Position.xyz;
            ivec2 textureDim = textureSize(boundTexture, 0);
            vec3 posMoveDir = vec3(mix(prevViewRightNormal, viewRightNormal, timeFrac), 0);
            vec3 minPos = pos;
            vec3 maxPos = pos + (posMoveDir * textureDim.x) + (vec3(0, 0, 1) * textureDim.y);

            // Triangle strip ordering is: v0 v1 v2, v2 v1 v3
            // We also need to be going counter-clockwise.
            // Also the UV's are inverted, so draw from 1 down to 0 along the Y.

            // fuzzDist is going to be the center of min/max.
            // This keeps the fuzz consistent across the texture.
            vec4 glPosMin = mvp * vec4(minPos.x, minPos.y, minPos.z, 1);
            vec4 glPosMax = mvp * vec4(maxPos.x, maxPos.y, maxPos.z, 1);
            fuzzDist = (glPosMin.${Depth} + glPosMax.${Depth}) / 2;
            // Render distance squared in 2d space for fade in/out effect
            renderDistSquared = distSquared(viewPos.xy, pos.xy);

            gl_Position = glPosMin;
            dist = (mvpNoPitch * vec4(minPos.x, minPos.y, minPos.z, 1)).${Depth};
            uvFrag = vec2(leftU, 1);
            lightLevelFrag = lightLevelOut[0];
            alphaFrag = alphaOut[0];
            fuzzFrag = fuzzOut[0];
            colorMapTranslationFrag = colorMapTranslationOut[0];
            sectorColorMapIndexFrag = sectorColorMapIndexOut[0];
            EmitVertex();

            gl_Position = mvp * vec4(maxPos.x, maxPos.y, minPos.z, 1);
            dist = (mvpNoPitch * vec4(maxPos.x, maxPos.y, minPos.z, 1)).${Depth};
            uvFrag = vec2(rightU, 1);
            lightLevelFrag = lightLevelOut[0];
            alphaFrag = alphaOut[0];
            fuzzFrag = fuzzOut[0];
            colorMapTranslationFrag = colorMapTranslationOut[0];
            sectorColorMapIndexFrag = sectorColorMapIndexOut[0];
            EmitVertex();

            gl_Position = mvp * vec4(minPos.x, minPos.y, maxPos.z, 1);
            dist = (mvpNoPitch * vec4(minPos.x, minPos.y, maxPos.z, 1)).${Depth};
            uvFrag = vec2(leftU, 0);
            lightLevelFrag = lightLevelOut[0];
            alphaFrag = alphaOut[0];
            fuzzFrag = fuzzOut[0];
            colorMapTranslationFrag = colorMapTranslationOut[0];
            sectorColorMapIndexFrag = sectorColorMapIndexOut[0];
            EmitVertex();

            gl_Position = glPosMax;
            dist = (mvpNoPitch * vec4(maxPos.x, maxPos.y, maxPos.z, 1)).${Depth};
            uvFrag = vec2(rightU, 0);
            lightLevelFrag = lightLevelOut[0];
            alphaFrag = alphaOut[0];
            fuzzFrag = fuzzOut[0];
            colorMapTranslationFrag = colorMapTranslationOut[0];
            sectorColorMapIndexFrag = sectorColorMapIndexOut[0];
            EmitVertex();
    
            EndPrimitive();
        }  
    "
    .Replace("${SectorColorMapVar}", ShaderVars.PaletteColorMode ? "in int sectorColorMapIndexOut[];" : "in vec3 sectorColorMapIndexOut[];")
    .Replace("${SectorColorMapFrag}", ShaderVars.PaletteColorMode ? "flat out int sectorColorMapIndexFrag;" : "flat out vec3 sectorColorMapIndexFrag;")
    .Replace("${Depth}", ShaderVars.Depth);

    protected override string? FragmentShader() => @"
        #version 330

        in vec2 uvFrag;
        in float dist;
        in float fuzzDist;
        in float renderDistSquared;
        flat in float lightLevelFrag;
        flat in float alphaFrag;
        flat in float fuzzFrag;
        flat in float colorMapTranslationFrag;

        ${SectorColorMapFragVariables}

        ${OutFragColor}

        uniform int hasInvulnerability;
        uniform float fuzzFrac;
        uniform sampler2D boundTexture;
        uniform samplerBuffer colormapTexture;
        uniform float lightLevelMix;
        uniform int extraLight;
        uniform vec3 colorMix;
        uniform float distanceOffset;
        uniform float fuzzDiv;
        uniform int paletteIndex;
        uniform int colormapIndex;
        uniform int lightMode;
        uniform float gammaCorrection;
        uniform float maxDistanceSquared;
        uniform float fadeDistance;
        uniform float renderFuzz;
        uniform int renderFuzzRefractionColor;
        uniform ivec2 screenBounds;

        ${OitVariables}
        ${FuzzFunction}

        void main()
        {
            ${LightLevelFragFunction}
            ${SectorColorMapFragFunction}
            ${FragColorFunction}
        }
    "
    .Replace("${LightLevelFragFunction}", LightLevel.FragFunction)
    .Replace("${FuzzFunction}", FragFunction.FuzzFunction)
    .Replace("${FragColorFunction}", FragFunction.FragColorFunction(FragColorFunctionOptions.Fuzz | FragColorFunctionOptions.Alpha | FragColorFunctionOptions.Colormap, ColorMapFetchContext.Entity, GetOitOptions(), GetPostProcess()))
    .Replace("${SectorColorMapFragVariables}", SectorColorMap.FragVariables)
    .Replace("${SectorColorMapFragFunction}", SectorColorMap.FragFunction)
    .Replace("${OitVariables}", FragFunction.OitFragVariables(GetOitOptions()))
    .Replace("${OutFragColor}", GetOutFragColor());

    private string GetPostProcess() 
    {
        string clearAlpha = @"        
        fragColor.a = fragColor.a > 0.5 ? 1.0 : 0.0;
        if (fragColor.a <= 0)
            discard;";

        if (GetOitOptions() != OitOptions.None)
            clearAlpha = string.Empty;

        return clearAlpha + @"
        if (renderDistSquared > maxDistanceSquared - fadeDistance) {
            float fade = (maxDistanceSquared - renderDistSquared) / fadeDistance;
            fragColor.a *= fade;
        }";
    }

    private OitOptions GetOitOptions()
    {
        if (this is EntityTransparentProgram)
            return OitOptions.OitTransparentPass;
        if (this is EntityCompositeProgram)
            return OitOptions.OitCompositePass;
        if (this is EntityFuzzRefractionProgram)
            return OitOptions.OitFuzzRefractionPass;
        return OitOptions.None;
    }

    private string GetOutFragColor()
    {
        var options = GetOitOptions();
        if (options == OitOptions.OitTransparentPass)
            return "";
        return "out vec4 fragColor;";
    }
}
