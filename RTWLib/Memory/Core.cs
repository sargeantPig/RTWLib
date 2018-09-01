
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Helpers;
using System.IO;
namespace RTWLib.Memory
{
	public static class RTWCore
	{
		public static Core core;

	}

	public class Core
	{
		public MemorySharp memory;

		public Core(Process process)
		{
			process.Start();
			//string name = process.ProcessName;
			//System.Threading.Thread.Sleep(5000);
			//memory = new MemorySharp(ApplicationFinder.FromProcessName(name).First());
		}

		public void Suspend()
		{
			memory.Threads.SuspendAll();
		}

		public void Output()
		{ }
	}
}
