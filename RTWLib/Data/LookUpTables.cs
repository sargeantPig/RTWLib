using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RTWLib.Objects.Descr_strat;

namespace RTWLib.Data
{
	public class LookUpTables
	{
		static public Dictionary<FactionOwnership, string> dic_factions = new Dictionary<FactionOwnership, string>()
		{
			{FactionOwnership.armenia, "armenia"},
			{FactionOwnership.britons, "britons" },
			{FactionOwnership.carthage, "carthage" },
			{FactionOwnership.dacia, "dacia" },
			{FactionOwnership.eastern, "eastern" },
			{FactionOwnership.egypt, "egypt" },
			{FactionOwnership.gauls, "gauls" },
			{FactionOwnership.germans, "germans" },
			{FactionOwnership.greek_cities, "greek_cities" },
			{FactionOwnership.macedon, "macedon" },
			{FactionOwnership.numidia, "numidia" },
			{FactionOwnership.parthia, "parthia" },
			{FactionOwnership.pontus, "pontus" },
			{FactionOwnership.romans_brutii, "romans_brutii" },
			{FactionOwnership.romans_julii, "romans_julii" },
			{FactionOwnership.romans_scipii, "romans_scipii" },
			{FactionOwnership.romans_senate, "romans_senate" },
			{FactionOwnership.scythia, "scythia" },
			{FactionOwnership.seleucid, "seleucid" },
			{FactionOwnership.slave, "slave" },
			{FactionOwnership.spain, "spain" },
			{FactionOwnership.thrace, "thrace" },
			{FactionOwnership.barbarian, "barbarian" },
			{FactionOwnership.carthaginian, "carthaginian" },
			{FactionOwnership.egyptian, "egyptian" },
			{FactionOwnership.greek, "greek" },
			{FactionOwnership.none, "none" },
			{FactionOwnership.roman, "roman" },
			//{FactionOwnership.nomad, "nomad" }//Ahowls vanilla enhancement addition


		};
		static public Dictionary<Attributes, string> dic_attributes = new Dictionary<Attributes, string>()
		{
			{Attributes.cantabrian_circle, "cantabrian_circle" },
			{Attributes.can_run_amok, "can_run_amok" },
			{Attributes.can_sap, "can_sap" },
			{Attributes.command, "command" },
			{Attributes.druid, "druid" },
			{Attributes.frighten_foot, "frighten_foot" },
			{Attributes.frighten_mounted, "frighten_mounted" },
			{Attributes.general_unit, "general_unit"},
			{Attributes.general_unit_upgrade, "general_unit_upgrade" },
			{Attributes.hide_anywhere, "hide_anywhere" },
			{Attributes.hide_forest, "hide_forest" },
			{Attributes.hide_improved_forest, "hide_improved_forest" },
			{Attributes.hide_long_grass, "hide_long_grass" },
			{Attributes.mercenary_unit, "mercenary_unit" },
			{Attributes.no_custom, "no_custom" },
			{Attributes.sea_faring, "sea_faring" },
			{Attributes.warcry, "warcry" },
			{Attributes.hardy, "hardy" },
			{Attributes.very_hardy, "very_hardy" },
			{Attributes.screeching_women, "screeching_women" }
		};
		static public Dictionary<FormationTypes, string> dic_formations = new Dictionary<FormationTypes, string>()
		{
			{FormationTypes.horde, "horde" },
			{FormationTypes.none, "none" },
			{FormationTypes.phalanx, "phalanx" },
			{FormationTypes.schiltrom, "schiltrom" },
			{FormationTypes.square, "square" },
			{FormationTypes.testudo, "testudo" },
			{FormationTypes.wedge, "wedge"}
		};
		static public Dictionary<MissileType, string> dic_missleTypes = new Dictionary<MissileType, string>()
		{
			{MissileType.arrow, "arrow" },
			{MissileType.arrow2, "arrow2"}, //Ahowls vanilla enhancement addition
			{MissileType.francisca, "francisca"},//Ahowls vanilla enhancement addition
			{MissileType.ballista, "ballista" },
			{MissileType.bullet, "bullet" },
			{MissileType.heavy_onager, "heavy_onager" },
			{MissileType.javelin, "javelin" },
			{MissileType.MT_no, "no" },
			{MissileType.onager, "onager" },
			{MissileType.pilum, "pilum" },
			{MissileType.repeating_ballista, "repeating_ballista" },
			{MissileType.scorpion, "scorpion" },
			{MissileType.stone, "stone" },
			{MissileType.head, "head" },
			{MissileType.boulder, "boulder" },
			{MissileType.big_boulder, "big_boulder" },
			{MissileType.harpoon, "harpoon" }//Ahowls vanilla enhancement addition
		};
		static public Dictionary<WeaponType, string> dic_weapons = new Dictionary<WeaponType, string>()
		{
			{WeaponType.melee, "melee" },
			{WeaponType.missile, "missile" },
			{WeaponType.siege_missile, "siege_missile" },
			{WeaponType.thrown, "thrown" },
			{WeaponType.WT_no, "no" }
		};
		static public Dictionary<TechType, string> dic_techs = new Dictionary<TechType, string>()
		{
			{TechType.archery, "archery" },
			{TechType.blade, "blade" },
			{TechType.other, "other" },
			{TechType.siege, "siege" },
			{TechType.simple, "simple" },
			{TechType.TT_no, "no" }
		};
		static public Dictionary<DamageType, string> dic_damageTypes = new Dictionary<DamageType, string>()
		{
			{DamageType.blunt, "blunt" },
			{DamageType.DM_no, "no" },
			{DamageType.fire, "fire" },
			{DamageType.piercing, "piercing" },
			{DamageType.slashing, "slashing" }
		};
		static public Dictionary<SoundType, string> dic_sounds = new Dictionary<SoundType, string>()
		{
			{SoundType.axe, "axe" },
			{SoundType.knife, "knife" },
			{SoundType.mace, "mace" },
			{SoundType.spear, "spear" },
			{SoundType.ST_no, "none" },
			{SoundType.sword, "sword" }
		};
		static public Dictionary<ArmourSound, string> dic_armourSounds = new Dictionary<ArmourSound, string>()
		{
			{ArmourSound.flesh, "flesh" },
			{ArmourSound.leather, "leather" },
			{ArmourSound.metal, "metal" }
		};
		static public Dictionary<Statmental_discipline, string> dic_mentalDiscipline = new Dictionary<Statmental_discipline, string>()
		{
			{Statmental_discipline.berserker, "berserker" },
			{Statmental_discipline.disciplined, "disciplined" },
			{Statmental_discipline.impetuous, "impetuous" },
			{Statmental_discipline.low, "low" },
			{Statmental_discipline.normal, "normal" }
		};
		static public Dictionary<Statmental_training, string> dic_mentalTraining = new Dictionary<Statmental_training, string>()
		{
			{Statmental_training.highly_trained, "highly_trained" },
			{Statmental_training.trained, "trained" },
			{Statmental_training.untrained, "untrained" }
		};
		static public Dictionary<Stat_pri_attr, string> dic_primaryAttribute = new Dictionary<Stat_pri_attr, string>()
		{
			{Stat_pri_attr.ap, "ap" },
			{Stat_pri_attr.area, "area" },
			{Stat_pri_attr.bp, "bp" },
			{Stat_pri_attr.fire, "fire" },
			{Stat_pri_attr.launching, "launching" },
			{Stat_pri_attr.long_pike, "long_pike" },
			{Stat_pri_attr.PA_no, "no" },
			{Stat_pri_attr.pa_spear, "spear" },
			{Stat_pri_attr.pa_thrown, "thrown" },
			{Stat_pri_attr.prec, "prec" },
			{Stat_pri_attr.short_pike, "short_pike" },
			{Stat_pri_attr.spear_bonus_4, "spear_bonus_4" },
			{Stat_pri_attr.spear_bonus_6, "spear_bonus_6" }, //Ahowls vanilla enhancement addition
			{Stat_pri_attr.spear_bonus_8, "spear_bonus_8" },
			{Stat_pri_attr.spear_bonus_1, "spear_bonus_1" },
			{Stat_pri_attr.spear_bonus_2, "spear_bonus_2" },
			{Stat_pri_attr.spear_bonus_3, "spear_bonus_3" },
			{Stat_pri_attr.spear_bonus_5, "spear_bonus_5" },
			{Stat_pri_attr.spear_bonus_7, "spear_bonus_7" },
			{Stat_pri_attr.spear_bonus_9, "spear_bonus_9" },
			{Stat_pri_attr.spear_bonus_10, "spear_bonus_10" },
			{Stat_pri_attr.spear_bonus_11, "spear_bonus_11" },
			{Stat_pri_attr.spear_bonus_12, "spear_bonus_12" },
			{Stat_pri_attr.thrown_ap, "thrown ap" },
			{Stat_pri_attr.light_spear, "light_spear" }
		};
		static public Dictionary<Cultures, string> dic_cultures = new Dictionary<Cultures, string>()
		{
			{Cultures.barbarian, "barbarian, britons, dacia, gauls, germans, scythia, spain" },
			{Cultures.carthaginian, "carthaginian, numidia, carthage" },
			{Cultures.eastern, "eastern, armenia, parthia, pontus" },
			{Cultures.egyptian, "egyptian, egypt" },
			{Cultures.greek, "greek, greek_cities, macedon, seleucid, thrace" },
			{Cultures.none, "none" },
			{Cultures.numidian, "numidian" },
			{Cultures.parthian, "parthian" },
			{Cultures.roman, "roman, romans_scipii, romans_brutii, romans_julii, romans_senate" }
		};

	    public string LookUpString<T>(T key)
		{
			MemberInfo[] members = this.GetType().GetMembers();
			FieldInfo[] fi = this.GetType().GetFields();

			foreach (FieldInfo obj in fi)
			{
				Type typeCheck = new Dictionary<T, string>().GetType();
				Type keyType = key.GetType();
				Type[] kv = typeCheck.GenericTypeArguments;
				string objName = obj.FieldType.FullName;

				if (keyType.Name == kv[0].Name && typeCheck.FullName == objName)
				{

					Dictionary<T, string> values = (Dictionary<T, string>)obj.GetValue(new Dictionary<T, string>());

					return values[key];
					
				}

			}

			return null;

		}

		public dynamic LookUpKey<T>(string value)
		{
			MemberInfo[] members = this.GetType().GetMembers();
			FieldInfo[] fi = this.GetType().GetFields();

			foreach (FieldInfo obj in fi)
			{
				Type typeCheck = new Dictionary<T, string>().GetType();
				Type[] kv = typeCheck.GenericTypeArguments;
				string objName = obj.FieldType.FullName;

				if (typeCheck.FullName == objName && kv[0] == typeof(T))
				{

					Dictionary<T, string> values = (Dictionary<T, string>)obj.GetValue(new Dictionary<T, string>());

					

					foreach (KeyValuePair<T, string> keyValues in values)
					{
						string[] split;
						if (keyValues.Value.Contains(","))
						{
							split = keyValues.Value.Split(',');
							foreach (string str in split)
							{
								if (str.Trim() == value)
									return keyValues.Key;
							}
						}
						else if (keyValues.Value == value)
							return keyValues.Key;
					}


				}

			}

			return null;


		}

	}
}
