using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
	public class Region
	{
		public string name = "";
		public string cityName = "";
		public string factionCreator = "";
		public string rebelTribe = "";
		public string[] resources;
		public int[] rgb = { 0, 0, 0 };
		public int x = 0, y = 0, triumphVal, farmLevel;

		public Region(string n, int[] color)
		{
			name = n;
			rgb = color;
		}

		public Region()
		{ }

		public string Output()
		{
			return string.Format("{0}".CRL() +
				"\t{1}".CRL() +
				"\t{2}".CRL() +
				"\t{3}".CRL() +
				"\t{4}".CRL() +
				"\t{5}".CRL() +
				"\t{6}".CRL() +
				"\t{7}".CRL(), name, cityName, factionCreator, rebelTribe, 
				LibFuncs.ArrayToString(rgb, false, false, true, 0),
				LibFuncs.ArrayToString(resources, false, false, true, 0),
				triumphVal,
				farmLevel);
		}

	}
}
