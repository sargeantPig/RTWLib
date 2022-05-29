using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Extensions;
namespace RTWLib.Functions.Remaster
{
	public partial class FileWrapper : FileBase, IFile
	{
		public List<Base<string>> objects = new List<Base<string>>();
		bool quoteTags, noDepth;
		char delim, arrDelim, openArray, closeArray;
		string identifier;

		public FileWrapper(bool quoteTags, bool noDepth, char delim, char arrDelim, 
			char openArray, char closeArray, FileNames name, string path = "", string descr = "", string identifier = null) 
			:base(name, path, descr)
		{
			this.quoteTags = quoteTags;
			this.delim = delim;
			this.identifier = identifier;
			this.arrDelim = arrDelim;
			this.noDepth = noDepth;
			this.openArray = openArray;
			this.closeArray = closeArray;

		}

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			currentLine = "";
			StreamReader dmb = new StreamReader(paths[0]);
			lineNumber = 0;

			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}

			while ((currentLine = dmb.ReadLine()) != null)
			{
				bool success = Parse(currentLine, objects);
				lineNumber++;
			}

			dmb.Close();
		}
		override public string Output()
		{
			string str = "";
			foreach (var d in objects)
			{
				if (typeof(Group<string>) == d.GetType())
					str += ((Group<string>)d).Output();
			}
			return str;
		}
		public string SimpleOutput()
		{
			string str = "";
			foreach (var d in objects)
			{
				if (typeof(Group<string>) == d.GetType())
					str += ((Group<string>)d).SimpleOutput();
			}
			return str;
		}
		public void ToFileSimple(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(SimpleOutput());
			sw.Close();
		}
		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
		}
		public string[] GetValues(List<Base<string>> obj, params string[] terms)
		{
			foreach (var t in obj) {
				string s = terms.FirstOrDefault(x => x == t.tag);
				if (t.tag == s ) {
					if (t.GetType() == typeof(Group<string>))
						return GetValues(((Group<string>)t).objects, terms);
				}

				if (t.tag == terms.Last())
				{
					if (t.GetType() == typeof(Base<string>))
						return EStr.Array(t.value);
					else if (t.GetType() == typeof(Group<string>))
						return EStr.Array(t.value);
					else if (t.GetType() == typeof(Array<string>))
						return ((Array<string>)t).items;
				}
			}

			return null;
		}

		public static FileWrapper CreateSMF(string[] paths)
        {
			FileWrapper smf = new FileWrapper(true, false, ':', ',', '[', ']', FileNames.descr_sm_faction);
			int line;
			string str;
			smf.Parse(paths, out line, out str);

			return smf;
		}
	}
}
