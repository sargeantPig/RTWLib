﻿using RTWLib.Medieval2;
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
        bool dead_until_resurrected { get; set; }
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

        public bool dead_until_resurrected { get; set; } = false;

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
            dead_until_resurrected = faction.dead_until_resurrected;
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
                "faction\t" + name + ", " + ai[0] + "\r\n";

            if(superFaction != "")
               output += "superfaction " + superFaction + "\r\n";
            if (dead_until_resurrected)
                output += "dead_until_resurrected" + "\r\n";

            output += "denari\t" + denari.ToString() + "\r\n";

            if (dead_until_resurrected)
                return output;

            foreach (Settlement settlement in settlements)
            {
                output += settlement.outputSettlement();

                output += "\r\n";
            }

            output += "\r\n";

            
            foreach (DSCharacter character in characters)
            {
                if (character.age == 0)
                    continue;
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
