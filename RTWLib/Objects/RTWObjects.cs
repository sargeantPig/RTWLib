using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;

namespace RTWLib.Objects
{

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
