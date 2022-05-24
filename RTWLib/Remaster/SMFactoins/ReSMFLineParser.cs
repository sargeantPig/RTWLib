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
        int item = 0;
        string prevLine = string.Empty;

        protected bool Parse(string line, List<ReSMFBase<string>> det)
        {
            string data = line.GetSubStr(':');//.Split('\t', ',', ' ').CleanStringArray();
           
            //string firstWord = line.GetFirstWord(null, 0, ' ').Trim('/', ' ');
            string firstWord = line.GetQuotedWord();

            if (firstWord.StartsWith("{") || line == "[") {
                depth++;
                return false;
            }
            else if (firstWord.StartsWith("}") || firstWord.StartsWith("]")) {
                depth--;
                return false;
            }
            else if (firstWord.StartsWith(";") || line == "" || line == "\t")
                return false;

            bool isArray = false;
            string[] sepData;
            if (data.Contains("["))
            {
                isArray = true;
                string attri = data.GetSubStr('[', ']');
                if (attri.Contains("\""))
                    sepData = attri.GetQuotedWords().ToArray();
                else sepData = attri.Split(',').CleanStrings().CleanStringArray();
            }
            else sepData = new string[1] { data };

            if (data.Count() == 0) {
                if (det.Count == 0)
                {
                    det.Add(new SMFGroup<string>());
                    det.Last().ProcessFaction(firstWord, depth);
                    return true;
                }
                var li = GetObjectByDepth(det);
                li.Add(new SMFGroup<string>());
                li.Last().ProcessFaction(firstWord, depth);
            }

            else {
                if (sepData.Count() == 1 && !isArray)
                {
                    var li = GetObjectByDepth(det);
                    li.Add(new ReSMFBase<string>());
                    li.Last().ProcessLine(firstWord, sepData, depth);
                }
                else
                {
                    var li = GetObjectByDepth(det);
                    li.Add(new MapColour<string>());
                    ((MapColour<string>)li.Last()).ProcessLine(firstWord, sepData, depth);
                }
            }
            prevLine = line.Trim('\t');
            return true;

        }

        protected List<ReSMFBase<string>> GetObjectByDepth(List<ReSMFBase<string>> det, int curDepth = 0)
        {
            if (depth == curDepth)
                return det;

            for(int i = det.Count-1; i >= 0; i--) {
                if (typeof(SMFGroup<string>) == det[i].GetType())
                {
                    curDepth++;
                    return GetObjectByDepth(((SMFGroup<string>)det[i]).objects, curDepth);
                }
            }
            return null;
        }
    }
}
