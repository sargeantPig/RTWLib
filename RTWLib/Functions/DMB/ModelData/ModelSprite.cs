using RTWLib.Extensions;
using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTWLib.Functions.DMB.ModelData
{
    public class ModelSprite : IdmbOutput
    {
        string tag = "model_sprite";
        List<string> factions;
        float value;
        string filepath;

        public ModelSprite()
        { }

        public void ProcessLine(string[] data)
        {
            bool intFirst;
            bool addToFaction = false;
            float val;

            intFirst = float.TryParse(data[0], out val);

            if (intFirst)
            {
                value = val;
                addToFaction = false;
            }
            else if (!intFirst)
            {
                addToFaction = true;
                factions = new List<string>();
            }
            foreach (var d in data)
            {
                if (addToFaction && !intFirst)
                {
                    if (d.Contains("/"))
                        filepath = d;
                    else
                    {
                        factions.Add(d);
                    }
                }
                else if (intFirst)
                {
                    intFirst = false;
                    addToFaction = true;
                    continue;
                }    
            }

        }
        public string Output()
        {
            string str = "";
         
            if(factions == null)
                str = String.Format("{0}{1}{2}, {3}", tag,
                    StrFormat.GetNewWhiteSpace(tag), 
                    value, 
                    filepath );
            else if(factions.Count > 0)
                str = String.Format("{0}{1}{2}{3}, {4}", tag,
                   StrFormat.GetNewWhiteSpace(tag),
                   factions.ToArray().ArrayToString(false, false, true, 1),
                   value,
                   filepath );

            return str.CRL();

        }
    }
}
