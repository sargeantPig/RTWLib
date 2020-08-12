using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Data;
namespace RTWLib.Functions
{
	public class NamesFile : FileBase, IFile
	{
		Dictionary<string, List<string>> names = new Dictionary<string, List<string>>();

        public NamesFile(bool log_on) 
			: base(FileNames.names, @"data\descr_names.txt", "Lists character names")
        {
            is_on = log_on;
        }
		override public void Parse(string[] path, out int lineNumber, out string currentLine)
		{
			LookUpTables lt = new LookUpTables();
			StreamReader sr = new StreamReader(path[0]);
			string line = "";
			string faction ="";
			lineNumber = 0;
			currentLine = "";
			while ((line = sr.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				if (line.StartsWith("faction"))
				{
					string[] split = line.Split(' ');
					faction = split[1].Trim();
					names.Add(faction, new List<string>());
				}

				if (Functions_General.GetFirstWord(line) == "characters" && !line.StartsWith(";"))
				{
					line = sr.ReadLine();
					while (!line.Contains("women") && !line.Contains("faction") && !line.Contains("surnames"))
					{
                        if (line.Trim() != "" && !line.Trim().Contains(";;") && !line.Trim().Contains(";") && CheckForNonASCII(line.Trim()))
                        {
                            names[faction].Add(line.Trim());
                        }
                            line = sr.ReadLine();
					}
				}
			}
		}
		public string GetRandomName(Random rnd, string faction)
		{
			return names[faction][rnd.Next(names[faction].Count())];
		}
		public string GetRandomUniqueName(Random rnd, string faction, List<string> usedNames)
		{
			List<string> unusedNames = new List<string>();

			foreach (string name in names[faction])
			{
				if (!usedNames.Contains(name))
					unusedNames.Add(name);
			}

			return unusedNames[rnd.Next(unusedNames.Count())];
		}
		private bool CheckForNonASCII(string str)
		{
			foreach (char c in str)
			{
				if (c < byte.MaxValue)
					continue;
				else return false;
					
			}
			return true;
		}
	}
}
