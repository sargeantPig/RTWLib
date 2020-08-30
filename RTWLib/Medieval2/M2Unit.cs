using RTWLib.Data;
using RTWLib.Functions;
using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2Unit : Unit
    {
        public M2StatWeapons stat_ter; //M2TW
        public Stat_pri_attr stat_ter_ex; //M2TW
        public string stat_ter_attr; //M2tw
        public string stat_armour_ex; //M2TW
        public string stat_pri_ex; // M2TW
        public string bannerFaction; //M2TW BANNER
        public string bannerHoly; //M2TW HOLY banner
        public string stat_sec_ex; //M2TW
        public string stat_stl; //M2TW
        public string armour_ug_levels;
        public string armour_ug_models;
        public string era0;
        public string era1;
        public string era2;
        public string info_pic_dir;
        public string card_pic_info;
        public string unit_info;
        new public M2StatWeapons primaryWeapon;

        public M2Unit() : base(8)
        {
            primaryWeapon = new M2StatWeapons();
            stat_ter = new M2StatWeapons();
            stat_ter_ex = Stat_pri_attr.no;
        }

        public override string unitOutput()
        {
            LookUpTables lookTables = new LookUpTables();
            string unitString = "";

            bool firstAttr = false;
            Action<Action> setAndTagChanged = (action) =>
            {
                if (firstAttr == false)
                    firstAttr = true;
                else unitString += ", ";
                action();
            };


            unitString += (
                "type\t\t\t\t " + type + "\r\n" +
                "dictionary\t\t\t " + dictionary + "\r\n" +
                "category\t\t\t " + category + "\r\n" +
                "class\t\t\t\t " + unitClass + "\r\n" +
                "voice_type\t\t\t " + voiceType + "\r\n");

            unitString += "soldier\t\t\t\t " + soldier.name + ", " + soldier.number.ToString() + ", " + soldier.extras.ToString() + ", " + soldier.collisionMass.UniversalOutput();

            //unitString +=("\r\n");

            if (officer.Count > 0)
            {
                if (officer[0] != null)
                {
                    unitString += ("\r\nofficer\t\t\t\t " + officer[0]);
                }
            }

            if (officer.Count > 1)
            {
                if (officer[1] != null)
                {
                    unitString += ("\r\nofficer\t\t\t\t " + officer[1]);
                }
            }

            if (officer.Count > 2)
            {
                if (officer[2] != null)
                {
                    unitString += ("\r\nofficer\t\t\t\t " + officer[2]);
                }
            }

            if (engine != null)
                unitString += ("\r\nengine\t\t\t " + engine);

            if (animal != null)
                unitString += ("\r\nanimal\t\t\t " + animal);

            if (mount != null)
                unitString += ("\r\nmount\t\t\t " + mount);

            if (mountEffect.mountType.Count() > 0)
            {

                unitString += "\r\nmount_effect\t\t ";
                for (int i = 0; i < mountEffect.mountType.Count(); i++)
                {
                    if (mountEffect.modifier[i] < 0)
                        setAndTagChanged(() => unitString += (mountEffect.mountType[i]) + " " + mountEffect.modifier[i].ToString());
                    else setAndTagChanged(() => unitString += (mountEffect.mountType[i]) + " +" + mountEffect.modifier[i].ToString());
                }
            }


            if (naval != null)
                unitString += ("\r\nship\t\t\t\t " + naval);

            unitString += ("\r\nattributes\t\t\t "); // write attributes

            firstAttr = false;
            if (attributes.HasFlag(Attributes.sea_faring))
                setAndTagChanged(() => unitString += Attributes.sea_faring.ToString());
            if (attributes.HasFlag(Attributes.can_run_amok))
                setAndTagChanged(() => unitString += Attributes.can_run_amok.ToString());
            if (attributes.HasFlag(Attributes.can_sap))
                setAndTagChanged(() => unitString += Attributes.can_sap.ToString());
            if (attributes.HasFlag(Attributes.cantabrian_circle))
                setAndTagChanged(() => unitString += Attributes.cantabrian_circle.ToString());
            if (attributes.HasFlag(Attributes.command))
                setAndTagChanged(() => unitString += Attributes.command.ToString());
            if (attributes.HasFlag(Attributes.druid))
                setAndTagChanged(() => unitString += Attributes.druid.ToString());
            if (attributes.HasFlag(Attributes.frighten_foot))
                setAndTagChanged(() => unitString += Attributes.frighten_foot.ToString());
            if (attributes.HasFlag(Attributes.frighten_mounted))
                setAndTagChanged(() => unitString += Attributes.frighten_mounted.ToString());
            if (attributes.HasFlag(Attributes.general_unit))
                setAndTagChanged(() => unitString += Attributes.general_unit.ToString());
            if (attributes.HasFlag(Attributes.general_unit_upgrade))
                setAndTagChanged(() => unitString += Attributes.general_unit_upgrade.ToString());
            if (attributes.HasFlag(Attributes.hide_anywhere))
                setAndTagChanged(() => unitString += Attributes.hide_anywhere.ToString());
            if (attributes.HasFlag(Attributes.hide_forest))
                setAndTagChanged(() => unitString += Attributes.hide_forest.ToString());
            if (attributes.HasFlag(Attributes.hide_improved_forest))
                setAndTagChanged(() => unitString += Attributes.hide_improved_forest.ToString());
            if (attributes.HasFlag(Attributes.hide_long_grass))
                setAndTagChanged(() => unitString += Attributes.hide_long_grass.ToString());
            if (attributes.HasFlag(Attributes.mercenary_unit))
                setAndTagChanged(() => unitString += Attributes.mercenary_unit.ToString());
            if (attributes.HasFlag(Attributes.no_custom))
                setAndTagChanged(() => unitString += Attributes.no_custom.ToString());
            if (attributes.HasFlag(Attributes.warcry))
                setAndTagChanged(() => unitString += Attributes.warcry.ToString());
            if (attributes.HasFlag(Attributes.screeching_women))
                setAndTagChanged(() => unitString += Attributes.screeching_women.ToString());
            if (attributes.HasFlag(Attributes.hardy))
                setAndTagChanged(() => unitString += Attributes.hardy.ToString());
            if (attributes.HasFlag(Attributes.very_hardy))
                setAndTagChanged(() => unitString += Attributes.very_hardy.ToString());
            if (attributes.HasFlag(Attributes.power_charge))
                setAndTagChanged(() => unitString += Attributes.power_charge.ToString());
            if (attributes.HasFlag(Attributes.can_swim))
                setAndTagChanged(() => unitString += Attributes.can_swim.ToString());
            if (attributes.HasFlag(Attributes.is_peasant))
                setAndTagChanged(() => unitString += Attributes.is_peasant.ToString());
            if (attributes.HasFlag(Attributes.free_upkeep_unit))
                setAndTagChanged(() => unitString += Attributes.free_upkeep_unit.ToString());
            if (attributes.HasFlag(Attributes.can_withdraw))
                setAndTagChanged(() => unitString += Attributes.can_withdraw.ToString());
            if (attributes.HasFlag(Attributes.knight))
                setAndTagChanged(() => unitString += Attributes.knight.ToString());
            if (attributes.HasFlag(Attributes.gunpowder_unit))
                setAndTagChanged(() => unitString += Attributes.gunpowder_unit.ToString());
            if (attributes.HasFlag(Attributes.start_not_skirmishing))
                setAndTagChanged(() => unitString += Attributes.start_not_skirmishing.ToString());
            if (attributes.HasFlag(Attributes.stakes))
                setAndTagChanged(() => unitString += Attributes.stakes.ToString());
            if (attributes.HasFlag(Attributes.fire_by_rank))
                setAndTagChanged(() => unitString += Attributes.fire_by_rank.ToString());
            if (attributes.HasFlag(Attributes.cannot_skirmish))
                setAndTagChanged(() => unitString += Attributes.cannot_skirmish.ToString());


            unitString += ("\r\n");

            unitString += ("formation\t\t\t "); // write formation
            foreach (float num in formation.FormationTight)
                unitString += (num.UniversalOutput() + ", ");
            foreach (float num in formation.FormationSparse)
                unitString += (num.UniversalOutput() + ", ");
            unitString += (formation.FormationRanks + ", ");

            firstAttr = false;
            if (formation.FormationFlags.HasFlag(FormationTypes.phalanx)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.phalanx));
            if (formation.FormationFlags.HasFlag(FormationTypes.testudo)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.testudo));
            if (formation.FormationFlags.HasFlag(FormationTypes.schiltrom)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.schiltrom));
            if (formation.FormationFlags.HasFlag(FormationTypes.horde)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.horde));
            if (formation.FormationFlags.HasFlag(FormationTypes.square)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.square));
            if (formation.FormationFlags.HasFlag(FormationTypes.wedge)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.wedge));
            if (formation.FormationFlags.HasFlag(FormationTypes.shield_wall)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.shield_wall));

            unitString += ("\r\n");

            unitString += ("stat_health\t\t\t "); //write health
            firstAttr = false;

            foreach (int health in heatlh)
            {
                setAndTagChanged(() => unitString += (health));

            }

            unitString += ("\r\n");



            unitString += ("stat_pri\t\t\t "); // write primary weapon

            foreach (int atk in primaryWeapon.attack)
                unitString += (atk + ", ");

            unitString += (primaryWeapon.missileType + ", ");

            foreach (int miss in primaryWeapon.Missleattri)
                unitString += (miss + ", ");

            unitString += (
                lookTables.LookUpString(primaryWeapon.WeaponFlags) + ", " +
                 primaryWeapon.TechFlags.ToString() + ", " +
                lookTables.LookUpString(primaryWeapon.DamageFlags) + ", " +
                primaryWeapon.SoundFlags + ", ");

            if (primaryWeapon.musket_shot_set != null)
                unitString += primaryWeapon.musket_shot_set + ", ";

            firstAttr = false;
            foreach (float atkd in primaryWeapon.attackdelay)
            {
                setAndTagChanged(() => unitString += (atkd));
            }

            unitString += ("\r\n");


            if (stat_pri_ex != null)
            {
                unitString += ("stat_pri_ex\t\t " + stat_pri_ex);
                unitString += ("\r\n");
            }
            

            unitString += ("stat_pri_attr\t\t "); //attributes

            firstAttr = false;
            if (priAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (priAttri.HasFlag(Stat_pri_attr.bp)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.bp));
            if (priAttri.HasFlag(Stat_pri_attr.pa_spear)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.pa_spear));
            if (priAttri.HasFlag(Stat_pri_attr.long_pike)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.long_pike));
            if (priAttri.HasFlag(Stat_pri_attr.short_pike)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.short_pike));
            if (priAttri.HasFlag(Stat_pri_attr.prec)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.prec));
            if (priAttri.HasFlag(Stat_pri_attr.pa_thrown)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.pa_thrown));
            if (priAttri.HasFlag(Stat_pri_attr.launching)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.launching));
            if (priAttri.HasFlag(Stat_pri_attr.area)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.area));
            if (priAttri.HasFlag(Stat_pri_attr.no)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.no));
            if (priAttri.HasFlag(Stat_pri_attr.thrown_ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.thrown_ap));
            if (priAttri.HasFlag(Stat_pri_attr.fire)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.fire));
            if (priAttri.HasFlag(Stat_pri_attr.light_spear)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.light_spear));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_2)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_2));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_4)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_4));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_6)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_6));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_8)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_8));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_10)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_10));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_12)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_12));

            unitString += ("\r\n");

            unitString += ("stat_sec\t\t\t "); // secondary weapon
            foreach (int atk in secondaryWeapon.attack)
                unitString += (atk + ", ");

            if (stat_sec_ex != null)
            {
                unitString += ("stat_sec_ex\t\t " + stat_sec_ex);
                unitString += ("\r\n");
            }
            unitString += (secondaryWeapon.missileType + ", ");
            foreach (int miss in secondaryWeapon.Missleattri)
                unitString += (miss + ", ");
            unitString += (
                lookTables.LookUpString(secondaryWeapon.WeaponFlags) + ", " +
                secondaryWeapon.TechFlags.ToString() + ", " +
                lookTables.LookUpString(secondaryWeapon.DamageFlags) + ", " +
                secondaryWeapon.SoundFlags + ", ");



            firstAttr = false;
            foreach (float atkd in secondaryWeapon.attackdelay)
            {
                setAndTagChanged(() => unitString += (atkd));
            }

            unitString += ("\r\n");

            firstAttr = false;
            unitString += ("stat_sec_attr\t\t ");

            if (secAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (secAttri.HasFlag(Stat_pri_attr.bp)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.bp));
            if (secAttri.HasFlag(Stat_pri_attr.pa_spear)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.pa_spear));
            if (secAttri.HasFlag(Stat_pri_attr.long_pike)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.long_pike));
            if (secAttri.HasFlag(Stat_pri_attr.short_pike)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.short_pike));
            if (secAttri.HasFlag(Stat_pri_attr.prec)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.prec));
            if (secAttri.HasFlag(Stat_pri_attr.pa_thrown)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.pa_thrown));
            if (secAttri.HasFlag(Stat_pri_attr.launching)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.launching));
            if (secAttri.HasFlag(Stat_pri_attr.area)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.area));
            if (secAttri.HasFlag(Stat_pri_attr.no)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.no));
            if (secAttri.HasFlag(Stat_pri_attr.thrown_ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.thrown_ap));
            if (secAttri.HasFlag(Stat_pri_attr.fire)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.fire));
            if (secAttri.HasFlag(Stat_pri_attr.light_spear)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.light_spear));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_2)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_2));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_4)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_4));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_6)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_6));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_8)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_8));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_10)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_10));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_12)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_12));


            unitString += ("\r\n");

            

            if (stat_ter.missileType != null)
            {
                unitString += ("stat_ter\t\t\t ");

                foreach (int atk in stat_ter.attack)
                    unitString += (atk + ", ");

                unitString += (stat_ter.missileType + ", ");

                foreach (int miss in stat_ter.Missleattri)
                    unitString += (miss + ", ");

                unitString += (
                    lookTables.LookUpString(stat_ter.WeaponFlags) + ", " +
                     stat_ter.TechFlags.ToString() + ", " +
                    lookTables.LookUpString(stat_ter.DamageFlags) + ", " +
                    stat_ter.SoundFlags + ", ");

                if (stat_ter.musket_shot_set != null)
                    unitString += stat_ter.musket_shot_set + ", ";

                firstAttr = false;
                foreach (float atkd in primaryWeapon.attackdelay)
                {
                    setAndTagChanged(() => unitString += (atkd));
                }

                unitString += ("\r\n");
            }
            if (stat_ter_ex != Stat_pri_attr.no)
            {
                unitString += ("stat_ter_ex\t\t " + stat_ter_ex.ToString());
                unitString += ("\r\n");
            }
            if (stat_ter_attr != null)
            {
                unitString += ("stat_ter_attr\t\t " + stat_ter_attr);
                unitString += ("\r\n");
            }

            unitString += ("stat_pri_armour\t\t ");
            foreach (int numb in primaryArmour.stat_pri_armour)
                unitString += (numb + ", ");
            unitString += lookTables.LookUpString(primaryArmour.armour_sound);

            unitString += ("\r\n");

            if (stat_armour_ex != null)
            {
                unitString += ("stat_armour_ex\t\t\t " + stat_armour_ex);
                unitString += ("\r\n");
            }

            unitString += ("stat_sec_armour\t\t ");
            foreach (int numb in secondaryArmour.stat_sec_armour)
                unitString += (numb + ", ");
            unitString += lookTables.LookUpString(secondaryArmour.sec_armour_sound);

            unitString += ("\r\n");

            unitString += ("stat_heat\t\t\t " + heat);

            unitString += ("\r\n");

            unitString += ("stat_ground\t\t\t ");
            firstAttr = false;
            foreach (int numb in ground)
            {
                setAndTagChanged(() => unitString += numb);
            }

            unitString += ("\r\n");

            unitString += ("stat_mental\t\t\t " + mental.morale + ", ");
            unitString += (lookTables.LookUpString(mental.discipline) + ", " + lookTables.LookUpString(mental.training));

            unitString += ("\r\n");

            unitString += ("stat_charge_dist\t " + chargeDistance);

            unitString += ("\r\n");

            unitString += ("stat_fire_delay\t\t " + fireDelay);

            unitString += ("\r\n");

            unitString += ("stat_food\t\t\t " + food[0] + ", " + food[1]);

            unitString += ("\r\n");

            unitString += ("stat_cost\t\t\t ");

            firstAttr = false;
            foreach (int cost in cost)
            {
                setAndTagChanged(() => unitString += (cost));
            }


            unitString += ("\r\n");

            if (stat_stl != null)
            {
                unitString += ("stat_stl\t\t\t " + stat_stl);
                unitString += ("\r\n");
            }

            if (armour_ug_levels != null)
            {
                unitString += ("armour_ug_levels\t\t\t " + armour_ug_levels);
                unitString += ("\r\n");
            }

            if (armour_ug_models != null)
            {
                unitString += ("armour_ug_models\t\t\t " + armour_ug_models);
                unitString += ("\r\n");
            }

            unitString += ("ownership\t\t\t ");

            firstAttr = false;
            foreach (string faction in ownership)
            {
                setAndTagChanged(() => unitString += faction);
            }

            unitString += ("\r\n");

            if (era0 != null)
            {
                unitString += ("era0\t\t\t " + era0);
                unitString += ("\r\n");
            }
            if (era1 != null)
            {
                unitString += ("era1\t\t\t " + era1);
                unitString += ("\r\n");
            }
            if (era2 != null)
            {
                unitString += ("era2\t\t\t " + era2);
                unitString += ("\r\n");
            }
            if (info_pic_dir != null)
            {
                unitString += ("info_pic_dic\t\t\t " + info_pic_dir);
                unitString += ("\r\n");
            }
            if (card_pic_info != null)
            {
                unitString += ("card_pic_info\t\t\t " + card_pic_info);
                unitString += ("\r\n");
            }
            if (unit_info != null)
            {
                unitString += ("unit_info\t\t\t " + unit_info);
                unitString += ("\r\n");
            }

            string[] lines = unitString.Split('\r', '\n').CleanStringArray();
            Dictionary<EDULineEnums, int> multiple = new Dictionary<EDULineEnums, int>();

            for (int i = 0; i < lines.Count(); i++)
            {
                EDULineEnums identifier;
                bool isIdentifier = Enum.TryParse<EDULineEnums>(Functions_General.GetFirstWord(lines[i]).Capitalise(), out identifier);

                if (isIdentifier )
                {
                    if (comments.ContainsKey(identifier))
                    {
                        if (comments[identifier] is string)
                            lines[i] += "\t\t" + comments[identifier];
                        else if (comments[identifier] is List<string>)
                        {
                            if (!multiple.ContainsKey(identifier))
                                multiple.Add(identifier, 0);
                            else multiple[identifier] += 1;

                            lines[i] += "\t\t" + ((List<string>)comments[identifier])[multiple[identifier]];

                        }

                    }
                }
            }

            return lines.ArrayToString(false, true) + "\r\n";
        }


    }
}
