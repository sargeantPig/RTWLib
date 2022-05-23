using ImageMagick;
using LibNoise;
using RTWLib.Extensions;
using RTWLib.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RTWLib.MapGen;
using RTWLib.MapGen.Voronoi;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
        public void GenerateClimates(Random rnd)
        {
            map.mapClimates = new MagickImage(MapColours.mapClimates[Climates.FertileTemperateGrassland],  width, height);
            int[,] climate = new int[width, height];
            int[,] mask = map.GetHeightMask;

            LibVoronoi climates = new LibVoronoi(rnd);
            LibVoronoi fuzz = new LibVoronoi(rnd);
            climates.GeneratePoints(0, width, 0, height, 100, 20, mask, 1, 1, (x, y) => x >= y, true);
            fuzz.GeneratePoints(0, width, 0, height, 50, (int)frequency, mask, 1, 1, (x, y) => x >= y);
            climates.LinkVoronoiGrid(fuzz);


            // float[,] temperature = GenerateNoise(4, 0.001f, 1.3f, 
            //  (x, y) => (x*(height/2)) - ((float)Math.Abs(map.DistanceToEquator(y))));
            /*map.ImportTemperatures(temperature);
            float[,] initialMoisture = GenerateNoise(4, 0.001f, 1.3f,
                (x, y) => (x * (height / 2)) + ((float)Math.Abs(map.DistanceToEquator(y))));
            DiscernClimates(temperature, initialMoisture);


            float[,] rainfall = SimulateRainfall(mask, temperature, rnd);
            map.mapRainfall = initialMoisture.ToImage();
            map.mapTemperature = temperature.ToImage();
            map.RefreshClimateImage();
            */

        }

        void DiscernClimates(LibVoronoi climate, LibVoronoi fuzz)
        {
            HashSet<VoronoiPoint> pixels = new HashSet<VoronoiPoint>();

            foreach (var point in climate.points)
            {
                point.value = rnd.Next(0, MapColours.mapClimates.Count);



            }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {


                }
        }

        protected float[,] SimulateRainfall(int[,] heights, float[,] temperature, Random rnd)
        {
            float[,] rainfall = new float[width, height];

            for (int y = 0; y < height; y++) {
                int wetness = 0;
                for (int x = 0; x < width; x++) {
                   // SimulateRain(heights, temperature, ref rainfall, ref wetness, x, y, rnd);
                    map.mapTiles[x, y].UpdateTile(wetness);
                }

                for (int x = width -1; x > -1; x--) {
                  //  SimulateRain(heights, temperature, ref rainfall, ref wetness, x, y, rnd);
                    map.mapTiles[x, y].UpdateTile(wetness);
                }
            }

            for (int x = 0; x < width; x++)
            {
                int wetness = 0;
                for (int y = 0; y < height; y++)
                {
                   // SimulateRain(heights, temperature, ref rainfall, ref wetness, x, y, rnd);
                    map.mapTiles[x, y].UpdateTile(wetness);
                }

                for (int y = height - 1; y > -1; y--)
                {
                   // SimulateRain(heights, temperature, ref rainfall, ref wetness, x, y, rnd);
                    map.mapTiles[x, y].UpdateTile(wetness);
                }
            }


            return rainfall;
        }

        void SimulateRain(int[,] heights, float[,] temperature, ref float[,] rainfall, ref int wetness, int x, int y, Random rnd)
        {
            int latitude = y - (int)map.DistanceToEquator(new int[] { x, y });
            if (heights[x, y] <= 0)
            {
                //Gather water
                float evap = (1 * temperature[x, y]) * 10;
                wetness += (int)evap;
            }
            else if (heights[x, y] > 0)
            {
                //Precipitation
                if (wetness > 0)
                {
                    int rand = rnd.Next(-2, wetness + 1);

                    if (rand <= 0 && wetness > 0)
                    {
                        float evap = (1 * temperature[x, y]) * 10;
                        rainfall[x, y] -= evap;
                        wetness += (int)evap;
                    }
                    else rainfall[x, y] += rand;
                }

            }


        }
        
    }
}
