using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Functions;
using RTWLib.MapGen.Voronoi;
using System.ComponentModel.Design.Serialization;
using RTWLib.Extensions;
using LibNoise.Combiner;
using LibNoise.Renderer;
using System.Threading;
using System.Net;
using LibNoise.Modifier;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
        public void GenerateRegions(Random rng)
        {
            Logger.Logger log = new Logger.Logger();
            int lineNumber;
            string currentLine;

            Descr_Region dr = new Descr_Region(true, regionsImagePath, descrRegionPath);
            dr.Parse(new string[] { descrRegionPath, regionsImagePath }, out lineNumber, out currentLine);
            map.mapRegions = new MagickImage(MapColours.mapWaterColours[WaterColours.RegionWater], width , height );
            int maxRegions = dr.regions.Count();
            int[,] regionstga = new int[width, height];
            var regList = dr.regions.Values.ToArray();
            int[,] mask;
            LibVoronoi voronoi = new LibVoronoi(rng);
            LibVoronoi regEd = new LibVoronoi(rng);

            mask = map.GetHeightMask;
            voronoi.GeneratePoints(0, width , 0, height , 100, maxRegions,  mask, 1, 1, (x, y) => x >= y, true);

            if (voronoi.points.Count != maxRegions)
            {
                log.PLog("Region limit not reached!");
                log.DisplayLog();
            }

            regEd.GeneratePoints(0, width , 0 , height, 25, (int)frequency, mask, 1, 1, (x, y) => x >= y);
            voronoi.LinkVoronoiGrid(regEd, 10);
            UpdateImage(map.mapRegions);

            List<int[]> threadBounds = new List<int[]>();
            float segment = (width / threads);

            for (int i = 0; i < threads; i++)
            {
                int index = i;
                if (index == threads - 1)
                {
                    int max = (int)((index * segment) + segment);
                    int rmax = (width - max) + max;
                    threadBounds.Add(new int[4] { (int)(index * segment), 0, rmax, height });
                }
                else threadBounds.Add(new int[4] { (int)(index * segment), 0, (int)((index * segment) + segment), height });
                threadManager.CreateThread(() => DiscernRegions(map.mapRegions, mask, voronoi, regEd, threadBounds[index], regList, index + threadManager.threads.Count, rng));
            }

            threadManager.Start();

        }

        public void DiscernRegions(MagickImage mapRegions, int[,] mask, LibVoronoi voronoi, LibVoronoi detail, int[]bounds, Region[] regList, int threadNo, Random fuzz)
        {
            HashSet<VoronoiPoint> pixels = new HashSet<VoronoiPoint>();
             for (int x = bounds[0]; x < bounds[2]; x++)
             {
                for (int y = bounds[1]; y < bounds[3]; y++)
                {
                    if (mask[x, y] >= 1)
                    {
                        double detailDis;
                        double normDis; 
                        int detailVal = detail.ClosestValue(new int[] { x, y }, out detailDis);
                        int normVal = voronoi.ClosestValue(new int[] { x, y }, out normDis);
                        int ind = voronoi.ClosestIndex(new int[] { x, y });

                        if (x == voronoi.points[ind].x && y == voronoi.points[ind].y)
                        {
                            pixels.Add(new VoronoiPoint(x, y));
                            pixels.Last().value = -2;
                            continue;
                        }


                        pixels.Add(new VoronoiPoint(x, y));

                        if (detailDis < normDis)
                            pixels.Last().value = detailVal;
                        else if(normDis <= detailDis)
                            pixels.Last().value = ind;
                    }
                    else
                    {
                        int ind = voronoi.ClosestIndex(new int[] { x, y });

                        if (x == voronoi.points[ind].x && y == voronoi.points[ind].y)
                        {
                            pixels.Add(new VoronoiPoint(x, y));
                            pixels.Last().value = -2;
                            continue;
                        }


                        pixels.Add(new VoronoiPoint(x, y));
                        pixels.Last().value = -1;
                    }
                }
             }

            PaintPixel(pixels, map.mapRegions, regList);

            Console.WriteLine("Thread {0} Exiting", threadNo);

            return;
        }

        
        protected void PaintPixel(HashSet<VoronoiPoint> pixels, MagickImage map, Region[] regList)
        {
            using (IPixelCollection<UInt16> pix = map.GetPixels())
            {
                foreach (var pixel in pixels)
                {
                    MagickColor col;
                    if (pixel.value == -1)
                        col = MapColours.mapWaterColours[WaterColours.RegionWater];
                    else if (pixel.value == -2)
                        col = MagickColor.FromRgb(0, 0, 0);
                    else col = MagickColor.FromRgb((byte)regList[pixel.value].rgb[0], (byte)regList[pixel.value].rgb[1], (byte)regList[pixel.value].rgb[2]);
                    this.map.mapTiles[pixel.x, pixel.y].region = pixel.value;
                    pix.ModifyPixel(pixel.x, pixel.y, col);
                }
            }

            UpdateImage(this.map.mapRegions);

        }
    }
}
