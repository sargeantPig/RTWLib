using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTWLib.Memory;
using System.Security.Principal;
using RTWLib.Data;
using System.Drawing;

namespace RTWLib.Functions
{
	public interface IFile
	{
		void Parse(string[] path);
		string Log(string txt);
		string Output();
        FileNames Name
        {
            get;
        }
		string FilePath
		{
			get;
		}

		string Description
		{
			get;
		}
	}

	static public class Functions_General
	{
		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}

		public static void Shuffle<T>(this IList<T> list, Random rnd)
		{
			for (var i = 0; i < list.Count; i++)
				list.Swap(i, rnd.Next(i, list.Count));
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			var temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}

		public static int CompareNameEnd(CheckBox c1, CheckBox c2)
		{
			string[] split1 = c1.Name.Split('_');
			string[] split2 = c2.Name.Split('_');

			string value1 = split1.Last();
			string value2 = split2.Last();

			return value1.CompareTo(value2);
		}

		public static string RemoveFirstWord(string String)
		{
			string newString = "";

			string[] Temp = String.Split(' ');

			int i = 0;

			foreach (string temp in Temp)
			{
				if (i != 0)
					newString += temp + " ";

				i++;
			}

			return newString;
		}

		public static string RemoveFirstWord(string String, char delimiter)
		{
			string newString = "";

			string[] Temp = String.Split(delimiter);

			int i = 0;

			foreach (string temp in Temp)
			{
				if (i != 0)
					newString += temp;

				if (temp == "")
					newString += '\t';
				else newString += ' ';

				i++;
			}

			return newString;
		}

		public static string GetFirstWord(string String)
		{
			string[] Temp = String.Split(' ');

			return Temp[0].Trim();
		}

		public static string RemoveLastWord(string String)
		{
			string newString = "";

			string[] Temp = String.Split(' ');

			int i = 0;

			foreach (string temp in Temp)
			{
				if (i != Temp.Count() - 1)
					newString += temp + " ";

				i++;
			}

			return newString;
		}

		public static T RandomFlag<T>(Random rnd)
		{
			Array flags = Enum.GetValues(typeof(T));
			var a = (T)flags.GetValue(rnd.Next(flags.Length));

			return a;
		}

		public static T RandomFlag<T>(Random rnd, int min, int max)
		{
			Array flags = Enum.GetValues(typeof(T));
			var a = (T)flags.GetValue(rnd.Next(min, max));

			return a;
		}

		public static bool Contains(this string str, List<string> list)
		{
			foreach (string s in list)
			{
				if (str.Contains(s))
					return true;
			}

			return false;
		}
		
		public static double DistanceTo(int[] a, int[] b)
		{
			double dis = Math.Sqrt(Math.Pow((a[0] - b[0]), 2) + Math.Pow((a[1] - b[1]), 2));

			return dis;
		}

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
			p.StartInfo.CreateNoWindow = false;
			p.StartInfo.Arguments = args[0];
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;

			return p;
		}

		public static bool IsAdministrator()
		{
			return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
					  .IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
