using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2Settlement : Settlement, ISettlement
    {

        public string type;
        public M2Settlement(M2Settlement s) : base(s)
        {
            this.type = s.type;
        }

        public M2Settlement(string level, string reg, string creator, List<DSBuilding> buildings, int yrFounded, int pop, string type) 
            : base(level, reg, creator, buildings, yrFounded, pop)
        {
            this.type = type;
        }


        public M2Settlement()
        { }

        override public string outputSettlement()
        {

            string settlement =
                "\r\nsettlement";

            if (type != null)
            {
                settlement += type;
            }
            
            settlement +=
                "\r\n{" +
                "\r\n\t" + "level " + s_level +
                "\r\n\t" + "region " + region +
                "\r\n\r\n\t" + "year_founded " + Convert.ToString(yearFounded) +
                "\r\n\t" + "population " + Convert.ToString(population) +
                "\r\n\t" + "plan_set " + plan_set +
                "\r\n\t" + "faction_creator " + faction_creator;

            foreach (DSBuilding b in b_types)
            {
                settlement += b.Output();
            }

            settlement += "\r\n}";

            return settlement;
        }


    }
}
