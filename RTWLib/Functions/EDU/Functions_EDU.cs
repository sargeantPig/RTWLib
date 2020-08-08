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
	public partial class EDU : FileBase, IFile
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
			int counter = -1;

			StreamReader edu = new StreamReader(paths[0]);

			lineNumber = 0;
			currentLine = "";
			while ((line = edu.ReadLine()) != null)
			{
				currentLine = line;
				lineNumber++;
				ParseLine(line, ref counter);
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
