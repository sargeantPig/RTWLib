using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTWLib.Memory;
using System.Security.Principal;
using RTWLib.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.IO;
using System.CodeDom;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace RTWLib.Functions
{
	public interface IFile
	{
		void Parse(string[] path, out int lineNumber, out string currentLine);
		string Log(string txt);
		string Output();
        FileNames Name
        {
            get;
        }
		string FilePath
		{
			get;
		}
		string Description
		{
			get;
		}
		void ToFile(string filepath);
		void ToFile(string filepath, Encoding encoder);

	}

	public interface IDescrStrat
	{
		void ShuffleFactions(Random rnd);
		void MoveFactionToTopOfStrat(string name);
		void CleanUp();
		void Parse(string[] path, out int lineNumber, out string currentLine);
		string Log(string txt);
		string Output();
		FileNames Name
		{
			get;
		}
		List<string> GetAllPlayableFactions
		{
			get;
		}
		string FilePath
		{
			get;
		}
		string Description
		{
			get;
		}
		void ToFile(string filepath);
	}


	

	public class FileData<T>
	{
		Dictionary<T, int> expectedSizes;

		public FileData(Dictionary<T, int> data)
		{
			this.expectedSizes = data;
		}

		public void Add(T key, int value)
		{
			if (!expectedSizes.ContainsKey(key))
				expectedSizes.Add(key, value);
		}

		public int this[T i]
		{
			get { return expectedSizes[i]; }
			set { expectedSizes[i] = value; }
		}
			
	}





	static public class LibFuncs
	{
		/// <summary>
		/// Converts an array of numbers as strings to an array of ints.
		/// </summary>
		/// <returns></returns>
		
		
	}
}
