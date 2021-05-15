using LibNoise.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Extensions;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;

namespace RTWLib.Functions
{
    public class DataString
    {
        Dictionary<string, string> attributes = new Dictionary<string, string>();
        Dictionary<string, int> attributeLineNumbers = new Dictionary<string, int>();
        string[] lines;
        public char[] dataTags;
        public string[] dataNames;
        public string Format { get; set; }

        public char commentTag;

        int locX;
        int locY;
        public DataString(string data, char[] tags, string[] tagnames, char commentTag = '¬')
        {
            this.dataTags = tags;
            this.dataNames = tagnames;
            this.commentTag = commentTag;
            lines = data.Split('\n');
        }

        public DataString()
        {
        }

        public DataString(string data, DataString meta)
        {
            this.dataTags = meta.dataTags;
            this.dataNames = meta.dataNames;
            this.commentTag = meta.commentTag;
            lines = data.Split('\n','\r');
        }

        public void CombineDataPoints()
        {
            if (dataTags == null)
                dataTags = new char[0];
            if (dataNames == null)
                dataNames = new string[0];

            List<string> combined = new List<string>();
            int index = -1;
            foreach (string str in lines)
            {
                if (str.Contains(dataNames.ToList())
                    || str.Contains(dataTags.ToList())
                    || str.Contains(commentTag))
                {
                    combined.Add(str);
                    index++;
                }
                else if(str != string.Empty)
                {
                    combined[index] += str.CRL();
                }
            }

            lines = combined.PigsToArray().CleanStringArray(false);
        }


        public void FormulateAttributes(char[] splitChars, params char[] trimChars)
        {
            if (dataTags == null)
                dataTags = new char[0];
            if (dataNames == null)
                dataNames = new string[0];
            int index = -1;
            foreach (string str in lines)
            {
                index++;
                if (str.Contains(dataNames.ToList())
                    || str.Contains(dataTags.ToList()))
                {
                    string[] split = str.Split(splitChars);
                    split[0] = split[0].Trim(trimChars);

                    if (ContainsAttribute(split[0]))
                    {
                        attributes.Add(split[0] + "#", split[1]);
                        attributeLineNumbers.Add(split[0] + "#", index);
                    }
                    else
                    {
                        attributes.Add(split[0], split[1]);
                        attributeLineNumbers.Add(split[0], index);
                    }
                }
                
            }

        }

        public void EditAttribute(string name, string value, bool append = false)
        {
            if (!ContainsAttribute(name))
            { return; }

            if (!append)
                attributes[name] = value;
            else attributes[name] += value;

            UpdateLines(name, value, append);
        }

        void UpdateLines(string name, string value, bool append)
        {
            int lineNumber = attributeLineNumbers[name];

            if (!append)
            {
                lines[lineNumber] = String.Format(Format, name, value);
            }
            else lines[lineNumber] = string.Format(Format, name, attributes[name].Trim() + value);
        }

        public string _ConsoleOutputAttribute(string name)
        {
            if (!ContainsAttribute(name))
                return "error";

            Console.WriteLine(lines[attributeLineNumbers[name]]);

            return lines[attributeLineNumbers[name]]; 

        }

        bool ContainsAttribute(string name)
        {
            if (!attributes.ContainsKey(name))
            { Console.WriteLine("No key Found"); return false; }
            else return true;
        }

        public string Output()
        {
            return lines.ArrayToString(false, true, false, 1, false);
        }

        public void RemoveAttribute(string name)
        {
            if (!ContainsAttribute(name))
                return;

            int lineNo = attributeLineNumbers[name];
            lines[lineNo] = "";
            lines.CleanStringArray();

        } 

        public void AddAttribute(string name, string value)
        {
            if (ContainsAttribute(name))
                return;

            List<string> lst = lines.ToList();
            attributes.Add(name, value);
            attributeLineNumbers.Add(name, lst.Count);
            lst.Add(string.Format(Format, name, value));
            lines = lst.PigsToArray();
        }
    }
}
