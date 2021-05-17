using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using RTWLib.Functions;
using System.Net.Http.Headers;
using System.Diagnostics;
using RTWLib.Extensions;

namespace RTWLib.Logger
{
	public class Logger
	{
		//Will handle logging any errors, outputs to RTWLlog.txt after every method completion
		public static string AssemblyPrefix { get; set; }
		public string ModulePrefix  { get; set; }
		public string Type;
		 
		public static string[] AssemblyWatchList { get; set; }

		static string LOGFILE = "RTWLlog.txt";
		public static string current { get; set; }

		static string fileFound = "File Found ";
		static string fileNotFound = "File not found: ";
		static string dirFound = "Directory Found: ";
		static string dirNotFound = "Directory Not Found: ";
		static string isAdmin = "Running as administrator ";
		static string isNotAdmin = "Not running as administrator ";

        public bool is_on = true;
		static public bool isAdministrator = false;
		public string fileName = "";
		static public string lineText = "";
		static public int lineNumber = 0;
		public Logger()
		{
		}
		public Logger(string assemblyPrefix, params string[] assemblyWatchList)
		{
			Logger.AssemblyPrefix = assemblyPrefix;
			Logger.AssemblyWatchList = assemblyWatchList;
		}
		private void OutputToConsole(string logtxt)
		{
			Console.WriteLine("\r\n" + logtxt);
		}

		public void LoadError()
		{
			PLog("Load failed - Check log for details.");
			DisplayLog();
		}

		public string PLog(string logtxt)
		{
            if (!is_on)
                return logtxt;

			StreamWriter SW = new StreamWriter(LOGFILE, true);
			SW.WriteLine(AssemblyPrefix + ":" + ModulePrefix + ": " + logtxt + " -- " + DateTime.Now + "\r\n");
			SW.Close();

			OutputToConsole(logtxt);
			current = AssemblyPrefix + ":" + ModulePrefix + ": " + logtxt;

			ModulePrefix = "";

			return logtxt;
		}

		public string ExceptionLog(Exception ex, bool hasLines = true)
		{
			ModulePrefix = GetLastCalledMethod<object>(ex, out Type);

			string newLine = "\r\n";
			if (hasLines)
			{
				return this.PLog(ex.Message + newLine +
					"Error in: " + Type + newLine +
					"At Line: " + lineNumber.ToString() + newLine +
					"'" + lineText + "'" + newLine +
					ex.InnerException);

			}
			else
			{
				return this.PLog(ex.Message + newLine +
					"Error in: " + Type + newLine +
					ex.InnerException);
			}
		}


		public string ExceptionCurrentLog(Exception ex, string msg, bool hasLines = true)
		{
			ModulePrefix = GetLastCalledMethod<object>(ex, out Type);

			string newLine = "\r\n";
			if (hasLines)
			{
				return this.PLog(msg + newLine + ex.Message + newLine +
					"Error in: " + Type + newLine +
					"At Line: " +lineNumber.ToString() + newLine +
					"'" + lineText + "'" + newLine +
					ex.InnerException);
			}
			else
			{
				return this.PLog(ex.Message + newLine +
					"Error in: " + Type + newLine +
					ex.InnerException);
			}
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
				current = PLog(fileFound + file);
				return true;
			}
			else
				current = PLog(fileNotFound + Directory.GetCurrentDirectory() + "\\" + file);

			return false;
		}

		public bool AdminCheck()
		{
			bool admin = ProcessHelper.IsAdministrator();

			if (admin) {
				current = PLog(isAdmin);
				return true;
			}
			else
				current = PLog(isNotAdmin);

			Logger.isAdministrator = admin;

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

		public DialogResult DisplayRetry()
		{
			DialogResult result = MessageBox.Show(current, 
				"Warning", 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Warning);

			return result;

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

		public string ContinueParseAndCountLine(ref StreamReader sr, ref int lineCounter)
		{
			lineCounter++;
			return sr.ReadLine();
			
		}

		public static string GetLastCalledMethod<T>(Exception ex, out string type)
		{
			var stackTrace = new System.Diagnostics.StackTrace(ex);
			var lastFrame = stackTrace.GetFrames();

			string methodName = "";

			if (lastFrame == null)
			{
				type = "null";
				return "null";
			}

			foreach (StackFrame sf in lastFrame)
			{
				string[] split;
				string fullname = sf.GetMethod().DeclaringType.AssemblyQualifiedName;
				split = fullname.Split('.');

				if (AssemblyWatchList.Contains(split[0]))
				{
					type = sf.GetMethod().DeclaringType.Name;
					methodName = sf.GetMethod().Name;
					return methodName;
				}

			}
			type = "";
			return methodName;
		}
	}
}
