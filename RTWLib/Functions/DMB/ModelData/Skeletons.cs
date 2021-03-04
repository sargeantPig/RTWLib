using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB
{
    public class Skeleton : IdmbOutput
    {
        string tag = "skeleton";
        List<string> names;

        public Skeleton()
        { }

        public void ProcessLine(string[] data)
        {
            names = new List<string>();
            foreach (var d in data)
                names.Add(d);
        }

        public string Output()
        {
            string str = "";

            if (names != null)
                str = string.Format("{0}{1}{2}", tag, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(names.ToArray()));

            return str.CarriageReturnNewLine();
        
        }
    }

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
                    skeleton = new List<string>();
                    AddSkeletons(data, ref skeleton);
                    break;
                case tag + horse:
                    skeleHorse = new List<string>();
                    AddSkeletons(data, ref skeleHorse);
                    break;
                case tag + elephant:
                    skeleElephant = new List<string>();
                    AddSkeletons(data, ref skeleElephant);
                    break;
                case tag + chariot:
                    skeleChariot = new List<string>();
                    AddSkeletons(data, ref skeleChariot);
                    break;
                case tag + camel:
                    skeleCamel = new List<string>();
                    AddSkeletons(data, ref skeleCamel);
                    break;

            }
        }

        void AddSkeletons(string[] data, ref List<string> skellies)
        {
            foreach (var d in data)
                skellies.Add(d);
        
        }

        public string Output()
        {
            string str = "";

            if (skeleton != null)
                str = string.Format("{0}{1}{2}", tag, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(skeleton.ToArray(), false, false, true, 0));
            if(skeleHorse != null)
                str = string.Format("{0}{1}{2}{3}", tag, horse, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(skeleton.ToArray(), false, false, true, 0));
            if (skeleElephant != null)
                str = string.Format("{0}{1}{2}{3}", tag, elephant, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(skeleton.ToArray(), false, false, true, 0));
            if(skeleChariot != null)
                str = string.Format("{0}{1}{2}{3}", tag, chariot, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(skeleton.ToArray(), false, false, true, 0));
            if (skeleCamel != null)
                str = string.Format("{0}{1}{2}{3}", tag, camel, LibFuncs.GetNewWhiteSpace(tag),
                    LibFuncs.ArrayToString(skeleton.ToArray(), false, false, true, 0));

            return str.CarriageReturnNewLine();
        
        }


    }


}
