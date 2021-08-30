using RTWLib.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
	public class FileBase : Logger.Logger, IFile
	{
		FileNames name;
		string FILEPATH;
		string DESCRIPTION;
		public FileData data = new FileData();


		public FileBase(FileNames name, string filePath, string description)
		{
			this.name = name;
			this.FILEPATH = filePath;
			this.DESCRIPTION = description;

		}
		public FileBase(FileNames name, string filePath, string description, FileData data)
		{
			this.name = name;
			this.FILEPATH = filePath;
			this.DESCRIPTION = description;
			this.data = new FileData(data);
		}

		virtual public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			string tempdata = string.Empty;
			lineNumber = -1;
			currentLine = string.Empty;
			using (var stream = new StreamReader(paths[0]))
			{
				tempdata = stream.ReadToEnd();
			}

			data = new FileData(tempdata, data);
		}

		virtual public string Output()
		{
			return data.Output();
		}

		virtual public void ToFile(string filepath, Encoding encoder)
		{
			using (StreamWriter sw = new StreamWriter(filepath, false, encoder))
			{
				sw.Write(Output());
			}
		}

		virtual public void ToFile(string filepath)
		{
			using (StreamWriter sw = new StreamWriter(filepath))
			{
				sw.Write(Output());
			}
		}

		public FileNames Name
		{
			get { return name; }
		}
		public string FilePath
		{
			get { return FILEPATH; }
		}

		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}
	}
}
