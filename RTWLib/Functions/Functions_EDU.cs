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
	public class EDU : Logger.Logger, IFile, ICloneable
	{
        FileNames name = FileNames.export_descr_unit;
		public const string FILEPATH = @"randomiser\data\export_descr_unit.txt";
		const string DESCRIPTION = "Units";
		public List<Unit> units = new List<Unit>();

		public EDU(bool log_on)
		{
            is_on = log_on;
        }

		public EDU(EDU edu)
		{
			units = new List<Unit>(edu.units);
            
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}

		public void Parse(string[] paths)
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

			while ((line = edu.ReadLine()) != null)
			{
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
						units[counter].soldier.number = Convert.ToInt16(splitted[1].Trim());
						units[counter].soldier.extras = Convert.ToInt16(splitted[2].Trim());
						units[counter].soldier.collisionMass = (float)Convert.ToDouble(splitted[3].Trim());
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
							units[counter].mountEffect.modifier.Add(Convert.ToInt16(reSplit[i]));
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
							trimmed = STRING.Trim();
							units[counter].attributes |= lookUp.LookUpKey<Attributes>(trimmed);
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
								units[counter].formation.FormationTight[i] = (float)Convert.ToDouble(STRING.Trim());
							else if (a < 2)
							{
								units[counter].formation.FormationSparse[a] = (float)Convert.ToDouble(STRING.Trim());
								a++;
							}
							else if (b < 1)
							{
								units[counter].formation.FormationRanks = Convert.ToInt16(STRING.Trim());
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
						units[counter].heatlh[0] = Convert.ToInt16(splitted[0]);
						units[counter].heatlh[1] = Convert.ToInt16(splitted[1]);
					});
				}

				else if (line.StartsWith("stat_pri_attr"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						foreach (string STRING in splitted)
						{
							units[counter].priAttri |= lookUp.LookUpKey<Stat_pri_attr>(STRING.Trim());
						}
					});
				}

				else if (line.StartsWith("stat_pri_armour"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].primaryArmour.stat_pri_armour[0] = Convert.ToInt16(splitted[0]);
						units[counter].primaryArmour.stat_pri_armour[1] = Convert.ToInt16(splitted[1]);
						units[counter].primaryArmour.stat_pri_armour[2] = Convert.ToInt16(splitted[2]);

						units[counter].primaryArmour.armour_sound = lookUp.LookUpKey<ArmourSound>(splitted[3].Trim());
					});
				}

				else if (line.StartsWith("stat_pri"))
				{
					trimApply(() => {

						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].primaryWeapon.attack[0] = Convert.ToInt16(splitted[0]);
						units[counter].primaryWeapon.attack[1] = Convert.ToInt16(splitted[1]);
						units[counter].primaryWeapon.Missleattri[0] = Convert.ToInt16(splitted[3]);
						units[counter].primaryWeapon.Missleattri[1] = Convert.ToInt16(splitted[4]);
						units[counter].primaryWeapon.attackdelay[0] = (float)Convert.ToDouble(splitted[9]);
						units[counter].primaryWeapon.attackdelay[1] = (float)Convert.ToDouble(splitted[10]);

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

						units[counter].secondaryArmour.stat_sec_armour[0] = Convert.ToInt16(splitted[0]);
						units[counter].secondaryArmour.stat_sec_armour[1] = Convert.ToInt16(splitted[1]);
						units[counter].secondaryArmour.sec_armour_sound = lookUp.LookUpKey<ArmourSound>(splitted[2].Trim());
					});
				}

				else if (line.StartsWith("stat_sec"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].secondaryWeapon.attack[0] = Convert.ToInt16(splitted[0]);
						units[counter].secondaryWeapon.attack[1] = Convert.ToInt16(splitted[1]);

						units[counter].secondaryWeapon.Missleattri[0] = Convert.ToInt16(splitted[3]);
						units[counter].secondaryWeapon.Missleattri[1] = Convert.ToInt16(splitted[4]);

						units[counter].secondaryWeapon.attackdelay[0] = (float)Convert.ToDouble(splitted[9]);
						units[counter].secondaryWeapon.attackdelay[1] = (float)Convert.ToDouble(splitted[10]);
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

						units[counter].heat = Convert.ToInt16(trimmed);
					});
				}

				else if (line.StartsWith("stat_ground"))
				{
					trimApply(() => {

						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].ground[0] = Convert.ToInt16(splitted[0]);
						units[counter].ground[1] = Convert.ToInt16(splitted[1]);
						units[counter].ground[2] = Convert.ToInt16(splitted[2]);
						units[counter].ground[3] = Convert.ToInt16(splitted[3]);
					});
				}

				else if (line.StartsWith("stat_mental"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].mental.morale = Convert.ToInt16(splitted[0]);
						units[counter].mental.discipline = lookUp.LookUpKey<Statmental_discipline>(splitted[1].Trim());
						units[counter].mental.training = lookUp.LookUpKey<Statmental_training>(splitted[2].Trim());

					});
				}

				else if (line.StartsWith("stat_charge_dist"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].chargeDistance = Convert.ToInt16(trimmed);
					});
				}

				else if (line.StartsWith("stat_fire_delay"))
				{
					trimApply(() => {
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].fireDelay = Convert.ToInt16(trimmed);


					});
				}

				else if (line.StartsWith("stat_food"))
				{
					trimApply(() =>
					{
						string[] splitted = trimmed.Split(',');

						trimmed = splitted[0].TrimEnd();

						units[counter].food[0] = Convert.ToInt16(splitted[0]);
						units[counter].food[1] = Convert.ToInt16(splitted[1]);
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

							units[counter].cost[i] = Convert.ToInt16(STRING);
							i++;
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

		public Unit FindUnit(string name)
		{
			Unit unit = units.Find(x => x.dictionary.Contains(name));
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
		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public string Output()
		{
			string output = "";

			foreach (Unit unit in units)
			{
				output += unit.unitOutput();
			}

			return output;
		}
        public FileNames Name
        {
            get { return name; }
        }
        public string FilePath
		{
			get { return FILEPATH; }
		}

		public Object Clone()
		{
			return new EDU(this);
		}
	}
}
