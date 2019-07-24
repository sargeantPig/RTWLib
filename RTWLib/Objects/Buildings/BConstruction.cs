using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class Bconstruction
    {
        public int turnsToBuild;
        public int cost;
        public string settlement_min;
        public List<string> upgrades = new List<string>();

        public Bconstruction()
        {


        }


        public string outputConstruction()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "construction  " + turnsToBuild.ToString() + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "cost  " + cost.ToString() + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "settlement_min " + settlement_min + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "upgrades" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";

            foreach (string upgrade in upgrades)
            {
                a += Functions.Helpers_EDB.EDBTabSpacers[3] + upgrade + "\r\n";

            }

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

            return a;

        }

    }

}
