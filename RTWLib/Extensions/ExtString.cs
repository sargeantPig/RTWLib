using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
	public static class EString
    {

		public static readonly string NewLine = "\r\n";

		public static string[] Array(params string[] strings)
		{
			return strings;
		}

		public static char[] Array(params char[] chars)
		{
			return chars;
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

		public static string GetPathFrom(this string fullpath, string pathStart, int counterMatchesToIgnore)
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
					if (matchCount > counterMatchesToIgnore)
						found = true;
				}
				if (found)
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
			for (int i = 0; i < split.Count() - 1; i++)
			{
				partpath += split[i] + @"\";
			}

			return partpath;

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
			for (int i = 0; i < split.Count(); i++)
			{
				if (i == split.Count() - 1)
					output += split[i];
				else output += split[i] + replacer;
			}
			return output;
		}

		public static string RemoveFirstWord(this string String, string[] specialCases = null, int spacesToInclude = 0, char delimiter = ' ')
		{
			string newString = "";
			string[] Temp = String.Trim().Split(delimiter);

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

		public static string RemoveFirstWord(this string String, char delimiter)
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

		public static string GetFirstWord(this string String, string[] specialCases = null, int spacesToInclude = 0, char delimiter = ' ')
		{
			string[] Temp = String.Trim().Split(delimiter);
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

		public static string[] SplitFirst(this string str, char tagDelim, char paramDelim)
		{
			

			string[] split = str.Split(tagDelim).CleanStringArray();

			if(split.Count() > 1)
				split[1] = split.OutputArrayAsLine(" ", 0);
			List<string> temp = new List<string>() {split[0]};

			if(split.Count() > 1)
				temp.AddRange(split[1].Split(paramDelim));

			return temp.ToArray();

		}

		public static string Capitalise(this string input, bool allWords = false)
		{
			char[] characters = input.ToCharArray();

			if (characters.Count() == 0)
				return "";

			if (allWords)
			{
				for (int i = 0; i < characters.Count(); i++)
				{
					if (characters[i] == ' ')
					{
						if (i + 1 < characters.Count())
						{
							if (char.IsLetter(characters[i + 1]))
								characters[i + 1] = char.ToUpper(characters[i + 1]);
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
			foreach (string word in split)
			{
				newString += word;
			}
			return newString;
		}

		public static string RemoveLastWord(this string String)
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

		public static bool Contains(this string str, params string[] list)
		{
			foreach (string s in list)
			{
				if (str.Contains(s))
					return true;
			}

			return false;
		}

		public static bool Contains(this string str, List<char> list)
		{
			foreach (char s in list)
			{
				if (str.Contains(s))
					return true;
			}

			return false;
		}

		public static string CRL(this string str, int amount = 1)
		{
			for (int i = 0; i < amount; i++)
			{
				str += "\r\n";
			}
			return str;
		}

		public static string CRL()
		{
			return "\r\n";
		}

		public static string[] StringToArray(this string str, char delimeter = ',')
		{
			var split = str.Split(delimeter).CleanStringArray();
			return split;
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

		public static bool StartsWithChar(this string s, char c)
		{
			char firstLetter = s.ToArray()[0];
			if (firstLetter == c)
				return true;

			return false;

		}
	}
}
