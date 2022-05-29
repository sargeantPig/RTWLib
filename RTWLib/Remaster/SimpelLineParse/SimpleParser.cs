using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Functions.Remaster
{
    public partial class FileWrapper
    {
        int depth = 0;
        int item = 0;
        string prevLine = string.Empty;

        protected bool Parse(string line, List<Base<string>> det)
        {
            string data = string.Empty;
            if (!quoteTags)
                data = line.GetSubStr(delim, false);//.Split('\t', ',', ' ').CleanStringArray();
            else data = line.GetSubStr(delim);


            if (data.Contains(";"))
                data = data.Split(';')[0];
            //string firstWord = line.GetFirstWord(null, 0, ' ').Trim('/', ' ');
            string firstWord = string.Empty;
            if (quoteTags)
                firstWord = line.GetQuotedWord();
            else firstWord = line.Split(delim)[0];
            if (firstWord.StartsWith("{") || line == "[")
            {
                depth++;
                return false;
            }
            else if (firstWord.StartsWith("}") || firstWord.StartsWith("]"))
            {
                depth--;
                return false;
            }
            else if (firstWord.StartsWith(";") || line == "" || line == "\t" || line == " ")
                return false;

            firstWord.TrimStart(' ');
            data = data.Trim();

            bool isArray = false;
            string[] sepData;
            if (data.Contains(arrDelim))
            {
                isArray = true;

                if (!noDepth)
                {
                    string attri = data.GetSubStr(openArray, true, closeArray);
                    if (attri.Contains("\""))
                        sepData = attri.GetQuotedWords().ToArray();
                    else sepData = attri.Split(',').CleanStrings().CleanStringArray();
                }
                else  sepData = data.Split(arrDelim).CleanStrings().CleanStringArray();

            }
            else sepData = new string[1] { data };

            if (data.Count() == 0 || identifier == firstWord)
            {
                if (det.Count == 0)
                {
                    det.Add(new Group<string>());
                    det.Last().ProcessFaction(firstWord, depth, sepData);
                    return true;
                }

                if (!noDepth)
                {
                    var li = GetObjectByDepth(det);
                    li.Add(new Group<string>());
                    li.Last().ProcessFaction(firstWord, depth, sepData);
                }
                else
                {
                    var li = det;
                    li.Add(new Group<string>());
                    li.Last().ProcessFaction(firstWord, depth, sepData);
                }
            }

            else
            {
                if (sepData.Count() == 1 && !isArray)
                {
                    var li = GetObjectByDepth(det);
                    li.Add(new Base<string>());
                    li.Last().ProcessLine(firstWord, sepData, depth);
                }
                else
                {
                    var li = GetObjectByDepth(det);
                    li.Add(new Array<string>());
                    ((Array<string>)li.Last()).ProcessLine(firstWord, sepData, depth);
                }
            }
            prevLine = line.Trim('\t');
            return true;

        }

        protected List<Base<string>> GetObjectByDepth(List<Base<string>> det, int curDepth = 0)
        {
            int d = depth;

            if (d == curDepth)
                if (noDepth)
                    return ((Group<string>)det[det.Count - 1]).objects;
                else return det;

            for (int i = det.Count - 1; i >= 0; i--)
            {
                if (typeof(Group<string>) == det[i].GetType())
                {
                    curDepth++;
                    return GetObjectByDepth(((Group<string>)det[i]).objects, curDepth);
                }
            }
            return null;
        }


    }
}
