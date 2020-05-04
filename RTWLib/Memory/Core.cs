
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Logger;
using RTWLib.Functions;
using System.Threading;
namespace RTWLib.Memory
{
	public static class RTWCore
	{
		public static Core core = new Core();

		public static IntPtr address_year = new IntPtr(0x24CCB50);
	}

	public class Core : Logger.Logger
	{
		string pName;
		string arguments;

		public Core() { }

		public void StartProcess(string[] args)
		{
			Process p = Functions_General.ExecuteCommand("RomeTW.exe", args);
			p.Start();
			pName = p.ProcessName;
			arguments = args[0];
		}

	}
}
