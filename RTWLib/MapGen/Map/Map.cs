using ImageMagick;
using LibNoise;
using LibNoise.Primitive;
using RTWLib.Extensions;
using RTWLib.MapGen.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{
    public class Map
    {
        public MapTile[,] mapTiles;

        public MagickImage mapHeights, mapRegions, mapFeatures, mapClimates, 
            mapGround, mapTemperature, mapRainfall;

        public int equator;

        int width, height;
        public Map(int width, int height)
        {
            mapTiles = new MapTile[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    mapTiles[x, y] = new MapTile();

            equator = height / 2;

            this.width = width;
            this.height = height;
        }

        public int[,] GetHeightMask
        {
            get {
                int[,] mask;

                using (IPixelCollection pixels = mapHeights.GetPixels())
                {
                    mask = pixels.ToIntArray(mapHeights.Width, mapHeights.Height, true);
                }
                return mask;
            }
        }

        public double DistanceToEquator(int[] posa)
        {
            return posa.DistanceTo(new int[] { posa[0], equator });
        }

        public int GetHeight(int[] pos)
        {
            return (int)mapTiles[pos[0], pos[1]].height;
        }

        public void CreateNewProvincialCampaign(string baseFolder)
        { 
            
            
        }

        public void ImportTemperatures(float[,] temperatures)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    this.mapTiles[x, y].temperature = temperatures[x, y] * 10;
                }
        }

        public void RefreshClimateImage()
        {
            using (var pixels = mapClimates.GetPixels())
            {
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                    {
                        MagickColor col = MapColours.mapClimates[mapTiles[x, y].climate];
                        pixels.ModifyPixel(x, y, col);
                    }
            }
        
        }
    }
}
