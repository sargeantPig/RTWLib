using ImageMagick;
using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
        public void GenerateTerrain(Random rnd)
        {
            int rwidth = (width * 2) + 1;
            int rheight = (height * 2) + 1;

            int[,] groundTypes = new int[rwidth, rheight];
            map.mapGround = new MagickImage(MapColours.mapGroundTypes[GroundTypes.SeaDeep], rwidth, rheight);

            int[,] mask = map.GetHeightMask;
            List<int[]> threadBounds = new List<int[]>();
            float segment = (rwidth / threads);

            for (int i = 0; i < threads; i++)
            {
                int index = i;
                if (index == threads - 1)
                {
                    int max = (int)((index * segment) + segment);
                    int rmax = (rwidth - max) + max;
                    threadBounds.Add(new int[4] { (int)(index * segment), 0, rmax, rheight });
                }
                else threadBounds.Add(new int[4] { (int)(index * segment), 0, (int)((index * segment) + segment), rheight });
                threadManager.CreateThread(() => SetGroundTypes(map.mapGround, mask, threadBounds[index], index + threadManager.threads.Count));
            }

            threadManager.Start();

            //get height
            //get climate
            //simulate rainfall,


        }

        public void SetGroundTypes(MagickImage ground, int[,] mask, int[] bounds, int threadNo)
        {
            for (int x = bounds[0]; x < bounds[2]; x++)
            {
                for (int y = bounds[1]; y < bounds[3]; y++)
                {
                    if (mask[x, y] > 0)
                        ExtMagick.ModifyPixelThreadSafe(ground, x, y, MapColours.mapGroundTypes[GroundTypes.FertileMedium]);
                    else
                        ExtMagick.ModifyPixelThreadSafe(ground, x, y, MapColours.mapGroundTypes[GroundTypes.SeaDeep]);
                }
            }
        }
    }
}
