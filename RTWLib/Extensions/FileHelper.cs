using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Extensions
{
    public static class FileHelper
    {
		public static void RenameFile(string old, string newFile)
		{
			if (File.Exists(old))
			{
				if (File.Exists(newFile))
					File.Delete(newFile);

				File.Move(old, newFile);
			}
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
	}
}
