using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTWLib.Extensions
{
    public static class Comparisons
    {
		public static int CompareNameEnd(CheckBox c1, CheckBox c2)
		{
			string[] split1 = c1.Name.Split('_');
			string[] split2 = c2.Name.Split('_');

			string value1 = split1.Last();
			string value2 = split2.Last();

			return value1.CompareTo(value2);
		}

		public static int CompareNameEnd(Control c1, Control c2)
		{
			string[] split1 = c1.Name.Split('_');
			string[] split2 = c2.Name.Split('_');

			string value1 = split1.Last();
			string value2 = split2.Last();

			return value1.CompareTo(value2);
		}
	}
}
