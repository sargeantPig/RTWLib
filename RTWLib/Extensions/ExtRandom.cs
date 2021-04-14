using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class ExtRandom
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
		public static T RandomFlag<T>(this Random rnd)
		{
			Array flags = Enum.GetValues(typeof(T));
			var a = (T)flags.GetValue(rnd.Next(flags.Length));

			return a;
		}
		public static T RandomFlag<T>(this Random rnd, int min, int max)
		{
			Array flags = Enum.GetValues(typeof(T));
			var a = (T)flags.GetValue(rnd.Next(min, max));

			return a;
		}
	}
}
