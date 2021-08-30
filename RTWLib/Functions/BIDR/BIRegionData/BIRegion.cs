using RTWLib.Extensions;
using RTWLib.Objects.Descr_strat;
using RTWLib.Remaster.DR.RegionData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
	public class BIRegion : Region
	{
		public string legion;
		public string[] religions;

		public BIRegion(string n, int[] color)
		{
			name = n;
			rgb = color;
		}

		public BIRegion()
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
				"\t{7}".CRL() +
				"\t{8}".CRL() +
				"\t{9}".CRL(), name, legion, cityName, factionCreator, rebelTribe, 
				rgb.ArrayToString(false, false, true, 0),
				resources.ArrayToString(false, false, true, 0),
				triumphVal,
				farmLevel, 
				religions.ArrayToString(false, false, false, 0, false));
		}

	}
}
