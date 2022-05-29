using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using ImageMagick;
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

    public class CoreAttitudes<T>
    {
        string text;
        public Dictionary<string, Dictionary<object, List<string>>> attitudes;
        public CoreAttitudes(string text)
        {
            attitudes = new Dictionary<string, Dictionary<object, List<string>>>();
            this.text = text;
        }

        public CoreAttitudes(CoreAttitudes<object> ca)
        {
            attitudes = new Dictionary<string, Dictionary<object, List<string>>>(ca.attitudes);
            this.text = ca.text;
        }

        public string OutputMulti(bool is_bi = false)
        {
            string output = "";
            output += "\r\n\r\n";

            foreach (var faction in attitudes)
            {
                foreach (var relation in faction.Value)
                {
                    if (is_bi && (int)relation.Key == 600)
                    {
                        output +=
                        text + "\t"
                        + faction.Key + ",\t"
                        + "at_war_with" + "\t";
                    }
                    else if (is_bi && (int)relation.Key == 0)
                    {
                        output +=
                        text + "\t"
                        + faction.Key + ",\t"
                        + "allied_to" + "\t";
                    }
                    else
                    {
                        output +=
                            text + "\t"
                            + faction.Key + ",\t"
                            + relation.Key.ToString() + "\t";
                    }
                    for (int i = 0; i < relation.Value.Count; i++)
                    {
                        output += relation.Value[i];

                        if (i + 1 != relation.Value.Count)
                        {
                            output += ", ";
                        }
                    }
                    output += "\r\n";
                }
            }

            return output;

        }

        public string OutputSingle(bool is_bi = false)
        {
            string output = "";
            output += "\r\n\r\n";

            foreach (var faction in attitudes)
            {
                foreach (var relation in faction.Value)
                {
                    foreach (var rel in relation.Value)
                    {
                        if (is_bi && (int)relation.Key == 600)
                        {
                            output +=
                            text + "\t"
                            + faction.Key + ",\t"
                            + "at_war_with" + "\t"
                            + rel + "\r\n";
                        }
                        else if (is_bi && (int)relation.Key == 0)
                        {
                            output +=
                            text + "\t"
                            + faction.Key + ",\t"
                            + "allied_to" + "\t" 
                            + rel + "\r\n";
                        }
                        else
                        {
                            output +=
                                text + "\t"
                                + faction.Key + ",\t"
                                + relation.Key.ToString() + "\t"
                                + rel + "\r\n";
                        }
                    }
                }
            }

            return output;
        }

        public object DoesFactionHaveRelations(string target, string relation) //gets value and checks if relation exists, returns -1 if not found
        {
            if (attitudes.ContainsKey(target))
            {
                foreach (KeyValuePair<object, List<string>> valFac in attitudes[target])
                {
                    foreach (string fo in valFac.Value)
                    {
                        if (fo == relation)
                        {
                            return valFac.Key;
                        }
                    }
                }
            }

            foreach (var fac in attitudes)
            {
                foreach (var secFac in fac.Value)
                {
                    foreach (var rel in secFac.Value)
                    {
                        if (rel == target && relation == fac.Key)
                            return secFac.Key;
                    }
                }
            }

            return -1;
        }    
        
        public DiplomaticPosition GetDiplomaticPosition(int value)
        {
            if (value < 200)
                return DiplomaticPosition.Allied;
            else if (value < 400)
                return DiplomaticPosition.Neutral;
            else if (value < 600)
                return DiplomaticPosition.Hostile;
            else if (value >= 600)
                return DiplomaticPosition.War;

            return 0;
        }

        public DiplomaticPosition GetDiplomaticPosition(float value)
        {
            if (value < 200)
                return DiplomaticPosition.Allied;
            else if (value < 400)
                return DiplomaticPosition.Neutral;
            else if (value < 600)
                return DiplomaticPosition.Hostile;
            else if (value >= 600)
                return DiplomaticPosition.War;

            return 0;
        }

        public void PopulateRelationTree(ref TreeView tree)
        {
            foreach (var priFac in attitudes)
            {
                foreach (var relValue in priFac.Value)
                {
                    foreach (string secFac in relValue.Value)
                    {
                        DiplomaticPosition dp = GetDiplomaticPosition((int)relValue.Key);

                        switch (dp)
                        {
                            case DiplomaticPosition.Allied:
                                tree.Nodes[priFac.Key].Nodes["Relationships"].Nodes["Allied"].Nodes.Add(secFac);
                                tree.Nodes[secFac].Nodes["Relationships"].Nodes["Allied"].Nodes.Add(priFac.Key);
                                break;
                            case DiplomaticPosition.Hostile:
                                tree.Nodes[priFac.Key].Nodes["Relationships"].Nodes["Hostile"].Nodes.Add(secFac);
                                tree.Nodes[secFac].Nodes["Relationships"].Nodes["Hostile"].Nodes.Add(priFac.Key);
                                break;
                            case DiplomaticPosition.Neutral:
                                tree.Nodes[priFac.Key].Nodes["Relationships"].Nodes["Neutral"].Nodes.Add(secFac);
                                tree.Nodes[secFac].Nodes["Relationships"].Nodes["Neutral"].Nodes.Add(priFac.Key);
                                break;
                            case DiplomaticPosition.Suspicous:
                                tree.Nodes[priFac.Key].Nodes["Relationships"].Nodes["Suspicous"].Nodes.Add(secFac);
                                tree.Nodes[secFac].Nodes["Relationships"].Nodes["Suspicous"].Nodes.Add(priFac.Key);
                                break;
                            case DiplomaticPosition.War:
                                tree.Nodes[priFac.Key].Nodes["Relationships"].Nodes["At War"].Nodes.Add(secFac);
                                tree.Nodes[secFac].Nodes["Relationships"].Nodes["At War"].Nodes.Add(priFac.Key);
                                break;

                        }
                    }
                }
            }
        }

        public string[] GetRelationships(DiplomaticPosition value, string faction)
        {
            List<string> relations = new List<string>();
            if (attitudes.ContainsKey(faction))
            {
                foreach (KeyValuePair<object, List<string>> valFac in attitudes[faction])
                {
                    if (typeof(T) == typeof(int))
                    {
                        if (GetDiplomaticPosition((int)valFac.Key) == value)
                        {
                            foreach (string fo in valFac.Value)
                            {
                                relations.Add(fo);
                            }
                        }
                    }

                    else if (typeof(T) == typeof(float))
                    {
                        if (GetDiplomaticPosition((float)valFac.Key) == value)
                        {
                            foreach (string fo in valFac.Value)
                            {

                                relations.Add(fo);
                            }
                        }
                    }
                }
            }
            return relations.ToArray();
        }
    }
}
