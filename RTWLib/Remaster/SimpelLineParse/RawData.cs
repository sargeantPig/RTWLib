using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Extensions;
namespace RTWLib.Functions.Remaster
{
    public class RawData
    {

        public string tag, prefix, suffix = string.Empty;
        public string[] array;
        public bool group = false;
        List<string> data = new List<string>();
        public RawData() { }

        public void ReadData(string line, char sep, char arrOpen, char arrClose, string next, int depth)
        {
            List<string> list = new List<string>();
            int tagsRead = 0;
            bool open = true;
            string current = string.Empty;
            char prev = '?';
            bool quotes = false;
            bool isGroup;
            int preInd = -1, suffInd = -1, dataInd = 1;
            int[] arrayInd = new int[2] { -1, -1 };


            foreach (char c in line)
            {
                if (c == '"')
                    quotes = !quotes;

                if ((c == sep || c == ',') && prev != c && !quotes)
                {
                    if(current != "")
                        list.Add(current.Trim());
                    current = string.Empty;
                    tagsRead++;

                    if (list.Last() == "and")
                        suffInd = list.Count;

                    if (list.Last() == "requires")
                        preInd = list.Count;
                    continue;
                }

                if (c == arrOpen)
                {
                    arrayInd[0] = list.Count;
                    continue;
                }
                if (c == arrClose)
                {
                    arrayInd[1] = list.Count;
                    continue;
                }
                current += c;
                prev = c;

            }

            if (current != string.Empty)
                list.Add(current);

            list = list.ToArray().CleanStringArray().ToList();
            tag = list[0];

            int dataStop = 0;

            if (preInd > -1)
            {
                dataInd = preInd + 1;
                prefix = list[preInd - 1] + " " + list[preInd];
            }
            if (suffInd > -1)
            {
                dataStop = suffInd - 1;
                suffix = list[suffInd - 1] + " " + list[suffInd];
            }
            if (arrayInd[0] > -1)
            {
                array = new string[arrayInd[1] - arrayInd[0]];
                int ab = 0;
                for (int i = arrayInd[0]; i < arrayInd[1]; i++)
                {
                    array[ab] = list[i];
                    ab++;
                }
                dataStop = arrayInd[0] - 1;
            }
            if (dataStop == 0)
                dataStop = list.Count() - 1;

            if (dataStop >= dataInd)
            {
                for (int i = dataInd; i <= dataStop; i++)
                {
                    this.data.Add(list[i]);

                }
            }
        } 
    }
            
}


