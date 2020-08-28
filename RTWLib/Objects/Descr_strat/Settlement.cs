using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public interface ISettlement
    { 
        List<DSBuilding> b_types { get; set; }
        string s_level { get; set; }
        string region { get; set; }
        string faction_creator { get; set; }
        int yearFounded { get; set; }
        int population { get; set; }
    }


    public class Settlement : ISettlement
    {
        public List<DSBuilding> b_types { get; set; }

        public string plan_set = "default_set";
        public string s_level { get; set; }
        public string region { get; set; }
        public string faction_creator { get; set; }

        public int yearFounded { get; set; }
        public int population { get; set; }

        public Settlement(string level, string reg, string creator, List<DSBuilding> buildings, int yrFounded, int pop)
        {
            s_level = level;
            region = reg;
            faction_creator = creator;
            yearFounded = yrFounded;
            population = pop;
            b_types = buildings;


        }

        public Settlement(Settlement s)
        {
            s_level = s.s_level;
            region = s.region;
            faction_creator = s.faction_creator;
            yearFounded = s.yearFounded;
            population = s.population;
            b_types = s.b_types;


        }

        public Settlement()
        {
            b_types = new List<DSBuilding>();
        }

        virtual public string outputSettlement()
        {

            string settlement =
                "\r\nsettlement" +
                "\r\n{" +
                "\r\n\t" + "level " + s_level +
                "\r\n\t" + "region " + region +
                "\r\n\t" + "year_founded " + Convert.ToString(yearFounded) +
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
