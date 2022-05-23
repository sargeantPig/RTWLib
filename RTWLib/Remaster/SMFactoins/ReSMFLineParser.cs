using RTWLib.Data;
using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.Remaster
{
    public partial class ReSMFactions
    { 
        int depth = 0;
        string prevLine = string.Empty;
        protected bool ParseLine(string line, Dictionary<string, ReSMFaction> details)
        {
            string[] data = line.GetSubStr(':').Split('\t', ',', ' ').CleanStringArray();
            //string firstWord = line.GetFirstWord(null, 0, ' ').Trim('/', ' ');

            string firstWord = line.GetQuotedWord();
            if (firstWord.StartsWith("{"))
            {
                depth++;
            }
            else if (firstWord.StartsWith("}"))
            {
                depth--;
            }
            else if (firstWord.StartsWith(";"))
                return false;

            string clean = firstWord;

            if (depth == 0 && (prevLine == "[" || prevLine == "}," ))
            {// new faction
                details.Add(clean, new ReSMFaction());
                details.Last().Value.faction = new ReSMFBase<string>();
                details.Last().Value.faction.ProcessFaction(firstWord);
            }

            if (depth == 1)
            {
                switch(clean)
                {
                    case "string":
                        details.Last().Value.name = new ReSMFBase<string>();
                        details.Last().Value.name.ProcessLine(clean, data);
                        break;
                    case "description":
                        details.Last().Value.descr = new ReSMFBase<string>();
                        details.Last().Value.descr.ProcessLine(clean, data);
                        break;
                    case "culture":
                        details.Last().Value.culture = new ReSMFBase<string>();
                        details.Last().Value.culture.ProcessLine(clean, data);
                        break;
                    case "ethnicity":
                        details.Last().Value.ethnicity = new ReSMFBase<string>();
                        details.Last().Value.ethnicity.ProcessLine(clean, data);
                        break;
                    case "tags":
                        details.Last().Value.tags = new ReSMFBase<object[]>();
                        details.Last().Value.tags.ProcessLine(clean, data);
                        break;
                    case "available in custom battles":
                        details.Last().Value.custBattleAvailable = new ReSMFBase<bool>();
                        details.Last().Value.custBattleAvailable.ProcessLine(clean, data);
                        break;
                    case "prefer naval invasions":
                        details.Last().Value.navalInvader = new ReSMFBase<bool>();
                        details.Last().Value.navalInvader.ProcessLine(clean, data);
                        break;
                    case "default battle ai personality":
                        details.Last().Value.aiPersona = new ReSMFBase<string>();
                        details.Last().Value.aiPersona.ProcessLine(clean, data);
                        break;
                    case "allow reproduction":
                        details.Last().Value.reproduction = new ReSMFBase<bool>();
                        details.Last().Value.reproduction.ProcessLine(clean, data);
                        break;
                }
            }

            if(depth == 2)
            {
                switch (clean)
                {
                    case "men":
                        details.Last().Value.men = new ReSMFBase<string>();
                        details.Last().Value.men.ProcessLine(clean, data);
                        break;
                    case "women":
                        details.Last().Value.women = new ReSMFBase<string>();
                        details.Last().Value.women.ProcessLine(clean, data);
                        break;
                    case "surnames":
                        details.Last().Value.surnames = new ReSMFBase<string>();
                        details.Last().Value.surnames.ProcessLine(clean, data);
                        break;
                    case "loading screen icon":
                        details.Last().Value.loadScrIcon = new ReSMFBase<string>();
                        details.Last().Value.loadScrIcon.ProcessLine(clean, data);
                        break;
                    case "standard index":
                        details.Last().Value.stdIndex = new ReSMFBase<int>();
                        details.Last().Value.stdIndex.ProcessLine(clean, data);
                        break;
                    case "rebel standard index":
                        details.Last().Value.rebStdInd = new ReSMFBase<int>();
                        details.Last().Value.rebStdInd.ProcessLine(clean, data);
                        break;
                    case "logo index":
                        details.Last().Value.logoInd = new ReSMFBase<int>();
                        details.Last().Value.logoInd.ProcessLine(clean, data);
                        break;
                    case "rebel logo index":
                        details.Last().Value.rebLogoInd = new ReSMFBase<int>();
                        details.Last().Value.rebLogoInd.ProcessLine(clean, data);
                        break;
                    case "strat symbol model":
                        details.Last().Value.stratSymModel = new ReSMFBase<string>();
                        details.Last().Value.stratSymModel.ProcessLine(clean, data);
                        break;
                    case "strat rebel symbol model":
                        details.Last().Value.sratRebSymModel = new ReSMFBase<string>();
                        details.Last().Value.sratRebSymModel.ProcessLine(clean, data);
                        break;
                    case "primary":
                        details.Last().Value.factionColour = new MapColour();
                        details.Last().Value.factionColour.ProcessLine(data, 0);
                        break;
                    case "secondary":
                        details.Last().Value.factionColour.ProcessLine(data, 1);
                        break;
                    case "intro":
                        details.Last().Value.intro = new ReSMFBase<string>();
                        details.Last().Value.intro.ProcessLine(clean, data);
                        break;
                    case "victory":
                        details.Last().Value.victory = new ReSMFBase<string>();
                        details.Last().Value.victory.ProcessLine(clean, data);
                        break;
                    case "defeat":
                        details.Last().Value.defeat = new ReSMFBase<string>();
                        details.Last().Value.defeat.ProcessLine(clean, data);
                        break;
                }
            }

            if (depth == 3)
            { 
                switch(clean)
                {
                    case "background":
                        details.Last().Value.familyTree = new MapColour(4);
                        details.Last().Value.familyTree.ProcessLine(data, 0);
                        break;
                    case "font":
                        details.Last().Value.familyTree.ProcessLine(data, 1);
                        break;
                    case "selected line":
                        details.Last().Value.familyTree.ProcessLine(data, 2);
                        break;
                    case "unselected line":
                        details.Last().Value.familyTree.ProcessLine(data, 3);
                        break;

                }

            }

            prevLine = line.Trim('\t');
            return true;

        }

    }
}
