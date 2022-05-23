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
    public class MapColour
    {
        string priTag = "primary_colour";
        string secTag = "secondary_colour";

        public Color[] colours { get; set; }

        public MapColour()
        {
            colours = new Color[2];
        }
        public MapColour(int number)
        {
            colours = new Color[number];
        }
        public void ProcessLine(string[] data, int colIndex)
        {
            object[] clean = data.RemoveElementsAt(data.Count() - 1).RemoveElementsAt(0);
            colours[colIndex] = GetColour(clean);
        }

        public string Output()
        {
            return string.Format("primary_colour{0}red {1}, green {2}, blue {3}\r\n" +
                "secondary_colour{4}red {5}, green {6}, blue {7}\r\n",
                StrFormat.GetTabSpacing(priTag, 7), colours[0].R, colours[0].G, colours[0].B,
                StrFormat.GetTabSpacing(secTag, 7), colours[1].R, colours[1].G, colours[1].B);  
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
