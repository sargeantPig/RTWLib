using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
	public interface IFile
	{
		Task Parse();
		string Log(string txt);
		string Description
		{
			get;
		}
	}

	static public class Functions_General
	{
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

			return Temp[0];
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

		public static double DistanceTo(int[] a, int[] b)
		{
			double dis = Math.Sqrt(Math.Pow((a[0] - b[0]), 2) + Math.Pow((a[1] - b[1]), 2));

			return dis;
		}

	}
}
