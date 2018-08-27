using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace RTWLib.Logger
{
	public class Logger
	{
		//Will handle logging any errors, outputs to RTWLlog.txt after every method completion
		const string LOGFILE = "RTWLlog.txt";
		string current = "";

		string dirFound = "Directory Found: ";
		string dirNotFound = "Directory Not Found: ";

		private void OutputToConsole(string logtxt)
		{
			Console.WriteLine("\r\n" + logtxt);
		}

		public string PLog(string logtxt)
		{
			StreamWriter SW = new StreamWriter(LOGFILE, true);
			SW.WriteLine(logtxt + " -- " + DateTime.Now);
			SW.Close();

			OutputToConsole(logtxt);
			current = logtxt;

			return logtxt;
		}

		public bool DirectoryCheck(string directory)
		{
			if (Directory.Exists(directory))
			{
				current = PLog(dirFound + directory);
				return true;
			}
			else
				current = PLog(dirNotFound + directory);

			return false;
		}

		public bool FileCheck(string file)
		{
			if (File.Exists(file))
			{
				current = PLog(dirFound + file);
				return true;
			}
			else
				current = PLog(dirNotFound + file);

			return false;
		}

		public void Output(string logtxt)
		{
			current += logtxt;
			OutputToConsole(logtxt);

		}

		public void DisplayLog()
		{
			DialogResult result2 = MessageBox.Show(current,
			"Error",
			MessageBoxButtons.OK,
			MessageBoxIcon.Error);

			Application.Exit();
		}
	}
}
