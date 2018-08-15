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
	public class Functions_SMF : Logger.Logger
	{
		Dictionary<FactionOwnership, Color[]> factionColours = new Dictionary<FactionOwnership, Color[]>();

		public Dictionary<FactionOwnership, Color[]> ParseSMFactions(string filepath)
		{
			StreamReader edu = new StreamReader(filepath);

			string line, faction = "";

			LookUpTables lut = new LookUpTables();

			Output("Retrieving Faction Colours" + "\r\n");
			FactionOwnership f = FactionOwnership.none;
			while ((line = edu.ReadLine()) != null)
			{
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
					factionColours.Add(f, new Color[2] {col, col});
				}

				if (trim.StartsWith("secondary_colour"))
				{
					Color col = SMFGetColour(trim);
					factionColours[f][1] = col;
				}
			}
			return factionColours;
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

	}

	public class SM_Factions : Logger.Logger
	{
		Dictionary<FactionOwnership, Color[]> factionColours;

		public SM_Factions(Dictionary<FactionOwnership, Color[]> dic)
		{
			factionColours = new Dictionary<FactionOwnership, Color[]>(dic);
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
	}
}
