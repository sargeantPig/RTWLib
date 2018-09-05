
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Helpers;
using System.IO;
using RTWLib.Logger;
using RTWLib.Functions;
using System.Threading;
using Binarysharp.MemoryManagement.Native;
namespace RTWLib.Memory
{
	public static class RTWCore
	{
		public static Core core = new Core();

		public static IntPtr address_year = new IntPtr(0x24CCB50);
	}

	public class Core : Logger.Logger
	{
		public MemorySharp memory;

		public int campaign_year = 0;
		string pName;
		string arguments;

		public Core(Process process)
		{
			process.Start();
			pName = process.ProcessName;
			System.Threading.Thread.Sleep(10000);
			memory = new MemorySharp(ApplicationFinder.FromProcessName(pName).First());
		}

		public Core() { }

		public Task TestLoop(int iter, int target, string name, Func<Task> func)
		{
			

			for (int i = 0; i < iter; i++)
			{
				int year = 0;
				int tempyear = 0;
				Stopwatch stopwatch = new Stopwatch();
				Stopwatch timer = new Stopwatch();
				float elasped = 0;
				bool result = false;
				PLog("\r\n\r\n" + name + " Test Begun\r\n\r\n");

				while (memory == null)
				{
					Thread.Sleep(10000);
					Reload();
				}

				var window = memory.Windows.MainWindow;

				timer.Start();
				stopwatch.Start();
				while (memory.IsRunning)
				{
					elasped = stopwatch.Elapsed.Minutes;
					year = memory.Read<int>(RTWCore.address_year);

					if (year >= target || elasped > 5)
					{
						result = true;
						memory.Native.Kill();
					}

					if (tempyear != year)
					{
						elasped = 0;
						stopwatch.Restart();
					}

					tempyear = year;
				}

				stopwatch.Stop();
				string timeElasped = timer.Elapsed.ToString();

				PLog("Time Elasped: " + timeElasped
					+ "\r\nYear Reached: " + year.ToString()
					+ "\r\nTest Result: " + result.ToString());


				if (i < iter)
					StartProcess(new string[] { arguments });

				while (!memory.IsRunning) ;

			}

			if (memory.IsRunning) //kill if process is still open after tests
				memory.Native.Kill();

			return Task.CompletedTask;
		}

		public void Suspend()
		{
			memory.Threads.SuspendAll();
		}

		public void StartProcess(string[] args)
		{
			Process p = Functions_General.ExecuteCommand("RomeTW.exe", args);
			p.Start();
			pName = p.ProcessName;
			arguments = args[0];
		}

		public void Reload()
		{
			try
			{
				var a = ApplicationFinder.FromProcessName(pName);
				memory = new MemorySharp(ApplicationFinder.FromProcessName(pName).First());
			}
			catch {

			}
		}

		public void Output()
		{ }
	}
}
