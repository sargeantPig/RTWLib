using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;

namespace RTWLib.Objects.Descr_strat
{
    public class FactionRelationship
    {
        public Dictionary<FactionOwnership, List<KeyValuePair<int, FactionOwnership>>> attitudes;
        public FactionRelationship()
        {
            attitudes = new Dictionary<FactionOwnership, List<KeyValuePair<int, FactionOwnership>>>();

        }

        public FactionRelationship(FactionRelationship ca)
        {
            attitudes = new Dictionary<FactionOwnership, List<KeyValuePair<int, FactionOwnership>>>(ca.attitudes);
        }

        public string Output()
        {
            string output = "";
            output += "\r\n\r\n";

            foreach (var faction in attitudes)
            {
                foreach (var relation in faction.Value)
                {
                    output +=
                        "faction_relationship\t"
                        + LookUpTables.dic_factions[faction.Key] + ",\t\t"
                        + relation.Key.ToString() + "\t\t" + LookUpTables.dic_factions[relation.Value] + "\r\n";
                }
            }

            return output;
        }

    }
}
