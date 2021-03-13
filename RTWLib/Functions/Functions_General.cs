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
using System.CodeDom;
using System.Collections;
using System.Text.RegularExpressions;

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

	public class FileData<T>
	{
		Dictionary<T, int> expectedSizes;

		public FileData(Dictionary<T, int> data)
		{
			this.expectedSizes = data;
		}

		public void Add(T key, int value)
		{
			if (!expectedSizes.ContainsKey(key))
				expectedSizes.Add(key, value);
		}

		public int this[T i]
		{
			get { return expectedSizes[i]; }
			set { expectedSizes[i] = value; }
		}
			
	}


	static public class LibFuncs
	{
		/// <summary>
		/// Get a random percent eg. 0.5 
		/// minpercent eg. 500 = (50%), 1000 = (100%) 100 = (10%) 10 = (1%)
		/// </summary>
		/// <returns></returns>
		public static float RandPercent(this Random rnd, int minPercent, int maxPercent)
		{
			int randomInt = rnd.Next(minPercent, maxPercent + 1);
			float rndPercent = (float)(randomInt / 1000f);
			return rndPercent;
		}
		/// <summary>
		/// Catches a divide by 0 and returns 1 or 0 according to optional param
		/// </summary>
		/// <returns></returns>
		public static float SafeDivide(float a, float b)
		{
			float result;

			if (a == 0 || b == 0)
				return 0;

			result = a / b;
			return result;
		}

		public static List<string> RemoveDuplicates(this List<string> a)
		{
			List<string> seen = new List<string>();

			for (int i = 0; i < a.Count; i++)
			{
				if (seen.Contains(a[i]))
					continue;
				else seen.Add(a[i]);
			}
			return seen;
		}

		public static string UniversalOutput(this float input)
		{
			return input.ToString().GetSafeRTWDoubleStr();	
		}

		public static string GetNewWhiteSpace(string ident, int def = 20)
		{
			int il = identifierLength(ident, def);
			return identSpacing(il);
		}

		public static int identifierLength(string ident, int def = 20)
		{
			return def - ident.Length;
		}
		public static string identSpacing(int identifierLength)
		{
			string spaces = "";

			for (int i = 0; i < identifierLength; i++)
				spaces += " ";
			return spaces;
		}

		public static int Clamp(this int value, int min, int max)
		{
			if (value < min)
				return min;
			else if (value > max)
				return max;
			else return value;
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

		public static void RenameFile(string old, string newFile)
		{
			if (File.Exists(old))
			{
				if (File.Exists(newFile))
					File.Delete(newFile);

				File.Move(old, newFile);
			}
		}

		public static string GetPathFrom( this string fullpath, string pathStart, int counterMatchesToIgnore)
		{ 
			string[] split = fullpath.Split('\\');
			bool found = false;
			string partpath = "";
			int matchCount = 0;
			foreach (string line in split)
			{
				if (line.Contains(pathStart))
				{
					matchCount++;
					if(matchCount > counterMatchesToIgnore)
						found = true;
				}
				if(found)
				{
					partpath += line + @"\";
				}
			}

			return partpath;
		}

		public static string CutFileFromPath(this string fullpath)
		{
			string[] split = fullpath.Split('\\');
			string partpath = "";
			for(int i = 0; i < split.Count() - 1; i++ )
			{
				partpath += split[i] + @"\";
			}

			return partpath;

		}

		public static void ReplaceFile(string old, string newFile, bool overwrite = false)
		{
			if (File.Exists(old) && !overwrite)
			{
				return;
			}

			else
			{
				if (File.Exists(old))
					File.Delete(old);

				File.Move(newFile, old);
			}
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

		public static string RemoveFirstWord(string String, string[] specialCases = null, int spacesToInclude = 0, char delimiter = ' ')
		{
			string newString = "";
			string[] Temp = String.Split(delimiter);

			if (specialCases == null)
				specialCases = new string[0];

			if (spacesToInclude < 1 || !specialCases.Contains(Temp[0]))
			{
				int i = 0;
				foreach (string temp in Temp)
				{
					if (i != 0)
						newString += temp + " ";
					i++;
				}
				return newString;
			}

			else
			{
				for (int i = 0; i < Temp.Count(); i++)
				{
					if (i > spacesToInclude)
						newString += Temp[i] + " ";
				}
				return newString;
			}
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

		public static string GetFirstWord(string String, string[] specialCases = null, int spacesToInclude = 0, char delimiter = ' ')
		{
			string[] Temp = String.Split(delimiter);
			string newString = string.Empty;

			if (specialCases == null)
				specialCases = new string[0];

			if (spacesToInclude < 1 || Temp.Count() < 2 || !specialCases.Contains(Temp[0]))
				return Temp[0].Trim();

			else
			{
				for (int i = 0; i <= spacesToInclude; i++)
					newString += Temp[i] + " ";

				return newString.Trim();

			}
		}

		public static string Capitalise(this string input, bool allWords = false)
		{
			char[] characters = input.ToCharArray();

			if (characters.Count() == 0)
				return "";

			if (allWords)
			{
				for(int i =0; i < characters.Count(); i++)
				{
					if (characters[i] == ' ')
					{
						if (i + 1 < characters.Count())
						{
							if(char.IsLetter(characters[i+1]))
								characters[i+1] = char.ToUpper(characters[i+1]);
						}
					}
				}
			}

			characters[0] = char.ToUpper(characters[0]);
			return new string(characters);
		}

		public static string RemoveSpaces(this string input)
		{
			string[] split = input.Split(' ');
			string newString = "";
			foreach(string word in split)
			{
				newString += word;
			}
			return newString;
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

		public static string CarriageReturnNewLine(this string str, int amount = 1)
		{
			for (int i = 0; i < amount; i++)
			{
				str += "\r\n";
			}
			return str;
		}

		public static string ArrayToString(this float[] array, bool idx = false, bool insertNewlines = false, bool removeTrailingComma = false, int newLineCount = 1, string extension = "")
		{
			string value = "";
			int i = 0;
			foreach (float str in array)
			{
				if (idx)
					value += i.ToString() + ": ";
				value += str.ToString() + extension;

				if (insertNewlines)
					for (int nl = 0; nl < newLineCount; nl++)
						value += "\r\n";
				else value += ", ";


				i++;
			}

			if (removeTrailingComma)
				value = value.Trim().TrimEnd(',');

			return value;
		}

		public static string ArrayToString(this int[] array, bool idx = false, bool insertNewlines = false, bool removeTrailingComma = false, int newLineCount = 1)
		{
			string value = "";
			int i = 0;
			foreach (int str in array)
			{
				if (idx)
					value += i.ToString() + ": ";
				value += str.ToString();

				if (insertNewlines)
					for (int nl = 0; nl < newLineCount; nl++)
						value += "\r\n";
				else value += ", ";


				i++;
			}

			if (removeTrailingComma)
				value = value.Trim().TrimEnd(',');

			return value;
		}

		public static string ArrayToString(this string[] array, bool idx = false, bool insertNewlines = false, bool removeTrailingComma = false, int newLineCount = 1)
		{
			string value = "";
			int i = 0;
			foreach (string str in array)
			{
				if (idx)
					value += i.ToString() + ": ";
				value += str;

				if (insertNewlines)
					for(int nl = 0; nl < newLineCount; nl++)
						value += "\r\n";
				else value += ", ";
				

				i++;
			}

			if (removeTrailingComma)
				value = value.Trim().TrimEnd(',');

			return value;
		}

		public static string[] StringToArray(this string str, char delimeter = ',')
		{
			var split = str.Split(',').CleanStringArray();
			return split;
		}

		public static object GetRandomItemFromArray(this object[] array, Random rng)
		{
			object[] shuffled = array.Shuffle(rng);

			return shuffled[rng.Next(shuffled.Count())];
		}

		public static object GetRandomItemFromList(this List<object> array, Random rng)
		{ 
			object[] shuffled = array.ToArray().Shuffle(rng);

			return shuffled[rng.Next(shuffled.Count())];
		}

		public static object[] Shuffle(this object[] a, Random rng, int iterationMax = 100)
		{
			object[] b = a;

			int iterations = rng.Next(100, iterationMax);

			for (int i = iterations; iterations > 0; iterations--)
			{
				object temp;
				int moveFrom = rng.Next(0, 4);
				int moveTo = rng.Next(0, 4);


				temp = b[moveTo];  //{2, 0, 3, 1}
				b[moveTo] = b[moveFrom];
				b[moveFrom] = temp;
			}

			return b;
		}

		public static string DropComments(this string line)
		{
			string s = "";
			foreach (char c in line.ToCharArray())
			{
				if (c != ';')
					s += c;
				else return s;
			}

			return s;
		}

		public static string DropAndOutComments(this string line, out string comment)
		{
			string s = "";
			bool commentFound = false;
			comment = "";
			foreach (char c in line.ToCharArray())
			{
				if (c != ';' && !commentFound)
					s += c;
				else
					commentFound = true;

				if (commentFound)
					comment += c; 
			}

			return s;
		}

		public static bool ContainsMatch(this List<string> a, List<string> b, out string match)
		{
			foreach (string str in b)
			{
				if (a.Contains(str))
				{
					match = str;
					return true;
				}
				
			}
			match = "";
			return false;
		}
	}
}
