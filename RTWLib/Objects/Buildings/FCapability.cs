using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class Fcapability
    {
        public List<string> effectList = new List<string>();

        public Fcapability()
        {




        }

        public string outputFcapa()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "faction_capability" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";


            foreach (string eff in effectList)
            {
                a += Functions.Helpers_EDB.EDBTabSpacers[3] + eff + "\r\n";
            }

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

            return a;



        }

    }

}
