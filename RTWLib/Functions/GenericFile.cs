using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    public static class GenericFile
    {
        public static FileBase CreateSMF(string fileLocation)
        {
            FileData fd = new FileData(null, EStr.Array("faction"), '\t', ',', EStr.Array(' '), ';', 1, "", ',');
            FileBase file = new FileBase(RTWLib.Data.FileNames.none, string.Empty, string.Empty);
            file.data = new FileData(fd);
            file.Parse(new string[] { fileLocation }, out Logger.Logger.lineNumber, out Logger.Logger.lineText);
            file.data.FormulateAttributes(true, false);
            return file;
        }
        public static FileBase CreateEDU(string fileLocation)
        {
            FileData fd = new FileData(null, EStr.Array("faction"), '\t', ',', EStr.Array(' '), ';', 1, "", ',');
            FileBase file = new FileBase(RTWLib.Data.FileNames.none, string.Empty, string.Empty);
            file.data = new FileData(fd);
            file.Parse(new string[] { fileLocation }, out Logger.Logger.lineNumber, out Logger.Logger.lineText);
            file.data.FormulateAttributes(true, false);
            return file;
        }

    }
}
