using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using RTWLib.Data;

namespace RTWLib.Functions.Remaster
{	
	public partial class ReSMFactions : FileBase, IFile
	{
		public Dictionary<string, ReSMFaction> facDetails = new Dictionary<string, ReSMFaction>();

		public ReSMFactions() 
			: base(FileNames.descr_sm_faction, @"data\descr_sm_factions.txt", "Describes the faction colours")
		{
		}
		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			currentLine = "";
			StreamReader dmb = new StreamReader(paths[0]);
			lineNumber = 0;

			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}

			while ((currentLine = dmb.ReadLine()) != null)
			{
				bool success = ParseLine(currentLine, facDetails);
				lineNumber++;
			}

			dmb.Close();
		}

		public Color GetFactionColour(string faction, int index)
		{
			if (facDetails.ContainsKey(faction))
				return facDetails[faction].factionColour.colours[index];
			else return new Color();
		}

		public override string Output()
		{
			string str = "";
			foreach (var faction in facDetails)
				str += faction.Value.Output();

			return str;
		}

		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
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
}
