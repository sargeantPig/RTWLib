using ImageMagick;
using LibNoise;
using LibNoise.Primitive;
using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{
    public class Map
    {
        public MapTile[,] mapTiles;

        public MagickImage mapHeights, mapRegions, mapFeatures, mapClimates, mapGround;

        public int equator;

        int width, height;
        public Map(int width, int height)
        {
            mapTiles = new MapTile[width, height];
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


        public void CreateNewProvincialCampaign(string baseFolder)
        { 
            
            

        }

        

    }
}
