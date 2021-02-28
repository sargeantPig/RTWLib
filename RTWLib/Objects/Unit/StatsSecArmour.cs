using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Objects
{
    public class StatSecArmour
    {
        /// <summary>
        /// [1] armour factor
        /// [2] defensive skill factor
        /// </summary>
        public int[] secArmour;
        /// <summary>
        /// sound type when hit = flesh, leather, or metal
        /// </summary>
        public ArmourSound secArmSound;

        public StatSecArmour()
        {
            secArmour = new int[2];
        }

    }
}
