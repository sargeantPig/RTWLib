using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Functions
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

        public void ProcessLine(string[] data, int colIndex)
        {
            colours[colIndex] = GetColour(data);
        }

        public string Output()
        {
            return string.Format("primary_colour{0}red {1}, green {2}, blue {3}\r\n" +
                "secondary_colour{4}red {5}, green {6}, blue {7}\r\n",
                LibFuncs.GetTabSpacing(priTag, 7), colours[0].R, colours[0].G, colours[0].B,
                LibFuncs.GetTabSpacing(secTag, 7), colours[1].R, colours[1].G, colours[1].B);  
         }

        private Color GetColour(string[] data)
        {
            string[] digits = new string[data.Count()];

            for (int i = 0; i < data.Count(); i++)
            {
                data[i] = data[i].Trim(',');
                digits[i] = data[i].Split(' ')[1]; 
            }
            return Color.FromArgb(Convert.ToInt32(digits[0]), Convert.ToInt32(digits[1]), Convert.ToInt32(digits[2]));

        }
    }
}
