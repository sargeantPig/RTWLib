using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Remaster.DR.RegionData
{
    public class Religion
    {
        public string name;
        public int influence;

        public Religion(string name, int influence)
        {
            this.name = name;
            this.influence = influence;
        }
		public string Output()
		{
            return string.Format("{0} {1} ", name, influence); 
		}
	}
}
