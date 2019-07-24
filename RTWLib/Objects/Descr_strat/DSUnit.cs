using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class DSUnit
    {
        string name;
        int exp;
        int armour;
        int weapon;

        public DSUnit(string alias, int Exp, int Armour, int Weapon)
        {
            name = alias.TrimEnd(' ');
            exp = Exp;
            armour = Armour;
            weapon = Weapon;
        }

        public DSUnit(DSUnit dsu)
        {
            name = dsu.name;
            exp = dsu.exp;
            armour = dsu.armour;
            weapon = dsu.weapon;
        }

        public string Output()
        {
            string output = "unit\t\t" + name + "\t\t\texp " + exp.ToString() + " armour " + armour.ToString() + " weapon " + weapon.ToString() + "\r\n";
            return output;
        }
    }
}
