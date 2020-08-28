using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
	public class FileScheme //helps find the locations of specific values like attack within a file
	{
		Dictionary<string, Dictionary<string, int>> scheme;

		public FileScheme()
		{
			this.scheme = new Dictionary<string, Dictionary<string, int>>();
		}

		public FileScheme(Dictionary<string, Dictionary<string, int>> scheme)
		{
			this.scheme = scheme;
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

	public enum EDULineEnums
    {
        Type,
        Dictionary,
        Category,
        Class,
        Voice_type,
        Soldier,
        Officer,
        Engine,
        Animal,
        Mount,
        Ship,
        Mount_effect,
        Attributes,
        Formation,
        Stat_health,
        Stat_pri,
        Stat_pri_attr,
        Stat_sec,
        Stat_sec_attr,
        Stat_pri_armour,
        Stat_sec_armour,
        Stat_ground,
        Stat_heat,
        Stat_mental,
        Stat_charge_dist,
        Stat_fire_delay,
        Stat_food,
        Stat_cost,
        Ownership,

        //m2tw only
        Stat_ter,
        Stat_ter_ex,
        Stat_ter_attr,
        Stat_armour_ex,
        Stat_pri_ex,
        BannerFaction,
        BannerHoly,
        Stat_sec_ex,
        Stat_stl,
        Armour_ug_levels,
        Armour_ug_models,
        Era0,
        Era1,
        Era2,
        Info_pic_dir,
        Card_pic_info,
        Unit_info,

}
}
