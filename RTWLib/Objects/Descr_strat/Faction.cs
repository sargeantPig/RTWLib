using RTWLib.Medieval2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public interface IFaction
    {
        string name { get; set; }
        string[] ai { get; set; }
        string superFaction { get; set; }
        List<ISettlement> settlements { get; set; }
        List<object> characters { get; set; }
        List<ICharacterRecord> characterRecords { get; set; }
        List<string> relatives { get; set; }
        int denari { get; set; }
    }


    public class Faction : IFaction
    {
        public string name { get; set; }
        public string[] ai { get; set; }
        public string superFaction { get; set; }
        public List<ISettlement> settlements { get; set; }
        public List<object> characters { get; set; }
        public List<ICharacterRecord> characterRecords { get; set; }
        public List<string> relatives { get; set; }
        public int denari { get; set; }

        public Faction()
        {
            ai = new string[2];
            settlements = new List<ISettlement>();
            characters = new List<object>();
            characterRecords = new List<ICharacterRecord>();
            relatives = new List<string>();

        
        }

        public Faction(Faction faction)
        {
            ai = new string[2];
            name = faction.name;
            ai[0] = faction.ai[0];
            ai[1] = faction.ai[1];
            superFaction = faction.superFaction;
            settlements = new List<ISettlement>(faction.settlements);

            characters = new List<object>(faction.characters);

            characterRecords = new List<ICharacterRecord>(faction.characterRecords);
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

        virtual public string Output()
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
                output += "\r\n\r\n";
            }
            
            
            foreach (string rel in relatives)
            {
                output += "relative \t" + rel + "\r\n";
            }

            return output;
        }
    }
}
