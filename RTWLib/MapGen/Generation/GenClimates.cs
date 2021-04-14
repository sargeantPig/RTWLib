using ImageMagick;
using LibNoise;
using RTWLib.Extensions;
using RTWLib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
        public void GenerateClimates(Random rnd)
        {
            map.mapClimates = new MagickImage(MapColours.mapClimates[Climates.Alpine],  width, height);
            int[,] mask = map.GetHeightMask;
            float[,] temperature = GenerateNoise(4, 1.5f, 1.3f, 
                (x, y) => (x*(height/2)) - ((float)Math.Abs(map.DistanceToEquator(y))).SafeDivide(height/2.0f));
            int[,] moisture = new int[width, height];
            moisture = moisture.InitiliseWithValue(0);
            //int[,] rainfall = SimulateRainfall(mask, temperature);
           
        }



        protected int[,] SimulateRainfall(int[,] heights, float[,] temperature)
        {
            int[,] rainfall = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                int wetness = 0;
                for (int x = 0; x < width; x++)
                {
                    int latitude = y - (int)map.DistanceToEquator(new int[] { x, y });
                    if (heights[x, y] <= 0)
                    { 
                        
                    }
                
                }
            }

            return rainfall;
        }

        
    }
}
