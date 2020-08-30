using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    public class M2DSCharacter : DSCharacter
    {
        public string gender;

        public M2DSCharacter(M2DSCharacter character) : base(character)
        {
            this.gender = character.gender;        
        }


        public M2DSCharacter(string Name, Random rnd)
        {
            name = Name;
            type = "general";
            age = rnd.Next(20, 50);
        }

        public M2DSCharacter() : base()
        { }

        override public string Output()
        {
            string output = "";

            output +=
                "character\t" + name + ", " + type + ", " + gender;


            if (type == "spy" || type == "diplomat" || rank == null || type == "admiral")
            {
                output += ", age " + age.ToString() + ", x " + coords[0].ToString() + ", y " + coords[1].ToString() + " \r\n";

                if (traits != null && traits != "")
                    output += "traits " + traits.Trim() + " \r\n";

                if (ancillaries != null && ancillaries != "")
                    output += "ancillaries " + ancillaries.Trim() + "\r\n";

                if (type == "admiral" || type == "named character" || type == "general")
                {
                    output += "army\r\n";
                    foreach (DSUnit str in army)
                    {
                        output += str.Output("weapon_lvl");
                    }
                }
                return output;
            }

            if(rank != null)
                output += ", " + rank + ", age " + age.ToString() + ", x " + coords[0].ToString() + ", y " + coords[1].ToString() + " \r\n";
            else output += ", age " + age.ToString() + ", x " + coords[0].ToString() + ", y " + coords[1].ToString() + " \r\n";


            if (traits != null && traits != "")
                output += "traits " + traits.Trim() + " \r\n";

            if (ancillaries != null && ancillaries != "")
                output += "ancillaries " + ancillaries.Trim() + "\r\n";


            output += "\r\narmy\r\n";

            foreach (DSUnit str in army)
            {
                output += str.Output("weapon_lvl");
            }

            return output;

        }

    }
}
