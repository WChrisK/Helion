using Helion.Geometry.Boxes;
using Helion.Geometry.Vectors;
using Helion.Resources;
using NLog;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Helion.Graphics.Fonts;

public static class TrueTypeFont
{
    private const int RenderFontSize = 64;
    private const char StartCharacter = (char)32;
    private const char EndCharacter = (char)126;
    private const int CharCount = EndCharacter - StartCharacter + 1;

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Reads a TTF from the data provided.
    /// </summary>
    /// <param name="name">The name of the font.</param>
    /// <param name="data">The data for the font.</param>
    /// <returns>The font, or null if it is not a TTF data set.</returns>
    public static Font? From(string name, byte[] data)
    {
        // I have no idea if this can throw, or if nulls get returned (and
        // that would be an exception anyways...) so I'm playing it safe.
        try
        {
            FontCollection fontCollection = new();
            using (MemoryStream stream = new(data))
            {
                FontFamily fontFamily = fontCollection.Add(stream);
                SixLabors.Fonts.Font imageSharpFont = fontFamily.CreateFont(RenderFontSize);
                RichTextOptions richTextOptions = new(imageSharpFont);

                string text = ComposeRenderableCharacters();
                Dictionary<char, Image> charImages = new Dictionary<char, Image>();

                // Use this to compute the maximum height needed for the entire font, so that all of the character
                // bitmaps can have the same height dimension.
                FontRectangle fontBounds = TextMeasurer.MeasureBounds(text, richTextOptions);

                foreach (char c in text)
                {
                    string charString = $"{c}";
                    // To measure the amount of room we need to render each character, we are using character advance
                    // for the width dimension.  Advance is how far "over" the renderer needs to move before drawing the
                    // next character.
                    // For height, we are using the maximum height dimension of the entire font, as computed above.
                    FontRectangle charAdvance = TextMeasurer.MeasureAdvance(charString, richTextOptions);

                    using (Image<Rgba32> charImage = new(
                        (int)Math.Ceiling(charAdvance.X + charAdvance.Width),
                        (int)Math.Ceiling(fontBounds.Y + fontBounds.Height)))
                    {
                        charImage.Mutate(ctx =>
                        {
                            ctx.Fill(Color.Transparent.ToImageSharp);
                            ctx.DrawText(richTextOptions, charString, Color.White.ToImageSharp);
                        });

                        charImages[c] = Image.FromImageSharp(charImage, ns: ResourceNamespace.Fonts)!;
                    }
                }

                var (glyphs, image) = ComposeFontGlyphs(charImages);
                return new Font(name, glyphs, image, isTrueTypeFont: true);
            }
        }
        catch (Exception e)
        {
            Log.Error("Unable to read TTF font, unexpected error: {0}", e.Message);
            return null;
        }
    }

    private static string ComposeRenderableCharacters()
    {
        var chars = Enumerable.Range(StartCharacter, CharCount).Select(char.ConvertFromUtf32);
        return string.Join("", chars);
    }

    private static (Dictionary<char, Glyph>, Image) ComposeFontGlyphs(Dictionary<char, Image> charImages)
    {
        Dictionary<char, Glyph> glyphs = new();

        int width = charImages.Values.Select(i => i.Width).Sum();
        int height = charImages.Values.Select(i => i.Height).Max();
        Image image = new(width, height, ImageType.Argb, Vec2I.Zero, ResourceNamespace.Fonts);

        int offsetX = 0;
        Vec2F totalDimension = (width, height);
        foreach ((char c, Image charImage) in charImages)
        {
            Vec2I start = (offsetX, 0);
            Box2I location = (start, start + (charImage.Width, charImage.Height));
            Vec2F uvStart = location.Min.Float / totalDimension;
            Vec2F uvEnd = location.Max.Float / totalDimension;
            Box2F uv = (uvStart, uvEnd);
            Glyph glyph = new(c, uv, location);
            glyphs[c] = glyph;

            charImage.DrawOnTopOf(image, (offsetX, 0));

            offsetX += charImage.Width;
        }

        return (glyphs, image);
    }
}
