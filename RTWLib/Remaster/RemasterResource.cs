using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Objects.Descr_strat;

namespace RTWLib.Remaster
{
    public class RemasterResource : Resource
    {
        int quantity; 

        public RemasterResource(string Rname, int quantity, int[] coords) : base(Rname, coords)
        {
            this.quantity = quantity;
        }

        public override string Output()
        {
            string output = "";
            output += "resource\t" + base.name + ",         " + quantity + ",         " + base.coordinates[0].ToString() + ",  " + base.coordinates[1] + "\r\n";
            return output;
        }
    }
}
