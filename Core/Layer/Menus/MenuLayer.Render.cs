using Helion.Geometry;
using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using Helion.Graphics;
using Helion.Menus;
using Helion.Menus.Base;
using Helion.Menus.Base.Text;
using Helion.Menus.Impl;
using Helion.Render.Common;
using Helion.Render.Common.Enums;
using Helion.Render.Common.Renderers;
using Helion.Render.Common.Textures;
using Helion.Util;
using NAudio.Wave.SampleProviders;
using System;
using static Helion.Render.Common.RenderDimensions;

namespace Helion.Layer.Menus;

public partial class MenuLayer
{
    private const int ActiveMillis = 500;
    private const int SelectedOffsetX = -32;
    private const int SelectedOffsetY = 5;

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

        bool shouldRenderSaveGameDetails =
            menu is SaveMenu { IsSaveMenu: false }
            && m_config.Game.ExtendedSaveGameInfo;

        int offsetY = menu.TopPixelPadding;
        for (int i = 0; i < menu.Components.Count; i++)
        {
            IMenuComponent component = menu.Components[i];
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
                    DrawSaveRow(hud, saveRowComponent, isSelected, wasSelected, ref offsetY, shouldRenderSaveGameDetails);
                    break;
                default:
                    throw new Exception($"Unexpected menu component type for drawing: {component.GetType().FullName}");
            }

            if (isSelected)
            {
                m_previousSelectedComponent = menu.CurrentComponent;
            }
        }

        if (menu.CurrentComponent is MenuSaveRowComponent && m_saveGameSummary != null)
        {
            RenderSaveGameDetails(hud);
        }
    }

    private void DrawText(IHudRenderContext hud, MenuTextComponent text, ref int offsetY)
    {
        hud.Text(text.Text, text.FontName, text.Size, (0, offsetY), out Dimension area, both: Align.TopMiddle);
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

    private void DrawSaveRow(IHudRenderContext hud, MenuSaveRowComponent saveRowComponent, bool isSelected,
        bool wasPreviouslySelected, ref int offsetY, bool detailsEnabled)
    {
        const int LeftOffset = 32;
        const int DesiredRowVerticalPadding = 4;
        const int SelectionOffsetX = 6;
        const string FontName = Constants.Fonts.Small;
        int fontSize = hud.GetFontMaxHeight(FontName);
        const string LeftBarName = "M_LSLEFT";
        const string MiddleBarName = "M_LSCNTR";
        const string RightBarName = "M_LSRGHT";

        // draw row background
        if (!hud.Textures.TryGet(LeftBarName, out var leftHandle) ||
            !hud.Textures.TryGet(MiddleBarName, out var midHandle) ||
            !hud.Textures.TryGet(RightBarName, out var rightHandle))
        {
            return;
        }

        int offsetX = LeftOffset;
        Dimension leftDim = leftHandle.Dimension;
        Dimension midDim = midHandle.Dimension;
        Dimension rightDim = rightHandle.Dimension;

        hud.Image(LeftBarName, (offsetX, offsetY));
        offsetX += leftDim.Width;

        int menuRowWidth = detailsEnabled ? 180 : 248;

        int blocks = (int)Math.Ceiling((menuRowWidth - leftDim.Width - rightDim.Width) / (double)midDim.Width) + 1;
        for (int i = 0; i < blocks; i++)
        {
            hud.Image(MiddleBarName, (offsetX, offsetY));
            offsetX += midDim.Width;
        }

        hud.Image(RightBarName, (offsetX, offsetY));

        int rowBgHeight = MathHelper.Max(leftDim.Height, midDim.Height, rightDim.Width);

        // draw text
        string saveText = saveRowComponent.Text.Length > blocks ? saveRowComponent.Text.Substring(0, blocks) : saveRowComponent.Text;
        int textVerticalPadding = (rowBgHeight - fontSize) / 2;
        Vec2I origin = (LeftOffset + leftDim.Width + 4, offsetY + textVerticalPadding);
        hud.Text(saveText, FontName, fontSize, origin, out Dimension textArea);

        // draw row selector
        if (isSelected)
        {
            string selectedName = ShouldDrawActive ? Constants.MenuSelectIconActive : Constants.MenuSelectIconInactive;
            if (hud.Textures.TryGet(selectedName, out var handle))
            {
                Vec2I selectedOffset = TranslateDoomOffset(handle.Offset);
                selectedOffset += (LeftOffset - handle.Dimension.Width - SelectionOffsetX, offsetY - 2);
                hud.Image(selectedName, selectedOffset);
            }
        }

        // Vanilla graphics are 14px high, some PWADs are higher. Target a 4px gap and eat into it if needed.
        int rowTotalHeight = Math.Max(rowBgHeight, textArea.Height);
        int rowVerticalPadding = Math.Max(0, (14 + DesiredRowVerticalPadding) - rowTotalHeight);
        offsetY += rowTotalHeight + rowVerticalPadding;

        if (isSelected && detailsEnabled)
        {
            if (!wasPreviouslySelected)
            {
                m_saveGameSummary = saveRowComponent.SaveGame == null
                    ? null
                    : new SaveGameSummary(saveRowComponent.SaveGame);

                m_saveGameTexture = m_saveGameSummary?.UpdateSaveGameTexture(hud);
            }
        }
    }

    private void RenderSaveGameDetails(IHudRenderContext hud)
    {
        if (m_saveGameSummary == null)
        {
            return;
        }

        const int TextSize = 4;
        const string Font = Constants.Fonts.Small;
        const int BoxWidth = 80;
        const int ThumbnailHeight = 60;
        const int BoxHeight = ThumbnailHeight + 6 * 4 + 3;

        Vec2I boxUpperLeftBorder = (229, 31);
        Vec2I boxLowerRightBorder = boxUpperLeftBorder + (BoxWidth + 2, BoxHeight + 2);

        Vec2I boxUpperLeft = (230, 32);
        Vec2I boxLowerRight = boxUpperLeft + (BoxWidth, BoxHeight);

        hud.PushAlpha(0.65f);
        DrawBorderBox(hud, new HudBox(boxUpperLeftBorder, boxLowerRightBorder), Color.DarkGray, 1);
        hud.FillBox((boxUpperLeft, boxLowerRight), Color.Black);
        hud.PopAlpha();

        if (m_saveGameTexture != null)
        {
            var imageBox = new HudBox(boxUpperLeft, boxUpperLeft + (BoxWidth, ThumbnailHeight));
            hud.Image(SaveGameSummary.TEXTURENAME, imageBox);
        }

        Vec2I offset = boxUpperLeft + (2, ThumbnailHeight + 2);

        hud.Text(m_saveGameSummary.MapName, Font, TextSize, offset, out Dimension area, maxWidth: BoxWidth);
        offset += (0, area.Height);

        hud.Text(m_saveGameSummary.Date, Font, TextSize, offset, out area);
        offset += (0, area.Height);
        offset += (0, area.Height);

        foreach (string str in m_saveGameSummary?.Stats ?? Array.Empty<string>())
        {
            hud.Text(str, Constants.Fonts.Small, 4, offset, out area);
            offset += (0, area.Height);
        }
    }

    private void DrawBorderBox(IHudRenderContext hud, HudBox box, Color color, int size)
    {
        HudBox topLine = new((box.TopLeft.X + size, box.TopLeft.Y), (box.TopRight.X - size, box.TopRight.Y + size));
        HudBox bottomLine = new((box.BottomLeft.X + size, box.BottomLeft.Y - size), (box.BottomRight.X - size, box.BottomRight.Y));
        HudBox leftLine = new(box.TopLeft, (box.BottomLeft.X + size, box.BottomLeft.Y));
        HudBox rightLine = new((box.TopRight.X - size, box.TopRight.Y), box.BottomRight);
        hud.FillBox(topLine, color);
        hud.FillBox(bottomLine, color);
        hud.FillBox(leftLine, color);
        hud.FillBox(rightLine, color);
    }
}
