using RTWLib.Objects.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    class M2Building : Building
    {
        public SettlementType settlementType = SettlementType.None;

        public M2Building()
        { }

        public M2Building(M2Building mbuilding)
        {
            settlementType = mbuilding.settlementType;
            buildingName = mbuilding.buildingName;
            factionsRequired = new List<string>(mbuilding.factionsRequired);
            capability = mbuilding.capability;
            construction = mbuilding.construction;
            Bconvert_to = mbuilding.Bconvert_to;
            fcapability = mbuilding.fcapability;
            material = mbuilding.material;
        }

        public override string outputBuilding()
        {
            string a = "";
            string setType = GetSettlementType();

            if(setType == null)
                a += Functions.Helpers_EDB.EDBTabSpacers[1] + buildingName +
                " requires factions { ";
            else
                a += Functions.Helpers_EDB.EDBTabSpacers[1] + buildingName + " " + setType +
                " requires factions { ";



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

        string GetSettlementType()
        {
            switch (settlementType)
            {
                case SettlementType.Castle: return "castle";
                case SettlementType.City: return "city";
                case SettlementType.None: return null;
                default: return null; 
            }
        }
    }
}
