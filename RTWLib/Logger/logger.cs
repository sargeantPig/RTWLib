using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using RTWLib.Functions;
namespace RTWLib.Logger
{
	public class Logger
	{
		//Will handle logging any errors, outputs to RTWLlog.txt after every method completion
		const string LOGFILE = "RTWLlog.txt";
		string current = "";

		string dirFound = "Directory Found: ";
		string dirNotFound = "Directory Not Found: ";
		string isAdmin = "Running as administrator";
		string isNotAdmin = "Not running as administrator";

		private void OutputToConsole(string logtxt)
		{
			Console.WriteLine("\r\n" + logtxt);
		}

		public string PLog(string logtxt)
		{
			StreamWriter SW = new StreamWriter(LOGFILE, true);
			SW.WriteLine(logtxt + " -- " + DateTime.Now + "\r\n");
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

		public bool AdminCheck()
		{
			bool admin = Functions_General.IsAdministrator();

			if (admin) {
				current = PLog(isAdmin);
				return true;
			}
			else
				current = PLog(isNotAdmin + ": unit info fix will be disabled.");

			return false;
		}

		public void Output(string logtxt)
		{
			current += logtxt;
			OutputToConsole(logtxt);

		}

		public void DisplayLogExit()
		{
			DialogResult result2 = MessageBox.Show(current,
			"Error",
			MessageBoxButtons.OK,
			MessageBoxIcon.Error);

			Application.Exit();
			Environment.Exit(0);
		}

		public void DisplayLog()
		{
			DialogResult result2 = MessageBox.Show(current,
			"Error",
			MessageBoxButtons.OK,
			MessageBoxIcon.Error);
		}

		public void CleanLog()
		{
			if (File.Exists(LOGFILE))
			{
				FileInfo f = new FileInfo(LOGFILE);
				var size = f.Length;

				if (size > 100000)
				{
					File.WriteAllText(LOGFILE, "### RTW LOGGER ###\r\n\r\n");
				}

			}

		}
	}
}
