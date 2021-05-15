using RTWLib.MapGen.Voronoi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class ExtList
    {
		public static object GetRandomItemFromList(this List<object> array, Random rng)
		{
			object[] shuffled = array.ToArray().Shuffle(rng);

			return shuffled[rng.Next(shuffled.Count())];
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

		public static bool ContainsCoordinate(this List<int[]> a, int[] find)
		{
			int ind = a.FindIndex(x => x[0] == find[0] && x[1] == find[1]);

			if (ind > -1)
				return true;
			else return false;
		}

		public static bool ContainsCoordinate(this HashSet<int[]> a, int[] find)
		{
			return a.Contains(find);
		}

		public static void AddElementsUnique(this HashSet<int[]> a, HashSet<int[]> elements)
		{
			foreach (var e in elements)
			{
				if (!a.Contains(e))
					a.Add(e);
			}
		}

		public static bool ContainsItem(this HashSet<int[]> a, int[] b)
		{
			if (a.Count == 0)
				return false;

			foreach (var e in a)
			{

				if (b[0] == e[0] && b[1] == e[1])
					return true;
			}

			return false;
		}

		public static HashSet<int[]> GetSurroundingPoints(this int[,] mask, HashSet<int[]> ignore, int[] p, int width, int height, Func<int, int, bool> op, int valueToFind = 1)
		{
			HashSet<int[]> found = new HashSet<int[]>();
			int xinc = 1;
			int xdec = 1;
			int yinc = 1;
			int ydec = 1;
			if (p[0] + xinc >= width)
				xinc = 0;
			if (p[0] - xdec < 0)
				xdec = 0;
			if (p[1] + yinc >= height)
				yinc = 0;
			if (p[1] - ydec < 0)
				ydec = 0;

			if (op(mask[p[0] + xinc, p[1]], valueToFind) && (p[0] + xinc != p[0]) && !ignore.ContainsItem(new int[] { p[0] + xinc, p[1] }))
				found.Add(new int[] { p[0] + xinc, p[1] });
			if (op(mask[p[0] - xdec, p[1]], valueToFind) && (p[0] - xdec != p[0]) && !ignore.ContainsItem(new int[] { p[0] - xdec, p[1] }))
				found.Add(new int[] { p[0] - xdec, p[1] });
			if (op(mask[p[0], p[1] + yinc], valueToFind) && (p[1] + yinc != p[1]) && !ignore.ContainsItem(new int[] { p[0], p[1] + yinc }))
				found.Add(new int[] { p[0], p[1] + yinc });
			if (op(mask[p[0], p[1] - ydec], valueToFind) && (p[1] - ydec != p[1]) && !ignore.ContainsItem(new int[] { p[0], p[1] - ydec }))
				found.Add(new int[] { p[0], p[1] - ydec });


			return found;
		}

		public static bool ContainsItem(this List<VoronoiPoint>a, VoronoiPoint b)
		{
			if (a.Count == 0)
				return false;

			foreach (var e in a)
			{

				if (b.x == e.x && b.y == e.y)
					return true;
			}

			return false;
		}

		public static string[] PigsToArray(this List<string> data)
		{
			string[] array = new string[data.Count];
			for (int i = 0; i < data.Count; i++)
			{
				array[i] = data[i];
			}
			return array;
		}

		public static bool ContainsItem(this VoronoiPoint[] a, VoronoiPoint b)
		{
			if (a[0] == null)
				return false;

			foreach (var e in a)
			{
				if (e == null)
					continue;
				if (b.x == e.x && b.y == e.y)
					return true;
			}

			return false;
		}
	}
}
