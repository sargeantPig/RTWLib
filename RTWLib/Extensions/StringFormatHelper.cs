using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class StrFormat
    {
		public static string GetTabSpacing(string ident, int maxtabs, int lettersPerTab = 4)
		{
			int tabs = maxtabs - (ident.Length / lettersPerTab);
			return identSpacing(tabs, '\t');
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
		public static string identSpacing(int identifierLength, char spacingChar = ' ')
		{
			string spaces = "";

			for (int i = 0; i < identifierLength; i++)
				spaces += spacingChar;
			return spaces;
		}
	}
}
