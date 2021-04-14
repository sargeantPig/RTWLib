using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.MapGen.Generation
{
    public class MapParameters
    {
        //Temperature Boundaries 
        int cold = 0;
        int lukeWarm = 25;
        int warm = 50;
        int hot = 75;
        int veryHot = 200;

        //Moisture Boundaries
        int dry = 0;
        int slightlyWet = 5;
        int wet = 10;
        int soaking = 15;
        int flooded = 20;

        //Heights Boundaries
        int low = 100;
        int med = 200;
        int high = 255;

        public Climates GetClimate(float temperature, float moisture, float heights)
        {
            TopoRes res = TopoRes.none;
            if (temperature <= cold)
                res |= TopoRes.Cold;
            else if (temperature <= warm)
                res |= TopoRes.Warm;
            else if (temperature <= hot)
                res |= TopoRes.Hot;
            else if (temperature <= lukeWarm)
                res |= TopoRes.LukeWarm;
            else if (temperature <= hot)
                res |= TopoRes.Hot;
            else if (temperature <= hot)
                res |= TopoRes.Hot;

            if (moisture <= dry)
                res |= TopoRes.Dry;
            else if (moisture <= wet)
                res |= TopoRes.Wet;
            else if (moisture <= flooded)
                res |= TopoRes.Flooded;

            if (heights <= low)
                res |= TopoRes.Low;
            else if (heights <= med)
                res |= TopoRes.Med;
            else if (heights <= high)
                res |= TopoRes.High;

            return DiscernClimate(res);

        }

        public Climates DiscernClimate(TopoRes tr)
        {
            TopoRes subArctic = TopoRes.Cold | TopoRes.Dry | TopoRes.Low;
            TopoRes subArcticMed = TopoRes.Cold | TopoRes.Dry | TopoRes.Med;
            TopoRes subArcticHigh = TopoRes.Cold | TopoRes.Dry | TopoRes.High;

            TopoRes alpine = TopoRes.Cold | TopoRes.SlightlyWet | TopoRes.Low;
            TopoRes alpineMed = TopoRes.Cold | TopoRes.SlightlyWet | TopoRes.Med;

            TopoRes highland = TopoRes.Cold | TopoRes.Wet | TopoRes.High;
            TopoRes highlandSWet = TopoRes.Cold | TopoRes.SlightlyWet | TopoRes.High;

            TopoRes swamp = TopoRes.Cold | TopoRes.Flooded | TopoRes.Low;
            TopoRes swampLWarm = TopoRes.LukeWarm | TopoRes.Flooded | TopoRes.Low;
            TopoRes swampWarm = TopoRes.Warm | TopoRes.Flooded | TopoRes.Low;

            if (tr.HasFlag(TopoRes.Cold) && tr.HasFlag(TopoRes.Wet) && (tr.HasFlag(TopoRes.Low) || tr.HasFlag(TopoRes.Med) || tr.HasFlag(TopoRes.High)))
            { 
            
            
            }

            return Climates.Alpine;
        }
    }

    [Flags]
    public enum TopoRes
    { 
        none = 1 << 0,

        Dry = 1 << 1,
        SlightlyWet = 1 << 2,
        Wet = 1 << 3,
        Soaking = 1 << 4,
        Flooded = 1 << 5,

        Low = 1 << 6,
        Med = 1 << 7,
        High = 1 << 8,

        Cold = 1 << 9,
        LukeWarm = 1 << 10,
        Warm = 1 << 11,
        Hot = 1 << 12,
        VeryHot = 1 << 13
    }

}
