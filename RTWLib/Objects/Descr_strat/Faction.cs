using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class Faction
    {
        public string name { get; set; }
        public string[] ai = new string[2];
        public string superFaction { get; set; }
        public List<Settlement> settlements = new List<Settlement>();
        public List<DSCharacter> characters = new List<DSCharacter>();
        public List<CharacterRecord> characterRecords = new List<CharacterRecord>();
        public List<string> relatives = new List<string>();
        public int denari { get; set; }

        public Faction()
        { }

        public Faction(Faction faction)
        {
            name = faction.name;
            ai[0] = faction.ai[0];
            ai[1] = faction.ai[1];
            superFaction = faction.superFaction;
            settlements = new List<Settlement>(faction.settlements);
            characters = new List<DSCharacter>(faction.characters);
            characterRecords = new List<CharacterRecord>(faction.characterRecords);
            relatives = new List<string>(faction.relatives);
            denari = faction.denari;


        }

        public void Clear()
        {
            name = "";
            ai[0] = "";
            ai[1] = "";
            superFaction = "";
            settlements.Clear();
            characters.Clear();
            characterRecords.Clear();
            relatives.Clear();
        }

        public string Output()
        {
            string output = "";

            output +=
                "faction\t" + name + ", " + ai[0] + " " + ai[1] + "\r\n" +
                "superfaction " + superFaction + "\r\n" +
                "denari\t" + denari.ToString() + "\r\n";

            foreach (Settlement settlement in settlements)
            {
                output += settlement.outputSettlement();

                output += "\r\n";

            }

            output += "\r\n";

            foreach (DSCharacter character in characters)
            {
                output += character.Output();
                output += "\r\n";
            }

            foreach (CharacterRecord rec in characterRecords)
            {
                output += rec.Output();
            }

            foreach (string rel in relatives)
            {
                output += "relative \t" + rel + "\r\n";
            }

            return output;
        }
    }
}
