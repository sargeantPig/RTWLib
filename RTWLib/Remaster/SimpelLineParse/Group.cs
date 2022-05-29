using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.Remaster
{
    public class Group<T> : Base<T> 
    {
        public List<Base<T>> objects = new List<Base<T>>();
        public Group() : base() { }
        new public string Output()
        {
            string output = string.Empty;
            foreach (object o in objects)
            {
                if (typeof(Group<string>) == o.GetType())
                {
                    output += ((Group<string>)o).Output();
                }
                else if (typeof(Array<string>) == o.GetType())
                {
                    output += ((Array<string>)o).Output();
                }
                else output += ((Base<string>)o).Output();
            }

            if (tag == "factions")
                return string.Format("{0}{1}\r\n{2}[\r\n{3}{4}],\r\n",
                    StrFo.tab(tabDepth), FormatTag(tag), StrFo.tab(tabDepth), output, StrFo.tab(tabDepth));
            else return string.Format("{0}{1}\r\n{2}{{\r\n{3}{4}}},\r\n",
                    StrFo.tab(tabDepth), FormatTag(tag), StrFo.tab(tabDepth), output, StrFo.tab(tabDepth));

        }

        new public string SimpleOutput()
        {
            string output = string.Empty;
            foreach (object o in objects)
            {
                if (typeof(Group<string>) == o.GetType())
                {
                    output += ((Group<string>)o).SimpleOutput();
                }
                else if (typeof(Array<string>) == o.GetType())
                {
                    output += ((Array<string>)o).SimpleOutput();
                }
                else output += ((Base<string>)o).SimpleOutput();
            }

            return string.Format("{0}{1}{2}\r\n{3}\r\n", tag, StrFo.GetNewWhiteSpace(tag), value, output);
        }
    }
}
