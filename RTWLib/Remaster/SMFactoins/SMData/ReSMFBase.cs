using RTWLib.Extensions;
using System;
using System.Linq;

namespace RTWLib.Functions.Remaster
{
    public class ReSMFBase<T>
    {
        public string tag { get; set; }
        public T value { get; set; }

        public int tabDepth = 0;

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
                    return string.Format("{0}{1}{2},\r\n",
                        StrFo.tab(tabDepth), FormatTag(tag), "true");
                }
                else
                {
                    return string.Format("{0}{1}{2},\r\n",
                        StrFo.tab(tabDepth), FormatTag(tag), "false");
                }
            }

            bool result;
            if(bool.TryParse((string)(object)value, out result))
            {
                return string.Format("{0}{1}    {2}\r\n",
                StrFo.tab(tabDepth), FormatTag(tag), FormatValue((string)(object)value, false));
            }


            int res;
            if(int.TryParse((string)(object)value,out res))
            {
                return string.Format("{0}{1}    {2}\r\n",
                StrFo.tab(tabDepth), FormatTag(tag), FormatValue((string)(object)value, false));
            }

            else return string.Format("{0}{1}    {2}\r\n",
                StrFo.tab(tabDepth), FormatTag(tag), FormatValue((string)(object)value, true));

        }

        public string FormatTag(string toForm)
        {
            return string.Format("\"{0}\":", toForm);
        }

        public string FormatValue(string toForm, bool quote = true)
        {
            if(quote)
                return string.Format("\"{0}\",", toForm);
            else return string.Format("{0},", toForm);
        }

        public void ProcessLine(string tag, string[] data, int depth)
        {
            this.tag = tag;
            this.tabDepth = depth;
            if (typeof(T) == typeof(int))
            {
                int val = Convert.ToInt32(data[0]);
                this.value = (T)(object)val;
            }
            else if (typeof(T) == typeof(string))
            {
                string clean = data[0].GetQuotedWord();

                clean = clean.Trim(',', ' ');
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
                for (int i = 0; i < clean.Count(); i++)
                {
                    clean[i] = ((string)clean[i]).GetQuotedWord();
                }
                this.value = (T)(object)clean;
            }
        }

        public void ProcessFaction(string tag, int depth)
        {
            this.tag = tag;
            this.tabDepth = depth;
        }
    }
}
