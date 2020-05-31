using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;

namespace RTWLib.Objects
{
	public class Region
	{
		public string name = "";
		public string cityName = "";
		public string faction_creator = "";
		public string[] resources;
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

	




}
