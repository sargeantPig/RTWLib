using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Extensions;

namespace RTWLib.Functions
{
    public class SMFBase<T>
    {
        public string tag { get; set; }
        public T value { get; set; }

        public SMFBase()
        { 
            
        }

        public string Output()
        {
            if (typeof(T) == typeof(bool))
            {
                string yesNo = "";

                if ((bool)(object)value == true)
                {
                    return string.Format("{0}{1}{2}\r\n",
                        tag, StrFormat.GetTabSpacing(tag, 7), "yes");
                }
                else
                {
                    return string.Format("{0}{1}{2}\r\n",
                        tag, StrFormat.GetTabSpacing(tag, 7), "no");
                }     
            }
            return string.Format("{0}{1}{2}\r\n",
                tag, StrFormat.GetTabSpacing(tag, 7), value);
        }

        public void ProcessLine(string tag, string[] data)
        {
            this.tag = tag;

            if (typeof(T) == typeof(int))
            {
                int val = Convert.ToInt32(data[0]);
                this.value = (T)(object)val;
            }
            else if (typeof(T) == typeof(string))
            {
                this.value = (T)(object)data[0];
            }
            else if (typeof(T) == typeof(bool))
            {
                if (data[0] == "yes")
                    this.value = (T)(object)true;
                if (data[0] == "no")
                    this.value = (T)(object)false;
            }
        }
    }
}
