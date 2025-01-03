﻿using Helion.Geometry;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Buffers;
using System.IO;

namespace Helion.Graphics;

public static class ImageExtensions
{
    public static bool SavePng(this Image image, string path)
    {
        using FileStream fs = new(path, FileMode.CreateNew);
        return SavePng(image, fs, null);
    }

    public static bool SavePng(this Image image, Stream stream, Dimension? resize)
    {
        try
        {
            var pixels = image.Pixels;
            var data = ArrayPool<byte>.Shared.Rent(pixels.Length * 4); // rgba -> [r, g, b]

            if (image.ImageType == ImageType.Rgba)
            {
                for (int i = 0; i < pixels.Length; i++)
                {
                    uint pixel = pixels[i];
                    int offset = i * 4;
                    data[offset] = (byte)((pixel) & 0xFF);
                    data[offset + 1] = (byte)((pixel >> 8) & 0xFF);
                    data[offset + 2] = (byte)((pixel >> 16) & 0xFF);
                    data[offset + 3] = 255;
                }
            }
            else
            {
                for (int i = 0; i < pixels.Length; i++)
                {
                    uint pixel = pixels[i];
                    int offset = i * 4;
                    data[offset] = (byte)((pixel >> 16) & 0xFF);
                    data[offset + 1] = (byte)((pixel >> 8) & 0xFF);
                    data[offset + 2] = (byte)((pixel) & 0xFF);
                    data[offset + 3] = 255;
                }
            }

            using var pixelImage = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(data, image.Width, image.Height);
            if (resize.HasValue)
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(resize.Value.Width, resize.Value.Height),
                    Mode = ResizeMode.Crop,
                    PadColor = SixLabors.ImageSharp.Color.Black
                };

                pixelImage.Mutate(x => x.Resize(resizeOptions));
            }

            ArrayPool<byte>.Shared.Return(data);

            pixelImage.SaveAsPng(stream, null);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
