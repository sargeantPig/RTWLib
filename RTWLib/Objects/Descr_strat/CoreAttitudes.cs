using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Objects;
namespace RTWLib.Objects.Descr_strat
{
    public class CoreAttitudes
    {
        string text;
        public Dictionary<FactionOwnership, Dictionary<int, List<FactionOwnership>>> attitudes;
        public CoreAttitudes(string text)
        {
            attitudes = new Dictionary<FactionOwnership, Dictionary<int, List<FactionOwnership>>>();
            this.text = text;
        }

        public CoreAttitudes(CoreAttitudes ca)
        {
            attitudes = new Dictionary<FactionOwnership, Dictionary<int, List<FactionOwnership>>>(ca.attitudes);
            this.text = ca.text;
        }

        public string OutputMulti()
        {
            string output = "";
            output += "\r\n\r\n";

            foreach (var faction in attitudes)
            {
                foreach (var relation in faction.Value)
                {
                    output +=
                        text + "\t"
                        + LookUpTables.dic_factions[faction.Key] + ",\t"
                        + relation.Key.ToString() + "\t";

                    for (int i = 0; i < relation.Value.Count; i++)
                    {
                        output += LookUpTables.dic_factions[relation.Value[i]];

                        if (i + 1 != relation.Value.Count)
                        {
                            output += ", ";
                        }
                        else output += "\r\n";
                    }
                }
            }

            return output;

        }

        public string OutputSingle()
        {
            string output = "";
            output += "\r\n\r\n";

            foreach (var faction in attitudes)
            {
                foreach (var relation in faction.Value)
                {
                    foreach (var rel in relation.Value)
                    {
                        output +=
                            text + "\t"
                            + LookUpTables.dic_factions[faction.Key] + ",\t"
                            + relation.Key.ToString() + "\t" + LookUpTables.dic_factions[rel] + "\r\n";
                    }
                }
            }

            return output;

        }
    }
}
