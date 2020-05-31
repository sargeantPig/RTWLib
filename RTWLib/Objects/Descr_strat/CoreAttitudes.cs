using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Objects;
namespace RTWLib.Objects.Descr_strat
{

    public enum DiplomaticPosition
    { 
        Allied,
        Suspicous,
        Neutral,
        Hostile,
        War
    }

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

        public int DoesFactionHaveRelations(FactionOwnership target, FactionOwnership relation) //gets value and checks if relation exists, returns -1 if not found
        {
            if (attitudes.ContainsKey(target))
            {
                foreach (KeyValuePair<int, List<FactionOwnership>> valFac in attitudes[target])
                {
                    foreach (FactionOwnership fo in valFac.Value)
                    {
                        if (fo == relation)
                            return valFac.Key;
                    }
                }
            }
            return -1;
        }

        public DiplomaticPosition GetDiplomaticPosition(int value)
        {
            if (value < 100)
                return DiplomaticPosition.Allied;
            else if (value < 200)
                return DiplomaticPosition.Suspicous;
            else if (value < 400)
                return DiplomaticPosition.Neutral;
            else if (value < 600)
                return DiplomaticPosition.Hostile;
            else if (value >= 600)
                return DiplomaticPosition.War;

            return 0;
        }

        public string[] GetRelationships(DiplomaticPosition value, FactionOwnership faction)
        {
            LookUpTables lt = new LookUpTables();
            List<string> relations = new List<string>();
            if (attitudes.ContainsKey(faction))
            {
                foreach (KeyValuePair<int, List<FactionOwnership>> valFac in attitudes[faction])
                {
                    if (GetDiplomaticPosition(valFac.Key) == value)
                    {
                        foreach (FactionOwnership fo in valFac.Value)
                        {

                            relations.Add(lt.LookUpString(fo));
                        }

                    }
                }
            }
            return relations.ToArray();
        }
    }
}
