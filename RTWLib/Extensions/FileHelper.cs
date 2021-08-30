using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

		public static void RenameDirectory(string dir, string newName)
		{
			if (Directory.Exists(dir))
			{
				Directory.Move(dir, newName);
			}
				
		}

		public static bool FolderExists(string folder)
		{
			if (Directory.Exists(folder))
				return true;
			else return false;


		}
	}
}
