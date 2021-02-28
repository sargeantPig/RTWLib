using RTWLib.Data;
using RTWLib.Functions;
using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2DS : Descr_Strat
    {
        float timescale;
        CoreAttitudes<float> m2twAttitudes = new CoreAttitudes<float>("faction_standings");

        public M2DS() : base()
        { 
        
        }

        override public void Parse(string[] filepath, out int lineNumber, out string currentLine)
        {
            lineNumber = 0;
            currentLine = "";
            if (!FileCheck(filepath[0]))
            {
                DisplayLog();
                return;
            }
            LookUpTables tb = new LookUpTables();

            string PATH = filepath[0];
            string line;

            StreamReader strat = new StreamReader(PATH);
            string faction = "";
            M2Faction newFaction = new M2Faction();
            M2DSCharacter newCharacter = new M2DSCharacter();
            bool newfactionReady = false;
            bool newcharacterReady = false;
            //get factions

            while ((line = strat.ReadLine()) != null)
            {
                lineNumber++;
                currentLine = line;
                if (line.StartsWith("campaign") && !line.Contains("script"))
                {
                    string[] split = line.Split('\t');
                    campaign = split[2];
                }

                if (line.StartsWith("playable"))
                {
                    while ((line = this.ContinueParseAndCountLine(ref strat, ref lineNumber)).TrimEnd() != "end")
                    {
                        playableFactions.Add(line.Trim());

                    }
                }

                if (line.StartsWith("unlockable"))
                {
                    while ((line = this.ContinueParseAndCountLine(ref strat, ref lineNumber)).TrimEnd() != "end")
                    {
                        unlockableFactions.Add(line.Trim());

                    }
                }

                if (line.StartsWith("nonplayable"))
                {
                    while ((line = this.ContinueParseAndCountLine(ref strat, ref lineNumber)).TrimEnd() != "end")
                    {
                        campaignNonPlayable.Add(line.Trim());
                    }
                }

                if (line.StartsWith("start_date"))
                {
                    string temp = LibFuncs.RemoveFirstWord(line, '\t');
                    startDate = temp.Trim();

                }

                if (line.StartsWith("end_date"))
                {
                    string temp = LibFuncs.RemoveFirstWord(line, '\t');
                    endDate = temp.Trim();

                }

                if (line.StartsWith("timescale"))
                {
                    string temp = LibFuncs.RemoveFirstWord(line, '\t');
                    timescale = (float)Convert.ToDouble(temp.Trim());
                }

                if (line.StartsWith("brigand_spawn_value"))
                {
                    string temp = LibFuncs.RemoveFirstWord(line);
                    brigand_spawn_value = Convert.ToInt32(temp.Trim());
                }

                if (line.StartsWith("pirate_spawn_value"))
                {
                    string temp = LibFuncs.RemoveFirstWord(line);
                    pirate_spawn_value = Convert.ToInt32(temp.Trim());
                }

                if (line.StartsWith("landmark"))
                {
                    string[] split = line.Split('\t');
                    string name = split[1].Trim();
                    string x = split[2].Replace(",", "").Trim();
                    int[] coords = new int[] { Convert.ToInt32(x), Convert.ToInt32(split[3].Trim()) };
                    Landmark res = new Landmark(name, coords);
                    landmarks.Add(res);
                }

                if (line.StartsWith("resource"))
                {
                    string[] split = line.Split('\t');
                    string name = split[1].Trim();
                    name = name.Replace(",", "");
                    string x = split[2].Replace(",", "").Trim();

                    int[] coords = new int[] { Convert.ToInt32(x), Convert.ToInt32(split[3].Trim()) };
                    Resource res = new Resource(name, coords);
                    resources.Add(res);
                }


                if (line.StartsWith("faction") && !line.StartsWith("faction_relationships") && !line.StartsWith("faction_standings"))
                {
                    if (newfactionReady)
                    { //catch final character
                        if(newCharacter.gender != null)
                            newFaction.characters.Add(new M2DSCharacter(newCharacter));

                        factions.Add(new M2Faction(newFaction));
                        newFaction = new M2Faction();
                        newfactionReady = false;
                        newCharacter = new M2DSCharacter();
                        newcharacterReady = false;
                    }
                    newfactionReady = true;

                    newFaction.Clear();
                    string[] split = line.Split(',', ' ', '\t');
                    faction = split[1];

                    newFaction.name = faction;
                    newFaction.ai[0] = split[3].Trim();
                    newFaction.ai[1] = split[4].Trim();
                    //FactionRosters.AddFactionKey(tb.LookUpKey<FactionOwnership>(split[1]));

                }

                if (line.StartsWith("superfaction"))
                {
                    string superfac = LibFuncs.RemoveFirstWord(line);
                    newFaction.superFaction = superfac.Trim();
                }

                if (line.StartsWith("denari"))
                {
                    string[] den = line.Split('\t', ' ');
                    newFaction.denari = Convert.ToInt32(den[1].Trim());
                }

                if (line.StartsWith("denari_kings_purse"))
                {
                    string[] den = line.Split('\t', ' ').CleanStringArray();
                    newFaction.kings_purse = Convert.ToInt32(den[1].Trim());
                }

                if (line.StartsWith("dead_until_resurrected"))
                    newFaction.isDeadUntilResurrected = true;

                if (line.StartsWith("ai_label"))
                {
                    string[] den = line.Split('\t', ' ').CleanStringArray();
                    newFaction.ai_label = den[1].Trim();
                }

                if (line.StartsWith("settlement"))
                {
                    string[] typeCheck = line.Split(' '); //check for castle

                    

                    M2Settlement tempSettlement;
                    List<DSBuilding> b_types = new List<DSBuilding>();

                   
                    string s_level = "", region = "", faction_creator = "", type = "";

                    if (typeCheck.Count() > 1)
                        type = typeCheck[1];


                     int yearFounded = 0, population = 100;

                    while ((line = this.ContinueParseAndCountLine(ref strat, ref lineNumber).TrimEnd()) != "}")
                    {
                        if (line.Trim().StartsWith("level"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            s_level = trimmed;

                        }

                        else if (line.Trim().StartsWith("region"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            region = trimmed;
                        }

                        else if (line.Trim().StartsWith("year_founded"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            yearFounded = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("population"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            population = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("faction_creator"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            faction_creator = trimmed;

                        }

                        else if (line.Trim().StartsWith("type"))
                        {
                            string trimmed = LibFuncs.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            DSBuilding dsb = new DSBuilding();
                            string[] split = line.Split(' ');

                            dsb.type = split[1].Trim();
                            dsb.name = split[2].Trim();

                            b_types.Add(dsb);

                        }
                    }

                    //Output("\n" + "Added: " + region + "\n");
                    tempSettlement = new M2Settlement(s_level, region, faction_creator, b_types, yearFounded, population, type);
                    newFaction.settlements.Add(tempSettlement);


                }


                if (line.StartsWith("character") && !line.StartsWith("character_record"))
                {
                    if (newcharacterReady)
                    {
                        newFaction.characters.Add(newCharacter);
                        newcharacterReady = false;
                    }
                    newcharacterReady = true;

                    newCharacter = new M2DSCharacter();

                    string[] split = line.Split('\t', ',');

                    if (split.Count() == 7)
                    {
                        newCharacter.name = split[1].Trim();
                        newCharacter.type = split[2].Trim();
                        newCharacter.gender = split[3].Trim();
                        string[] ageSplit = split[4].Split(' ');
                        newCharacter.age = Convert.ToInt32(ageSplit[2].Trim());
                        string[] xsplit = split[5].Split(' ');
                        string[] ysplit = split[6].Split(' ');
                        newCharacter.coords[0] = Convert.ToInt32(xsplit[2].Trim());
                        newCharacter.coords[1] = Convert.ToInt32(ysplit[2].Trim());

                        line = this.ContinueParseAndCountLine(ref strat, ref lineNumber); //move to traits
                        string traits = LibFuncs.RemoveFirstWord(line);
                        newCharacter.traits = traits.Trim();
                    }

                    else if (split.Count() == 8)
                    {
                        newCharacter.name = split[1].Trim();
                        newCharacter.gender = split[3].Trim();
                        newCharacter.type = split[2].Trim();
                        newCharacter.rank = split[4].Trim();
                        string[] ageSplit = split[5].Split(' ');
                        newCharacter.age = Convert.ToInt32(ageSplit[2].Trim());
                        string[] xsplit = split[6].Split(' ');
                        string[] ysplit = split[7].Split(' ');
                        newCharacter.coords[0] = Convert.ToInt32(xsplit[2].Trim());
                        newCharacter.coords[1] = Convert.ToInt32(ysplit[2].Trim());
                    }
                }

                if (line.StartsWith("traits"))
                {
                    string traits = LibFuncs.RemoveFirstWord(line);
                    newCharacter.traits = traits.Trim();
                }

                if (line.StartsWith("ancillaries"))
                {
                    string ancillaries = LibFuncs.RemoveFirstWord(line);
                    newCharacter.ancillaries = ancillaries;
                }

                if (line.StartsWith("unit"))
                {
                    string[] army = line.Split('\t', ' ');
                    bool nameFetched = false;
                    string name = "";
                    int exp = 0;
                    int weapon = 0;
                    int armour = 0;
                    for (int i = 0; i < army.Length; i++)
                    {
                        if (army[i] == "armour")
                        {
                            nameFetched = true;
                            armour = Convert.ToInt32(army[i + 1]);
                        }

                        if (army[i] == "weapon")
                        {
                            nameFetched = true;
                            weapon = Convert.ToInt32(army[i + 1]);
                        }

                        if (army[i] == "exp")
                        {
                            nameFetched = true;
                            exp = Convert.ToInt32(army[i + 1]);
                        }

                        if (army[i] != "unit" && !nameFetched)
                            name += army[i] + " ";
                    }

                    newCharacter.army.Add(new DSUnit(name.Trim(), exp, armour, weapon));
                }

                if (line.StartsWith("character_record"))
                {
                    string record = LibFuncs.RemoveFirstWord(line, '\t');//

                    M2CharacterRecord cr = new M2CharacterRecord();

                    string[] recordSplit = record.Split(',');
                    cr.name = recordSplit[0].Trim();
                    cr.gender = recordSplit[1].Trim();
                  
                    string[] age = recordSplit[2].Split(' ');
                    cr.age = Convert.ToInt32(age[2].Trim());

                    cr.status = recordSplit[3].Trim();
                    cr.leader = recordSplit[4].Trim();

                    newFaction.characterRecords.Add(new M2CharacterRecord(cr));
                }

                if (line.StartsWith("relative"))
                {
                    string relative = LibFuncs.RemoveFirstWord(line, '\t');
                    newFaction.relatives.Add(relative);
                }

                if (line.StartsWith("faction_standings"))
                {
                    string[] split = line.Split('\t', ',');

                    split = Misc_Data.CleanStringArray(split);

                    int count = split.Count();
                    count -= 3; //amount of faction entries required

                    string fo = split[1];

                    Dictionary<object, List<string>> f_a = new Dictionary<object, List<string>>();
                    for (int i = 0; i < count; i++)
                    {
                        float temp = (float)Convert.ToDouble(split[2]);
                        string f = split[i + 3];
                        if (!f_a.ContainsKey(temp))
                            f_a.Add(temp, new List<string> { f });
                        else f_a[temp].Add(f);
                    }

                    if (!m2twAttitudes.attitudes.ContainsKey(fo))
                    {
                        m2twAttitudes.attitudes.Add(fo, new Dictionary<object, List<string>>(f_a));
                    }

                    else
                    {
                        foreach (var cf in f_a)
                        {
                            foreach (var toAdd in cf.Value)
                            {
                                if (!m2twAttitudes.attitudes[fo].ContainsKey(cf.Key))
                                {
                                    m2twAttitudes.attitudes[fo].Add(cf.Key, new List<string> { toAdd });
                                }

                                else m2twAttitudes.attitudes[fo][cf.Key].Add(toAdd);
                            }
                        }
                    }
                }

               /* if (line.StartsWith("faction_relationships"))
                {
                    string[] split = line.Split('\t', ',');

                    split = Misc_Data.CleanStringArray(split);

                    int count = split.Count();
                    count -= 3; //amount of faction entries required

                    string fo = split[1];

                    Dictionary<int, List<string>> f_a = new Dictionary<int, List<string>>();
                    for (int i = 0; i < count; i++)
                    {
                        int temp = Convert.ToInt32(split[2]);
                        string f = split[i + 3];
                        if (!f_a.ContainsKey(temp))
                            f_a.Add(temp, new List<string> { f });
                        else f_a[temp].Add(f);
                    }

                    if (!factionRelationships.attitudes.ContainsKey(fo))
                    {
                        factionRelationships.attitudes.Add(fo, new Dictionary<int, List<string>>(f_a));
                    }

                    else
                    {
                        foreach (var cf in f_a)
                        {
                            foreach (var toAdd in cf.Value)
                            {
                                if (!factionRelationships.attitudes[fo].ContainsKey(cf.Key))
                                {
                                    factionRelationships.attitudes[fo].Add(cf.Key, new List<string> { toAdd });
                                }

                                else factionRelationships.attitudes[fo][cf.Key].Add(toAdd);

                            }
                        }
                    }
                };*/
            }

            if (newfactionReady) //catches final faction
            { //catch final character
                newFaction.characters.Add(newCharacter);
                factions.Add(new M2Faction(newFaction));
                newfactionReady = false;
                newCharacter = new M2DSCharacter();
                newcharacterReady = false;
            }

            strat.Close();

            //Descr_Strat ds = new Descr_Strat(settlementOwnership, settlements, ds_data);
        }

        override public string Output()
        {
            string output = "";

            output +=
                "campaign\t\t" + campaign + "\r\n" +
                "playable\r\n";

            foreach (string str in playableFactions)
            {
                output += "\t" + str + "\r\n";
            }

            output +=
                "end\r\n" +
                "unlockable\r\n";

            foreach (string str in unlockableFactions)
            {
                output += "\t" + str + "\r\n";
            }

            output +=
                "end\r\n" +
                "nonplayable\r\n";

            foreach (string str in campaignNonPlayable)
            {
                output += "\t" + str + "\r\n";
            }

            output +=
                "end\r\n\r\n";

            output +=
                "start_date\t" + startDate + "\r\n" +
                "end_date\t" + endDate + "\r\n\r\n" +
                "timescale\t" + timescale + "\r\n\r\n" +
                "marian_reforms_disabled\r\n" +
                "rebelling_characters_active\r\n" +
                "gladiator_uprising_disabled\r\n" +
                "night_battles_enabled\r\n" +
                "show_date_as_turns\r\n" +
                "brigand_spawn_value " + brigand_spawn_value.ToString() + "\r\n" +
                "pirate_spawn_value " + pirate_spawn_value.ToString() + "\r\n\r\n";

            foreach (Landmark lm in landmarks)
            {
                output += lm.Output();
            }

            output += "\r\n\r\n";

            foreach (Resource res in resources)
            {
                output += res.Output();
            }

            output += "\r\n\r\n";

            foreach (IFaction fac in factions)
            {
                output += ((M2Faction)fac).Output();
            }

            output += "\r\n";
            output += m2twAttitudes.OutputMulti();

            output += "\r\nscript\r\ncampaign_script.txt\r\n";

            return output;
        }
        override public void ToFile(string filepath)
        {
            StreamWriter sw = new StreamWriter(filepath);
            sw.Write(Output());
            sw.Close();
        }
    }
}
