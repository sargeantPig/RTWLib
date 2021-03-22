using RTWLib.Data;
using RTWLib.Medieval2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB
{ 
    public partial class DMB : FileBase, IFile
    {
		public List<BattleModel> battleModels = new List<BattleModel>();
		public DMB(bool isLogOn) : base(FileNames.battle_models, "", "")
		{
			this.is_on = isLogOn;
		}
		/// <summary>
		/// Adds a mercenary line to model
		/// </summary>
		public void AddMercLineToEachModel()
		{
			foreach (var b in battleModels)
			{
				b.AddMercLine();
			}
		}

		public BattleModel FindBattleModel(string name)
		{
			return battleModels.Find(x => x.modelType.type == name);
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
				bool success = ParseLine(currentLine, battleModels);
				lineNumber++;
			}
		}

		public override string Output()
		{
			string str = "";
			str = String.Format("{0}",
				DMB.IdmbArrayToString(battleModels.ToArray()));
			return str;
		}

		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
		}
	}
}
