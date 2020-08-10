using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Objects;
using RTWLib.Data;
using RTWLib.Objects.Descr_strat;
namespace RTWLib.Functions
{
    public class Descr_Strat : FileBase, IFile
    {
        public List<string> ds_data = new List<string>();
        public string campaign = "";
        public List<string> playableFactions = new List<string>();
        public List<string> campaignNonPlayable = new List<string>();
        public List<string> unlockableFactions = new List<string>();
        public string startDate = "";
        public string endDate = "";
        public int brigand_spawn_value = 0;
        public int pirate_spawn_value = 0;
        public List<Landmark> landmarks = new List<Landmark>();
        public List<Resource> resources = new List<Resource>();
        public List<Faction> factions = new List<Faction>();
        public CoreAttitudes coreAttitudes = new CoreAttitudes("core_attitudes");
        public CoreAttitudes factionRelationships = new CoreAttitudes("faction_relationships");

        public Descr_Strat() 
            : base(FileNames.descr_strat, @"data\world\maps\campaign\imperial_campaign\descr_strat.txt", "Campaign Info and Setup")
        { }

        override public void Parse(string[] filepath, out int lineNumber, out string currentLine)
        {
            if (!FileCheck(filepath[0]))
                DisplayLogExit();

            LookUpTables tb = new LookUpTables();

            string PATH = filepath[0];
            string line;

            StreamReader strat = new StreamReader(PATH);
            string faction = "";
            Faction newFaction = new Faction();
            DSCharacter newCharacter = new DSCharacter();
            bool newfactionReady = false;
            bool newcharacterReady = false;
            //get factions
            lineNumber = 0;
            currentLine = "";
            while ((line = strat.ReadLine()) != null)
            {
                lineNumber++;
                currentLine = line;
                if (line.StartsWith("campaign"))
                {
                    string[] split = line.Split('\t');
                    campaign = split[2];
                }

                if (line.StartsWith("playable"))
                {
                    while ((line = strat.ReadLine()) != "end")
                    {
                        playableFactions.Add(line.Trim());

                    }
                }

                if (line.StartsWith("unlockable"))
                {
                    while ((line = strat.ReadLine()) != "end")
                    {
                        unlockableFactions.Add(line.Trim());

                    }
                }

                if (line.StartsWith("nonplayable"))
                {
                    while ((line = strat.ReadLine()) != "end")
                    {
                        campaignNonPlayable.Add(line.Trim());
                    }
                }

                if (line.StartsWith("start_date"))
                {
                    string temp = Functions_General.RemoveFirstWord(line, '\t');
                    startDate = temp.Trim();

                }

                if (line.StartsWith("end_date"))
                {
                    string temp = Functions_General.RemoveFirstWord(line, '\t');
                    endDate = temp.Trim();

                }

                if (line.StartsWith("brigand_spawn_value"))
                {
                    string temp = Functions_General.RemoveFirstWord(line);
                    brigand_spawn_value = Convert.ToInt32(temp.Trim());
                }

                if (line.StartsWith("pirate_spawn_value"))
                {
                    string temp = Functions_General.RemoveFirstWord(line);
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


                if (line.StartsWith("faction") && !line.StartsWith("faction_relationships"))
                {
                    if (newfactionReady)
                    { //catch final character
                        newFaction.characters.Add(new DSCharacter(newCharacter));
                        factions.Add(new Faction(newFaction));
                        newfactionReady = false;
                        newCharacter = new DSCharacter();
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

                    Output("Retrieving: " + faction + " campaign information");
                }

                if (line.StartsWith("superfaction"))
                {
                    string superfac = Functions_General.RemoveFirstWord(line);
                    newFaction.superFaction = superfac.Trim();
                }

                if (line.StartsWith("denari"))
                {
                    string[] den = line.Split('\t');
                    newFaction.denari = Convert.ToInt32(den[1].Trim());
                }

                if (line.StartsWith("settlement"))
                {
                    Settlement tempSettlement;
                    List<DSBuilding> b_types = new List<DSBuilding>();

                    string s_level = "", region = "", faction_creator = "";

                    int yearFounded = 0, population = 100;

                    while ((line = strat.ReadLine().TrimEnd()) != "}")
                    {
                        if (line.Trim().StartsWith("level"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            s_level = trimmed;

                        }

                        else if (line.Trim().StartsWith("region"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            region = trimmed;
                        }

                        else if (line.Trim().StartsWith("year_founded"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            yearFounded = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("population"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            population = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("faction_creator"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            faction_creator = trimmed;

                        }

                        else if (line.Trim().StartsWith("type"))
                        {
                            string trimmed = Functions_General.RemoveFirstWord(line);
                            trimmed = trimmed.Trim();

                            DSBuilding dsb = new DSBuilding();
                            string[] split = line.Split(' ');

                            dsb.type = split[1].Trim();
                            dsb.name = split[2].Trim();

                            b_types.Add(dsb);

                        }
                    }

                    //Output("\n" + "Added: " + region + "\n");
                    tempSettlement = new Settlement(s_level, region, faction_creator, b_types, yearFounded, population);
                    newFaction.settlements.Add(tempSettlement);


                }


                if (line.StartsWith("character") && !line.StartsWith("character_record"))
                {
                    if (newcharacterReady)
                    {
                        newFaction.characters.Add(new DSCharacter(newCharacter));
                        newcharacterReady = false;
                    }
                    newcharacterReady = true;

                    newCharacter = new DSCharacter();

                    string[] split = line.Split('\t', ',');

                    if (split.Count() == 7)
                    {
                        newCharacter.name = split[1].Trim();
                        newCharacter.type = split[2].Trim();
                        string[] ageSplit = split[3].Split(' ');
                        newCharacter.age = Convert.ToInt32(ageSplit[2].Trim());
                        string[] xsplit = split[5].Split(' ');
                        string[] ysplit = split[6].Split(' ');
                        newCharacter.coords[0] = Convert.ToInt32(xsplit[2].Trim());
                        newCharacter.coords[1] = Convert.ToInt32(ysplit[2].Trim());

                        line = strat.ReadLine(); //move to traits
                        string traits = Functions_General.RemoveFirstWord(line);
                        newCharacter.traits = traits.Trim();
                    }

                    else if (split.Count() == 8)
                    {
                        newCharacter.name = split[1].Trim();
                        newCharacter.type = split[2].Trim();
                        newCharacter.rank = split[3].Trim();
                        string[] ageSplit = split[4].Split(' ');
                        newCharacter.age = Convert.ToInt32(ageSplit[2].Trim());
                        string[] xsplit = split[6].Split(' ');
                        string[] ysplit = split[7].Split(' ');
                        newCharacter.coords[0] = Convert.ToInt32(xsplit[2].Trim());
                        newCharacter.coords[1] = Convert.ToInt32(ysplit[2].Trim());
                    }
                }

                if (line.StartsWith("traits"))
                {
                    string traits = Functions_General.RemoveFirstWord(line);
                    newCharacter.traits = traits.Trim();
                }

                if (line.StartsWith("ancillaries"))
                {
                    string ancillaries = Functions_General.RemoveFirstWord(line);
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
                    string record = Functions_General.RemoveFirstWord(line, '\t');//

                    CharacterRecord cr = new CharacterRecord();

                    string[] recordSplit = record.Split(',');
                    cr.name = recordSplit[0].Trim();
                    cr.gender = recordSplit[1].Trim();

                    string[] command = recordSplit[2].Split(' ');

                    cr.command = Convert.ToInt32(command[2].Trim());

                    string[] influence = recordSplit[3].Split(' ');
                    cr.influence = Convert.ToInt32(influence[2].Trim());

                    string[] management = recordSplit[4].Split(' ');
                    cr.management = Convert.ToInt32(management[2].Trim());

                    string[] subterfuge = recordSplit[5].Split(' ');
                    cr.subterfuge = Convert.ToInt32(subterfuge[2].Trim());

                    string[] age = recordSplit[6].Split(' ');
                    cr.age = Convert.ToInt32(age[2].Trim());

                    cr.status = recordSplit[7].Trim();
                    cr.leader = recordSplit[8].Trim();

                    newFaction.characterRecords.Add(new CharacterRecord(cr));
                }

                if (line.StartsWith("relative"))
                {
                    string relative = Functions_General.RemoveFirstWord(line, '\t');
                    newFaction.relatives.Add(relative);
                }

                if (line.StartsWith("core_attitudes"))
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
                        string f =split[i + 3];
                        if (!f_a.ContainsKey(temp))
                            f_a.Add(temp, new List<string> { f });
                        else f_a[temp].Add(f);
                    }

                    if (!coreAttitudes.attitudes.ContainsKey(fo))
                    {
                        coreAttitudes.attitudes.Add(fo, new Dictionary<int, List<string>>(f_a));
                    }
                    
                    else
                    {
                        foreach (var cf in f_a)
                        {
                            foreach (var toAdd in cf.Value)
                            {
                                if (!coreAttitudes.attitudes[fo].ContainsKey(cf.Key))
                                {
                                    coreAttitudes.attitudes[fo].Add(cf.Key, new List<string> { toAdd });
                                }

                                else coreAttitudes.attitudes[fo][cf.Key].Add(toAdd);
                            }
                        }
                    }
                }

                if (line.StartsWith("faction_relationships"))
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
                        string f =split[i + 3];
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
                };
            }

            if (newfactionReady) //catches final faction
            { //catch final character
                newFaction.characters.Add(new DSCharacter(newCharacter));
                factions.Add(new Faction(newFaction));
                newfactionReady = false;
                newCharacter = new DSCharacter();
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

            foreach (Faction fac in factions)
            {
                output += fac.Output();
            }

            output += "\r\n";
            output += coreAttitudes.OutputMulti();
            output += factionRelationships.OutputSingle();
            return output;
        }
        override public void ToFile(string filepath)
        {
            StreamWriter sw = new StreamWriter(filepath);
            sw.Write(Output());
            sw.Close();
        }

        //remove spqr from factions
        public void RemoveSPQR()
        {
            int spqrIndex = factions.FindIndex(x => x.name == LookUpTables.dic_factions[FactionOwnership.romans_senate]);
            factions.RemoveAt(spqrIndex);

            int rjInd = factions.FindIndex(x => x.name == LookUpTables.dic_factions[FactionOwnership.romans_julii]);
            int rbInd = factions.FindIndex(x => x.name == LookUpTables.dic_factions[FactionOwnership.romans_brutii]);
            int rsInd = factions.FindIndex(x => x.name == LookUpTables.dic_factions[FactionOwnership.romans_scipii]);

            factions[rjInd].superFaction = "";
            factions[rbInd].superFaction = "";
            factions[rsInd].superFaction = "";

            spqrIndex = campaignNonPlayable.FindIndex(x => x == LookUpTables.dic_factions[FactionOwnership.romans_senate]);
            campaignNonPlayable.RemoveAt(spqrIndex);

            RemoveSenateRelations(ref coreAttitudes);
            RemoveSenateRelations(ref factionRelationships);
        }
        public void ShuffleFactions(Random rnd)
        {
            factions.Shuffle(rnd);
        }
        void RemoveSenateRelations(ref CoreAttitudes coreA)
        {
            coreA.attitudes.Remove("romans_senate");

            foreach (var attitude in coreA.attitudes)
            {
                int index = -1;

                foreach (var temp in attitude.Value)
                {
                    index = temp.Value.FindIndex(x => x == "romans_senate");
                    if (index > -1)
                    {
                        attitude.Value[temp.Key].RemoveAt(index);
                    }
                }
            }
        }
        public int GetArmyCount(int facIndex)
        {
            int count = 0;

            if (facIndex > -1 && facIndex < factions.Count())
            {
                foreach (DSCharacter character in factions[facIndex].characters)
                {
                    if (character.type == "named character" || character.type == "general")
                    {
                        count++;
                    }
                }
            }

            return count;

        }
        public int GetAgentCount(int facIndex)
        {
            int count = 0;

            if (facIndex > -1 && facIndex < factions.Count())
            {
                foreach (DSCharacter character in factions[facIndex].characters)
                {
                    if (character.type == "spy" || character.type == "diplomat")
                    {
                        count++;
                    }
                }
            }

            return count;

        }
        public int GetNavyCount(int facIndex)
        {
            int count = 0;

            if (facIndex > -1 && facIndex < factions.Count())
            {
                foreach (DSCharacter character in factions[facIndex].characters)
                {
                    if (character.type == "admiral")
                    {
                        count++;
                    }
                }
            }

            return count;

        }
	}
}
 