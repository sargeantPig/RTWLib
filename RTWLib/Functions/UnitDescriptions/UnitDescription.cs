using RTWLib.Data;
using RTWLib.Functions.DMB;
using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.UnitDescriptions
{
    public class UnitDescription
    {
        public static readonly string div = "¬----------------";

        public string internalName { get; set; }
        public string externalName { get; set; }
        public string longDescr { get; set; }
        public string shortDescr { get; set; }

        public string LDTag { get; set; }
        public string SDTag { get; set; }
        public string nameTag { get; set; }
        public UnitDescription()
        { }

        public void AddHiddenStats(Unit unit, BattleModel bm, int percentile)
        {
            
            string stats = "\\n\\n";
            string Uclass = string.Format("{0} {1}, in the top {2}% of units\\n\\n", unit.uClass.Capitalise(), unit.category.Capitalise(), percentile );

            string priWeapon = GetWeaponStats(unit, unit.priWep, bm, true);
            string secWeapon = GetWeaponStats(unit, unit.secWep, bm, false);
            string groundTypes = GetGroundModifier(unit.ground);
            string armour = GetArmour(unit, unit.priArm);
            string morale = GetMorale(unit);
            longDescr = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                stats,
                Uclass,
                priWeapon,
                secWeapon,
                armour,
                morale,
                groundTypes) + longDescr;
        }

        string GetGroundModifier(int[] groundTypes)
        {
            string gm = "";

            gm = string.Format("Ground Bonus\\n(scrub {0}) (sand {1}) (forest {2}) (snow {3})\\n-------------\\n\\n",
                groundTypes[0], groundTypes[1], groundTypes[2], groundTypes[3]);

            return gm;

        }

        string GetWeaponStats(Unit unit, StatWeapons weapon, BattleModel bm, bool isPrimary)
        {
            string weaponStr = "";
            string tag = "";
            string ap = "";
            if (isPrimary)
            {
                tag = "Weapon (p): ";
                if (unit.priAttri.HasFlag(Stat_pri_attr.ap) || unit.priAttri.HasFlag(Stat_pri_attr.thrown_ap))
                    ap = "AP";
            }
            else
            {
                tag = "Weapon (s): ";
                if (unit.secAttri.HasFlag(Stat_pri_attr.ap) || unit.secAttri.HasFlag(Stat_pri_attr.thrown_ap))
                    ap = "AP";
            }
            string wepname = bm.skeletons.GetWeaponType(unit, isPrimary, unit.category == "handler", unit.engine);
            string speed = bm.skeletons.GetSpeed(isPrimary);

            

            if (weapon.WepFlags == Data.WeaponType.missile || weapon.WepFlags == Data.WeaponType.thrown)
            {
                weaponStr = string.Format("{0} {1} ({2}) {3} {4}, cost to upgrade {5}\\nRange: ({6}m)\\nAmmo: ({7})\\n",
                    tag, wepname, weapon.atk[0], speed, ap, unit.cost[3],
                    weapon.missAttr[0],
                    weapon.missAttr[1]); ;
            }

            else if(weapon.WepFlags != Data.WeaponType.no)
            {
                weaponStr = string.Format("{0} {1} ({2}) {3} {4}\\n",
                   tag, wepname, weapon.atk[0], speed, ap);
            }

            return weaponStr + "\\n";
        }

        string GetArmour(Unit unit, StatPriArmour armour)
        {
            return string.Format("Armour: {0}, cost to upgrade {1}\\n\\n", armour.armSound, unit.cost[4]);
        }


        string GetMorale(Unit unit)
        {
            string discipline = unit.mental.discipline.ToString().Capitalise();
            string morale = unit.mental.morale.ToString();
            string training = unit.mental.training.ToString().Capitalise();
            return string.Format("Morale: {0}, {1}\\nTraining: {2}\\n\\n", morale, discipline, training);
        }
        public string Output()
        {
            string str = "¬".CRL();
            str += string.Format(@"{0}{1}{2}{3}{4}{5}{6}",
                nameTag + "\t", externalName.CRL(2),
                LDTag.CRL(), longDescr.CRL(2),
                SDTag.CRL(), shortDescr.CRL(2),
                div.CRL(2));

            return str;
        }
    }
}
