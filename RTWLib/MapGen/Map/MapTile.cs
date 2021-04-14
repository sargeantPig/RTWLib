using RTWLib.MapGen.Voronoi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{
    public class MapTile
    {
        public int height, ground, feature, region;
        public Climates climate;
        public float moisture, temperature;
        public VoronoiPoint pos;


        public MapTile()
        { }

        public VoronoiPoint GetRegionPos
        {
            get { return new VoronoiPoint(pos.x / 2, pos.y / 2); }
        }


    }

    public class PolarTile : MapTile
    {
        public void UpdateTile(float moisture)
        {
            this.moisture += moisture;

            this.moisture -= (temperature / 2);

            if (moisture <= 0)
                climate = Climates.SubArctic;
            else if (moisture <= 10 && height <= 100)
                climate = Climates.Alpine;
            else if (moisture > 10 && height > 100)
                climate = Climates.Highland;
            else climate = Climates.SubArctic;
        }
    }


    public class PlainsTile : MapTile
    {
        public void UpdateTile(float moisture)
        {
            this.moisture += moisture;

            this.moisture -= (temperature / 2);

            if (moisture <= 2)
                climate = Climates.InfertileTermperateGrassland;
            else if (moisture <= 5 && temperature <= 20)
                climate = Climates.FertileTemperateGrassland;
            else if (temperature < 30)
                climate = Climates.Mediterranean;
            else if (temperature > 30)
                climate = Climates.SemiArid;
            else climate = Climates.FertileTemperateGrassland;

            if (moisture > 20 && height <= 1)
                climate = Climates.Swamp;

        }


    }

    public class ForestTile : MapTile
    {
        public void UpdateTile(float moisture)
        {
            this.moisture += moisture;

            this.moisture -= (temperature / 2);

            if (moisture <= 2)
                climate = Climates.LightTemperateForest;
            else if (moisture <= 10 && temperature <= 20)
                climate = Climates.DeepTemperateForest;
            else climate = Climates.LightTemperateForest;

            if (moisture > 20 && height <= 1)
                climate = Climates.Swamp;

        }


    }
    public class DesertTile : MapTile
    {
        public void UpdateTile(float moisture)
        {
            this.moisture += moisture;

            this.moisture -= (temperature / 2);

            if (moisture <= 0 && height < 100)
                climate = Climates.SandyDesert;
            else if (moisture <= 0 && height < 255)
                climate = Climates.RockyDesert;
            else climate = Climates.SandyDesert;

            if (moisture <= 0 && height <= 3)
                climate = Climates.InfertileTermperateGrassland;
            else if (moisture <= 20 && height <= 3)
                climate = Climates.FertileTemperateGrassland;

            if (moisture > 20 && height <= 1)
                climate = Climates.Swamp;

        }


    }

}
