using RTWLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.UnitDescriptions
{
    public class UnitDescription
    {
        public static readonly string div = "¬----------------";

        public string internalName { get; set; }
        public string externalName { get; set; }
        public string longDescr { get; set; }
        public string shortDescr { get; set; }

        public string LDTag { get; set; }
        public string SDTag { get; set; }
        public string nameTag { get; set; }
        public UnitDescription()
        { }

        public void AddHiddenStats(Unit unit)
        { 
            
        
        }

        public string Output()
        {
            string str = "¬".CarriageReturnNewLine();
            str += string.Format("{0}{1}{2}{3}{4}{5}{6}",
                nameTag + "\t", externalName.CarriageReturnNewLine(2),
                LDTag.CarriageReturnNewLine(), longDescr.CarriageReturnNewLine(2),
                SDTag.CarriageReturnNewLine(), shortDescr.CarriageReturnNewLine(2),
                div.CarriageReturnNewLine(2));

            return str;
        }
    }
}
