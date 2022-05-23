﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Extensions;

namespace RTWLib.Functions.Remaster
{
    public class ReSMFBase<T>
    {
        public string tag { get; set; }
        public T value { get; set; }

        public ReSMFBase()
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
                string clean = data[0].GetQuotedWord();
                this.value = (T)(object)clean;
            }
            else if (typeof(T) == typeof(bool))
            {
                if (data[0] == "yes")
                    this.value = (T)(object)true;
                if (data[0] == "no")
                    this.value = (T)(object)false;
            }
            else if (typeof(T) == typeof(object[]))
            {
                object[] clean = data.RemoveElementsAt(data.Count() - 1).RemoveElementsAt(0);
                for(int i =0; i < clean.Count(); i++)
                {
                    clean[i] = ((string)clean[i]).GetQuotedWord();
                }
                this.value = (T)(object)clean;

            }
        }

        public void ProcessFaction(string tag)
        {
            this.tag = tag;
        }
    }
}
