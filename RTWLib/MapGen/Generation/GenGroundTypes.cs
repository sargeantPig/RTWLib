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
        public void GenerateTerrain(int[,] heights, Random rnd)
        {
            int[,] groundTypes = new int[width, height]; 
            map.mapGround = new MagickImage(MapColours.mapWaterColours[WaterColours.HeightsWater], width, height);



            //get height
            //get climate
            //simulate rainfall,


        }

    }
}
