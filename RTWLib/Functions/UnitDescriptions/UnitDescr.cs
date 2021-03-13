using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.UnitDescriptions
{
	public partial class UnitDescrs : FileBase, IFile
	{

		List<UnitDescription> unitDescriptions = new List<UnitDescription>();
		public UnitDescrs() : base(Data.FileNames.export_units, "", "")
		{


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
