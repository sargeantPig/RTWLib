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
        public int[] stat_sec_armour;
        /// <summary>
        /// sound type when hit = flesh, leather, or metal
        /// </summary>
        public ArmourSound sec_armour_sound;

        public StatSecArmour()
        {
            stat_sec_armour = new int[2];
        }

    }
}
