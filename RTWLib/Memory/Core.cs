
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

	//EXPERIMENTAL 
	public static class RTWCore
	{
		public static Core core = new Core();

		public static IntPtr address_year = new IntPtr(0x24CCB50);
	}

	public class Core : Logger.Logger
	{
		string pName;
		string arguments;
		StreamReader output;

		Process p;
		public Core() { }

		public void StartProcess(string[] args, string name)
		{
			p = LibFuncs.ExecuteCommand(name, args);
			p.Start();
			
			pName = p.ProcessName;
			arguments = args[0];
			//p.ErrorDataReceived += DataRecieved;
			output = p.StandardOutput;
			//p.EnableRaisingEvents = true;
			//p.Exited += RomeExited;
		}

		public void Watch()
		{
		}

		public void DataRecieved(object sender, EventArgs e)
		{
			Console.WriteLine(((Process)sender).StandardError.ReadToEnd());
			Console.WriteLine(e.ToString());
		
		}

		public void RomeExited(object sender, EventArgs e)
		{
			Console.WriteLine(((Process)sender).ExitCode.ToString());
			Console.WriteLine(e.ToString());

		}

	}
}
