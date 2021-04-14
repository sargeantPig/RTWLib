using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Functions;
using RTWLib.Extensions;
namespace RTWLib.Objects
{
    public class Unit : IComparable
    {
        /// <summary>
        /// internal name of the unit.
        /// </summary>
        public string type;
        /// <summary>
        /// tag for looking up the screen name
        /// </summary>
        public string dic;
        /// <summary>
        /// infantry, cavalry, siege, handler, ship or non_combatant
        /// </summary>
        public string category;
        /// <summary>
        /// light, heavy, spearman, or missle
        /// </summary>
        public string uClass;
        /// <summary>
        /// determines the type of voice used by the unit
        /// </summary>
        public string voice;
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
        public int[] health;
        /// <summary>
        /// primary weapon stats
        /// </summary>
        public StatWeapons priWep;
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
        public StatWeapons secWep;
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
        public StatPriArmour priArm;
        /// <summary>
        /// Details of animal's or vehicle's defenses (note riden horses do not have a separate defence)
        /// </summary>
        public StatSecArmour secArmr;
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
        public int chargeDist;
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
        public float pointValue = 0;
        /// <summary>
        /// comments written by a user in the file loaded
        /// </summary>
        public Dictionary<EDULineEnums, object> comments = new Dictionary<EDULineEnums, object>();

        bool pointsCalculated = false;
        public Unit(int costValues = 6)
        {
            soldier = new Soldier();
            mountEffect = new MountEffect();
            formation = new Formation();
            priWep = new StatWeapons();
            secWep = new StatWeapons();
            priArm = new StatPriArmour();
            secArmr = new StatSecArmour();
            mental = new Mentality();
            officer = new List<string>();
            ownership = new List<string>();
            health = new int[2];
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
            unitString += LineOutput(dic,
                "dictionary");
            unitString += LineOutput(category,
                "category");
            unitString += LineOutput(uClass,
                "class");
            unitString += LineOutput(voice,
                "voice_type");

            unitString += SpecialOutput(new object[] { soldier.name, soldier.number,
                soldier.extras, soldier.collMass.UniversalOutput()},
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

            unitString += SpecialOutput(new object[] { formation.FormationTight[0].UniversalOutput(),
            formation.FormationTight[1].UniversalOutput(),
            formation.FormationSparse[0].UniversalOutput(), formation.FormationSparse[1].UniversalOutput(),
            formation.FormationRanks, EnumFlagsToString(formation.FormationFlags)},
            "formation");


            unitString += ListOutput(health,
                "stat_health");

            unitString += SpecialOutput(new object[] {priWep.atk[0], priWep.atk[1],
            priWep.missType, priWep.missAttr[0], priWep.missAttr[1],
            priWep.WepFlags, priWep.TechFlags, priWep.DmgFlags,
            priWep.SoundFlags, priWep.atkDly[0], priWep.atkDly[1] },
            "stat_pri");


            unitString += LineOutput(EnumFlagsToString(priAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }),
                "stat_pri_attr");

            unitString += SpecialOutput(new object[] {secWep.atk[0], secWep.atk[1],
            secWep.missType, secWep.missAttr[0], secWep.missAttr[1],
            secWep.WepFlags, secWep.TechFlags, secWep.DmgFlags,
            secWep.SoundFlags, secWep.atkDly[0], secWep.atkDly[1] },
            "stat_sec");

            unitString += LineOutput(EnumFlagsToString(secAttri, false, new Dictionary<Enum, string>() {
                {Stat_pri_attr.thrown_ap, "thrown ap" }
            }),
                "stat_sec_attr");

            unitString += SpecialOutput(new object[] { priArm.priArm[0], priArm.priArm[1],
            priArm.priArm[2], priArm.armSound},
            "stat_pri_armour");

            unitString += SpecialOutput(new object[] { secArmr.secArmour[0], secArmr.secArmour[1],
                secArmr.secArmSound},
            "stat_sec_armour");

            unitString += LineOutput(heat,
                "stat_heat");
            unitString += ListOutput(ground,
                "stat_ground");

            unitString += SpecialOutput(new object[] {mental.morale, mental.discipline,
                mental.training}, "stat_mental");


            unitString += LineOutput(chargeDist,
                "stat_charge_dist");

            unitString += LineOutput(fireDelay,
                "stat_fire_delay");

            unitString += ListOutput(food, "stat_food");
            unitString += ListOutput(cost, "stat_cost");
            unitString += ListOutput(ownership.ToArray(), "ownership");

            string[] lines = unitString.Split('\r', '\n').CleanStringArray();
            Dictionary<EDULineEnums, int> multiple = new Dictionary<EDULineEnums, int>();

            for (int i = 0; i < lines.Count(); i++)
            {
                EDULineEnums identifier;
                bool isIdentifier = Enum.TryParse<EDULineEnums>(lines[i].GetFirstWord().Capitalise(), out identifier);

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
            return 20 - ident.Length;
        }

        string identSpacing(int identifierLength)
        {
            string spaces = "";

            for (int i = 0; i < identifierLength; i++)
                spaces += " ";
            return spaces;
        }

        public virtual void CalculatePointValue(bool forceUpdate = false)
        {
            if (pointsCalculated && !forceUpdate)
                return;

            float points = 0f;

            Dictionary<string, float> statWeights = new Dictionary<string, float>()
            {
                {"health", 2f },
                {"attackFactor", 1f },
                {"attackBonus", 0.8f },
                {"attackDelay", 1f},
                {"siegeAttack", 0.2f},
                {"missileAttack", 0.8f },
                {"missileRange", 0.1f},
                {"soldierNumber", 1f},
                {"armourFactor", 1f},
                {"armourShield", 1f},
                {"armourDefence", 1f},
                {"morale", 1.2f},
                {"berserker", 1.7f},
                {"impetuous", 1f},
                {"disciplined", 1.4f},
                {"low", 0.8f},
                {"normal", 1f},
                {"highlyTrained", 1.4f},
                {"trained", 1f},
                {"untrained", 0.8f},
                {"frightenFoot", 1.1f},
                {"frightenMounted", 1.1f},
                {"command", 1.1f},
            };

            points += health[0] * statWeights["health"];
            //points += soldier.number * statWeights["soldierNumber"];
            points += priArm.priArm[0] * statWeights["armourFactor"];
            points += priArm.priArm[1] * statWeights["armourDefence"];
            points += priArm.priArm[2] * statWeights["armourShield"];
            points += mental.morale * statWeights["morale"];

            if (this.uClass == "missile")
            {
                if (this.category != "siege")
                {
                    points += priWep.missAttr[0] * statWeights["missileRange"];
                    points += priWep.atk[0] * statWeights["missileAttack"];
                }
                else
                {
                    points += secWep.missAttr[0] * statWeights["missileRange"];
                    points += secWep.atk[0] * statWeights["siegeAttack"];
                }
            }

            else
            {
                points += priWep.atk[0] * statWeights["attackFactor"];
                points += priWep.atk[1] * statWeights["attackBonus"];
            }

            switch (mental.discipline)
            {
                case Statmental_discipline.berserker:
                    points *= statWeights["berserker"];
                    break;
                case Statmental_discipline.disciplined:
                    points *= statWeights["disciplined"];
                    break;
                case Statmental_discipline.impetuous:
                    points *= statWeights["impetuous"];
                    break;
                case Statmental_discipline.low:
                    points *= statWeights["low"];
                    break;
                case Statmental_discipline.normal:
                    points *= statWeights["normal"];
                    break;

            }
            switch (mental.training)
            {
                case Statmental_training.highly_trained:
                    points *= statWeights["highlyTrained"];
                    break;
                case Statmental_training.trained:
                    points *= statWeights["trained"];
                    break;
                case Statmental_training.untrained:
                    points *= statWeights["untrained"];
                    break;

            }

            if (attributes.HasFlag(Attributes.frighten_foot))
                points *= statWeights["frightenFoot"];
            if (attributes.HasFlag(Attributes.frighten_mounted))
                points *= statWeights["frightenMounted"];
            if (attributes.HasFlag(Attributes.command))
                points *= statWeights["command"];

            pointValue = points * (((health[0] + health[1]) + 1));

            pointsCalculated = true;
        }


        private class ComparePointsHelper : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                var unit = Unit.CastForCompare(x, y);

                if (unit[0].pointValue > unit[1].pointValue)
                    return 1;
                else if (unit[0].pointValue < unit[1].pointValue)
                    return -1;
                else return 0;
            }
        }

        public static Unit[] CastForCompare(object x, object y)
        {
            return new Unit[] { (Unit)x, (Unit)y };
        }

        public static IComparer ComparePoints()
        {
            return (IComparer) new ComparePointsHelper();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

}
