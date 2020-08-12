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
	public class SM_Factions : FileBase, IFile
	{
		public Dictionary<string, Color[]> factionColours = new Dictionary<string, Color[]>();

		public SM_Factions() 
			: base(FileNames.descr_sm_faction, @"data\descr_sm_factions.txt", "Describes the faction colours")
		{
		}
		override public void Parse(string[] path, out int lineNumber, out string currentLine)
		{

			lineNumber = 0; ;
			currentLine = "";
			if (!FileCheck(path[0]))
			{
				DisplayLog();
				return;
			}
			StreamReader edu = new StreamReader(path[0]);

			string line, faction = "";

			LookUpTables lut = new LookUpTables();

			while ((line = edu.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				string trim = line.Trim();

				if (trim.StartsWith("faction"))
				{
					string[] split = trim.Split('\t');
					faction = split[6];

				}

				if (trim.StartsWith("primary_colour"))
				{
					Color col = SMFGetColour(trim);
					factionColours.Add(faction, new Color[2] { col, col });
				}

				if (trim.StartsWith("secondary_colour"))
				{
					Color col = SMFGetColour(trim);
					factionColours[faction][1] = col;
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

			foreach (KeyValuePair<string, Color[]> kv in factionColours)
			{
				output += kv.Key + " ---\r\n\tPrimary Colour " +   kv.Value[0].ToString()
					+ "\r\n\tSecondary Colour " + kv.Value[1].ToString() + "\r\n\r\n";
			}

			return output;
		}
	}
}
