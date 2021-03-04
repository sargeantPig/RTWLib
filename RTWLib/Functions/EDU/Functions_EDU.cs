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
using RTWLib.Medieval2;

namespace RTWLib.Functions.EDU
{
	public partial class EDU : FileBase, IFile
	{
		public List<Unit> units;

		Dictionary<int, string> fileComments = new Dictionary<int, string>();

		public EDU(bool log_on) 
			: base(FileNames.export_descr_unit, @"data\export_descr_unit.txt", "Unit details and stats")
		{
            is_on = log_on;
			units = new List<Unit>();

		}

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			lineNumber = 0;  
			currentLine = "";   
			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}
			LookUpTables lookUp = new LookUpTables();
			units.Clear();
			string txt_Output = "";
			string line = "";
			int counter = -1;

			StreamReader edu = new StreamReader(paths[0]);

			while ((line = edu.ReadLine()) != null)
			{
				KeyValuePair<EDULineEnums, object> comment;
				currentLine = line;
				lineNumber++;
				ParseLine(line, ref counter, lineNumber, out comment);
				if (counter > -1 && comment.Value != null)
				{
					if (units[counter].comments.ContainsKey(comment.Key))
					{
						if (units[counter].comments[comment.Key] is List<string>)
						{
							((List<string>)units[counter].comments[comment.Key]).Add((string)comment.Value);
						}
						else
						{
							string temp = (string)units[counter].comments[comment.Key];
							units[counter].comments[comment.Key] = new List<string>() { temp, (string)comment.Value };
						}						
					}
					else units[counter].comments.Add(comment.Key, comment.Value);
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
			Unit unit = units.Find(x => x.dic == name);

			if (unit == null)
			{
				unit = units.Find(x => x.type == name);
			}

			return unit;
		}
		public List<Unit> FindUnits(string name)
		{
			List<Unit> unit = units.FindAll(x => x.dic.Contains(name));
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

				//TODO add culture check

				List<Unit> unit = units.FindAll(x => x.ownership.Contains(faction));
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
		public List<Unit> FindUnitsByArgAndFaction(string[] args, string faction = null, bool useFaction = false, bool ignorePeasants = true)
		{
			List<Unit> selection = new List<Unit>();
			bool added = false;
			bool isFaction = false;
			foreach (Unit unit in units)
			{
				if (unit.dic.Contains("peasant") && ignorePeasants)
					continue;

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

					if (arg == "light infantry" && unit.category == "infantry" && unit.uClass == "light" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "heavy infantry" && unit.category == "infantry" && unit.uClass == "heavy" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "light cavalry" && unit.category == "cavalry" && unit.uClass == "light" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "heavy cavalry" && unit.category == "cavalry" && unit.uClass == "heavy" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile cavalry" && unit.category == "cavalry" && unit.uClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile infantry" && unit.category == "infantry" && unit.uClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "spearmen" && unit.uClass == "spearmen" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "missile" && unit.uClass == "missile" && !added)
					{
						selection.Add(unit);
						added = true;
					}

					if (arg == "handler" && unit.category == "handler" && !added)
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

			//TODO Add check against culture

			if (unit.ownership.Contains(faction))
				return true;
			else return false;

		}

		/// <summary>
		/// Get min, mid, max and total of unit points from list supplied
		/// </summary>
		/// <returns>
		/// float[4] min, mid, max, total 
		/// </returns>
		public static float[] GetUnitsPointData(List<Unit> units)
		{
			float min = float.MaxValue;
			float mid = 0;
			float max = 0;
			float total = 0;
			foreach (Unit u in units)
			{
				total += u.pointValue;
				if (min > u.pointValue)
					min = u.pointValue;
				if (max < u.pointValue)
					max = u.pointValue;
			}

			mid = (max - min) / units.Count;

			return new float[4] {min, mid, max, total};
		}

		public string[] GetUnitNameList()
		{
			string[] lstUnit = new string[units.Count];
			int ind = 0;
			foreach (Unit unit in units)
			{
				lstUnit[ind] = unit.dic;
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

}
