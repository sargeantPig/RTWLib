using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Objects;
using RTWLib.Data;
using RTWLib.Objects.Descr_strat;
using System.Security.Cryptography;
using System.Data.OleDb;
using RTWLib.Extensions;
using RTWLib.Remaster;

namespace RTWLib.Functions.Remaster
{
    public class RemasterDescr_Strat : Descr_Strat, IFile, IDescrStrat
    {
        new public List<RemasterResource> resources = new List<RemasterResource>();
        bool is_bi = false;
        public RemasterDescr_Strat(bool is_bi = false)
            : base()
        {
            coreAttitudes = new CoreAttitudes<int>("core_attitudes");
            factionRelationships = new CoreAttitudes<int>("faction_relationships");
            this.is_bi = is_bi;
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
            Faction newFaction = new Faction();
            DSCharacter newCharacter = new DSCharacter();
            bool newfactionReady = false;
            bool newcharacterReady = false;
            //get factions

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
                    string temp = line.RemoveFirstWord('\t');
                    startDate = temp.Trim();

                }

                if (line.StartsWith("end_date"))
                {
                    string temp = line.RemoveFirstWord('\t');
                    endDate = temp.Trim();

                }

                if (line.StartsWith("brigand_spawn_value"))
                {
                    string temp = line.RemoveFirstWord();
                    brigand_spawn_value = Convert.ToInt32(temp.Trim());
                }

                if (line.StartsWith("pirate_spawn_value"))
                {
                    string temp = line.RemoveFirstWord();
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
                    string[] split = line.Split('\t', ' ').CleanStringArray();
                    string name = split[1].Trim();
                    name = name.Replace(",", "");
                    string x = split[3].Replace(",", "").Trim();
                    int quantity = Convert.ToInt32(split[2].Trim(','));
                    int[] coords = new int[] { Convert.ToInt32(x), Convert.ToInt32(split[4].Trim()) };
                    RemasterResource res = new RemasterResource(name, quantity, coords);
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
                    string[] split = line.Split(',', ' ', '\t').CleanStringArray();
                    faction = split[1];

                    newFaction.name = faction;
                    newFaction.ai[0] = split[2].Trim();
                    newFaction.dead_until_resurrected = false;
                    //FactionRosters.AddFactionKey(tb.LookUpKey<FactionOwnership>(split[1]));

                }

                if (line.StartsWith("superfaction"))
                {
                    string superfac = line.RemoveFirstWord();
                    newFaction.superFaction = superfac.Trim();
                }

                if (line.StartsWith("dead_until_resurrected"))
                {
                    newFaction.dead_until_resurrected = true;
                }

                if (line.StartsWith("denari"))
                {
                    string[] den = line.Split('\t');
                    newFaction.denari = Convert.ToInt32(den[1].Trim());
                }

                if (line.StartsWith("settlement") && !newFaction.dead_until_resurrected)
                {
                    Settlement tempSettlement;
                    List<DSBuilding> b_types = new List<DSBuilding>();

                    string s_level = "", region = "", faction_creator = "";

                    int yearFounded = 0, population = 100;

                    while ((line = strat.ReadLine().TrimEnd()) != "}")
                    {
                        if (line.Trim().StartsWith("level"))
                        {
                            string trimmed = line.RemoveFirstWord();
                            trimmed = trimmed.Trim();

                            s_level = trimmed;

                        }

                        else if (line.Trim().StartsWith("region"))
                        {
                            string trimmed = line.RemoveFirstWord();
                            trimmed = trimmed.Trim();

                            region = trimmed;
                        }

                        else if (line.Trim().StartsWith("year_founded"))
                        {
                            string trimmed = line.RemoveFirstWord();
                            trimmed = trimmed.Trim();

                            yearFounded = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("population"))
                        {
                            string trimmed = line.RemoveFirstWord();
                            trimmed = trimmed.Trim();

                            population = Convert.ToInt32(trimmed);

                        }

                        else if (line.Trim().StartsWith("faction_creator"))
                        {
                            string trimmed = line.RemoveFirstWord();
                            trimmed = trimmed.Trim();

                            faction_creator = trimmed;

                        }

                        else if (line.Trim().StartsWith("type"))
                        {
                            string trimmed = line.RemoveFirstWord();
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


                if (line.StartsWith("character") && !line.StartsWith("character_record") && !newFaction.dead_until_resurrected)
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
                        string traits = line.RemoveFirstWord();
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

                if (line.StartsWith("traits") && !newFaction.dead_until_resurrected)
                {
                    string traits = line.RemoveFirstWord();
                    newCharacter.traits = traits.Trim();
                }

                if (line.StartsWith("ancillaries") && !newFaction.dead_until_resurrected)
                {
                    string ancillaries = line.RemoveFirstWord();
                    newCharacter.ancillaries = ancillaries;
                }

                if (line.StartsWith("unit") && !newFaction.dead_until_resurrected)
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

                if (line.StartsWith("character_record") && !newFaction.dead_until_resurrected)
                {
                    string record = line.RemoveFirstWord('\t');//

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

                if (line.StartsWith("relative") && !newFaction.dead_until_resurrected)
                {
                    string relative = line.RemoveFirstWord('\t');
                    newFaction.relatives.Add(relative);
                }

                if (line.StartsWith("core_attitudes") && !newFaction.dead_until_resurrected)
                {
                    string[] split = line.Split('\t', ',');

                    split = split.CleanStringArray();

                    int count = split.Count();
                    count -= 3; //amount of faction entries required

                    string fo = split[1];

                    Dictionary<object, List<string>> f_a = new Dictionary<object, List<string>>();
                    for (int i = 0; i < count; i++)
                    {
                        int temp = 0;
                        if (split[2].Trim() == "at_war_with")
                            temp = 600;
                        else if (split[2].Trim() == "allied_to")
                            temp = 0;
                        else temp = Convert.ToInt32(split[2]);
                        string f = split[i + 3];
                        if (!f_a.ContainsKey(temp))
                            f_a.Add(temp, new List<string> { f });
                        else f_a[temp].Add(f);
                    }

                    if (!coreAttitudes.attitudes.ContainsKey(fo))
                    {
                        coreAttitudes.attitudes.Add(fo, new Dictionary<object, List<string>>(f_a));
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

                if (line.StartsWith("faction_relationships") && !newFaction.dead_until_resurrected)
                {
                    string[] split = line.Split('\t', ',');

                    split = split.CleanStringArray();

                    int count = split.Count();
                    count -= 3; //amount of faction entries required

                    string fo = split[1];

                    Dictionary<object, List<string>> f_a = new Dictionary<object, List<string>>();
                    for (int i = 0; i < count; i++)
                    {
                        int temp = 0;
                        if (split[2].Trim() == "at_war_with")
                            temp = 600;
                        else if (split[2].Trim() == "allied_to")
                            temp = 0;
                        else temp = Convert.ToInt32(split[2]);
                        string f = split[i + 3];
                        if (!f_a.ContainsKey(temp))
                            f_a.Add(temp, new List<string> { f });
                        else f_a[temp].Add(f);
                    }

                    if (!factionRelationships.attitudes.ContainsKey(fo))
                    {
                        factionRelationships.attitudes.Add(fo, new Dictionary<object, List<string>>(f_a));
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

            foreach (RemasterResource res in resources)
            {
                output += res.Output();
            }

            output += "\r\n\r\n";

            foreach (Faction fac in factions)
            {
                output += fac.Output();
            }

            output += "\r\n";

            
            output += coreAttitudes.OutputMulti(is_bi);
            output += factionRelationships.OutputSingle(is_bi);
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
            int spqrIndex = factions.FindIndex((x => ((Faction)x).name == LookUpTables.dic_factions[FactionOwnership.romans_senate]));
            factions.RemoveAt(spqrIndex);

            int rjInd = factions.FindIndex(x => ((Faction)x).name == LookUpTables.dic_factions[FactionOwnership.romans_julii]);
            int rbInd = factions.FindIndex(x => ((Faction)x).name == LookUpTables.dic_factions[FactionOwnership.romans_brutii]);
            int rsInd = factions.FindIndex(x => ((Faction)x).name == LookUpTables.dic_factions[FactionOwnership.romans_scipii]);

            ((Faction)factions[rjInd]).superFaction = "";
            ((Faction)factions[rbInd]).superFaction = "";
            ((Faction)factions[rsInd]).superFaction = "";

            spqrIndex = campaignNonPlayable.FindIndex(x => x == LookUpTables.dic_factions[FactionOwnership.romans_senate]);
            campaignNonPlayable.RemoveAt(spqrIndex);

            RemoveSenateRelations(ref coreAttitudes);
            RemoveSenateRelations(ref factionRelationships);
        }
        public void ShuffleFactions(Random rnd)
        {
            factions.Shuffle(rnd);
        }
        void RemoveSenateRelations(ref CoreAttitudes<int> coreA)
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
        public void MoveFactionToTopOfStrat(string name)
        {
            UnlockFactions();

            int index = playableFactions.FindIndex(x => x == name);
            var temp = playableFactions[index];
            playableFactions.RemoveAt(index);
            campaignNonPlayable = new List<string>(playableFactions);
            playableFactions.Clear();
            playableFactions.Add(temp);
        }
        /// <summary>
        /// Make all cities level 1 with basic buildings
        /// </summary>
        public void CleanDS()
        {
            foreach (var fac in factions)
                foreach (var settlement in fac.settlements)
                {
                    settlement.s_level = "village";
                    settlement.population = 1000;
                    settlement.b_types.Clear();
                }
        }

        public void UnlockFactions()
        {
            foreach (string p in unlockableFactions)
                playableFactions.Add(p);

            unlockableFactions.Clear();

            foreach (string p in campaignNonPlayable)
                playableFactions.Add(p);

            campaignNonPlayable.Clear();
        }

        public int GetArmyCount(int facIndex)
        {
            int count = 0;

            if (facIndex > -1 && facIndex < factions.Count())
            {
                foreach (DSCharacter character in ((Faction)factions[facIndex]).characters)
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
                foreach (DSCharacter character in ((Faction)factions[facIndex]).characters)
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
                foreach (DSCharacter character in ((Faction)factions[facIndex]).characters)
                {
                    if (character.type == "admiral")
                    {
                        count++;
                    }
                }
            }

            return count;

        }
        public void CleanUp()
        {
            foreach (Faction faction in factions)
            {
                for (int c = 0; c < faction.characters.Count; c++)
                {
                    if (faction.characters.Count > 0)
                    {
                        var character = ((DSCharacter)faction.characters[c]);
                        if (character.name == null || character.type == null)
                            faction.characters.RemoveAt(c);
                    }
                }
            }
        }
    }
}
