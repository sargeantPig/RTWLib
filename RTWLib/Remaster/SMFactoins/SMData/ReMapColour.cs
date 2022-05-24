using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
using RTWLib.Extensions;

namespace RTWLib.Functions.Remaster
{
    public class MapColour<T> : ReSMFBase<T>
    {
        public string[] colour { get; set; }

        public MapColour()
        {
        }
        public MapColour(int number)
        {
        }

        new public void ProcessLine(string tag, string[] data, int depth)
        {
            this.tag = tag;
            this.tabDepth = depth;
            colour = data;
        }

        new public string Output()
        {
            int res;
            string str = string.Empty;
            if (colour.Count() != 0)
            {
                if (int.TryParse(colour[0], out res))
                    str = colour.ArrayToString(false, false, false, 1, true, false);
                else str = colour.ArrayToString(false, false, false, 1, true, true);
            }
            return string.Format(
                "{0}{1}   [{2}],\r\n",
                StrFo.tab(tabDepth), FormatTag(tag), str);
               
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
