using ImageMagick;
using LibNoise.Renderer;
using RTWLib.MapGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTWLib.Extensions
{
    public static class ExtMagick
    {
        public static object _lockObject = new object();

        public static void ModifyPixel(this IPixelCollection pixels, int x, int y, MagickColor col)
        {

              pixels.SetPixel(x, y, new ushort[] { col.R, col.G, col.B });
            
        }

        public static void ModifyPixelThreadSafe(MagickImage img, int x, int y, MagickColor col)
        {
            lock (_lockObject)
            {
                using (IPixelCollection pixels = img.GetPixels())
                {
                    pixels.SetPixel(x, y, new ushort[] { col.R, col.G, col.B });
                }
            }
        }

        public static int[,] ToIntArray(this IPixelCollection pixels, int width, int height, bool blackWhite = false)
        {
            int[,] pix = new int[width, height];
            foreach(var p in pixels)
            {
                if (blackWhite)
                {
                    MagickColor col = p.ToColor();
                    if (col.R == col.B && col.B == col.G)
                    {
                        pix[p.X, p.Y] = 1;
                    }
                    else pix[p.X, p.Y] = 0;

                }
                else pix[p.X, p.Y] = p.ToColor().R;
            }
            return pix;
        }
    }
}
