using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Buildings
{
    public class Building
    {
        public string buildingName; //eg governors_house
        public List<string> factionsRequired = new List<string>();
        public Bcapability capability;
        public Fcapability fcapability;
        public Bconstruction construction;
        public string Bconvert_to;
        public string material;

        public Building()
        {

        }

        public Building(Building b)
        {
            buildingName = b.buildingName;
            factionsRequired = new List<string>(b.factionsRequired);
            capability = b.capability;
            construction = b.construction;
            Bconvert_to = b.Bconvert_to;
            fcapability = b.fcapability;
            material = b.material;
        }


        public string outputBuilding()
        {
            string a = "";

            a += Functions.Helpers_EDB.EDBTabSpacers[1] + buildingName + " requires factions { ";

            foreach (string faction in factionsRequired)
            {
                a += faction + ", ";
            }

            a += "}" + "\r\n"
                + Functions.Helpers_EDB.EDBTabSpacers[1] + "{" + "\r\n";
            if (Bconvert_to != null)
                a += Functions.Helpers_EDB.EDBTabSpacers[2] + "convert_to " + Bconvert_to + "\r\n";

            a += capability.outputCapability();

            if (fcapability != null)
                a += fcapability.outputFcapa();

            if (material != null)
                a += Functions.Helpers_EDB.EDBTabSpacers[2] + "material " + material + "\r\n";

            a += construction.outputConstruction();
            a += Functions.Helpers_EDB.EDBTabSpacers[1] + "}" + "\r\n";

            return a;

        }


    }

}
