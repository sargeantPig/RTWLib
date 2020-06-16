using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using RTWLib.Functions;
using System.Net.Http.Headers;

namespace RTWLib.Logger
{
	public class Logger
	{
		//Will handle logging any errors, outputs to RTWLlog.txt after every method completion
		static string LOGFILE = "RTWLlog.txt";
		static string current = "";

		static string dirFound = "Directory Found: ";
		static string dirNotFound = "Directory Not Found: ";
		static string isAdmin = "Running as administrator";
		static string isNotAdmin = "Not running as administrator";

        public bool is_on = true;
		public string fileName = "";
		public string lineText = "";
		public int lineNumber = 0;
		private void OutputToConsole(string logtxt)
		{
			Console.WriteLine("\r\n" + logtxt);
		}

		public string PLog(string logtxt)
		{
            if (!is_on)
                return logtxt;
            
			StreamWriter SW = new StreamWriter(LOGFILE, true);
			SW.WriteLine(logtxt + " -- " + DateTime.Now + "\r\n");
			SW.Close();

			OutputToConsole(logtxt);
			current = logtxt;

			return logtxt;
		}

		public string ExceptionLog(Exception ex)
		{
			string newLine = "\r\n";
			return this.PLog(ex.Message + newLine +
				"Error in: " + this.fileName + newLine +
				"At Line: " + this.lineNumber.ToString() + newLine +
				"'" + this.lineText + "'" + newLine +
				ex.InnerException);
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
