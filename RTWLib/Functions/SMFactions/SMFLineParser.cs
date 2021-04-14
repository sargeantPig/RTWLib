using RTWLib.Data;
using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    public partial class SMFactions
    {
        protected bool ParseLine(string line, Dictionary<string, SMFaction> details)
        {
            string[] data = line.RemoveFirstWord('\t').Trim().TrimEnd(',').Split('\t', ',').CleanStringArray();
            string firstWord = line.GetFirstWord(null, 0, '\t').Trim();

            switch (firstWord)
            {
                case "faction":
                    details.Add(data[0], new SMFaction());
                    details.Last().Value.faction = new SMFBase<string>();
                    details.Last().Value.faction.ProcessLine(firstWord, data);
                    break;
                case "culture":
                    details.Last().Value.culture = new SMFBase<string>();
                    details.Last().Value.culture.ProcessLine(firstWord, data);
                    break;
                case "symbol":
                    details.Last().Value.symbol = new SMFBase<string>();
                    details.Last().Value.symbol.ProcessLine(firstWord, data);
                    break;
                case "rebel_symbol":
                    details.Last().Value.rebelSymbol = new SMFBase<string>();
                    details.Last().Value.rebelSymbol.ProcessLine(firstWord, data);
                    break;
                case "primary_colour":
                    details.Last().Value.factionColour = new MapColour();
                    details.Last().Value.factionColour.ProcessLine(data, 0);
                    break;
                case "secondary_colour":
                    details.Last().Value.factionColour.ProcessLine(data, 1);
                    break;
                case "loading_logo":
                    details.Last().Value.loadingLogo = new SMFBase<string>();
                    details.Last().Value.loadingLogo.ProcessLine(firstWord, data);
                    break;
                case "standard_index":
                    details.Last().Value.stdIndex = new SMFBase<int>();
                    details.Last().Value.stdIndex.ProcessLine(firstWord, data);
                    break;
                case "logo_index":
                    details.Last().Value.logoInd = new SMFBase<int>();
                    details.Last().Value.logoInd.ProcessLine(firstWord, data);
                    break;
                case "small_logo_index":
                    details.Last().Value.smallLogoInd = new SMFBase<int>();
                    details.Last().Value.smallLogoInd.ProcessLine(firstWord, data);
                    break;
                case "triumph_value":
                    details.Last().Value.triumphVal = new SMFBase<int>();
                    details.Last().Value.triumphVal.ProcessLine(firstWord, data);
                    break;
                case "intro_movie":
                    details.Last().Value.introMovie = new SMFBase<string>();
                    details.Last().Value.introMovie.ProcessLine(firstWord, data);
                    break;
                case "victory_movie":
                    details.Last().Value.victoryMovie = new SMFBase<string>();
                    details.Last().Value.victoryMovie.ProcessLine(firstWord, data);
                    break;
                case "defeat_movie":
                    details.Last().Value.defeatMovie = new SMFBase<string>();
                    details.Last().Value.defeatMovie.ProcessLine(firstWord, data);
                    break;
                case "death_movie":
                    details.Last().Value.deathmovie = new SMFBase<string>();
                    details.Last().Value.deathmovie.ProcessLine(firstWord, data);
                    break;
                case "custom_battle_availability":
                    details.Last().Value.custBattleAvailable = new SMFBase<bool>();
                    details.Last().Value.custBattleAvailable.ProcessLine(firstWord, data);
                    break;
                case "can_sap":
                    details.Last().Value.canSap = new SMFBase<bool>();
                    details.Last().Value.canSap.ProcessLine(firstWord, data);
                    break;
                case "prefers_naval_invasions":
                    details.Last().Value.navalInvader = new SMFBase<bool>();
                    details.Last().Value.navalInvader.ProcessLine(firstWord, data);
                    break;
            }

            return true;
        }

    }
}
