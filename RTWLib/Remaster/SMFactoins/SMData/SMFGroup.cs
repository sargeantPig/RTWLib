using RTWLib.Extensions;
using RTWLib.Functions.Remaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.Remaster
{
    public class SMFGroup<T> : ReSMFBase<T>
    {

        public List<ReSMFBase<T>> objects = new List<ReSMFBase<T>>();
        
        public SMFGroup() : base() { } 



        new public string Output()
        {
            string output = string.Empty;
            foreach(object o in objects)
            {
                if (typeof(SMFGroup<string>) == o.GetType()) {
                    output += ((SMFGroup<string>)o).Output();
                }
                else if (typeof(MapColour<string>) == o.GetType())
                {
                    output += ((MapColour<string>)o).Output();
                }
                else output += ((ReSMFBase<string>)o).Output();
            }

            if(tag=="factions")
                return string.Format("{0}{1}\r\n{2}[\r\n{3}{4}],\r\n", 
                    StrFo.tab(tabDepth), FormatTag(tag), StrFo.tab(tabDepth), output, StrFo.tab(tabDepth));
            else return string.Format("{0}{1}\r\n{2}{{\r\n{3}{4}}},\r\n",
                    StrFo.tab(tabDepth), FormatTag(tag), StrFo.tab(tabDepth), output, StrFo.tab(tabDepth));

        }

    }
}
