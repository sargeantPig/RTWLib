using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTWLib.Objects;
using RTWLib.Logger;
using RTWLib.Functions;
using RTWLib.Data;

namespace RTWLib.Functions
{
	public class EDU : FileBase, IFile
	{
		public List<Unit> units = new List<Unit>();
		public static FileScheme edu_scheme = new FileScheme();

		public EDU(bool log_on) 
			: base(FileNames.export_descr_unit, @"data\export_descr_unit.txt", "Unit details and stats")
		{
            is_on = log_on;
			edu_scheme.Add("soldier", "name", 0);
			edu_scheme.Add("soldier", "number", 1);
			edu_scheme.Add("soldier", "extras", 2);
			edu_scheme.Add("soldier", "collisionMass", 3);
			edu_scheme.Add("formation", "tight_side", 0);
			edu_scheme.Add("formation", "tight_back", 1);
			edu_scheme.Add("formation", "sparse_side", 0);
			edu_scheme.Add("formation", "sparse_back", 1);
			edu_scheme.Add("stat_pri_armour", "armour", 0);
			edu_scheme.Add("stat_pri_armour", "defence", 1);
			edu_scheme.Add("stat_pri_armour", "shield", 2);
			edu_scheme.Add("stat_pri_armour", "sound", 3);
			edu_scheme.Add("stat_pri", "attack", 0);
			edu_scheme.Add("stat_pri", "chargeBonus", 1);
			edu_scheme.Add("stat_pri", "missileType", 2);
			edu_scheme.Add("stat_pri", "missileRange", 3);
			edu_scheme.Add("stat_pri", "missileAmmo", 4);
			edu_scheme.Add("stat_pri", "weaponFlag", 5);
			edu_scheme.Add("stat_pri", "techFlag", 6);
			edu_scheme.Add("stat_pri", "damageType", 7);
			edu_scheme.Add("stat_pri", "soundType", 8);
			edu_scheme.Add("stat_pri", "attkDelay1", 9);
			edu_scheme.Add("stat_pri", "attkDelay2", 10);
			edu_scheme.Add("stat_sec", "attack", 0);
			edu_scheme.Add("stat_sec", "chargeBonus", 1);
			edu_scheme.Add("stat_sec", "missileType", 2);
			edu_scheme.Add("stat_sec", "missileRange", 3);
			edu_scheme.Add("stat_sec", "missileAmmo", 4);
			edu_scheme.Add("stat_sec", "weaponFlag", 5);
			edu_scheme.Add("stat_sec", "techFlag", 6);
			edu_scheme.Add("stat_sec", "damageType", 7);
			edu_scheme.Add("stat_sec", "soundType", 8);
			edu_scheme.Add("stat_sec", "attkDelay1", 9);
			edu_scheme.Add("stat_sec", "attkDelay2", 10);
			edu_scheme.Add("stat_sec_armour", "armour", 0);
			edu_scheme.Add("stat_sec_armour", "defence", 1);
			edu_scheme.Add("stat_sec_armour", "sound", 2);
			edu_scheme.Add("stat_ground", "scrub", 0);
			edu_scheme.Add("stat_ground", "sand", 1);
			edu_scheme.Add("stat_ground", "forest", 2);
			edu_scheme.Add("stat_ground", "snow", 3);
			edu_scheme.Add("stat_mental", "morale", 0);
			edu_scheme.Add("stat_mental", "discipline", 1);
			edu_scheme.Add("stat_mental", "training", 2);
			edu_scheme.Add("stat_cost", "turns", 0);
			edu_scheme.Add("stat_cost", "construct", 1);
			edu_scheme.Add("stat_cost", "upkeep", 2);
			edu_scheme.Add("stat_cost", "weaponUpgrade", 3);
			edu_scheme.Add("stat_cost", "armourUpgrade", 4);
			edu_scheme.Add("stat_cost", "custom", 5);
		}

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			if (!FileCheck(paths[0]))
				DisplayLogExit();

			LookUpTables lookUp = new LookUpTables();
			units.Clear();
			string txt_Output = "";
			string line = "";
			string trimmed = "";
			int counter = -1;

			StreamReader edu = new StreamReader(paths[0]);

			Action<Action> trimApply = (action) =>
			{
				trimmed = Functions_General.RemoveFirstWord(line);
				trimmed = trimmed.Trim();

				action();
			};

			lineNumber = 0;
			currentLine = "";
			while ((line = edu.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				if (line.StartsWith("type"))
				{
                    if (counter >= 0)
                    {
                        PLog("Loaded -- " + units[counter].dictionary);
                    }

                    counter++;
					units.Add(new Unit());
					trimApply(() => units[counter].type = trimmed);
				}

				else if (line.StartsWith("dictionary"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].dictionary = trimmed;
					});
				}

				else if (line.StartsWith("category"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].category = trimmed;
					});
				}

				else if (line.StartsWith("class"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].unitClass = trimmed;
					});
				}

				else if (line.StartsWith("voice_type"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].voiceType = trimmed;
					});
				}

				else if (line.StartsWith("soldier"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(',');
						units[counter].soldier.name = splitted[0].Trim();
						units[counter].soldier.number = Convert.ToInt32(splitted[1].Trim());
						units[counter].soldier.extras = Convert.ToInt32(splitted[2].Trim());
						float cm = splitted[3].UniversalParse();
						units[counter].soldier.collisionMass = (float)cm;
						// PLog("Error - unable to parse " + splitted[3].ToString());
							

					});
				}

				else if (line.StartsWith("officer"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].officer.Add(trimmed);
					});
				}

				else if (line.StartsWith("engine"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].engine = trimmed;
					});
				}

				else if (line.StartsWith("animal"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].animal = trimmed;
					});
				}

				else if (line.StartsWith("mount_effect"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						string newCombined = "";

						foreach (string full in splitted)
						{
							newCombined += full;
						}

						string[] reSplit = newCombined.Split(' ');

						int i = 0;
						foreach (string STRING in reSplit)
						{

							units[counter].mountEffect.mountType.Add(reSplit[i]);
							i++;
							units[counter].mountEffect.modifier.Add(Convert.ToInt32(reSplit[i]));
							i++;

							if (i > splitted.Count())
								break;

						}
					});
				}

				else if (line.StartsWith("mount"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].mount = trimmed;
					});
				}

				else if (line.StartsWith("ship"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(';');
						trimmed = splitted[0].TrimEnd();
						units[counter].naval = trimmed;
					});
				}


				else if (line.StartsWith("attributes"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');
						
						foreach (string STRING in splitted)
						{
							if (STRING != " ")
							{ 
								trimmed = STRING.Trim();
								object obj;
								if((obj = lookUp.LookUpKey<Attributes>(trimmed)) != null)
									units[counter].attributes |= lookUp.LookUpKey<Attributes>(trimmed);
							}
						}
					});
				}

				else if (line.StartsWith("formation"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						int i = 0;
						int a = 0;
						int b = 0;

						foreach (string STRING in splitted)
						{
							if (i < 2)
							{
								units[counter].formation.FormationTight[i] = STRING.Trim().UniversalParse();
							}
							else if (a < 2)
							{
								units[counter].formation.FormationSparse[a] = STRING.Trim().UniversalParse();
								a++;
							}
							else if (b < 1)
							{
								units[counter].formation.FormationRanks = Convert.ToInt32(STRING.Trim());
								edu_scheme.Add("formation", "ranks", 0);
								b++;
							}

							dynamic ft = lookUp.LookUpKey<FormationTypes>(STRING.Trim());

							if (ft != null)
								units[counter].formation.FormationFlags |= ft;

							i++;
						}

					});
				}

				else if (line.StartsWith("stat_health"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');
						units[counter].heatlh[0] = Convert.ToInt32(splitted[0]);
						units[counter].heatlh[1] = Convert.ToInt32(splitted[1]);
						edu_scheme.Add("health", "soldier", 0);
						edu_scheme.Add("formation", "companion", 1);
					});
				}

				else if (line.StartsWith("stat_pri_attr"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						foreach (string STRING in splitted)
						{
							if(STRING.ToCharArray().Count() >0)
								units[counter].priAttri |= lookUp.LookUpKey<Stat_pri_attr>(STRING.Trim());
						}
					});
				}

				else if (line.StartsWith("stat_pri_armour"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].primaryArmour.stat_pri_armour[0] = Convert.ToInt32(splitted[0]);
						units[counter].primaryArmour.stat_pri_armour[1] = Convert.ToInt32(splitted[1]);
						units[counter].primaryArmour.stat_pri_armour[2] = Convert.ToInt32(splitted[2]);
						units[counter].primaryArmour.armour_sound = lookUp.LookUpKey<ArmourSound>(splitted[3].Trim());
					});
				}

				else if (line.StartsWith("stat_pri"))
				{
					trimApply(() => {

						string[] splitted = trimmed.Split(',', ';');

						trimmed = splitted[0].Trim();

						units[counter].primaryWeapon.attack[0] = Convert.ToInt32(splitted[0]);
						units[counter].primaryWeapon.attack[1] = Convert.ToInt32(splitted[1]);
						units[counter].primaryWeapon.Missleattri[0] = Convert.ToInt32(splitted[3]);
						units[counter].primaryWeapon.Missleattri[1] = Convert.ToInt32(splitted[4]);
						units[counter].primaryWeapon.attackdelay[0] = splitted[9].UniversalParse();
						units[counter].primaryWeapon.attackdelay[1] = splitted[10].UniversalParse();

						units[counter].primaryWeapon.missletypeFlags = lookUp.LookUpKey<MissileType>(splitted[2].Trim());
						units[counter].primaryWeapon.WeaponFlags = lookUp.LookUpKey<WeaponType>(splitted[5].Trim());
						units[counter].primaryWeapon.TechFlags = lookUp.LookUpKey<TechType>(splitted[6].Trim());
						units[counter].primaryWeapon.DamageFlags = lookUp.LookUpKey<DamageType>(splitted[7].Trim());
						units[counter].primaryWeapon.SoundFlags = lookUp.LookUpKey<SoundType>(splitted[8].Trim());
					});
				}

				else if (line.StartsWith("stat_sec_attr"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						foreach (string STRING in splitted)
						{
							units[counter].secAttri |= lookUp.LookUpKey<Stat_pri_attr>(STRING.Trim());
						}
					});
				}

				else if (line.StartsWith("stat_sec_armour"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].secondaryArmour.stat_sec_armour[0] = Convert.ToInt32(splitted[0]);
						units[counter].secondaryArmour.stat_sec_armour[1] = Convert.ToInt32(splitted[1]);
						units[counter].secondaryArmour.sec_armour_sound = lookUp.LookUpKey<ArmourSound>(splitted[2].Trim());
					});
				}

				else if (line.StartsWith("stat_sec"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].secondaryWeapon.attack[0] = Convert.ToInt32(splitted[0]);
						units[counter].secondaryWeapon.attack[1] = Convert.ToInt32(splitted[1]);

						units[counter].secondaryWeapon.Missleattri[0] = Convert.ToInt32(splitted[3]);
						units[counter].secondaryWeapon.Missleattri[1] = Convert.ToInt32(splitted[4]);

						units[counter].secondaryWeapon.attackdelay[0] = splitted[9].UniversalParse();
						units[counter].secondaryWeapon.attackdelay[1] = splitted[10].UniversalParse();
						units[counter].secondaryWeapon.missletypeFlags = lookUp.LookUpKey<MissileType>(splitted[2].Trim());
						units[counter].secondaryWeapon.WeaponFlags = lookUp.LookUpKey<WeaponType>(splitted[5].Trim());
						units[counter].secondaryWeapon.TechFlags = lookUp.LookUpKey<TechType>(splitted[6].Trim());
						units[counter].secondaryWeapon.DamageFlags = lookUp.LookUpKey<DamageType>(splitted[7].Trim());
						units[counter].secondaryWeapon.SoundFlags = lookUp.LookUpKey<SoundType>(splitted[8].Trim());
					});
				}

				else if (line.StartsWith("stat_heat"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].heat = Convert.ToInt32(trimmed);
					});
				}

				else if (line.StartsWith("stat_ground"))
				{
					trimApply(() => {

						string[] splitted = trimmed.Split(',', ';');

						trimmed = splitted[0].TrimEnd();

						units[counter].ground[0] = Convert.ToInt32(splitted[0]);
						units[counter].ground[1] = Convert.ToInt32(splitted[1]);
						units[counter].ground[2] = Convert.ToInt32(splitted[2]);
						units[counter].ground[3] = Convert.ToInt32(splitted[3]);
					});
				}

				else if (line.StartsWith("stat_mental"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].mental.morale = Convert.ToInt32(splitted[0]);
						units[counter].mental.discipline = lookUp.LookUpKey<Statmental_discipline>(splitted[1].Trim());
						units[counter].mental.training = lookUp.LookUpKey<Statmental_training>(splitted[2].Trim());
					});
				}

				else if (line.StartsWith("stat_charge_dist"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].chargeDistance = Convert.ToInt32(trimmed);
					});
				}

				else if (line.StartsWith("stat_fire_delay"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].fireDelay = Convert.ToInt32(trimmed);


					});
				}

				else if (line.StartsWith("stat_food"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].food[0] = Convert.ToInt32(splitted[0]);
						units[counter].food[1] = Convert.ToInt32(splitted[1]);
					});
				}

				else if (line.StartsWith("stat_cost"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						int i = 0;
						foreach (string STRING in splitted)
						{
							string trim = STRING.TrimStart();
							if (trim.Contains(" "))
							{
								string[] split = STRING.Trim().Split(' ');
								units[counter].cost[i] = Convert.ToInt32(split[0].Trim());
							}
							else
							{
								units[counter].cost[i] = Convert.ToInt32(STRING);
								i++;
							}
						}
					});
				}

				else if (line.StartsWith("ownership"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						foreach (string STRING in splitted)
						{
							var a = lookUp.LookUpKey<FactionOwnership>(STRING.Trim());

							if (a != null)
								units[counter].ownership |= a;
							else units[counter].ownership |= lookUp.LookUpKey<FactionOwnership>(STRING.Trim());

						}


					});
				}
			}

			txt_Output += ("\n" + units.Count + "Units loaded from EDU");

			edu.Close();

		}
		override public string Output()
		{
			string output = "";

			foreach (Unit unit in units)
			{
				output += unit.unitOutput();
			}

			return output;
		}
		public Unit FindUnit(string name)
		{
			Unit unit = units.Find(x => x.dictionary.Contains(name));

			if (unit == null)
			{
				unit = units.Find(x => x.type.Contains(name));
			}

			return unit;
		}
		public List<Unit> FindUnits(string name)
		{
			List<Unit> unit = units.FindAll(x => x.dictionary.Contains(name));
			return unit;
		}
		public List<Unit> FindUnitsByFaction(string faction)
		{
			LookUpTables lut = new LookUpTables();

			try
			{
				string[] removaltags = new string[] {
					"non_combatant",
					"ship",
				};

				FactionOwnership f = lut.LookUpKey<FactionOwnership>(faction);
				Cultures fa = lut.LookUpKey<Cultures>(faction);
				FactionOwnership fb = lut.LookUpKey<FactionOwnership>(fa.ToString());

				List<Unit> unit = units.FindAll(x => x.ownership.HasFlag(f) || x.ownership.HasFlag(fb));
				List<Unit> removeUnit = new List<Unit>();
				foreach (Unit u in unit)
				{
					foreach (string rem in removaltags)
					{
						if (u.category == rem)
						{
							removeUnit.Add(u);
							break;
						}
					}
				}
				foreach (Unit u in removeUnit)
				{
					unit.Remove(u);
				}

				return unit;
			}
			catch(Exception ex)
			{
				Output(ex.ToString());
				Output("\r\nFaction is likely wrong");
			}
			return new List<Unit>();
		}
		public List<Unit> FindUnitsByArgAndFaction(string[] args, string faction = null, bool useFaction = false)
		{
			List<Unit> selection = new List<Unit>();
			bool added = false;
			bool isFaction = false;
			foreach (Unit unit in units)
			{
				if (useFaction)
				{
					isFaction = CheckUnitBelongsToFactionAndCulture(faction, unit);
					if (!isFaction)
						continue;

				}

				foreach (string arg in args)
				{

					if (arg == "All")
					{
						selection.Add(unit);
						added = true;
					}
					if (unit.category == arg && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "light infantry" && unit.category == "infantry" && unit.unitClass == "light" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "heavy infantry" && unit.category == "infantry" && unit.unitClass == "heavy" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "light cavalry" && unit.category == "cavalry" && unit.unitClass == "light" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "heavy cavalry" && unit.category == "cavalry" && unit.unitClass == "heavy" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile cavalry" && unit.category == "cavalry" && unit.unitClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile infantry" && unit.category == "infantry" && unit.unitClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "spearmen" && unit.unitClass == "spearmen" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile" && unit.unitClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "general" && (unit.attributes.HasFlag(Attributes.general_unit) || unit.attributes.HasFlag(Attributes.general_unit_upgrade)) && !added)
					{
						selection.Add(unit);
						added = true;
					}
				}

				added = false;
				
			}

			return selection;
		}
		public bool CheckUnitBelongsToFactionAndCulture(string faction, Unit unit)
		{
			LookUpTables lut = new LookUpTables();
			FactionOwnership f = lut.LookUpKey<FactionOwnership>(faction);
			Cultures fa = lut.LookUpKey<Cultures>(faction);
			FactionOwnership fb = lut.LookUpKey<FactionOwnership>(fa.ToString());

			if (unit.ownership.HasFlag(f) || unit.ownership.HasFlag(fb))
				return true;
			else return false;

		}
		public string[] GetUnitNameList()
		{
			string[] lstUnit = new string[units.Count];
			int ind = 0;
			foreach (Unit unit in units)
			{
				lstUnit[ind] = unit.dictionary;
				ind++;
			}
			return lstUnit;
		}

		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
		}
	}


	public class FileScheme //helps find the locations of specific values like attack within a file
	{
		Dictionary<string, Dictionary<string, int>> scheme;

		public FileScheme()
		{
			scheme = new Dictionary<string, Dictionary<string, int>>();
		}

		public void Add(string identifier, string component, int index)
		{
			if (!scheme.ContainsKey(identifier))
				scheme.Add(identifier, new Dictionary<string, int>() { { component, index } });
			else
			{
				if (!scheme[identifier].ContainsKey(component))
					scheme[identifier].Add(component, index);
			}
		}

		public int GetComponentIndex(string component)
		{
			foreach (KeyValuePair<string, Dictionary<string, int>> comp in scheme)
			{
				if (comp.Value.ContainsKey(component))
				{
					return comp.Value[component];
				}
			}
			return -1;
		}

		public Dictionary<string, int> GetComponents(string identifier)
		{
			if (scheme.ContainsKey(identifier))
				return scheme[identifier];
			else return null;
		}

		public Dictionary<string, Dictionary<string, int>> Scheme
		{
			get { return scheme; }
		}
	}

}
