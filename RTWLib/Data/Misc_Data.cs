using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Functions;
using RTWLib.Extensions;
namespace RTWLib.Data
{
	public static class Misc_Data
	{
		public static bool[,] regionWater { get; set; }
		public static bool[,] editRegionWater { get; set; }

		public static int[] GetClosestWater(int[] coords)
		{
			int distance = 10000;
			int[] waterCoords = new int[2];

			for (int x = 0; x < 255; x++)
			{
				for (int y = 0; y < 156; y++)
				{
					int temp = (int)new int[] { x, y }.DistanceTo(coords);
					if (temp < distance && editRegionWater[x, y])
					{
						distance = temp;
						waterCoords = new int[] { x, y };
					}
				}
			}

			editRegionWater[waterCoords[0], waterCoords[1]] = false;

			return waterCoords;

		}

		public static void RefreshRegionWater()
		{
			for (int x = 0; x < 255; x++)
			{
				for (int y = 0; y < 156; y++)
				{
					editRegionWater[x, y] = regionWater[x, y];
				}
			}

		}

		public static string[] RemoveThenOutWhiteSpace(this string[] data, out string whitespace)
		{
			List<string> newArray = new List<string>();
			bool nonWhitespace = false;
			whitespace = "";
			foreach (string str in data)
			{
				if (str != "" && str != " ")
				{
					newArray.Add(str.Trim());
					nonWhitespace = true;
				}
				else if ((str == "" || str == "\t") && !nonWhitespace)
					whitespace += " ";
			}
			return newArray.ToArray();
		}

	}

	public static class FilePaths
	{
		//folder paths
		readonly public static string RTWEXE = @"RomeTW.exe";
		readonly public static string IMPERIAL_CAMPAIGN = @"\world\maps\campaign\imperial_campaign";
		readonly public static string MAPS_BASE = @"\world\maps\base";
		//file names
		readonly public static string EXPORT_DESCR_UNIT = @"\export_descr_unit.txt";
		readonly public static string EXPORT_DESCR_BUILDINGS = @"\export_descr_buildings.txt";
		readonly public static string DESCR_STRAT = @"\descr_strat.txt";
		readonly public static string DESCR_REGIONS = @"\descr_regions.txt";
		readonly public static string MAP_REGIONS = @"\map_regions.tga";
		readonly public static string RADAR_MAP1 = @"\radar_map1.tga";
		readonly public static string DESCR_SM_FACTION = @"\descr_sm_factions.txt";
		readonly public static string DESCR_MODEL_BATTLE = @"\descr_model_battle.txt";
		



	}

    public enum FileNames
    {
        export_descr_buildings,
        export_descr_unit,
        descr_strat,
        descr_regions,
        descr_sm_faction,
        names,
		preferences,
		battle_models,
		export_units
    }

}
