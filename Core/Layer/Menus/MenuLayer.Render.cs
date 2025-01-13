using Helion.Geometry;
using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using Helion.Graphics;
using Helion.Layer.Options;
using Helion.Layer.Options.Sections;
using Helion.Menus;
using Helion.Menus.Base;
using Helion.Menus.Base.Text;
using Helion.Menus.Impl;
using Helion.Render.Common;
using Helion.Render.Common.Enums;
using Helion.Render.Common.Renderers;
using Helion.Render.Common.Textures;
using Helion.Util;
using Helion.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using static Helion.Render.Common.RenderDimensions;

namespace Helion.Layer.Menus;

public partial class MenuLayer
{
    private const int ActiveMillis = 500;
    private const int SelectedOffsetX = -32;
    private const int SelectedOffsetY = 5;

    private readonly List<string> m_mapNameLines = [];
    private readonly StringBuilder m_mapNameStringBuilder = new();

    private IMenuComponent? m_previousSelectedComponent;
    private IRenderableTextureHandle? m_saveGameTexture;
    private SaveGameSummary? m_saveGameSummary;

    private bool ShouldDrawActive => (m_stopwatch.ElapsedMilliseconds % ActiveMillis) <= ActiveMillis / 2;

    public void Render(IHudRenderContext hud)
    {
        Animation.Tick();
        hud.FillBox((0, 0, hud.Width, hud.Height), Color.Black, alpha: 0.5f);
        hud.DoomVirtualResolution(m_renderVirtualHudAction, hud);
    }

    private void RenderVirtualHud(IHudRenderContext hud)
    {
        if (!m_menus.TryPeek(out Menu? menu))
            return;

        var saveMenu = menu.CurrentComponent is MenuSaveRowComponent;
        var offsetY = menu.TopPixelPadding;
        var detailsEnabled = m_config.Game.ExtendedSaveGameInfo;
        var firstRow = true;

        if (saveMenu)
            DrawSaveMenuBox(hud, detailsEnabled);

        for (int i = 0; i < menu.Components.Count; i++)
        {
            var component = menu.Components[i];
            bool isSelected = ReferenceEquals(menu.CurrentComponent, component);
            bool wasSelected = ReferenceEquals(menu.CurrentComponent, m_previousSelectedComponent);

            switch (component)
            {
                case MenuImageComponent imageComponent:
                    DrawImage(hud, imageComponent, isSelected, ref offsetY, imageComponent.UpscaleWithText ? m_config.Hud.FontUpscalingFactor : 1);
                    break;
                case MenuPaddingComponent paddingComponent:
                    offsetY += paddingComponent.PixelAmount;
                    break;
                case MenuSmallTextComponent smallTextComponent:
                    DrawText(hud, smallTextComponent, ref offsetY);
                    break;
                case MenuLargeTextComponent largeTextComponent:
                    DrawText(hud, largeTextComponent, ref offsetY);
                    break;
                case MenuSaveRowComponent saveRowComponent:
                    if (firstRow)
                    {
                        offsetY++;
                        firstRow = false;
                    }
                    DrawSaveRow(hud, (SaveMenu)menu, saveRowComponent, isSelected, wasSelected, ref offsetY, detailsEnabled);
                    break;
                default:
                    throw new Exception($"Unexpected menu component type for drawing: {component.GetType().FullName}");
            }

            if (isSelected)
                m_previousSelectedComponent = menu.CurrentComponent;
        }

        if (saveMenu && m_saveGameSummary != null)
            RenderSaveGameDetails(hud);
    }

    private static void DrawText(IHudRenderContext hud, MenuTextComponent text, ref int offsetY)
    {
        var align = text.Align ?? Align.TopMiddle;
        if (align != Align.TopMiddle)
            offsetY = 0;

        hud.Text(text.Text, text.FontName, text.Size, (0, offsetY), out Dimension area, both: align);

        if (align == Align.TopMiddle)
            offsetY += area.Height;
    }

    private void DrawImage(IHudRenderContext hud, MenuImageComponent image, bool isSelected, ref int offsetY, int upscalingFactor)
    {
        int drawY = image.PaddingTopY + offsetY;
        if (image.AddToOffsetY)
            offsetY += image.PaddingTopY;

        if (hud.Textures.TryGet(image.ImageName, out var handle, upscalingFactor: upscalingFactor))
        {
            Vec2I offset = TranslateDoomOffset(handle.Offset);
            int offsetX = offset.X + image.OffsetX;

            hud.Image(image.ImageName, (offsetX, drawY + offset.Y), out HudBox area, both: image.ImageAlign, upscalingFactor: upscalingFactor);

            if (isSelected)
                DrawSelectedImage(hud, image, drawY, offsetX);

            if (!image.AddToOffsetY)
                return;

            if (image.OverrideY == null)
                offsetY += area.Height + offset.Y + image.PaddingBottomY;
            else
                offsetY += image.OverrideY.Value;
        }
        else if (!string.IsNullOrEmpty(image.Title))
        {
            const int FontSize = 12;
            const int TextOffsetX = 48;
            Dimension textDimensions = hud.MeasureText(image.Title, Constants.Fonts.Small, FontSize);
            hud.Text(image.Title, Constants.Fonts.Small, FontSize, (TextOffsetX, drawY), both: image.ImageAlign);
            offsetY += textDimensions.Height + 2;

            if (isSelected)
                DrawSelectedImage(hud, image, drawY, TextOffsetX);
        }
    }

    private void DrawSelectedImage(IHudRenderContext hud, MenuImageComponent image, int drawY, int offsetX)
    {
        string selectedName = (ShouldDrawActive ? image.ActiveImage : image.InactiveImage) ?? "";
        if (!hud.Textures.TryGet(selectedName, out var selectedHandle))
            return;

        offsetX += SelectedOffsetX;
        Vec2I selectedOffset = TranslateDoomOffset(selectedHandle.Offset);
        Vec2I drawPosition = selectedOffset + (offsetX, drawY - SelectedOffsetY);
        hud.Image(selectedName, drawPosition, both: image.ImageAlign);
    }

    private static int GetSaveRowWidth(bool detailsEnabled) => detailsEnabled ? 218 : 301;

    private void DrawSaveRow(IHudRenderContext hud, SaveMenu saveMenu, MenuSaveRowComponent saveRowComponent, bool isSelected,
        bool wasPreviouslySelected, ref int offsetY, bool detailsEnabled)
    {
        const string FontName = Constants.Fonts.Small;
        int fontSize = hud.GetFontMaxHeight(FontName);
        int menuRowWidth = GetSaveRowWidth(detailsEnabled);

        var textDimension = hud.MeasureText("_", FontName, fontSize);
        var textHeight = textDimension.Height; 

        string saveText;
        if (isSelected && saveMenu.IsTypingName)
        {
            var rowWidth = menuRowWidth - 10;
            //Account for cursor flashing
            if (!saveRowComponent.Text.EndsWith('_'))
                rowWidth -= textDimension.Width;
            saveText = hud.GetTypedText(saveRowComponent.Text, FontName, fontSize, rowWidth - 4);
        }
        else
        {
            saveText = hud.GetEllipsesText(saveRowComponent.Text, FontName, fontSize, menuRowWidth - 4);
        }
        
        var rowHeight = textHeight + 3;
        hud.PushOffset((17, 0));

        if (isSelected)
        {
            hud.PushAlpha(0.5f);
            HudBox box = new((0, offsetY), (menuRowWidth - 8, offsetY + rowHeight));
            hud.FillBox(box, Color.Blue);
            hud.PopAlpha();
        }

        hud.Text(saveText, FontName, fontSize, (1, offsetY + 2));
        offsetY += rowHeight;

        if (isSelected && detailsEnabled && !wasPreviouslySelected)
        {
            m_saveGameSummary = saveRowComponent.SaveGame == null
                ? null
                : new SaveGameSummary(saveRowComponent.SaveGame);

            m_saveGameTexture = m_saveGameSummary?.UpdateSaveGameTexture(hud);
        }

        hud.PopOffset();
    }

    private static void DrawSaveMenuBox(IHudRenderContext hud, bool detailsEnabled)
    {
        int height = 167;
        hud.PushOffset((16, 20));
        int saveRowWidth = GetSaveRowWidth(detailsEnabled);
        var box = new HudBox((0, 0), (saveRowWidth - 6, height));
        hud.PushAlpha(0.65f);
        hud.BorderBox(box, Color.DarkGray, 1);
        box = new HudBox((1, 1), (saveRowWidth - 7, height - 1));
        hud.FillBox(box, Color.Black);
        hud.PopAlpha();
        hud.PopOffset();
    }

    private void RenderSaveGameDetails(IHudRenderContext hud)
    {
        if (m_saveGameSummary == null)
            return;

        const int TextSize = 4;
        const string Font = Constants.Fonts.Small;
        const int BoxWidth = 80;
        const int ThumbnailHeight = 60;
        int BoxHeight = ThumbnailHeight + 5 * TextSize + 3;

        hud.LineWrap(m_saveGameSummary.MapName, Font, TextSize, BoxWidth - 4, m_mapNameLines, m_mapNameStringBuilder, 
            out var requiredHeight);
        BoxHeight += requiredHeight;

        Vec2I boxUpperLeftBorder = (229, 20);
        Vec2I boxLowerRightBorder = boxUpperLeftBorder + (BoxWidth + 2, BoxHeight + 2);

        Vec2I boxUpperLeft = (230, 21);
        Vec2I boxLowerRight = boxUpperLeft + (BoxWidth, BoxHeight);

        hud.PushAlpha(0.65f);
        hud.BorderBox(new HudBox(boxUpperLeftBorder, boxLowerRightBorder), Color.DarkGray, 1);
        hud.FillBox((boxUpperLeft, boxLowerRight), Color.Black);
        hud.PopAlpha();

        if (m_saveGameTexture == null)
        {
            hud.PushOffset(boxUpperLeft);
            var size = hud.MeasureText("No Image", Font, TextSize);
            hud.Text("No Image", Font, TextSize, (BoxWidth / 2 - size.Width / 2, ThumbnailHeight / 2 - size.Height / 2), textAlign: TextAlign.Center);
            hud.PopOffset();
        }
        else
        {
            var imageBox = new HudBox(boxUpperLeft, boxUpperLeft + (BoxWidth, ThumbnailHeight));
            hud.Image(SaveGameSummary.TEXTURENAME, imageBox);
        }

        Vec2I offset = boxUpperLeft + (2, ThumbnailHeight + 2);

        for (int i = 0; i< m_mapNameLines.Count; i++)
        {
            hud.Text(m_mapNameLines[i], Font, TextSize, offset, out var drawArea);
            offset += (0, drawArea.Height);
        }

        hud.Text(m_saveGameSummary.Date, Font, TextSize, offset, out var area);
        offset += (0, area.Height);
        offset += (0, area.Height);

        foreach (string str in m_saveGameSummary?.Stats ?? [])
        {
            hud.Text(str, Constants.Fonts.Small, 4, offset, out area);
            offset += (0, area.Height);
        }
    }
}
