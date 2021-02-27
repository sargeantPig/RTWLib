using RTWLib.Data;
using RTWLib.Functions;
using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2Unit : Unit
    {
        public M2StatWeapons stat_ter; //M2TW
        public string stat_ter_ex; //M2TW
        public Stat_pri_attr stat_ter_attr; //M2tw
        public string stat_armour_ex; //M2TW
        public string stat_pri_ex; // M2TW
        public string bannerFaction; //M2TW BANNER
        public string bannerUnit;
        public string bannerHoly; //M2TW HOLY banner
        public string stat_sec_ex; //M2TW
        public string stat_stl; //M2TW
        public List<string> armour_ug_levels;
        public List<string> armour_ug_models;
        public List<string> era0;
        public List<string> era1;
        public List<string> era2;
        public string info_pic_dir;
        public string card_pic_info;
        public string unit_info;
        public string accent;
        new public M2StatWeapons primaryWeapon;

        public M2Unit() : base(8)
        {
            primaryWeapon = new M2StatWeapons();
            stat_ter = new M2StatWeapons();
            stat_ter_ex = null;
            stat_ter_attr = Stat_pri_attr.no;
            armour_ug_levels = new List<string>();
            armour_ug_models = new List<string>();
            era0 = new List<string>();
            era1 = new List<string>();
            era2 = new List<string>();
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
            unitString += LineOutput(type,
                "type");
            unitString += LineOutput(dictionary,
                "dictionary");
            unitString += LineOutput(category,
                "category");
            unitString += LineOutput(uClass,
                "class");
            unitString += LineOutput(voiceType,
                "voice_type");
            unitString += LineOutput(accent,
                "accent");
            unitString += LineOutput(bannerFaction,
                "banner faction");
            unitString += LineOutput(bannerUnit,
                "banner unit");
            unitString += LineOutput(bannerHoly,
                "banner holy");
            unitString += SpecialOutput(new object[] { soldier.name, soldier.number,
                soldier.extras, soldier.collisionMass.UniversalOutput()},
                "soldier");
            if (officer.Count > 0)
                unitString += LineOutput(officer[0],
                    "officer");
            if (officer.Count > 1)
                unitString += LineOutput(officer[1],
                    "officer");
            if (officer.Count > 2)
                unitString += LineOutput(officer[2],
                    "officer");
            unitString += LineOutput(engine,
                 "engine");
            unitString += LineOutput(animal,
                "animal");
            unitString += LineOutput(mount,
                "mount");

            if (mountEffect.mountType.Count() > 0)
            {

                unitString += "\r\nmount_effect\t\t ";
                for (int i = 0; i < mountEffect.mountType.Count(); i++)
                {
                    if (mountEffect.modifier[i] < 0)
                        setAndTagChanged(() => unitString += (mountEffect.mountType[i]) + " " + mountEffect.modifier[i].ToString());
                    else setAndTagChanged(() => unitString += (mountEffect.mountType[i]) + " +" + mountEffect.modifier[i].ToString());
                }
                unitString += "\r\n";
            }

            unitString += LineOutput(naval,
                "ship");
            unitString += LineOutput(EnumFlagsToString(attributes),
                "attributes");
            unitString += SpecialOutput(new object[] { formation.FormationTight[0],
            formation.FormationTight[1],
            formation.FormationSparse[0], formation.FormationSparse[1],
            formation.FormationRanks, EnumFlagsToString(formation.FormationFlags)},
            "formation");
            unitString += ListOutput(health,
                "stat_health");
            unitString += SpecialOutput(new object[] {primaryWeapon.attack[0], primaryWeapon.attack[1],
            primaryWeapon.missileType, primaryWeapon.Missleattri[0], primaryWeapon.Missleattri[1],
            primaryWeapon.WeaponFlags, primaryWeapon.TechFlags, primaryWeapon.DamageFlags,
            primaryWeapon.SoundFlags, primaryWeapon.musket_shot_set, primaryWeapon.attackdelay[0], primaryWeapon.attackdelay[1] },
            "stat_pri");
            unitString += LineOutput(stat_pri_ex, 
                "stat_pri_ex");
            unitString += LineOutput(EnumFlagsToString(priAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }),
                "stat_pri_attr");
            unitString += SpecialOutput(new object[] {secondaryWeapon.attack[0], secondaryWeapon.attack[1],
            secondaryWeapon.missileType, secondaryWeapon.Missleattri[0], secondaryWeapon.Missleattri[1],
            secondaryWeapon.WeaponFlags, secondaryWeapon.TechFlags, secondaryWeapon.DamageFlags,
            secondaryWeapon.SoundFlags, secondaryWeapon.attackdelay[0], secondaryWeapon.attackdelay[1] },
            "stat_sec");
            unitString += LineOutput(stat_pri_ex, 
                "stat_pri_ex");
            unitString += LineOutput(EnumFlagsToString(secAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }),
                "stat_sec_attr");
            if (stat_ter.missileType != null)
            {
                unitString += SpecialOutput(new object[] {stat_ter.attack[0], stat_ter.attack[1],
                    stat_ter.missileType, stat_ter.Missleattri[0], stat_ter.Missleattri[1],
                    stat_ter.WeaponFlags, stat_ter.TechFlags, stat_ter.DamageFlags,
                    stat_ter.SoundFlags, stat_ter.musket_shot_set, stat_ter.attackdelay[0], stat_ter.attackdelay[1] },
                    "stat_ter");
            }
            unitString += LineOutput(stat_ter_ex, 
                "stat_ter_ex");
            if (stat_ter_attr != Stat_pri_attr.no)
            {
                unitString += LineOutput(EnumFlagsToString(stat_ter_attr), 
                    "stat_ter_attr");
            }

            unitString += SpecialOutput(new object[] { primaryArmour.stat_pri_armour[0], primaryArmour.stat_pri_armour[1],
            primaryArmour.stat_pri_armour[2], primaryArmour.armour_sound},
            "stat_pri_armour");
            unitString += LineOutput(stat_armour_ex,
                "stat_armour_ex");
            unitString += SpecialOutput(new object[] { secondaryArmour.stat_sec_armour[0], secondaryArmour.stat_sec_armour[1],
                secondaryArmour.sec_armour_sound},
                "stat_sec_armour");
            unitString += LineOutput(heat,
                "stat_heat");
            unitString += ListOutput(ground,
                "stat_ground");
            unitString += SpecialOutput(new object[] {mental.morale, mental.discipline,
                mental.training}, 
                "stat_mental");
            unitString += LineOutput(chargeDistance,
                "stat_charge_dist");
            unitString += LineOutput(fireDelay,
                "stat_fire_delay");
            unitString += ListOutput(food, 
                "stat_food");
            unitString += ListOutput(cost, 
                "stat_cost");
            unitString += LineOutput(stat_stl,
                "stat_stl");
            unitString += ListOutput(armour_ug_levels.ToArray(),
                "armour_ug_levels");
            unitString += ListOutput(armour_ug_models.ToArray(),
                "armour_ug_models");
            unitString += ListOutput(ownership.ToArray(),
                "ownership");
            if (era0.Count() > 0)
                unitString += ListOutput(era0.ToArray(),
                    "era 0");
            if (era1.Count() > 0)
                unitString += ListOutput(era1.ToArray(),
                    "era 1");
            if (era2.Count() > 0)
                unitString += ListOutput(era2.ToArray(), 
                    "era 2");
            unitString += LineOutput(info_pic_dir,
                "info_pic_dir");
            unitString += LineOutput(card_pic_info,
                "card_pic_info");
            unitString += LineOutput(unit_info,
                "unit_info");

            string[] lines = unitString.Split('\r', '\n').CleanStringArray();
            Dictionary<EDULineEnums, int> multiple = new Dictionary<EDULineEnums, int>();

            for (int i = 0; i < lines.Count(); i++)
            {
                EDULineEnums identifier;
                string strIdent = Functions_General.GetFirstWord(lines[i], null, 0, '\t').Capitalise();
                bool isIdentifier = Enum.TryParse<EDULineEnums>(strIdent, out identifier);

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
