using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    public class Attribute
    {
        string tag;
        public object[] items;
        public char Seperator { get; set; }

        public Attribute(string tag, object[] items, char seperator)
        {
            this.tag = tag;
            this.items = items;
            this.Seperator = seperator;
        }

        public string Output(string format)
        {
            return string.Format(format, tag, items.OutputArrayAsLine(Seperator.ToString()));
        }

        public object GetValue(int index, bool isNumeric)
        {
            if (index > items.Count() - 1)
                return "NULL";
            var item = items[index];
            if (isNumeric)
            {
                
                var match = Regex.Match((string)item, @"\d+");
                if (match != null)
                     return int.Parse(match.Value);
            }
            else return item;

            return "NULL";
        }

        public object this[int i]
        {
            get { return this.items[i]; }
            set { this.items[i] = value; }
        }


    }
}
