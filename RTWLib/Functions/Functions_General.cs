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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.IO;

namespace RTWLib.Functions
{
	public interface IFile
	{
		void Parse(string[] path, out int lineNumber, out string currentLine);
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
		void ToFile(string filepath);

	}

	public class FileBase : Logger.Logger, IFile
	{
		FileNames name;
		string FILEPATH;
		string DESCRIPTION;

		public FileBase(FileNames name, string filePath, string description)
		{
			this.name = name;
			this.FILEPATH = filePath;
			this.DESCRIPTION = description;
		
		}

		virtual public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			lineNumber = 0;
			currentLine = "base not implemented";
		}

		virtual public string Output()
		{
			string output = "base not implemented";

			return output;
		}

		virtual public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
		}

		public FileNames Name
		{
			get { return name; }
		}
		public string FilePath
		{
			get { return FILEPATH; }
		}

		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}
	}

	static public class Functions_General
	{
		public static string UniversalOutput(this float input)
		{
			return input.ToString().GetSafeRTWDoubleStr();	
		}

		public static string GetSafeRTWDoubleStr(this string input)
		{
			//trim excess
			input.Trim();

			//check for the correct seperator '.'
			string[] split = input.Split(',');
			string safe = "";
			if (split.Count() == 2) //is double
			{
				//contains invalid seperator
				//reconstruct
				safe = split[0] + "." + split[1];
				return safe;
			}
			else return input;

		}



		public static float UniversalParse(this string input)
		{
			float output = float.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
			return output;
		}


		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}

		public static void CleanStrings(this string[] a)
		{
			for (int i = 0; i < a.Count(); i++)
				a[i] = a[i].Trim();
		}

		public static string ReplaceSpaces(this string a, string replacer)
		{
			string[] split = a.Split(' ');
			string output = "";
			for(int i = 0; i < split.Count(); i++)
			{
				if (i == split.Count() - 1)
					output += split[i];
				else output += split[i] + replacer;
			}
			return output;
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

		public static string Capitalise(this string input)
		{
			char[] characters = input.ToCharArray();


			if (characters.Count() == 0)
				return "";
			
			

			characters[0] = char.ToUpper(characters[0]);

			return new string(characters);

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
