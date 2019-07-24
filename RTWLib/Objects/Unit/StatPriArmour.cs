using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Objects
{
    public class StatPriArmour
    {
        /// <summary>
        /// [1] armour factor
        /// [2] defensive skill factor
        /// [3] shield factor
        /// </summary>
        public int[] stat_pri_armour;
        /// <summary>
        /// sound type when hit = flesh, leather, or metal
        /// </summary>
        public ArmourSound armour_sound;

        public StatPriArmour()
        {
            stat_pri_armour = new int[3];
        }
    }

}
