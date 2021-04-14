using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class ExtFloat
    {
        public static string UniversalOutput(this float input)
        {
            return input.ToString().GetSafeRTWDoubleStr();
        }
		/// <summary>
		/// Catches a divide by 0 and returns 1 or 0 according to optional param
		/// </summary>
		/// <returns></returns>
		public static float SafeDivide(this float a, float b)
		{
			float result;

			if (a == 0 || b == 0)
				return 0;

			result = a / b;
			return result;
		}

		public static float NormaliseToRange(this float val, float min, float max)
		{
			return (val - min) / (max - min);
		}

		public static float Clamp(this float value, float min, float max)
		{
			if (value < min)
				return min;
			else if (value > max)
				return max;
			else return value;
		}
	}
}
