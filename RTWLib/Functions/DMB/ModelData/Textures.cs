using RTWLib.Extensions;
using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB.ModelData
{
    public class Texture : IdmbOutput
    {
        string tag = "texture";
        public List<string> factions { get; set;}
        public string filepath { get; set; }

        public Texture()
        { }

        public Texture(string faction, string filepath)
        {
            this.factions = new List<string>();
            this.factions.Add(faction);
            this.filepath = filepath;
        }

        public bool HasMercLine()
        {
            return factions.Contains("merc");
        }

        public void ProcessLine(string[] data)
        {
            factions = new List<string>();
            foreach (var d in data)
            {
                if (d.Contains("/"))
                    filepath = d;
                else
                    factions.Add(d);
            }
        }

        public string Output()
        {
            string str = "";

            if (factions != null)
                str = String.Format("{0}{1}{2}{3}", 
                    tag,
                    StrFormat.GetNewWhiteSpace(tag),
                   factions.ToArray().ArrayToString( false, false, false, 0),
                    filepath);
            else
            {
                str = String.Format("{0}{1}{2}", 
                    tag,
                    StrFormat.GetNewWhiteSpace(tag),
                    filepath);
            }

            return str.CRL();
            
        
        }

    }
}
