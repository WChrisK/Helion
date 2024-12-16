using System;

namespace Helion.Render.OpenGL.Renderers.Legacy.World.Shader;

[Flags]
public enum FragColorFunctionOptions
{
    None,
    AddAlpha = 1,
    Alpha = 2,
    Fuzz = 4,
    Colormap = 8
}

public enum ColorMapFetchContext { Default, Hud, Entity }

public enum OitOptions
{
    None,
    OitTransparentPass,
    OitCompositePass
}

public class FragFunction
{
    public static string OitFragVariables(OitOptions options)
    {
        switch (options)
        {
            case OitOptions.OitTransparentPass:
                return @"
                  layout (location = 0) out vec4 accum;
                  layout (location = 1) out vec2 accumCount;";
            case OitOptions.OitCompositePass:
                return @"
                    uniform sampler2D accum;
                    uniform sampler2D accumCount;

                    // get the max value between three values
                    float max3(vec3 v) 
                    {
	                    return max(max(v.x, v.y), v.z);
                    }
                    ";
            default:
                return "";
        }
    }

    public static string FuzzFunction =>
        @"        
        // These two functions are found here:
        // https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83
        float rand(vec2 n) {
            return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
        }

        float noise(vec2 p) {
            vec2 ip = floor(p);
            vec2 u = fract(p);
            u = u * u * (3.0 - 2.0 * u);

            float res = mix(
	            mix(rand(ip), rand(ip + vec2(1.0, 0.0)), u.x),
	            mix(rand(ip + vec2(0.0, 1.0)), rand(ip + vec2(1.0, 1.0)), u.x), u.y);
            return res * res;
        }";

    public const int FuzzDistanceStep = 96;

    public static string FuzzFragFunction =>
        @"if (fuzzFrag > 0)
        {
            // The division/floor is to chunk pixels together to make
            // blocks. A larger denominator makes it more blocky.
            // Dividing by the distance makes the fuzz look more detailed from far away instead of getting gigantic blocks.
            vec2 blockCoordinate = floor(gl_FragCoord.xy / ceil((fuzzDiv/(max(1, fuzzDist/" + FuzzDistanceStep + @")))));
            ${FuzzBlackColor}
            fragColor.w *= clamp(noise(blockCoordinate * fuzzFrac), 0.2, 0.45);
        }"
        .Replace("${FuzzBlackColor}",
            // Fetch black color from current palette. This takes the pre-blended black color with red/yellow/green palettes.
            ShaderVars.PaletteColorMode ? 
                "fragColor.xyz = texelFetch(colormapTexture, usePalette * paletteSize).rgb;" : 
                "fragColor.xyz = vec3(0, 0, 0);");

    public static string AlphaFlag(bool lightLevel)
    {
        if (ShaderVars.PaletteColorMode)
            return "";

        return
            @"// Check for the reserved alpha value to indicate a full bright pixel.
            float fullBrightFlag = float(fragColor.w == 0.9960784313725490196078431372549);
            " + (lightLevel ? "lightLevel = mix(lightLevel, 1, fullBrightFlag);\n" : "") +
            "fragColor.w = mix(fragColor.w, 1, fullBrightFlag);\n";
    }

    public static string ColorMapFetch(bool lightLevel, ColorMapFetchContext ctx)
    {
        if (!ShaderVars.PaletteColorMode)
            return "";

        string indexAdd = lightLevel ?
            // sectorColorMapIndexFrag is overriding the colormapIndex uniform
            @"
            int useColormap = int(mix(colormapIndex, sectorColorMapIndexFrag, float(sectorColorMapIndexFrag > 0)));
            ${EntityColorMapFrag}
            int usePalette = paletteIndex;
            int lightLevelOffset = (lightColorIndex * 256);
            lightLevelOffset = int(mix(lightLevelOffset, 32 * 256, float(hasInvulnerability)));
            lightLevelOffset = int(mix(lightLevelOffset, 0, float(lightLevelMix)));"
            .Replace("${EntityColorMapFrag}", ctx == ColorMapFetchContext.Entity ?
                // if useColormap is not default(0) then override with the uniform colormap. This overrides translations with boom colormaps etc.
                @"useColormap = int(mix(useColormap, colorMapTranslationFrag, float(useColormap == 0)));"
                : "")
            :
            @"
            int useColormap = colormapIndex${HudClearColorMap};
            int usePalette = paletteIndex;
            int lightLevelOffset = ${LightOffset};
            lightLevelOffset = int(mix(lightLevelOffset, 32 * 256, float(hasInvulnerability${HudDrawColorMapFrag})));
            ${HudClearPalette}"
            .Replace("${HudClearColorMap}", ctx == ColorMapFetchContext.Hud && ShaderVars.PaletteColorMode ? "* int(drawColorMapFrag)" : "")
            .Replace("${HudDrawColorMapFrag}", ctx == ColorMapFetchContext.Hud ? "* drawColorMapFrag" : "")
            .Replace("${HudClearPalette}", ctx == ColorMapFetchContext.Hud && ShaderVars.PaletteColorMode ?
                @"usePalette = int(mix(0, usePalette, float(drawPaletteFrag)));"
                : "")
            .Replace("${LightOffset}", ctx == ColorMapFetchContext.Hud ? "int(hudColorMapIndexFrag) * 256" : "0");
            
        // Use the alpha flag to indicate we need to fetch from the colormap buffer since we don't need it for fullbright.
        return @"
                const int paletteSize = 256 * 34;
                const int colormapSize = paletteSize * 14;
                float colormapFetchFlag = float(fragColor.w == 0.9960784313725490196078431372549);                
                fragColor.w = mix(fragColor.w, 1, colormapFetchFlag);
                ${IndexAdd}
                int texIndex = lightLevelOffset + (useColormap * colormapSize) + (usePalette * paletteSize + int(fragColor.r * 255.0));
                vec3 fetchColor = texelFetch(colormapTexture, texIndex).rgb;
                fragColor.rgb = mix(fragColor.rgb, fetchColor, vec3(colormapFetchFlag, colormapFetchFlag, colormapFetchFlag));
                "
                .Replace("${IndexAdd}", indexAdd);
    }

    public static string FragColorFunction(FragColorFunctionOptions options, ColorMapFetchContext ctx = ColorMapFetchContext.Default, 
        OitOptions oitOptions = OitOptions.None, string postProcess = "")
    {
        var fragColor = "fragColor = texture(boundTexture, uvFrag.st);";
        if (oitOptions == OitOptions.OitTransparentPass)
            fragColor = "vec4 fragColor = texture(boundTexture, uvFrag.st);";

        return
            fragColor +
            (options.HasFlag(FragColorFunctionOptions.Colormap) ? ColorMapFetch(true, ctx) : "")
            + AlphaFlag(true) +
            (options.HasFlag(FragColorFunctionOptions.Fuzz) ? FuzzFragFunction : "") +
            (ShaderVars.PaletteColorMode ? "\n" : "fragColor.xyz *= lightLevel;\n") +
            (options.HasFlag(FragColorFunctionOptions.AddAlpha) ?
                @"fragColor.w = fragColor.w * alphaFrag + addAlphaFrag;"
                +
                GetClearAlpha(oitOptions)
                :
                "") +
            (options.HasFlag(FragColorFunctionOptions.Alpha) ?
                "fragColor.w *= alphaFrag;" :
                "") +
            @"
            if (fragColor.w <= 0.0)
                discard;

            fragColor.xyz *= min(colorMix, 1);
"
            + (ShaderVars.PaletteColorMode ? "" : "fragColor.xyz *= min(sectorColorMapIndexFrag, 1);")
            + InvulnerabilityFragColor
            + GammaCorrection()
            + postProcess
            + Oit(oitOptions);
    }

    private static string GetClearAlpha(OitOptions oitOptions)
    {
        if (oitOptions != OitOptions.None)
            return "";

        return @"// Don't write partially transparent pixels for two-sided middle to fix issues with texture filtering.
                fragColor.w = mix(fragColor.a > 0.5 ? 1.0 : 0.0, fragColor.w, addAlphaFrag);";
    }

    private static string Oit(OitOptions options)
    {
        if (options == OitOptions.None)
            return @"";

        if (options == OitOptions.OitTransparentPass)
            return @"
                float weight = clamp(10 / (1e-5 + pow(dist/1000, 2)) + pow(dist/8192, 6), 100.0, 1000.0);
                accum = vec4(fragColor.rgb * fragColor.a, fragColor.a) * weight;
                accumCount = vec2(fragColor.a, 1);
            ";

        return @"
            ivec2 coords = ivec2(gl_FragCoord.xy);

            // r is accumulated alpha, g is accumulation count
            vec2 counter = texelFetch(accumCount, coords, 0).rg;
            float alphaComponent = counter.r;
            float countComponent = counter.g;

            if (countComponent == 0)
                discard;

	        vec4 accumulation = texelFetch(accum, coords, 0);
	        if (isinf(max3(abs(accumulation.rgb)))) 
		        accumulation.rgb = vec3(accumulation.a);

	        // prevent floating point precision bug
	        vec3 average_color = accumulation.rgb / max(accumulation.a, 0.00001f);
            
	        fragColor = vec4(average_color, alphaComponent / countComponent);";
    }

    public static string GammaCorrection() => "fragColor.rgb = pow(fragColor.rgb, vec3(1.0/gammaCorrection));";

    public static string InvulnerabilityFragColor =>
        ShaderVars.PaletteColorMode ? "" :
    @"
    if (hasInvulnerability != 0)
    {
        float gray = fragColor.x * 0.299 + fragColor.y * 0.587 + fragColor.z * 0.144;
        gray = 1 - gray;
        fragColor.xyz = vec3(gray, gray, gray);
    }
";
}
