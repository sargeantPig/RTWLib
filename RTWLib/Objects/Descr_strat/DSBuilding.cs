using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class DSBuilding
    {
        public string type;
        public string name;

        public DSBuilding()
        { }

        public DSBuilding(DSBuilding dS)
        {
            type = dS.type;
            name = dS.name;
        }

        public string Output()
        {
            return "\r\n\t" + "building" +
                    "\r\n\t" + "{" +
                    "\r\n\t\ttype " + type + " " + name + "\r\n" +
                "\t" + "}\r\n";
        }
    }
}
