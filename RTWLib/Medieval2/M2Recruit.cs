using RTWLib.Objects.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    class M2Recruit : Brecruit
    {

        public M2Recruit()
        { }

        public override string outputRecruit()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[3] + "recruit_pool " + "\"" + name + "\"" + "  " + startingPoints.ToString() + "  " + pointBuildingGains.ToString() + "  " + maximumPoints.ToString() + "  " + experience.ToString() + "  " + "requires factions " + "{ ";

            foreach (string faction in requiresFactions)
            {
                a += faction + ", ";

            }

            a += " }";

            return a;
        }

    }
}
