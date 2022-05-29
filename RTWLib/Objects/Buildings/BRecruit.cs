using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class Brecruit
    {
        public string name; //eg. carthaginian peasant
        public int experience;
        public int startingPoints;
        public double pointBuildingGains;
        public double maximumPoints;
        public List<string> requiresFactions = new List<string>();

        public Brecruit()
        {

        }

        public Brecruit(string name, int experience)
        {
            this.name = name;
            this.experience = experience;
        }

        virtual public string outputRecruit()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[3] + "recruit " + "\"" + name + "\"" + "  " + experience.ToString(); // + "  " + "requires factions " + "{ ";

            if (requiresFactions.Count == 0)
                return a;
            else a += "  " + "requires factions " + "{ ";
            foreach (string faction in requiresFactions)
            {
                a += faction + ", ";

            }

            a += " }";

            return a;
        }


    }

}
