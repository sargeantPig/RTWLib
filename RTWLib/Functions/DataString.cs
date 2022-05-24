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
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RTWLib.Functions
{
    public class FileData
    {
        public Dictionary<string, Dictionary<string, Attribute>> attributes = new Dictionary<string, Dictionary<string, Attribute>>();
        Dictionary<string, Dictionary<string, int>> attributeLineNumbers = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> dc = new Dictionary<string, Dictionary<string, int>>();
        string[] lines;
        public char[] dataTags;
        public string[] dataNames;
        public char tagSplit;
        public char paramSplit;
        public char[] trimChars;
        public string Format { get; set; }
        public int MaxSizeOfTag { get; set; } = 0;
        public bool WhiteSpaceBetweenAttributeBlocks = false;


        public char commentTag;
        public int newLineAmount = 1;
        public char dataSeperator;
        public int multiLineAttributes;
        string newAttributeSignifier = "";
        int locX;
        int locY;
        public FileData(string data, char[] tags, string[] tagnames, char tagSplit, char paramSplit, char[] trimChars, char commentTag = '¬', int newLineAmount = 1, string newAttributeSignifier = "",
            char dataSeperator = ',')
        {
            this.dataTags = tags;
            this.dataNames = tagnames;
            this.commentTag = commentTag;
            this.newLineAmount = newLineAmount;
            this.newAttributeSignifier = newAttributeSignifier;
            this.dataSeperator = dataSeperator;
            this.tagSplit = tagSplit;
            this.paramSplit = paramSplit;
            this.trimChars = trimChars;
            lines = data.Split('\n');

        }
        public FileData(char[] tags, string[] datanames, char tagSplit, char paramSplit, char[] trimChars, char commentTag = '¬', int newLineAmount = 1, string newAttributeSignifier = "", char dataSeperator = ',')
        {
            this.dataTags = tags;
            this.dataNames = datanames;
            this.commentTag = commentTag;
            this.newLineAmount = newLineAmount;
            this.dataSeperator = dataSeperator;
            this.newAttributeSignifier = newAttributeSignifier;
            this.tagSplit = tagSplit;
            this.paramSplit = paramSplit;
            this.trimChars = trimChars;
        }
        public FileData()
        {
        }
        public FileData(FileData meta)
        {
            this.dataTags = meta.dataTags;
            this.dataNames = meta.dataNames;
            this.commentTag = meta.commentTag;
            this.newLineAmount = meta.newLineAmount;
            this.tagSplit = meta.tagSplit;
            this.paramSplit = meta.paramSplit;
            this.trimChars = meta.trimChars;
            this.Format = meta.Format;
        }
        public FileData(string data, FileData meta)
        {
            this.dataTags = meta.dataTags;
            this.dataNames = meta.dataNames;
            this.commentTag = meta.commentTag;
            this.newLineAmount = meta.newLineAmount;
            this.tagSplit = meta.tagSplit;
            this.paramSplit = meta.paramSplit;
            this.trimChars = meta.trimChars;
            this.Format = meta.Format;
            lines = data.Split('\n','\r').CleanStringArray(false);
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
                if (str.Contains(dataNames)
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


        public void FormulateAttributes( bool valueAsTag, bool combineDataPoints)
        {

            if (combineDataPoints)
                CombineDataPoints();

            if (dataTags == null)
                dataTags = new char[0];
            if (dataNames == null)
                dataNames = new string[0];
            int index = -1;
            int dupCount = 0;
            string currentTag = "";
            foreach (string str in lines)
            {
                index++;
                if (str.StartsWithChar(commentTag) || str == string.Empty)
                    continue;
                string[] split = str.SplitFirst(tagSplit, paramSplit);
                
                if (split[0].ContainsEquals(dataNames)
                    || str.Contains(dataTags.ToList()))
                {
                    split[0] = split[0].Trim(trimChars);
                    currentTag = split[1].Trim();

                    if (!valueAsTag)
                    {
                        if (ContainsAttribute(split[0].Trim()))
                        {
                            InsertAttribute(split[0].Trim(), split[0].Trim(), split, index, true);
                        }
                        else
                        {
                            InsertAttribute(split[0].Trim(), split[0].Trim(), split, index, false);
                        }
                    }
                    else
                    {
                        if (ContainsAttribute(split[1]))
                        {
                            InsertAttribute(split[1].Trim(), split[0].Trim(), split, index, true);
                            dupCount++;
                        }
                        else
                        {
                            InsertAttribute(split[1].Trim(), split[0].Trim(), split, index, false);
                        }
                    }

                }
                else
                {
                    if(ContainsAttribute(currentTag, split[0]))
                        AddParam(currentTag, split[0], split, index, true);
                    else AddParam(currentTag, split[0], split, index, false);
                }
                
            }

        }

        public void AddParam(string key, string tag, string[] values, int lineNum, bool isdup)
        {
            if (!isdup && !dc[key].ContainsKey(tag))
                dc[key].Add(tag, 0);
            else dc[key][tag]++;

            if (isdup)
            {
                attributes[key].Add(tag + dc[key][tag], new Attribute(tag, values.RemoveElementsAt(0), dataSeperator));
                attributeLineNumbers[key].Add(tag + dc[key][tag], lineNum);
            }
            else
            {
                attributes[key].Add(tag, new Attribute(tag, values.RemoveElementsAt(0), dataSeperator));
                attributeLineNumbers[key].Add(tag, lineNum);
            }
        }

        public void InsertAttribute(string key, string tag, string[] values, int lineNum, bool isdup)
        {
            if (!isdup)
                dc.Add(key, new Dictionary<string, int> { { tag, 0 } });
            else dc[key][tag]++;
            if (isdup)
            {
                attributes.Add(key + dc[key][tag], new Dictionary<string, Attribute> { { tag, new Attribute(tag, values.RemoveElementsAt(0), dataSeperator) } });
                attributeLineNumbers.Add(key + dc[key][tag], new Dictionary<string, int> { { tag, lineNum } });
            }
            else
            {
                attributes.Add(key, new Dictionary<string, Attribute> { { tag, new Attribute(tag, values.RemoveElementsAt(0), dataSeperator) } });
                attributeLineNumbers.Add(key, new Dictionary<string, int> { { tag, lineNum } });
            }
        }


        public void EditAttribute(string name, string editstr, string attribute, int itemIndex, bool append = false)
        {
            if (!ContainsAttribute(name))
            { return; }

            if (!append)
                attributes[name][attribute].items[itemIndex] = string.Format("{0}", editstr);
            else attributes[name][attribute].items[itemIndex] += string.Format("{0}", editstr);

            UpdateLine(name, editstr, attribute, append);
        }

        void UpdateLine(string name, string editstr, string attribute, bool append)
        {
            int lineNumber = attributeLineNumbers[name][attribute];
            lines[lineNumber] = String.Format(Format, name, StrFo.GetNewWhiteSpace(name, MaxSizeOfTag), attributes[name][attribute].items.OutputArrayAsLine(", "));
        }

        public void RefreshLines()
        {
            foreach(var attr in attributeLineNumbers)
            {
                foreach(var param in attr.Value)
                {
                    int lineno = param.Value;
                    string p = param.Key;
                    string a = attr.Key;
                    lines[lineno] = String.Format(Format, p, StrFo.GetNewWhiteSpace(p, MaxSizeOfTag), attributes[a][p].items.OutputArrayAsLine(", "));
                }
            }
        }

        public string[] NeatenedFile()
        {
            string[] neat = lines;

            if (WhiteSpaceBetweenAttributeBlocks)
            {
                for (int i = 0; i < neat.Count(); i++)
                {
                    string[] split = lines[i].SplitFirst(tagSplit, paramSplit);
                    if (split[0].ContainsEquals(dataNames) || split[0].Contains(dataTags.ToList()))
                        lines[i] = string.Format("\r\n{0}", neat[i]);
                }
            }
            return neat;
        }

        public string _ConsoleOutputAttribute(string name, string attribute)
        {
            if (!ContainsAttribute(name))
                return "error";

            Console.WriteLine(lines[attributeLineNumbers[name][attribute]]);

            return lines[attributeLineNumbers[name][attribute]]; 

        }

        bool ContainsAttribute(string name)
        {
            if (!attributes.ContainsKey(name))
            { Console.WriteLine("No key Found"); return false; }
            else return true;
        }

        bool ContainsAttribute(string name, string attribute)
        {
            if (!attributes.ContainsKey(name))
            { Console.WriteLine("No key Found"); return false; }
            else if(attributes[name].ContainsKey(attribute))
            {
                return true;
            }

            return false;
        }

        public string Output()
        {
            return NeatenedFile().ArrayToString(false, true, false, newLineAmount, false);
        }

        public void RemoveAttribute(string name, string attribute)
        {
            if (!ContainsAttribute(name))
                return;
            int lineNo = attributeLineNumbers[name][attribute];
            attributeLineNumbers[name].Remove(attribute);
            attributes[name].Remove(attribute);
            lines[lineNo] = "";
            lines.CleanStringArray();

        }
        public Dictionary<string, Attribute> this[string i]
        {
            get { return this.attributes[i]; }
        }
    }
}
