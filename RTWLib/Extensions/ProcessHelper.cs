using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class ProcessHelper
    {
		public static void ExecuteCommand(string wd, string filename)
		{
			ProcessStartInfo ps = new ProcessStartInfo();

			ps.WorkingDirectory = @wd;
			ps.FileName = @filename;
			ps.CreateNoWindow = false;

			Process.Start(ps);
		}
		public static Process ExecuteCommand(string filename, string[] args)
		{

			Process p = new Process();
			p.StartInfo.FileName = @filename;
			//p.StartInfo.CreateNoWindow = false;
			p.StartInfo.Arguments = args[0];
			//p.StartInfo.UseShellExecute = false;
			//p.StartInfo.RedirectStandardError = true;
			//p.StartInfo.RedirectStandardOutput = true;
			return p;
		}
		public static bool IsAdministrator()
		{
			return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
					  .IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
