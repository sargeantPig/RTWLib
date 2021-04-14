using RTWLib.Data;
using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    public partial class Descr_Region
    {
        protected bool ParseLine(string line, Dictionary<string, Region> regions)
        {
            bool containsTab = line.Contains('\t');
            bool isComment = line.Contains(';');
            bool isNull = line == "";
            if (isComment || isNull)
                return false;

            string[] data = line.Trim().TrimEnd(',').Split('\t', ',').CleanStringArray();


            if (!containsTab)
            {
                regions.Add(data[0], new Region()); //add new region
                regions.Last().Value.name = data[0];
                currentVar = 0;
                return true;
            }

            Region last = regions.Last().Value;

            switch (currentVar)
            {
                case 0: ParseSimpleLine<string>(data, ref last.cityName);
                    break;
                case 1: ParseSimpleLine<string>(data, ref last.factionCreator);
                    break;
                case 2: ParseSimpleLine<string>(data, ref last.rebelTribe);
                    break;
                case 3: ParseList<int[]>(data, ref last.rgb);
                    break;
                case 4: ParseList<string[]>(data, ref last.resources);
                    break;
                case 5: ParseSimpleLine<int>(data, ref last.triumphVal);
                    break;
                case 6: ParseSimpleLine<int>(data, ref last.farmLevel); 
                    break;
            }
            currentVar++;
            return true;
        }
    }
}
