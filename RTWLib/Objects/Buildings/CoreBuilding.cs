using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class CoreBuilding
    {
        public string buildingType; //eg. "core_building"
        public List<string> levels = new List<string>();
        public List<Building> buildings = new List<Building>();
        public string CBconvert_to;

        public CoreBuilding()
        { }

        public string outputCoreBuilding()
        {
            string a = "";

            a += "building " + buildingType + "\r\n"
                + "{" + "\r\n";

            a += Functions.Helpers_EDB.EDBTabSpacers[0] + "levels ";

            foreach (string level in levels)
            {
                a += level + " ";

            }

            a += "\r\n" + Functions.Helpers_EDB.EDBTabSpacers[0] + "{" + "\r\n";

            foreach (Building building in buildings)
            {
                a += building.outputBuilding();

            }

            a += Functions.Helpers_EDB.EDBTabSpacers[0] + "}" + "\r\n";

            a += Functions.Helpers_EDB.EDBTabSpacers[0] + "plugins" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[0] + "{" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[0] + "}" + "\r\n"
                + "}";

            return a;
        }
    }
}
