using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2Faction : Faction
    {
        public string ai_label;
        public int kings_purse;

        public M2Faction()
        { 
        
        
        }

        public M2Faction(M2Faction faction) : base(faction)
        {
            this.ai_label = faction.ai_label;
            this.kings_purse = faction.kings_purse;
        }

        override public string Output()
        {
            string output = "";

            output +=
                "faction\t" + name + ", " + ai[0] + " " + ai[1] + "\r\n" +
                "ai_label\t\t" + ai_label + "\r\n" +
                "denari\t" + denari.ToString() + "\r\n" +
                "denari_kings_purse\t" + kings_purse.ToString() + "\r\n";

            foreach (Settlement settlement in settlements)
            {
                output += settlement.outputSettlement();

                output += "\r\n";

            }

            output += "\r\n";

            foreach (M2DSCharacter character in characters)
            {
                output += ((M2DSCharacter)character).Output();
                output += "\r\n";
            }

            output += "\r\n";
            foreach (M2CharacterRecord rec in characterRecords)
            {
                output += rec.Output();

            }
            output += "\r\n";

            foreach (string rel in relatives)
            {
                output += "relative \t" + rel + "\r\n";
            }

            output += "\r\n";
            return output;
        }
    }
}
