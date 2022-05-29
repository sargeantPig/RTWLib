using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.Remaster
{
    public class Array<T> : Base<T>
    {
        public string[] items { get; set; }

        public Array()
        {
        }
        public Array(int number)
        {
        }

        new public void ProcessLine(string tag, string[] data, int depth)
        {
            this.tag = tag;
            this.tabDepth = depth;
            items = data;
        }

        new public string Output()
        {
            int res;
            string str = string.Empty;
            if (items.Count() != 0)
            {
                if (int.TryParse(items[0], out res))
                    str = items.ArrayToString(false, false, false, 1, true, false);
                else str = items.ArrayToString(false, false, false, 1, true, true);
            }
            return string.Format(
                "{0}{1}   [{2}],\r\n",
                StrFo.tab(tabDepth), FormatTag(tag), str);

        }
        new public string SimpleOutput()
        {
            string str = string.Empty;
            str = items.ArrayToString(false, false, true);
            return string.Format("{0}{1}{2}\r\n", tag, StrFo.GetNewWhiteSpace(tag), str);
        }
        private Color GetColour(object[] data)
        {
            string[] digits = new string[data.Count()];

            for (int i = 0; i < data.Count(); i++)
            {
                data[i] = ((string)data[i]).Trim(',');
                digits[i] = ((string)data[i]);
            }
            return Color.FromArgb(Convert.ToInt32(digits[0]), Convert.ToInt32(digits[1]), Convert.ToInt32(digits[2]));
        }
    }
}
