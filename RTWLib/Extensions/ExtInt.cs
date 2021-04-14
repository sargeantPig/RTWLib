using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class ExtInt
    {
		public static int Clamp(this int value, int min, int max)
		{
			if (value < min)
				return min;
			else if (value > max)
				return max;
			else return value;
		}
	}
}
