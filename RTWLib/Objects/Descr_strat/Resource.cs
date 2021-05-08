using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class Resource
    {
        public string name = "";
        public int[] coordinates = new int[2];

        public Resource(string Rname, int[] coords)
        {
            name = Rname;
            coordinates = coords;

        }

        public virtual string Output()
        {
            string output = "";

            output += "resource\t" + name + ",\t" + coordinates[0].ToString() + ",\t" + coordinates[1] + "\r\n";

            return output;
        }

    }
}
