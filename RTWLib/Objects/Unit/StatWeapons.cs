using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Objects
{
    public class StatWeapons
    {
        /// <summary>
        /// [0] attack factor
        /// [1] attack bonus factor if charging
        /// </summary>
        public int[] attack;
        /// <summary>
        /// missile type fired (no if not a missile weapon type)
        /// </summary>
		public string missileType;
        /// <summary>
        /// [0] range of missile
        /// [1] amount of missle ammunition per man
        /// </summary>
		public int[] Missleattri;
        /// <summary>
        ///  Weapon type = melee, thrown, missile, or siege_missile
        /// </summary>
		public WeaponType WeaponFlags;
        /// <summary>
        /// Tech type = simple, other, blade, archery or siege
        /// </summary>
		public TechType TechFlags;
        /// <summary>
        /// Damage type = piercing, blunt, slashing or fire. (I don't think this is used anymore)
        /// </summary>
        public DamageType DamageFlags;
        /// <summary>
        /// Sound type when weapon hits = none, knife, mace, axe, sword, or spear
        /// </summary>
		public string SoundFlags;
        /// <summary>
        /// Min delay between attacks(in 1/10th of a second)
        /// </summary>
		public float[] attackdelay;

        public StatWeapons()
        {
            attack = new int[2];
            Missleattri = new int[2];
            attackdelay = new float[2];
        }
    }
}
