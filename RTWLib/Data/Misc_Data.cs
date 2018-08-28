using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Functions;
namespace RTWLib.Data
{
	public static class Misc_Data
	{
		public static bool[,] regionWater = new bool[255, 156];

		public static int[] GetClosestWater(int[] coords)
		{
			int distance = 10000;
			int[] waterCoords = new int[2];

			for (int x = 0; x < 255; x++)
			{
				for (int y = 0; y < 156; y++)
				{
					int temp = (int)Functions_General.DistanceTo(new int[] { x, y }, coords);
					if (temp < distance && regionWater[x,y])
					{
						distance = temp;
						waterCoords = new int[] { x, y  };
					}
				}
			}

			regionWater[waterCoords[0], waterCoords[1]] = false;

			return waterCoords;

		}

	}

	public static class FilePaths
	{
		//folder paths
		readonly public static string RTWEXE = @"RomeTW.exe";
		readonly public static string ROOT = @"data";
		readonly public static string IMPERIAL_CAMPAIGN = @"data\world\maps\campaign\imperial_campaign";
		readonly public static string MAPS_BASE = @"data\world\maps\base";
		//file names
		readonly public static string EXPORT_DESCR_UNIT = @"\export_descr_unit.txt";
		readonly public static string EXPORT_DESCR_BUILDINGS = @"\export_descr_buildings.txt";
		readonly public static string DESCR_STRAT = @"\descr_strat.txt";
		readonly public static string DESCR_REGIONS = @"\descr_regions.txt";
		readonly public static string MAP_REGIONS = @"\map_regions.tga";
		readonly public static string RADAR_MAP1 = @"\radar_map1.tga";
		readonly public static string DESCR_SM_FACTION = @"\descr_sm_factions.txt";

	}
}
