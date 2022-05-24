using RTWLib.Extensions;
using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB
{
    public class Skeletons : IdmbOutput
    {
        const string tag = "skeleton";
        const string horse = "_horse";
        const string elephant = "_elephant";
        const string chariot = "_chariot";
        const string camel = "_camel";
        List<string> skeleton;
        List<string> skeleHorse;
        List<string> skeleElephant;
        List<string> skeleChariot;
        List<string> skeleCamel;

        public Skeletons()
        { }

        public void ProcessLine(string ident, string[] data)
        {
            switch (ident)
            {
                case tag:
                    AddSkeletons(data, ref skeleton);
                    break;
                case tag + horse:
                    AddSkeletons(data, ref skeleHorse);
                    break;
                case tag + elephant:
                    AddSkeletons(data, ref skeleElephant);
                    break;
                case tag + chariot:
                    AddSkeletons(data, ref skeleChariot);
                    break;
                case tag + camel:
                    AddSkeletons(data, ref skeleCamel);
                    break;

            }
        }

        public string GetWeaponType(Unit unit, bool isPrimary, bool isHandler, string engine = "")
        {
            int index = 0;
            if (!isPrimary) index = 1;

            if (index >= skeleton.Count())
            {
                if (skeleton[index - 1].Contains("chariot"))
                    return "Chariot";
                else if (isHandler)
                    return "Animal";
                else if (engine != null)
                    return engine.Capitalise();
                else if (skeleton[index-1].Contains("elephant"))
                    return "Elephant";
                else return "";
            }

            if (skeleton[index].Contains("sword")) return "Sword";
            else if (skeleton[index].Contains("spear")) return "Spear";
            else if (skeleton[index].Contains("dagger")) return "Dagger";
            else if (skeleton[index].Contains("javelin")) return "Javelin";
            else if (skeleton[index].Contains("sling")) return "Sling";
            else if (skeleton[index].Contains("archer")) return "Bow";
            else if (skeleton[index].Contains("berserker")) return "Spiked Club";
            else if (skeleton[index].Contains("2handed")) return "2h Melee";
            else if (unit.priWep.TechFlags == Data.TechType.archery) return "Bow";
            else return "";
        }

        public string GetSpeed(bool isPrimary)
        {
            int index = 0;
            if (!isPrimary) index = 1;

            if (index >= skeleton.Count())
                return "";

            if (skeleton[index].Contains("slow"))
                return "slow";
            else if (skeleton[index].Contains("fast")) return "fast";
            else return "";
        }

        void AddSkeletons(string[] data, ref List<string> skellies)
        {
            if (skellies == null)
                skellies = new List<string>();
            foreach (var d in data)
                skellies.Add(d);
        
        }

        public string Output()
        {
            string str = ""; 

            if (skeleton != null)
                str += string.Format("{0}{1}{2}", tag, StrFo.GetNewWhiteSpace(tag),
                    skeleton.ToArray().ArrayToString(false, false, true, 0)).CRL();
            if(skeleHorse != null)
                str += string.Format("{0}{1}{2}{3}", tag, horse, StrFo.GetNewWhiteSpace(tag + horse),
                    skeleHorse.ToArray().ArrayToString(false, false, true, 0)).CRL();
            if (skeleElephant != null)
                str += string.Format("{0}{1}{2}{3}", tag, elephant, StrFo.GetNewWhiteSpace(tag + elephant),
                   skeleElephant.ToArray().ArrayToString(false, false, true, 0)).CRL();
            if(skeleChariot != null)
                str += string.Format("{0}{1}{2}{3}", tag, chariot, StrFo.GetNewWhiteSpace(tag + chariot),
                    skeleChariot.ToArray().ArrayToString(false, false, true, 0)).CRL();
            if (skeleCamel != null)
                str += string.Format("{0}{1}{2}{3}", tag, camel, StrFo.GetNewWhiteSpace(tag + camel),
                    skeleCamel.ToArray().ArrayToString(false, false, true, 0)).CRL();

            return str;
        
        }


    }


}
