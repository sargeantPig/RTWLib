using RTWLib.Data;
using RTWLib.Functions;
using RTWLib.Functions.EDU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    class M2EDU : EDU
    {

        public M2EDU(bool log_on) : base(log_on)
        { 
        
        
        
        }
		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			lineNumber = 0;
			currentLine = "";
			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}
			LookUpTables lookUp = new LookUpTables();
			units.Clear();
			string txt_Output = "";
			string line = "";
			int counter = -1;

			StreamReader edu = new StreamReader(paths[0]);

			while ((line = edu.ReadLine()) != null)
			{
				KeyValuePair<EDULineEnums, object> comment;
				currentLine = line;
				lineNumber++;
				ParseLine(line, ref counter, lineNumber, out comment);
				if (counter > -1 && comment.Value != null)
				{
					if (units[counter].comments.ContainsKey(comment.Key))
					{
						if (units[counter].comments[comment.Key] is List<string>)
						{
							((List<string>)units[counter].comments[comment.Key]).Add((string)comment.Value);
						}
						else
						{
							string temp = (string)units[counter].comments[comment.Key];
							units[counter].comments[comment.Key] = new List<string>() { temp, (string)comment.Value };
						}
					}
					else units[counter].comments.Add(comment.Key, comment.Value);
				}
			}
			txt_Output += ("\n" + units.Count + "Units loaded from EDU");

			edu.Close();

		}
		override public string Output()
		{
			string output = "";

			foreach (M2Unit unit in units)
			{
				output += unit.unitOutput();
			}

			return output;
		}


	}
}
