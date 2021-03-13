using RTWLib.Data;
using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.UnitDescriptions
{
    public partial class UnitDescrs
    {
        protected bool ParseSegment(string line, List<UnitDescription> unitDescr, StreamReader sr)
        {

            string[] data = LibFuncs.RemoveFirstWord(line, '\t').Trim().TrimEnd(',').Split('\t', ',').CleanStringArray();
            string firstWord = LibFuncs.GetFirstWord(line, null, 0, '\t').Trim();


            if (!firstWord.Contains("{"))
                return false;

            unitDescr.Add(new UnitDescription());
            UnitDescription u = unitDescr.Last();
            u.internalName = firstWord.Trim('{', '}');
            u.externalName = data[0].Trim();
            string ident = firstWord.TrimEnd('}');
            string LD = ident + "_descr}";
            string SD = ident + "_descr_short}";
            u.LDTag = LD;
            u.SDTag = SD;
            u.nameTag = firstWord;
            while ((line = sr.ReadLine().Trim()) != UnitDescription.div)
            {
                if (line == SD)
                {
                    line = sr.ReadLine();
                    u.shortDescr = line;
                }
                else if (line == LD)
                {
                    line = sr.ReadLine();
                    u.longDescr = line;
                }
            }

            return true;
        }

    }
}
