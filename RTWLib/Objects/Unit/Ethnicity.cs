using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Unit
{
    class Ethnicity
    {
        string faction, region, arg;

        public Ethnicity(string faction, string region, string arg)
        {
            this.faction = faction;
            this.region = region;
            this.arg = arg;
        }


    }
}
