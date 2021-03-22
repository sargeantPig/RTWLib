using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Functions.DMB;
using RTWLib.Functions.EDU;
using RTWLib.Objects;

namespace RTWLib.Functions.UnitDescriptions
{
	public partial class UnitDescrs : FileBase, IFile
	{

		List<UnitDescription> unitDescriptions = new List<UnitDescription>();
		public UnitDescrs() : base(Data.FileNames.export_units, "", "")
		{


		}

		public void ApplyHiddenStats(EDU.EDU edu, DMB.DMB dmb)
		{
			foreach (var descr in unitDescriptions)
			{
				Unit unit = edu.FindUnit(descr.internalName);
				BattleModel bm = dmb.FindBattleModel(unit.soldier.name);
				int percentile = edu.GetUnitPercentile(unit.type);

				
				descr.AddHiddenStats(unit, bm, percentile);
			}
		}

		

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			currentLine = "";
			StreamReader du = new StreamReader(paths[0], Encoding.Unicode);
			lineNumber = 0;

			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}

			while ((currentLine = du.ReadLine()) != null)
			{
				bool success = ParseSegment(currentLine, unitDescriptions, du);
				lineNumber++;
			}
		}
		public override string Output()
		{
			string str = "";
			foreach (UnitDescription ud in unitDescriptions)
			{
				str += ud.Output();
			}
			return str;
		}

		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath, false, Encoding.Unicode);
			sw.Write(Output());
			sw.Close();
		}
	}
}
