﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Functions;

namespace RTWLib.Objects
{
    public class Unit
    {
        /// <summary>
        /// internal name of the unit.
        /// </summary>
        public string type;
        /// <summary>
        /// tag for looking up the screen name
        /// </summary>
        public string dictionary;
        /// <summary>
        /// infantry, cavalry, siege, handler, ship or non_combatant
        /// </summary>
        public string category;
        /// <summary>
        /// light, heavy, spearman, or missle
        /// </summary>
        public string unitClass;
        /// <summary>
        /// determines the type of voice used by the unit
        /// </summary>
        public string voiceType;
        /// <summary>
        /// make-up of each soldier (name, number, collision etc)
        /// </summary>
        public Soldier soldier;
        /// <summary>
        /// name of officer model. There may be up to 0-3 officer lines per unit
        /// </summary>
        public List<string> officer;
        /// <summary>
        /// type of siege used ( ballista, scorpion, onager, heavy_onager, repeating_ballista)
        /// </summary>
        public string engine;
        /// <summary>
        /// the type of non-ridden on animals used by the  wardogs, pigs
        /// </summary>
        public string animal;
        /// <summary>
        /// type of animal or vehicle ridden on
        /// </summary>
        public string mount;
        /// <summary>
        /// type of ship used if applicable
        /// </summary>
        public string naval;
        /// <summary>
        /// modifiers vs different mounts
        /// </summary>
        public MountEffect mountEffect;
        /// <summary>
        /// A list of attributes and abilities the unit may have.Including
        /// sea_faring = can board ships
        /// hide_forest, hide_improved_forest, hide_long_grass, hide_anywhere   = defines where the unit can hide
        /// can_sap = Can dig tunnels under walls
        /// frighten_foot, frighten_mounted = Cause fear to certain nearby unit types
        /// can_run_amok = Unit may go out of control when riders lose control of animals
        /// general_unit = The unit can be used for a named character's bodyguard
        /// cantabrian_circle = The unit has this special ability
        /// no_custom = The unit may not be selected in custom battles
        /// command = The unit carries a legionary eagle, and gives bonuses to nearby units
        /// mercenary_unit = The unit is s mercenary unit available to all factions
        /// </summary>
        public Attributes attributes;
        /// <summary>
        /// formation values
        /// </summary>
        public Formation formation;
        /// <summary>
        /// [0] hit point of man
        /// [1] hitpoint of mount or attached animal (horses and camels do not have separate hitpoints.
        /// </summary>
        public int[] heatlh;
        /// <summary>
        /// primary weapon stats
        /// </summary>
        public StatWeapons primaryWeapon;
        /// <summary>
        /// primary weapon attributes any or all of
        /// ap = armour piercing.Only counts half of target's armour
        /// bp = body piercing.Missile can pass through men and hit those behind
        /// spear = Used for long spears.Gives bonuses fighting cavalry, and penalties against infantry
        /// long_pike = Use very long pikes.Phalanx capable units only
        /// short_pike = Use shorter than normal spears.Phalanx capable units only
        /// prec = Missile weapon is only thrown just before charging into combat
        /// thrown = The missile type if thrown rather than fired
        /// launching = attack may throw target men into the air
        /// area = attack affects an area, not just one man
        /// </summary>
        public Stat_pri_attr priAttri;
        /// <summary>
        /// secondary weapon stats
        /// </summary>
        public StatWeapons secondaryWeapon;
        /// <summary>
        /// primary weapon attributes any or all of
        /// ap = armour piercing.Only counts half of target's armour
        /// bp = body piercing.Missile can pass through men and hit those behind
        /// spear = Used for long spears.Gives bonuses fighting cavalry, and penalties against infantry
        /// long_pike = Use very long pikes.Phalanx capable units only
        /// short_pike = Use shorter than normal spears.Phalanx capable units only
        /// prec = Missile weapon is only thrown just before charging into combat
        /// thrown = The missile type if thrown rather than fired
        /// launching = attack may throw target men into the air
        /// area = attack affects an area, not just one man
        /// </summary>
        public Stat_pri_attr secAttri;
        /// <summary>
        /// details of the units defences
        /// </summary>
        public StatPriArmour primaryArmour;
        /// <summary>
        /// Details of animal's or vehicle's defenses (note riden horses do not have a separate defence)
        /// </summary>
        public StatSecArmour secondaryArmour;
        /// <summary>
        /// Extra fatigue suffered by the unit in hot climates
        /// </summary>
        public int heat;
        /// <summary>
        /// combat modifiers for different ground types
        /// [0] scrub modifier
        /// [1] sand modifier
        /// [2] forest modifier
        /// [3] snow modifier
        /// </summary>
        public int[] ground;
        /// <summary>
        /// mental stats of the unit
        /// </summary>
        public Mentality mental;
        /// <summary>
        /// distance from the enemy that the unit will start charging
        /// </summary>
        public int chargeDistance;
        /// <summary>
        /// delay between volleys, on top of the animation delay
        /// </summary>
        public int fireDelay;
        /// <summary>
        /// no longer used
        /// </summary>
        public int[] food;
        /// <summary>
        /// [0] number of turns to build
        /// [1] cost to construct
        /// [2] cost of upkeep
        /// [3] cost of weapon upgrades
        /// [4] cost of armour upgrades
        /// [5] cost in custom battles
        /// </summary>
        public int[] cost;
        /// <summary>
        /// factions that can may have this unit
        /// </summary>
        public List<string> ownership;
        /// <summary>
        /// a score given to the unit (randomiser use)
        /// </summary>
        public float unitPointValue = 0;
        /// <summary>
        /// comments written by a user in the file loaded
        /// </summary>
        public Dictionary<EDULineEnums, object> comments = new Dictionary<EDULineEnums, object>();
        public Unit(int costValues = 6)
        {
            soldier = new Soldier();
            mountEffect = new MountEffect();
            formation = new Formation();
            primaryWeapon = new StatWeapons();
            secondaryWeapon = new StatWeapons();
            primaryArmour = new StatPriArmour();
            secondaryArmour = new StatSecArmour();
            mental = new Mentality();
            officer = new List<string>();
            ownership = new List<string>();
            heatlh = new int[2];
            ground = new int[4];
            food = new int[2];
            cost = new int[costValues];
            
        }

        public virtual string unitOutput()
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
            unitString += LineOutput(unitClass, 
                "class");
            unitString += LineOutput(voiceType, 
                "voice_type");

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
                unitString += "\r\nmount_effect" + identSpacing(identifierLength("mount_effect"));
                for (int i = 0; i < mountEffect.mountType.Count(); i++)
                {
                    if(mountEffect.modifier[i] < 0)
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
           
            
            unitString += ListOutput(heatlh, 
                "stat_health");

            unitString += SpecialOutput(new object[] {primaryWeapon.attack[0], primaryWeapon.attack[1],
            primaryWeapon.missileType, primaryWeapon.Missleattri[0], primaryWeapon.Missleattri[1],
            primaryWeapon.WeaponFlags, primaryWeapon.TechFlags, primaryWeapon.DamageFlags,
            primaryWeapon.SoundFlags, primaryWeapon.attackdelay[0], primaryWeapon.attackdelay[1] }, 
            "stat_pri");


            unitString += LineOutput(EnumFlagsToString(priAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }), 
                "stat_pri_attr");

            unitString += SpecialOutput(new object[] {secondaryWeapon.attack[0], secondaryWeapon.attack[1],
            secondaryWeapon.missileType, secondaryWeapon.Missleattri[0], secondaryWeapon.Missleattri[1],
            secondaryWeapon.WeaponFlags, secondaryWeapon.TechFlags, secondaryWeapon.DamageFlags,
            secondaryWeapon.SoundFlags, secondaryWeapon.attackdelay[0], secondaryWeapon.attackdelay[1] },
            "stat_sec");

            unitString += LineOutput(EnumFlagsToString(secAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }), 
                "stat_sec_attr");

            unitString += SpecialOutput(new object[] { primaryArmour.stat_pri_armour[0], primaryArmour.stat_pri_armour[1],
            primaryArmour.stat_pri_armour[2], primaryArmour.armour_sound}, 
            "stat_pri_armour");

            unitString += SpecialOutput(new object[] { secondaryArmour.stat_sec_armour[0], secondaryArmour.stat_sec_armour[1],
                secondaryArmour.sec_armour_sound},
            "stat_sec_armour");

            unitString += LineOutput(heat, 
                "stat_heat");
            unitString += ListOutput(ground, 
                "stat_ground");

            unitString += SpecialOutput(new object[] {mental.morale, mental.discipline, 
                mental.training}, "stat_mental");


            unitString += LineOutput(chargeDistance,
                "stat_charge_dist");

            unitString += LineOutput(fireDelay,
                "stat_fire_delay");

            unitString += ListOutput(food, "stat_food");
            unitString += ListOutput(cost, "stat_cost");
            unitString += ListOutput(ownership.ToArray(), "ownership");

            string[] lines = unitString.Split('\r', '\n').CleanStringArray();
            Dictionary<EDULineEnums, int> multiple = new Dictionary<EDULineEnums, int>();

            for (int i =0; i < lines.Count(); i++)
            {
                EDULineEnums identifier;
                bool isIdentifier = Enum.TryParse<EDULineEnums>(Functions_General.GetFirstWord(lines[i]).Capitalise(), out identifier);

                if (isIdentifier)
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

            return lines.ArrayToString(false, true) + "\r\n";
        }

        public virtual string ListOutput<T>(T[] list, string identifier)
        {
            if (list.Count() == 0)
                return string.Empty;

            string line = identifier + identSpacing(identifierLength(identifier));

            for (int i = 0; i < list.Count(); i++)
            {
                if (i == list.Count() - 1)
                    line += list[i].ToString();
                else line += list[i].ToString() + ", ";
            }

            return line + "\r\n";
        }

        public virtual string LineOutput<T>(T data, string identifier)
        {
            string line = string.Empty;
            
            if (data == null)
                return line;
            else
                line += identifier + identSpacing(identifierLength(identifier)) + data.ToString();

            return line + "\r\n";
        }

        public virtual string SpecialOutput(object[] vars, string identifier)
        {
            if (vars.Count() == 0)
                return string.Empty;

            string line = identifier + identSpacing(identifierLength(identifier));

            for (int i = 0; i < vars.Count(); i++)
            {
                if (vars[i] == null)
                    continue;

                if (i == vars.Count() - 1)
                    line += vars[i].ToString();
                else line += vars[i].ToString() + ", ";
            }

            return line + "\r\n";

        }

        public virtual string EnumFlagsToString(Enum input, bool startWithComma = false, Dictionary<Enum, string> specialCases = null)
        {
            string newString = string.Empty;
            bool comma = false;
            foreach (Enum s in Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(s))
                {
                    string inpString = s.ToString();

                    if (specialCases != null)
                    {
                        if (specialCases.ContainsKey(s))
                            inpString = specialCases[s];
                    }

                    if (comma || startWithComma)
                        newString += ", ";
                    newString += inpString;
                    comma = true;
                }
            }

            return newString;
        
        }


        int identifierLength(string ident)
        {
            return 17 - ident.Length;
        }

        string identSpacing(int identifierLength)
        {
            string spaces = "";

            for (int i = 0; i < identifierLength; i++)
                spaces += " ";
            return spaces;
        }
    
    }

}
