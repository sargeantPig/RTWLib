using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;

namespace RTWLib.Objects
{

	public class Soldier
	{
		public string name; // name of model
		public int number, extras; // number of troops and number of extras (like dogs)
		public float collisionMass; // the collision mass

		public Soldier()
		{
		}
	}

	public class MountEffect
	{
		public List<string> mountType;
		public List<int> modifier;

		public MountEffect()
		{
			mountType = new List<string>();
			modifier = new List<int>();
		}
	}

	public class Formation
	{
		public float[] FormationTight;
		public float[] FormationSparse;
		public int FormationRanks;
		public FormationTypes FormationFlags;

		public Formation()
		{
			FormationTight = new float[2];
			FormationSparse = new float[2];
		}
	}

	public class StatWeapons
	{
		public int[] attack; //0, 1
		public MissileType missletypeFlags; // 2
		public int[] Missleattri; // 3, 4
		public WeaponType WeaponFlags; // 5
		public TechType TechFlags; // 6
		public DamageType DamageFlags; // 7
		public SoundType SoundFlags; //  8
		public float[] attackdelay; //9, 10

		public StatWeapons()
		{
			attack = new int[2];
			Missleattri = new int[2];
			attackdelay = new float[2];
		}
	}

	public class StatPriArmour
	{
		public int[] stat_pri_armour;
		public ArmourSound armour_sound;

		public StatPriArmour()
		{
			stat_pri_armour = new int[3];
		}
	}

	public class StatSecArmour
	{
		public int[] stat_sec_armour;
		public ArmourSound sec_armour_sound;

		public StatSecArmour()
		{
			stat_sec_armour = new int[2];
		}

	}

	public class Mentality
	{
		public int morale;
		public Statmental_discipline discipline;
		public Statmental_training training;

		public Mentality()
		{ }
	}

	public class Unit
	{
		public string type; //internal name
		public string dictionary; //tag for finding screen name
		public string category; // infantry, cavalry, siege, handler, ship or non_combatant
		public string unitClass; // light, heavy, spearman, or missle
		public string voiceType; //voice of the unit
		public Soldier soldier; //see soldier class
		public List<string> officer; //name of the officer model (can be up to three)
		public string engine; //type of siege used ( ballista, scorpion, onager, heavy_onager, repeating_ballista)
		public string animal; // The type of non-ridden on animals used by the  wardogs, pigs
		public string mount; // mount used by the unit
		public string naval;
		public MountEffect mountEffect; //modifiers vs different mounts
		public Attributes attributes; //abilities of unit
		public Formation formation; // formation values
		public int[] heatlh; // hitpoints and mount hitpoints
		public StatWeapons primaryWeapon;
		public Stat_pri_attr priAttri;
		public StatWeapons secondaryWeapon;
		public Stat_pri_attr secAttri;
		public StatPriArmour primaryArmour;
		public StatSecArmour secondaryArmour;
		public int heat; //fatigue suffered by units in hot climate
		public int[] ground; //
		public Mentality mental;
		public int chargeDistance;
		public int fireDelay;
		public int[] food;
		public int[] cost;
		public FactionOwnership ownership;

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

	public class CoreBuilding
	{
		public string buildingType; //eg. "core_building"
		public List<string> levels = new List<string>();
		public List<Building> buildings = new List<Building>();
		public string CBconvert_to;

		public CoreBuilding()
		{ }


		public string outputCoreBuilding()
		{
			string a = "";

			a += "building " + buildingType + "\r\n"
				+ "{" + "\r\n";

			a += Functions.Helpers_EDB.EDBTabSpacers[0] + "levels ";

			foreach (string level in levels)
			{
				a += level + " ";

			}

			a += "\r\n" + Functions.Helpers_EDB.EDBTabSpacers[0] + "{" + "\r\n";

			foreach (Building building in buildings)
			{
				a += building.outputBuilding();

			}

			a += Functions.Helpers_EDB.EDBTabSpacers[0] + "}" + "\r\n";

			a += Functions.Helpers_EDB.EDBTabSpacers[0] + "plugins" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[0] + "{" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[0] + "}" + "\r\n"
				+ "}";

			return a;
		}

	}

	public class Building
	{
		public string buildingName; //eg governors_house
		public List<string> factionsRequired = new List<string>();
		public Bcapability capability;
		public Fcapability fcapability;
		public Bconstruction construction;
		public string Bconvert_to;
		public string material;

		public Building()
		{

		}

		public Building(Building b)
		{
			buildingName = b.buildingName;
			factionsRequired = new List<string>(b.factionsRequired);
			capability = b.capability;
			construction = b.construction;
			Bconvert_to = b.Bconvert_to;
			fcapability = b.fcapability;
			material = b.material;
		}


		public string outputBuilding()
		{
			string a = "";

			a += Functions.Helpers_EDB.EDBTabSpacers[1] + buildingName + " requires factions { ";

			foreach (string faction in factionsRequired)
			{
				a += faction + ", ";
			}

			a += "}" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[1] + "{" + "\r\n";
			if (Bconvert_to != null)
				a += Functions.Helpers_EDB.EDBTabSpacers[2] + "convert_to " + Bconvert_to + "\r\n";

			a += capability.outputCapability();

			if (fcapability != null)
				a += fcapability.outputFcapa();

			if (material != null)
				a += Functions.Helpers_EDB.EDBTabSpacers[2] + "material " + material + "\r\n";

			a += construction.outputConstruction();
			a += Functions.Helpers_EDB.EDBTabSpacers[1] + "}" + "\r\n";

			return a;

		}


	}

	public class Bcapability
	{
		public List<Brecruit> canRecruit = new List<Brecruit>();
		public List<string> agentList = new List<string>();
		public List<string> effectList = new List<string>();

		public Bcapability()
		{ }

		public string outputCapability()
		{
			string a = "";

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "capability" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";

			foreach (Brecruit recruit in canRecruit)
			{
				a += recruit.outputRecruit() + "\r\n";

			}

			foreach (string agent in agentList)
			{
				a += Functions.Helpers_EDB.EDBTabSpacers[3] + agent + "\r\n";

			}

			foreach (string eff in effectList)
			{
				a += Functions.Helpers_EDB.EDBTabSpacers[3] + eff + "\r\n";
			}

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

			return a;

		}

	}

	public class Fcapability
	{
		public List<string> effectList = new List<string>();

		public Fcapability()
		{




		}

		public string outputFcapa()
		{
			string a = "";

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "faction_capability" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";


			foreach (string eff in effectList)
			{
				a += Functions.Helpers_EDB.EDBTabSpacers[3] + eff + "\r\n";
			}

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

			return a;



		}

	}

	public class Brecruit
	{
		public string name; //eg. carthaginian peasant
		public int experience;
		public int startingPoints;
		public double pointBuildingGains;
		public double maximumPoints;
		public List<string> requiresFactions = new List<string>();

		public Brecruit()
		{

		}

		public string outputRecruit()
		{
			string a = "";

			a += Functions.Helpers_EDB.EDBTabSpacers[3] + "recruit " + "\"" + name + "\"" + "  " + experience.ToString() + "  " + "requires factions " + "{ ";

			foreach (string faction in requiresFactions)
			{
				a += faction + ", ";

			}

			a += " }";

			return a;
		}


	}

	public class Bconstruction
	{
		public int turnsToBuild;
		public int cost;
		public string settlement_min;
		public List<string> upgrades = new List<string>();

		public Bconstruction()
		{


		}


		public string outputConstruction()
		{
			string a = "";

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "construction  " + turnsToBuild.ToString() + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "cost  " + cost.ToString() + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "settlement_min " + settlement_min + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "upgrades" + "\r\n"
				+ Functions.Helpers_EDB.EDBTabSpacers[2] + "{" + "\r\n";

			foreach (string upgrade in upgrades)
			{
				a += Functions.Helpers_EDB.EDBTabSpacers[3] + upgrade + "\r\n";

			}

			a += Functions.Helpers_EDB.EDBTabSpacers[2] + "}" + "\r\n";

			return a;

		}

	}

	public class Region
	{
		public string name = "";
		public string cityName = "";
		public string faction_creator = "";
		public int[] rgb = { 0, 0, 0 };
		public int x = 0, y = 0;

		public Region(string n, int[] color)
		{
			name = n;
			rgb = color;
		}

		public Region()
		{ }

		public string Output()
		{
			string output = "";

			output += name + "\r\n" +
				cityName + "\r\n\t" + x + ", " + y +
				"\r\n\t";

			foreach (int i in rgb)
			{
				output += i + ", ";

			}

			return output + "\r\n";
		}

	}

	public class Settlement
	{
		public List<DSBuilding> b_types = new List<DSBuilding>();

		string plan_set = "default_set";
		public string s_level, region, faction_creator;

		public int yearFounded, population;

		public Settlement(string level, string reg, string creator, List<DSBuilding> buildings, int yrFounded, int pop)
		{
			s_level = level;
			region = reg;
			faction_creator = creator;
			yearFounded = yrFounded;
			population = pop;
			b_types = buildings;


		}

		public Settlement(Settlement s)
		{
			s_level = s.s_level;
			region = s.region;
			faction_creator = s.faction_creator;
			yearFounded = s.yearFounded;
			population = s.population;
			b_types = s.b_types;


		}

		public Settlement()
		{ }

		public string outputSettlement()
		{

			string settlement =
				"\r\nsettlement" +
				"\r\n{" +
				"\r\n\t" + "level " + s_level +
				"\r\n\t" + "region " + region +
				"\r\n\t" + "year_founded " + Convert.ToString(yearFounded) +
				"\r\n\t" + "population " + Convert.ToString(population) +
				"\r\n\t" + "plan_set " + plan_set +
				"\r\n\t" + "faction_creator " + faction_creator;

			foreach (DSBuilding b in b_types)
			{
				settlement += b.Output();
			}

			settlement += "\r\n}";

			return settlement;

		}



	}

	public class UnitFaction
	{
		public string dicName;
		public List<string> factions = new List<string>();

		public UnitFaction()
		{ }

		public UnitFaction(UnitFaction uf)
		{
			dicName = uf.dicName;

			factions = new List<string>(uf.factions);
		}


	}

	public class Character
	{
		public string info;
		public string spacer = "  ->>>>  ";
		public int x;
		public int y;

		public Character(string line)
		{
			info = line;
		}

	}

	public class DSCharacter
	{
		public string sub_faction, name, type, rank, traits, ancillaries;
		public int age;
		public int[] coords = new int[2];
		public List<DSUnit> army = new List<DSUnit>();
		
		public DSCharacter()
		{
		}

		public DSCharacter(string Name, Random rnd)
		{
			name = Name;
			type = "general";
			age = rnd.Next(20, 50);
		}

		public DSCharacter(DSCharacter character)
		{
			sub_faction = character.sub_faction;
			name = character.name;
			type = character.type;
			rank = character.rank;
			traits = character.traits;
			ancillaries = character.ancillaries;
			age = character.age;
			coords[0] = character.coords[0];
			coords[1] = character.coords[1];
			army = new List<DSUnit>(character.army);
		}

		public string Output()
		{
			string output = "";

			output +=
				"character\t" + name + ", " + type;


			if (type == "spy" || type == "diplomat" || rank == null || type == "admiral")
			{
				output += ", age " + age.ToString() + ", , x " + coords[0].ToString() + ", y " + coords[1].ToString() + "\r\n";

				if(traits != null && traits != "")
					output += "traits " + traits + "\r\n";

				if (ancillaries != null && ancillaries != "")
					output += "ancillaries " + ancillaries + "\r\n";

				if (type == "admiral" || type == "named character" || type == "general")
				{
					output += "army\r\n";
					foreach (DSUnit str in army)
					{
						output += str.Output();
					}
				}

				return output;
			}
			output += ", " + rank + ", age " + age.ToString() + ", , x " + coords[0].ToString() + ", y " + coords[1].ToString() + "\r\n";

			

			output+= "traits " + traits + "\r\n" +
				"ancillaries " + ancillaries + "\r\n" +
				"army\r\n";

			foreach (DSUnit str in army)
			{
				output += str.Output();
			}

			return output;

		}



	}

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

	public class DSBuilding
	{
		public string type;
		public string name;

		public DSBuilding()
		{ }

		public DSBuilding(DSBuilding dS)
		{
			type = dS.type;
			name = dS.name;
		}

		public string Output()
		{
			return "\r\n\t" + "building" +
					"\r\n\t" + "{" +
					"\r\n\t\ttype " + type + " " + name + "\r\n" +
				"\r\n\t" + "}\r\n";
		}
	}

	public class CharacterRecord
	{
		public string name;
		public string gender;
		public int command;
		public int influence;
		public int management;
		public int subterfuge;
		public int age;
		public string status;
		public string leader;

		public CharacterRecord(CharacterRecord cr)
		{
			name = cr.name;
			gender = cr.gender;
			command = cr.command;
			influence = cr.influence;
			management = cr.management;
			subterfuge = cr.subterfuge;
			age = cr.age;
			status = cr.status;
			leader = cr.leader;
		}

		public CharacterRecord()
		{ }

		public string Output()
		{
			return "character_record\t\t" + name + ", \t" + gender +  ", command " + command.ToString() +  ", influence " +  influence.ToString() + ", management " + management.ToString() +
				", subterfuge " + subterfuge.ToString() + ", age " + age.ToString() + ", " + status + ", " + leader + " " + "\r\n";

		}

	}

	public class Faction
	{
		public string name { get; set; }
		public string[] ai = new string[2];
		public string superFaction { get; set; }
		public List<Settlement> settlements = new List<Settlement>();
		public List<DSCharacter> characters = new List<DSCharacter>();
		public List<CharacterRecord> characterRecords = new List<CharacterRecord>();
		public List<string> relatives = new List<string>();
		public int denari { get; set; }


		public Faction()
		{ }

		public Faction(Faction faction)
		{
			name = faction.name;
			ai[0] = faction.ai[0];
			ai[1] = faction.ai[1];
			superFaction = faction.superFaction;
			settlements = new List<Settlement>(faction.settlements);
			characters = new List<DSCharacter>(faction.characters);
			characterRecords = new List<CharacterRecord>(faction.characterRecords);
			relatives = new List<string>(faction.relatives);
			denari = faction.denari;


		}

		public void Clear()
		{
			name = "";
			ai[0] = "";
			ai[1] = "";
			superFaction = "";
			settlements.Clear();
			characters.Clear();
			characterRecords.Clear();
			relatives.Clear();
		}

		public string Output()
		{
			string output = "";

			output +=
				"faction\t" + name + ", " + ai[0] + " " + ai[1] + "\r\n" +
				"superfaction " + superFaction + "\r\n" +
				"denari\t" + denari.ToString() + "\r\n";

			foreach (Settlement settlement in settlements)
			{
				output += settlement.outputSettlement();

				output += "\r\n";

			}

			output += "\r\n";

			foreach (DSCharacter character in characters)
			{
				output += character.Output();
				output += "\r\n";
			}

			foreach (CharacterRecord rec in characterRecords)
			{
				output += rec.Output();
			}

			foreach (string rel in relatives)
			{
				output += "relative \t" + rel  + "\r\n";
			}

			return output;
		}
	}

	public class Resource
	{
		string name = "";
		int[] coordinates = new int[2];

		public Resource(string Rname, int[] coords)
		{
			name = Rname;
			coordinates = coords;

		}

		public string Output()
		{
			string output = "";

			output += "resource\t" + name + ",\t" + coordinates[0].ToString() + ",\t" + coordinates[1] + "\r\n";

			return output;
		}

	}

	public class Landmark
	{
		string name = "";
		int[] coordinates = new int[2];

		public Landmark(string Lname, int[] coords)
		{
			name = Lname;
			coordinates = coords;
		}

		public string Output()
		{
			string output = "";

			output += "landmark\t" + name + ",\t" + coordinates[0].ToString() + ",\t" + coordinates[1] + "\r\n";

			return output;
		}


	}

	public static class FactionRosters
	{
		static public Dictionary<FactionOwnership, List<Unit>> RTWrosters = new Dictionary<FactionOwnership, List<Unit>>();

		static public void AddUnitsToFaction(List<Unit> units, FactionOwnership faction)
		{
			RTWrosters[faction] = units;
		}

		static public void AddFactionKey(FactionOwnership faction)
		{
			if (!RTWrosters.ContainsKey(faction))
				RTWrosters.Add(faction, new List<Unit>());
		}
	}


	
	
}
