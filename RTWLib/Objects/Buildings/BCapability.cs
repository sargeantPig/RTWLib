using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class Bcapability
    {
        public List<Brecruit> canRecruit = new List<Brecruit>();
        public List<string> agentList = new List<string>();
        public List<string> effectList = new List<string>();

        public Bcapability()
        { }

        public string outputCapability()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "capability" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";

            foreach (Brecruit recruit in canRecruit)
            {
                a += recruit.outputRecruit() + "\r\n";

            }

            foreach (string agent in agentList)
            {
                a += Functions.Helpers_EDB.EDBTabSpacers[3] + agent + "\r\n";

            }

            foreach (string eff in effectList)
            {
                a += Functions.Helpers_EDB.EDBTabSpacers[3] + eff + "\r\n";
            }

            a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

            return a;

        }

    }

}
