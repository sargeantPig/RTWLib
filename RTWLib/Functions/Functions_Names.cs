using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Data;
namespace RTWLib.Functions
{
	public class NamesFile : Logger.Logger, IFile
	{
        FileNames name = FileNames.names;
		const string DESCRIPTION = "Name file";
		const string FILEPATH = @"randomiser\data\descr_names.txt";

		Dictionary<FactionOwnership, List<string>> names = new Dictionary<FactionOwnership, List<string>>();

        public NamesFile(bool log_on)
        {
            is_on = log_on;
        }

		public void Parse(string[] path)
		{
			LookUpTables lt = new LookUpTables();
			StreamReader sr = new StreamReader(path[0]);
			string line = "";
			FactionOwnership faction = FactionOwnership.none ;
			while ((line = sr.ReadLine()) != null)
			{
				if (line.StartsWith("faction"))
				{
					string[] split = line.Split(' ');
					faction = lt.LookUpKey<FactionOwnership>(split[1].Trim());
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
                            PLog("Loaded: " + names[faction].Last());
                        }
                            line = sr.ReadLine();
					}
				}
			}
		}

		public string GetRandomName(Random rnd, string faction)
		{
			LookUpTables lt = new LookUpTables();

			FactionOwnership fo = lt.LookUpKey<FactionOwnership>(faction);

			return names[fo][rnd.Next(names[fo].Count())];
		}

		public string GetRandomUniqueName(Random rnd, string faction, List<string> usedNames)
		{
			LookUpTables lt = new LookUpTables();
			FactionOwnership fo = lt.LookUpKey<FactionOwnership>(faction);

			List<string> unusedNames = new List<string>();

			foreach (string name in names[fo])
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

		public string Output()
		{
			string output = "";
			return output;
		}

		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}

        public FileNames Name
        {
            get { return name; }
        }

        public string FilePath
		{
			get { return FILEPATH; }
		}

	}
}
