using RTWLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib
{
    public static class RTWLIB
    {
		public static class Globals
		{
			readonly public static string ROMEEXE = "RomeTW.exe";
			readonly public static string M2TWEXE = "medieval2.exe";
			readonly public static string REMASTEREEXE = "Application.lnk";
		}


		public static class Folders
		{
			public static Logger.Logger LibLog = new Logger.Logger();
			/// <summary>
			/// Root, ModFolder, Location
			/// </summary>
			static string Pathformat = "{0}{1}{2}";

			public static string MODFOLDER = string.Empty;
			readonly public static string M_REMASTERROOT = @"Mods\My Mods\";
			readonly public static string M_M2TWROOT = @"mods\";
			readonly public static string DATA = @"data\";

			public static string ConstructPath(Game game, string fileLocation)
			{
				if (MODFOLDER == string.Empty)
				{
					LibLog.ExceptionCurrentLog(new Exception(), "Modfolder has not been set.", false);
					LibLog.DisplayLog();
					return string.Empty;
				}
				switch (game)
				{
					case Game.OGRome:
						return String.Format(Pathformat, string.Empty, MODFOLDER, fileLocation);
					case Game.REMASTER:
						return String.Format(Pathformat, M_REMASTERROOT, MODFOLDER, fileLocation);
					case Game.MED2:
						return String.Format(Pathformat, M_M2TWROOT, MODFOLDER, fileLocation);
					default:
						LibLog.ExceptionCurrentLog(new Exception(), "Modfolder has not been set.", false);
						LibLog.DisplayLog();
						return string.Empty;
				}
				
			}


		}


	}
}
