﻿using Helion.Graphics;
using Helion.Render.Common.Enums;
using Helion.Render.Common.Renderers;
using Helion.Render.Common.Textures;
using Helion.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helion.Util.Extensions;

public static class HudExtensions
{
    private readonly record struct HudImage(IHudRenderContext Hud, string Image, IRenderableTextureHandle Handle, Align Window, Align Anchor, float Alpha = 1f);

    public static IRenderableTextureHandle CreateOrReplaceImage(this IHudRenderContext hud, Image image, string imageName, ResourceNamespace resourceNamespace, bool repeatY = true)
    {
        return hud.Textures.CreateOrReplaceTexture(imageName, resourceNamespace, image, repeatY: repeatY);
    }

    public static bool RenderFullscreenImage(this IHudRenderContext hud, string image,
        Align window = Align.TopLeft, Align anchor = Align.TopLeft, float alpha = 1f, float aspectRatioDivisor = 1.2f)
    {
        if (!hud.Textures.TryGet(image, out var handle))
            return false;

        hud.VirtualDimension(handle.Dimension, ResolutionScale.Center, handle.Dimension.AspectRatio / aspectRatioDivisor, HudVirtualFullscreenImage,
            new HudImage(hud, image, handle, window, anchor, alpha));
        return true;
    }

    public static bool RenderStatusBar(this IHudRenderContext hud, string image)
    {
        if (!hud.Textures.TryGet(image, out var handle))
            return false;

        float statusBarRatio = handle.Dimension.Width * 2 / 480f;
        if (hud.Dimension.Width < 480)
            statusBarRatio = handle.Dimension.Width * hud.Dimension.AspectRatio / (float)hud.Dimension.Width;

        hud.VirtualDimension((handle.Dimension.Width, 200), ResolutionScale.Center, statusBarRatio, HudVirtualStatusBar,
            new HudImage(hud, image, handle, Align.Center, Align.Center));
        return true;
    }

    private static void HudVirtualFullscreenImage(HudImage hud)
    {
        hud.Hud.Image(hud.Image, (0, 0, hud.Handle.Dimension.Width, hud.Handle.Dimension.Height), hud.Window, hud.Anchor, alpha: hud.Alpha);
    }

    private static void HudVirtualStatusBar(HudImage hud)
    {
        hud.Hud.Image(hud.Image, (0, 0, hud.Handle.Dimension.Width, hud.Handle.Dimension.Height), both: Align.BottomLeft);
    }

    public static string GetEllipsesText(this IHudRenderContext hud, string text, string font, int fontSize, int maxWidth)
    {
        int nameWidth = hud.MeasureText(text, font, fontSize).Width;
        if (nameWidth <= maxWidth)
            return text;

        var textSpan = text.AsSpan();
        int sub = 1;
        while (sub < textSpan.Length && hud.MeasureText(textSpan, font, fontSize).Width > maxWidth)
        {
            textSpan = text.AsSpan(0, text.Length - sub);
            sub++;
        }

        if (textSpan.Length <= 3)
            return text;

        return string.Concat(text.AsSpan(0, textSpan.Length - 3), "...");
    }

    public static string GetTypedText(this IHudRenderContext hud, string text, string font, int fontSize, int maxWidth)
    {
        int nameWidth = hud.MeasureText(text, font, fontSize).Width;
        if (nameWidth <= maxWidth)
            return text;

        var textSpan = text.AsSpan();
        int sub = 1;
        while (sub < textSpan.Length && hud.MeasureText(textSpan, font, fontSize).Width > maxWidth)
        {
            textSpan = text.AsSpan(sub, text.Length - sub);
            sub++;
        }

        if (textSpan.Length - sub <= 0)
            return string.Empty;

        return text.AsSpan(sub).ToString();
    }

    public static ReadOnlySpan<char> TruncateText(this IHudRenderContext hud, string inputText, string font, int fontSize, int maxWidth)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            return inputText;
        }

        for (int i = 0; i < inputText.Length; i++)
        {
            if (hud.MeasureText(inputText.AsSpan(0, i + 1), font, fontSize).Width > maxWidth)
            {
                return inputText.AsSpan(0, i);
            }
        }

        return inputText;
    }

    public static void LineWrap(this IHudRenderContext hud, string inputText, string font, int fontSize, int maxWidth, List<string> lines, StringBuilder builder,
        out int requiredHeight)
    {
        lines.Clear();
        builder.Clear();
        if (string.IsNullOrEmpty(inputText))
        {
            requiredHeight = 0;
            return;
        }

        int maxTokenHeight = 0;
        int widthCounter = 0;

        int splitStart;
        int splitEnd = 0;
        for (int i = 0; i < inputText.Length; i++)
        {
            if (inputText[i] == ' ' || i == inputText.Length - 1)
            {
                splitStart = splitEnd;
                splitEnd = i;
            }
            else
            {
                continue;
            }

            splitEnd++;
            var token = inputText.AsSpan(splitStart, splitEnd - splitStart);
            var tokenSize = hud.MeasureText(token, font, fontSize);
            maxTokenHeight = Math.Max(maxTokenHeight, tokenSize.Height);

            if (widthCounter + tokenSize.Width > maxWidth)
            {
                lines.Add(builder.ToString());
                builder.Clear();
                widthCounter = 0;
            }

            builder.Append(token);
            widthCounter += tokenSize.Width;
        }

        // Flush the last line out of the StringBuilder
        if (builder.Length > 0)
            lines.Add(builder.ToString());

        requiredHeight = lines.Count * maxTokenHeight;
    }
}
