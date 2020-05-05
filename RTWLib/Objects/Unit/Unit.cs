using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
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
        public FactionOwnership ownership;
        /// <summary>
        /// a score given to the unit (randomiser use)
        /// </summary>
        public float unitPointValue = 0;

        public Unit()
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

            heatlh = new int[2];
            ground = new int[4];
            food = new int[2];
            cost = new int[6];
        }

        public string unitOutput()
        {
            LookUpTables lookTables = new LookUpTables();
            string unitString = "";

            unitString += (
                "type\t\t\t\t " + type + "\r\n" +
                "dictionary\t\t\t " + dictionary + "\r\n" +
                "category\t\t\t " + category + "\r\n" +
                "class\t\t\t\t " + unitClass + "\r\n" +
                "voice_type\t\t\t " + voiceType + "\r\n");

            unitString += "soldier\t\t\t\t " + soldier.name + ", " + soldier.number.ToString() + ", " + soldier.extras.ToString() + ", " + soldier.collisionMass.ToString();

            //unitString +=("\r\n");

            if (engine != null)
                unitString += ("\r\nengine\t\t\t " + engine);

            if (animal != null)
                unitString += ("\r\nanimal\t\t\t " + animal);

            if (mount != null)
                unitString += ("\r\nmount\t\t\t " + mount);

            if (officer.Count > 0)
            {
                if (officer[0] != null)
                {
                    unitString += ("\r\nofficer\t\t\t " + officer[0]);
                }
            }

            if (officer.Count > 1)
            {
                if (officer[1] != null)
                {
                    unitString += ("\r\nofficer\t\t\t " + officer[1]);
                }
            }

            if (officer.Count > 2)
            {
                if (officer[2] != null)
                {
                    unitString += ("\r\nofficer\t\t\t " + officer[2]);
                }
            }

            if (naval != null)
                unitString += ("\r\nship\t\t\t\t " + naval);

            unitString += ("\r\nattributes\t\t\t "); // write attributes

            bool firstAttr = false;
            Action<Action> setAndTagChanged = (action) =>
            {
                if (firstAttr == false)
                    firstAttr = true;
                else unitString += ", ";
                action();
            };
            if (attributes.HasFlag(Attributes.sea_faring))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.sea_faring));
            if (attributes.HasFlag(Attributes.can_run_amok))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.can_run_amok));
            if (attributes.HasFlag(Attributes.can_sap))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.can_sap));
            if (attributes.HasFlag(Attributes.cantabrian_circle))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.cantabrian_circle));
            if (attributes.HasFlag(Attributes.command))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.command));
            if (attributes.HasFlag(Attributes.druid))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.druid));
            if (attributes.HasFlag(Attributes.frighten_foot))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.frighten_foot));
            if (attributes.HasFlag(Attributes.frighten_mounted))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.frighten_mounted));
            if (attributes.HasFlag(Attributes.general_unit))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.general_unit));
            if (attributes.HasFlag(Attributes.general_unit_upgrade))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.general_unit_upgrade));
            if (attributes.HasFlag(Attributes.hide_anywhere))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.hide_anywhere));
            if (attributes.HasFlag(Attributes.hide_forest))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.hide_forest));
            if (attributes.HasFlag(Attributes.hide_improved_forest))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.hide_improved_forest));
            if (attributes.HasFlag(Attributes.hide_long_grass))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.hide_long_grass));
            if (attributes.HasFlag(Attributes.mercenary_unit))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.mercenary_unit));
            if (attributes.HasFlag(Attributes.no_custom))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.no_custom));
            if (attributes.HasFlag(Attributes.warcry))
                setAndTagChanged(() => unitString += lookTables.LookUpString(Attributes.warcry));


            unitString += ("\r\n");

            unitString += ("formation\t\t\t "); // write formation
            foreach (float num in formation.FormationTight)
                unitString += (num.ToString() + ", ");
            foreach (float num in formation.FormationSparse)
                unitString += (num.ToString() + ", ");
            unitString += (formation.FormationRanks + ", ");

            firstAttr = false;
            if (formation.FormationFlags.HasFlag(FormationTypes.phalanx)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.phalanx));
            if (formation.FormationFlags.HasFlag(FormationTypes.testudo)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.testudo));
            if (formation.FormationFlags.HasFlag(FormationTypes.schiltrom)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.schiltrom));
            if (formation.FormationFlags.HasFlag(FormationTypes.horde)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.horde));
            if (formation.FormationFlags.HasFlag(FormationTypes.square)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.square));
            if (formation.FormationFlags.HasFlag(FormationTypes.wedge)) setAndTagChanged(() => unitString += lookTables.LookUpString(FormationTypes.wedge));

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

            unitString += (lookTables.LookUpString(primaryWeapon.missletypeFlags) + ", ");

            foreach (int miss in primaryWeapon.Missleattri)
                unitString += (miss + ", ");

            unitString += (
                lookTables.LookUpString(primaryWeapon.WeaponFlags) + ", " +
                 lookTables.LookUpString(primaryWeapon.TechFlags) + ", " +
                lookTables.LookUpString(primaryWeapon.DamageFlags) + ", " +
                lookTables.LookUpString(primaryWeapon.SoundFlags) + ", ");

            firstAttr = false;
            foreach (float atkd in primaryWeapon.attackdelay)
            {
                setAndTagChanged(() => unitString += (atkd));
            }

            unitString += ("\r\n");

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
            if (priAttri.HasFlag(Stat_pri_attr.PA_no)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.PA_no));
            if (priAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_4)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_4));
            if (priAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (priAttri.HasFlag(Stat_pri_attr.spear_bonus_8)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_8));
            if (priAttri.HasFlag(Stat_pri_attr.thrown_ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.thrown_ap));
            if (priAttri.HasFlag(Stat_pri_attr.fire)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.fire));

            unitString += ("\r\n");

            unitString += ("stat_sec\t\t\t "); // secondary weapon
            foreach (int atk in secondaryWeapon.attack)
                unitString += (atk + ", ");


            unitString += (lookTables.LookUpString(secondaryWeapon.missletypeFlags) + ", ");
            foreach (int miss in secondaryWeapon.Missleattri)
                unitString += (miss + ", ");
            unitString += (
                lookTables.LookUpString(secondaryWeapon.WeaponFlags) + ", " +
                lookTables.LookUpString(secondaryWeapon.TechFlags) + ", " +
                lookTables.LookUpString(secondaryWeapon.DamageFlags) + ", " +
                lookTables.LookUpString(secondaryWeapon.SoundFlags) + ", ");



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
            if (secAttri.HasFlag(Stat_pri_attr.PA_no)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.PA_no));
            if (secAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_4)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_4));
            if (secAttri.HasFlag(Stat_pri_attr.ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.ap));
            if (secAttri.HasFlag(Stat_pri_attr.spear_bonus_8)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.spear_bonus_8));
            if (secAttri.HasFlag(Stat_pri_attr.thrown_ap)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.thrown_ap));
            if (secAttri.HasFlag(Stat_pri_attr.fire)) setAndTagChanged(() => unitString += lookTables.LookUpString(Stat_pri_attr.fire));

            unitString += ("\r\n");

            unitString += ("stat_pri_armour\t\t ");
            foreach (int numb in primaryArmour.stat_pri_armour)
                unitString += (numb + ", ");
            unitString += lookTables.LookUpString(primaryArmour.armour_sound);

            unitString += ("\r\n");

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

            unitString += ("ownership\t\t\t ");

            firstAttr = false;
            if (ownership.HasFlag(FactionOwnership.armenia)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.armenia));
            if (ownership.HasFlag(FactionOwnership.britons)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.britons));
            if (ownership.HasFlag(FactionOwnership.carthage)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.carthage));
            if (ownership.HasFlag(FactionOwnership.carthaginian)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.carthaginian));
            if (ownership.HasFlag(FactionOwnership.dacia)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.dacia));
            if (ownership.HasFlag(FactionOwnership.eastern)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.eastern));
            if (ownership.HasFlag(FactionOwnership.egypt)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.egypt));
            if (ownership.HasFlag(FactionOwnership.egyptian)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.egyptian));
            if (ownership.HasFlag(FactionOwnership.gauls)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.gauls));
            if (ownership.HasFlag(FactionOwnership.germans)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.germans));
            if (ownership.HasFlag(FactionOwnership.greek)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.greek));
            if (ownership.HasFlag(FactionOwnership.greek_cities)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.greek_cities));
            if (ownership.HasFlag(FactionOwnership.macedon)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.macedon));
            if (ownership.HasFlag(FactionOwnership.none)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.none));
            if (ownership.HasFlag(FactionOwnership.numidia)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.numidia));
            if (ownership.HasFlag(FactionOwnership.parthia)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.parthia));
            if (ownership.HasFlag(FactionOwnership.pontus)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.pontus));
            if (ownership.HasFlag(FactionOwnership.roman)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.roman));
            if (ownership.HasFlag(FactionOwnership.romans_brutii)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.romans_brutii));
            if (ownership.HasFlag(FactionOwnership.romans_julii)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.romans_julii));
            if (ownership.HasFlag(FactionOwnership.romans_scipii)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.romans_scipii));
            if (ownership.HasFlag(FactionOwnership.romans_senate)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.romans_senate));
            if (ownership.HasFlag(FactionOwnership.scythia)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.scythia));
            if (ownership.HasFlag(FactionOwnership.seleucid)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.seleucid));
            if (ownership.HasFlag(FactionOwnership.slave)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.slave));
            if (ownership.HasFlag(FactionOwnership.spain)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.spain));
            if (ownership.HasFlag(FactionOwnership.thrace)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.thrace));
            if (ownership.HasFlag(FactionOwnership.barbarian)) setAndTagChanged(() => unitString += lookTables.LookUpString(FactionOwnership.barbarian));

            unitString += ("\r\n\n");

            return unitString;
        }

    }

}
