using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace RTWLib.Logger
{
	public class Logger
	{
		//Will handle logging any errors, outputs to RTWLlog.txt after every method completion
		const string LOGFILE = "RTWLlog.txt";
		string current = "";


		private void OutputToConsole(string logtxt)
		{
			Console.WriteLine("\r\n" + logtxt);
		}

		public void Log(string logtxt)
		{
			StreamWriter SW = new StreamWriter(LOGFILE, true);
			SW.WriteLine(logtxt + " -- " + DateTime.Today);
			SW.Close();

			OutputToConsole(logtxt);
		}

		public void Output(string logtxt)
		{
			current += logtxt;
			OutputToConsole(logtxt);

		}




	}
}
