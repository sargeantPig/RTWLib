using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class Landmark
    {
        string name = "";
        int[] coordinates = new int[2];

        public Landmark(string Lname, int[] coords)
        {
            name = Lname;
            coordinates = coords;
        }

        public string Output()
        {
            string output = "";

            output += "landmark\t" + name + "\t" + coordinates[0].ToString() + ",\t" + coordinates[1] + "\r\n";

            return output;
        }
    }
}
