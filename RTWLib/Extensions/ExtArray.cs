using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
	public static class ExtArr
	{
		public static Color ColStr(this string[] a)
		{
			a.CleanStringArray();
			return Color.FromArgb(a[0].ToInt(), a[1].ToInt(), a[2].ToInt());
		}
		public static int[,] InitiliseWithValue(this int[,] a, int value)
		{
			for (int x = 0; x < a.GetLength(0); x++)
				for (int y = 0; y < a.GetLength(1); y++)
					a[x, y] = value;

			return a;
		}

		public static float[,] InitiliseWithValue(this float[,] a, float value)
		{
			for (int x = 0; x < a.GetLength(0); x++)
				for (int y = 0; y < a.GetLength(1); y++)
					a[x, y] = value;

			return a;
		}


		public static bool ContainsKeyExt(this Dictionary<int[,], int> a, int[,] b)
		{
			foreach (var key in a.Keys)
			{
				bool match = true;
				for (int x = 0; x < key.GetLength(0); x++)
				{
					for (int y = 0; y < key.GetLength(1); y++)
					{

						if (key[x, y] != b[x, y])
							match = false;

						if (!match)
							break;
					}
					if (!match)
						break;
				}

				if (match)
					return true;
			}

			return false;
		}

		public static void MinMax(this float[,] data, out float min, out float max)
		{
			min = int.MaxValue;
			max = 0;
			foreach (var item in data)
			{
				if (item < min)
					min = item;
				if (item > max)
					max = item;
			}
		}

		public static void MinMax(this int[,] data, out float min, out float max)
		{
			min = int.MaxValue;
			max = 0;
			foreach (var item in data)
			{
				if (item < min)
					min = item;
				if (item > max)
					max = item;
			}
		}

		public static MagickImage ToImage(this float[,] data)
		{
			MagickImage mi = new MagickImage(MagickColor.FromRgb(0, 0, 0), data.GetLength(0), data.GetLength(1));
			float min, max;
			data.MinMax(out min, out max);
			using(IPixelCollection<UInt16> pi = mi.GetPixels())
			{
				for (int y = 0; y < mi.Height; y++)
				{
					for (int x = 0; x < mi.Width; x++)
					{
						float norm = 0;
						byte bnorm = 0;
						if (data[x, y] != 0)
						{
							norm = (data[x, y].NormaliseToRange(min, max));
							bnorm = (byte)(norm * 255);
						}

						pi.ModifyPixel(x, y, MagickColor.FromRgb(bnorm, bnorm,bnorm));
					}
				}
			}

			return mi;
		}


		public static int[] ClampValues(this int[] a, int[] indexToClamp, int min, int max)
		{
			foreach(var i in indexToClamp)
				a[i] = a[i].Clamp(min, max);

			return a;
		}
		public static int CountValues(this int[,] a, int countIf =0)
		{
			int value = 0;
			for (int x = 0; x < a.GetLength(0); x++)
				for (int y = 0; y < a.GetLength(1); y++)
				{
					if (a[x, y] > countIf)
						value += 1;
				}

			return value;
		}

		public static int CountValues(this float[,] a, float countIf = 0)
		{
			int value = 0;
			for (int x = 0; x < a.GetLength(0); x++)
				for (int y = 0; y < a.GetLength(1); y++)
				{
					if (a[x, y] > countIf)
						value += 1;
				}

			return value;
		}

		public static bool CheckOutOfBounds(this int[] p, int width, int height)
		{
			if (p[0] < width && p[0] > -1 && p[1] < height && p[1] > -1)
			{
				return false;
			}
			else return true;
		}

		public static int[] ConvertToIntArray(this string[] data)
		{
			int[] ints = new int[data.Count()];

			for (int i = 0; i < data.Count(); i++)
				ints[i] = Convert.ToInt32(data[i]);

			return ints;

		}
		public static double DirectionTo(this int[] a, int[] b)
		{
			double dir = Math.Pow(Math.Tan(ExtFloat.SafeDivide((b[1] - a[1]), (b[0] - a[0]))), -1);
			return dir;
		}


		public static double DistanceTo(this int[] a, int[] b)
		{
			double dis = Math.Sqrt(Math.Pow((a[0] - b[0]), 2) + Math.Pow((a[1] - b[1]), 2));

			return dis;
		}

		public static double DistanceToNorm(this int[] a, int[] b)
		{
			double dis = Math.Pow((a[0] - b[0]), 2) + Math.Pow((a[1] - b[1]), 2);

			return dis;
		}

		public static string OutputArrayAsLine(this object[] array, string seperator, params int[] ignoreElements)
		{
			string value = "";
			for (int i = 0; i < array.Count(); i++)
			{
				if (!ignoreElements.Contains(i))
				{
					value += string.Format("{0}{1}", ((string)array[i]).Trim(), seperator);
				}
			}
			value = value.Trim().TrimEnd(seperator.ToArray());

			return value;
		}

		public static object[] RemoveElementsAt(this object[] array, params int[] indexes)
		{
			object[] newArr = new object[array.Count() - indexes.Count()];

			int index = 0;
			for (int i = 0; i < array.Count(); i++)
			{
				if (!indexes.Contains(i))
				{
					newArr[index] = array[i];
					index++;
				}
			}

			return newArr;
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
		public static string ArrayToString(this string[] array, bool idx = false, bool insertNewlines = false, bool removeTrailingComma = false, int newLineCount = 1, bool insertSeperator = true, bool quote = false)
		{
			string value = "";
			int i = 0;
			foreach (string str in array)
			{
				if (idx)
					value += i.ToString() + ": ";

				if (quote)
					value += string.Format("\"{0}\"", str);
				else value += str;
				if (insertNewlines)
					for (int nl = 0; nl < newLineCount; nl++)
						value += "\r\n";
				else if(insertSeperator) value += ", ";


				i++;
			}

			if (removeTrailingComma)
				value = value.Trim().TrimEnd(',');

			return value;
		}
		public static object GetRandomItemFromArray(this object[] array, Random rng)
		{
			object[] shuffled = array.Shuffle(rng);

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
		public static string[] CleanStringArray(this string[] data, bool trim = true)
		{
			List<string> newArray = new List<string>();
			foreach (string str in data)
			{
				if (str != "" && str != " ")
				{
					if(trim)
						newArray.Add(str.Trim());
					else newArray.Add(str);
				}
			}
			return newArray.ToArray();
		}

		public static float[,] NormaliseArray(this float[,] arr)
		{
			float min;
			float max;
			arr.MinMax(out min, out max);

			for (int x = 0; x < arr.GetLength(0); x++)
				for(int y = 0; y < arr.GetLength(1); y++)
				{
					arr[x,y] =  arr[x,y].NormaliseToRange(min, max);
				}

			return arr;

		}

		public static bool ContainsEquals(this string str, string[] array)
		{
			foreach (var s in array)
			{
				if (s == str)
					return true;
			}

			return false;
		}
	}
}
