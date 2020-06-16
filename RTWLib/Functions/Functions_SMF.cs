using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using RTWLib.Data;

namespace RTWLib.Functions
{	
	public class SM_Factions : Logger.Logger, IFile
	{
        FileNames name = FileNames.descr_sm_faction;
		public const string FILEPATH = @"randomiser\data\descr_sm_factions.txt";
		const string DESCRIPTION = "faction colours";
		public Dictionary<FactionOwnership, Color[]> factionColours = new Dictionary<FactionOwnership, Color[]>();

		public SM_Factions()
		{
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}

		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public void Parse(string[] path, out int lineNumber, out string currentLine)
		{
			if (!FileCheck(path[0]))
				DisplayLogExit();

			StreamReader edu = new StreamReader(path[0]);

			string line, faction = "";

			LookUpTables lut = new LookUpTables();

			Output("Retrieving Faction Colours" + "\r\n");
			FactionOwnership f = FactionOwnership.none;

			lineNumber = 0; ;
			currentLine = "";
			while ((line = edu.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				string trim = line.Trim();

				if (trim.StartsWith("faction"))
				{
					string[] split = trim.Split('\t');
					faction = split[6];
					Output("Getting Colours for: " + faction + "\r\n");

					f = lut.LookUpKey<FactionOwnership>(faction);
				}

				if (trim.StartsWith("primary_colour"))
				{
					Color col = SMFGetColour(trim);
					factionColours.Add(f, new Color[2] { col, col });
				}

				if (trim.StartsWith("secondary_colour"))
				{
					Color col = SMFGetColour(trim);
					factionColours[f][1] = col;
				}
			}
		}

		private Color SMFGetColour(string line)
		{
			string[] split = line.Split(' ');

			for (int i = 0; i < split.Count() - 1; i++)
			{
				split[i] = split[i].Trim(',');
			}
			return Color.FromArgb(Convert.ToInt32(split[1]), Convert.ToInt32(split[3]), Convert.ToInt32(split[5]));

		}

		public string OutputColours()
		{
			string output = "";

			foreach (KeyValuePair<FactionOwnership, Color[]> kv in factionColours)
			{
				output += kv.Key + " ---\r\n\tPrimary Colour " +   kv.Value[0].ToString()
					+ "\r\n\tSecondary Colour " + kv.Value[1].ToString() + "\r\n\r\n";
			}

			return output;
		}

		public string Output()
		{
			return null;
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
