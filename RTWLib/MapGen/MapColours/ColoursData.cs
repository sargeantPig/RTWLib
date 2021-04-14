using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen
{

    public enum GroundTypes
    { 
        FertileLow,
        FertileMedium,
        FertileHigh,
        Wilderness,
        MountainsHigh,
        MountainsLow,
        Hills,
        ForestDense,
        ForestSparse,
        Swamp,
        Ocean,
        SeaDeep,
        SeaShallow,
        Beach
    }

    public enum Climates
    { 
        SubArctic,
        Alpine,
        Highland,
        Swamp,
        DeepTemperateForest,
        LightTemperateForest,
        InfertileTermperateGrassland,
        FertileTemperateGrassland,
        Mediterranean,
        SemiArid,
        SandyDesert,
        RockyDesert
    
    }

    public enum WaterColours
    {
        RegionWater,
        HeightsWater

    }

    public static class MapColours
    {
        public static Dictionary<GroundTypes, MagickColor> mapGroundTypes = new Dictionary<GroundTypes, MagickColor>()
        {
            {GroundTypes.Beach, MagickColor.FromRgb(255,255,255) },
            {GroundTypes.FertileHigh, MagickColor.FromRgb(101,124,0) },
            {GroundTypes.FertileLow, MagickColor.FromRgb(0,128,128) },
            {GroundTypes.FertileMedium, MagickColor.FromRgb(96,160,64) },
            {GroundTypes.ForestDense, MagickColor.FromRgb(0,64,0) },
            {GroundTypes.ForestSparse, MagickColor.FromRgb(0,128,0) },
            {GroundTypes.Swamp, MagickColor.FromRgb(0,255,128) },
            {GroundTypes.Wilderness, MagickColor.FromRgb(0,0,0) },
            {GroundTypes.Hills, MagickColor.FromRgb(128,128,64) },
            {GroundTypes.MountainsHigh, MagickColor.FromRgb(196,128,128) },
            {GroundTypes.MountainsLow, MagickColor.FromRgb(98,65,65) },
            {GroundTypes.Ocean, MagickColor.FromRgb(64,0,0) },
            {GroundTypes.SeaDeep, MagickColor.FromRgb(128,0,0) },
            {GroundTypes.SeaShallow, MagickColor.FromRgb(196,0,0) }
        };

        public static Dictionary<Climates, MagickColor> mapClimates = new Dictionary<Climates, MagickColor>()
        {
            {Climates.SubArctic, MagickColor.FromRgb(255,255,255) },
            {Climates.Alpine, MagickColor.FromRgb(101,124,0) },
            {Climates.Highland, MagickColor.FromRgb(0,128,128) },
            {Climates.Swamp, MagickColor.FromRgb(96,160,64) },
            {Climates.DeepTemperateForest, MagickColor.FromRgb(0,64,0) },
            {Climates.LightTemperateForest, MagickColor.FromRgb(0,128,0) },
            {Climates.InfertileTermperateGrassland, MagickColor.FromRgb(0,255,128) },
            {Climates.Mediterranean, MagickColor.FromRgb(128,128,64) },
            {Climates.RockyDesert, MagickColor.FromRgb(196,128,128) },
            {Climates.SandyDesert, MagickColor.FromRgb(98,65,65) },
            {Climates.SemiArid, MagickColor.FromRgb(64,0,0) }
           
        };

        public static Dictionary<WaterColours, MagickColor> mapWaterColours = new Dictionary<WaterColours, MagickColor>()
        {
            {WaterColours.HeightsWater, MagickColor.FromRgb(0, 0, 253) },
            {WaterColours.RegionWater, MagickColor.FromRgb(41, 140, 233) }
        };

    }
}
