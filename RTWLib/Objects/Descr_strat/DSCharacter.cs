using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{
    public class DSCharacter
    {
        public string sub_faction, name, type, rank, traits, ancillaries;
        public int age;
        public int[] coords = new int[2];
        public List<DSUnit> army = new List<DSUnit>();

        public DSCharacter()
        {
        }

        public DSCharacter(string Name, Random rnd)
        {
            name = Name;
            type = "general";
            age = rnd.Next(20, 50);
        }

        public DSCharacter(DSCharacter character)
        {
            sub_faction = character.sub_faction;
            name = character.name;
            type = character.type;
            rank = character.rank;
            traits = character.traits;
            ancillaries = character.ancillaries;
            age = character.age;
            coords[0] = character.coords[0];
            coords[1] = character.coords[1];
            army = new List<DSUnit>(character.army);
        }

        virtual public string Output()
        {
            string output = "";

            output +=
                "character\t" + name + ", " + type;


            if (type == "spy" || type == "diplomat" || rank == null || type == "admiral")
            {
                output += ", age " + age.ToString() + ", , x " + coords[0].ToString() + ", y " + coords[1].ToString() + "\r\n";

                if (traits != null && traits != "")
                    output += "traits " + traits + "\r\n";

                if (ancillaries != null && ancillaries != "")
                    output += "ancillaries " + ancillaries + "\r\n";

                if (type == "admiral" || type == "named character" || type == "general")
                {
                    output += "army\r\n";
                    foreach (DSUnit str in army)
                    {
                        output += str.Output();
                    }
                }

                return output;
            }
            output += ", " + rank + ", age " + age.ToString() + ", , x " + coords[0].ToString() + ", y " + coords[1].ToString() + "\r\n";



            output += "traits " + traits + "\r\n" +
                "ancillaries " + ancillaries + "\r\n" +
                "army\r\n";

            foreach (DSUnit str in army)
            {
                output += str.Output();
            }

            return output;

        }
    }
}
