using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LibNoise;
using LibNoise.Builder;
using ImageMagick;
using System.Drawing;
using RTWLib.Extensions;
using LibNoise.Primitive;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using RTWLib.MapGen.Voronoi;
using LibNoise.Filter;
using LibNoise.Modifier;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms.PropertyGridInternal;
using System.ComponentModel;
using RTWLib.Memory;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
        protected void GenerateLandMasses(Random rng)
        {
            int tilesAvailable = width * height;
            float[,] geographyArea = new float[width, height];

            int tileNeeded = (int)((elevation / 100) * tilesAvailable);
            int totalLandMass;

            map.mapHeights = new MagickImage(MapColours.mapWaterColours[WaterColours.HeightsWater], width, height);
            while ((totalLandMass = geographyArea.CountValues()) < tileNeeded)
            {
                int[] borders; // minx, miny, maxx, maxy
                var newChunk = GenerateChunk(rng, (int)(0.60f * tilesAvailable), out borders );
                Combine(ref geographyArea, newChunk);
                DrawMap(map.mapHeights, geographyArea, borders);
            }


            ThreadStart[] workers = new ThreadStart[threads + 2];
            Thread[] work = new Thread[threads +  2];
            List<int[]> threadBounds = new List<int[]>();

            float segment = width / threads;

            //NeatenHeights(ref geographyArea, roughness, new int[4] { (int)(1 * segment), 0, (int)((1 * segment) + segment), height }, map.mapHeights);
            for (int i = 0; i < threads; i++)
            {
                int index = i;
                if (index == threads - 1)
                {
                    int max = (int)((index * segment) + segment);
                    int rmax = (width - max) + max;
                    threadBounds.Add(new int[4] { (int)(index * segment), 0, rmax, height });
                }
                 else threadBounds.Add(new int[4] {(int)(index*segment), 0, (int)((index*segment)+segment), height});
                threadManager.CreateThread(() => NeatenHeights(ref geographyArea, roughness, threadBounds[index], map.mapHeights, index.ToString()));
            }

            threadManager.Start();
            //DrawMap(map.mapHeights, geographyArea);
        }

        protected void DrawMap(MagickImage image, float[,] geog, int[] borders = null)
        {
            using (IPixelCollection<UInt16> pixels = image.GetPixels())
            {
                if(borders !=null)
                {
                    for (int x = borders[0]; x < borders[2]; x++)
                    {
                        for (int y = borders[1]; y < borders[3]; y++)
                        {
                            MagickColor col;
                            float val = (float)geog[x, y] / 100f;
                            byte bval = (byte)(val * 255);
                            if (bval < sealevel)
                                col = MapColours.mapWaterColours[WaterColours.HeightsWater];
                            else
                                col = MagickColor.FromRgb(bval, bval, bval);
                            pixels.ModifyPixel(x, y, col);
                        }
                    }

                }
                else if (borders == null)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            MagickColor col;
                            float val = (float)geog[x, y] / 100f;
                            byte bval = (byte)(val * 255);
                            if (bval < sealevel)
                                col = MapColours.mapWaterColours[WaterColours.HeightsWater];
                            else
                                col = MagickColor.FromRgb(bval, bval, bval);
                            pixels.ModifyPixel(x, y, col);
                        }
                    }
                }
            }

            UpdateImage(image);
        }

        protected void DrawPixels(MagickImage image, HashSet<int[]> pixelsCoords, float[,] geog)
        {
            using (IPixelCollection<UInt16> pixels = image.GetPixels())
            {

                foreach (var p in pixelsCoords)
                {
                    MagickColor col;
                    float val = (float)geog[p[0], p[1]] / 100f;
                    byte bval = (byte)(val * 255);
                    if (bval < sealevel)
                        col = MapColours.mapWaterColours[WaterColours.HeightsWater];
                    else
                        col = MagickColor.FromRgb(bval, bval, bval);
                    this.map.mapTiles[p[0], p[1]].height = bval;
                    pixels.ModifyPixel(p[0], p[1], col);
                }
            }

            UpdateImage(image);
        }

        protected void Combine(ref float[,] geog, float[,] chunk)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    geog[x, y] += chunk[x, y];
                }
        } 

        protected float[,] GenerateChunk(Random rng, int chunkLength, out int[] borders)
        {
            float[,] stencilArea = new float[width, height];
            stencilArea = stencilArea.InitiliseWithValue(0);
            //start position
            int x, y, pathLength;
            x = rng.Next(0, width);
            y = rng.Next(0, height);
            borders = new int[4] { x, y, x, y };
            pathLength = rng.Next(1, chunkLength);

            float previousStr = 2f;
            bool success = true;
            while (pathLength > 0 && success)
            {
                int dir = rng.Next(0, 4);
                float str = (float)rng.NextDouble();
                str += 0.2f;
                str *= 10;
                str += previousStr / pathLength;
                previousStr = str;
                success = PaintBrush(ref stencilArea, str, x, y);
                switch (dir)
                {
                    case 0: y -= 1;
                        break;
                    case 1:
                        y += 1;
                        break;
                    case 2:
                        x -= 1;
                        break;
                    case 3:
                        x += 1;
                        break;
                }

                if (x < borders[0])
                    borders[0] = x;
                if (x > borders[2])
                    borders[2] = x;
                if (y < borders[1])
                    borders[1] = y;
                if (y > borders[3])
                    borders[3] = y;

                borders = borders.ClampValues(new int[] {0, 2}, 0, width);
                borders = borders.ClampValues(new int[] {1, 3}, 0, height);

                pathLength -= 1;
            }

            return stencilArea;
        }

        protected bool PaintLand(ref float[,] stencil, int x, int y, float valToSet, bool ignoreOutOfBounds = false)
        {
            if (!CheckOutOfBounds(x, y))
            {
                stencil[x, y] = valToSet;
                return true;
            }
            else if(!ignoreOutOfBounds) return false;

            return true;
        }

        protected bool PaintBrush(ref float[,] stencil, float valToSet, int x, int y)
        {
            if (!PaintLand(ref stencil, x, y, valToSet))
                return false;
            if (!PaintLand(ref stencil, x + 1, y, valToSet))
                return false;
            if (!PaintLand(ref stencil, x, y + 1, valToSet))
                return false;

            return true;
        }

        public void RemoveSeaBlur(MagickImage map)
        {
            using (IPixelCollection<UInt16> pixels = map.GetPixels())
            {
                for(int x =0; x < map.Width; x++)
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        ushort r, g, b;
                        r = pixels.GetPixel(x, y).ToColor().R;
                        g = pixels.GetPixel(x, y).ToColor().G;
                        b = pixels.GetPixel(x, y).ToColor().B;
                        if ((b > r || b > g ) && (r > 0 && g > 0) && b > 30 && b < 70)
                        {
                            pixels.ModifyPixel(x, y, MapColours.mapWaterColours[WaterColours.HeightsWater]);
                        }

                        else if((b > r || b > g) && (r > 0 && g > 0)) 
                            pixels.ModifyPixel(x, y, MapColours.mapWaterColours[WaterColours.HeightsGround]);
                    }

                }
            }
        }


        private static object _lockObject1 = new object();
        private static object _lockObject2 = new object();
        protected void NeatenHeights(ref float[,] geog, int maxSize, int[] bounds,  MagickImage map, string thread)
        {
            HashSet<int[]> ignore = new HashSet<int[]>();
            for (int x = bounds[0]; x < bounds[2]; x++)
                for (int y = bounds[1]; y < bounds[3]; y++)
                {
                    int[] vec = new int[] { x, y };

                    if (GetSurroundingPoints(geog, new HashSet<int[]>(), new int[] { x, y }, (a, b) => a >= b, 1).Count > 1)
                    {
                        if (!ignore.ContainsItem(vec) && geog[x, y] < sealevel) //GetSurroundingPoints(geog, new HashSet<int[]>(), vec, (s,d) => x >= y, 1).Count > 2)
                        {
                            int[] borders;
                            var area = GetArea(geog, x, y, out borders, (a, b) => a <= b, 0, maxSize);

                            if (area.Count < maxSize + 1 && area.Count > 0)
                            {
                                foreach (var p in area)
                                {
                                    float s = geog[p[0], p[1]];
                                    geog[p[0], p[1]] = 1;

                                }
                            }
                            ignore.AddElementsUnique(area);


                            DrawPixels(map, area, geog);

                        }
                    }

                }

            Console.WriteLine("Thread: {0} - exiting", thread);
            return;
        }


        protected int[] AdjustBorders(int[] borders, int x, int y)
        {
            if (x < borders[0])
                borders[0] = x;
            if (x > borders[2])
                borders[2] = x;
            if (y < borders[1])
                borders[1] = y;
            if (y > borders[3])
                borders[3] = y;

            borders = borders.ClampValues(new int[] { 0, 2 }, 0, width);
            borders = borders.ClampValues(new int[] { 1, 3 }, 0, height);

            return borders;
        }

        public bool CheckOutOfBounds(int x, int y)
        {
            if (x < width && x > -1 && y < height && y > -1)
            {
                return false;
            }
            else return true;
        }

        protected HashSet<int[]> GetArea(float[,] mask, int xpos, int ypos, out int[] borders, Func<int, int, bool> op,  int valueToFind = 1, int stopSize = 50)
        {
            HashSet<int[]> ignore = new HashSet<int[]>();
            //Loop and find value, trace all connected values and store them
            borders = new int[4] { int.MaxValue, int.MaxValue, 0, 0 };
            if (mask[xpos, ypos] == valueToFind)
            {
                        //begin traversal
                HashSet<int[]> toCheck = new HashSet<int[]>();
                toCheck.Add(new int[] {xpos, ypos});

                while (toCheck.Count > 0)
                {
                    var newPoints = GetSurroundingPoints(mask, ignore, toCheck.Last(), (x, y) => x == y , valueToFind);
                    int[] last = toCheck.Last();
                    ignore.Add(last);
                    borders = AdjustBorders(borders, last[0], last[1]);
                    toCheck.Remove(last);
                    toCheck.AddElementsUnique(newPoints);

                    if (ignore.Count > stopSize)
                        break;

                }
            }

            return ignore;
        }

        public HashSet<int[]> GetSurroundingPoints(float[,] mask, HashSet<int[]> ignore, int[] p, Func<float, int, bool> op, int valueToFind = 1)
        {
            HashSet<int[]> found = new HashSet<int[]>();
            int xinc = 1;
            int xdec = 1;
            int yinc = 1;
            int ydec = 1;
            if (p[0] + xinc >= width)
                xinc = 0;
            if (p[0] - xdec < 0)
                xdec = 0;
            if (p[1] + yinc >= height)
                yinc = 0;
            if (p[1] - ydec < 0)
                ydec = 0;

            if ( op(mask[p[0] + xinc, p[1]], valueToFind) && (p[0] + xinc != p[0]) && !ignore.ContainsItem(new int[]{ p[0] + xinc, p[1] }))
                found.Add(new int[] { p[0] + xinc, p[1]});
            if (op(mask[p[0] - xdec, p[1]], valueToFind) && (p[0] - xdec != p[0]) && !ignore.ContainsItem(new int[]{ p[0] - xdec, p[1] } ))
                found.Add(new int[] { p[0] - xdec, p[1] });
            if (op(mask[p[0], p[1] + yinc], valueToFind) && (p[1] + yinc != p[1]) && !ignore.ContainsItem(new int[]{ p[0], p[1] + yinc }))
                found.Add(new int[] { p[0], p[1] + yinc });
            if (op(mask[p[0], p[1] - ydec], valueToFind) && (p[1] - ydec != p[1]) && !ignore.ContainsItem(new int[] { p[0], p[1] - ydec }))
                found.Add(new int[] { p[0], p[1] - ydec });


            return found;
        }

        protected float GetCombinedValue(SimplexPerlin[] noise, int x, int y)
        {
            float comb = 0.0f;
            float totalAmplitude = 0.0f;
            for (int i = 0; i < noise.Count(); i++)
            {
                int divisor = i*2;

                if (i == 0)
                    divisor = 1;

                float normalised = noise[i].GetValue(x * (frequency*divisor), y * (frequency*divisor));
                float amplitude = (1.0f / divisor);
                comb +=  amplitude * normalised;
                totalAmplitude += amplitude;
            }

            return (comb / totalAmplitude).NormaliseToRange(-1, 1);
        }

    }
}
