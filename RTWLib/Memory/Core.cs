
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
using RTWLib.Extensions;

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
		StreamReader error;

		Process p;
		public Core() { }

		public void StartProcess(string[] args, string name)
		{
			p = ProcessHelper.ExecuteCommand(name, args);
			//output = p.StandardOutput;
			//error = p.StandardError;
			//p.EnableRaisingEvents = true;
			//p.OutputDataReceived += P_OutputDataReceived;
			//p.ErrorDataReceived += P_ErrorDataReceived;
			p.Exited += P_Exited;
			p.Start();

			if (Logger.Logger.isAdministrator)
			{
				p.ProcessorAffinity = (IntPtr)0x0001;
				p.PriorityClass = ProcessPriorityClass.High;
			}
			
			p.WaitForExit();
		}

		private void P_Exited(object sender, EventArgs e)
		{
			this.PLog(String.Format("Exit Code: {0}", p.ExitCode));
		}

		private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			this.PLog(e.Data);
			this.DisplayLog();
		}

		private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			this.PLog(e.Data);
		}

		public void StartBat()
		{
			p = ProcessHelper.ExecuteCommand("str_randomiser.bat", new string[] { });
			p.Start();
			
		}
		public void Watch()
		{
		}

		public void RomeExited(object sender, EventArgs e)
		{
			Console.WriteLine(((Process)sender).ExitCode.ToString());
			Console.WriteLine(e.ToString());

		}

	}
}
