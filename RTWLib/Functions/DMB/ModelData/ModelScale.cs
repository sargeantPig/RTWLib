using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB.ModelData
{
    public class ModelScale : IdmbOutput
    {
        public bool Active { get; set; } = false;
        string tag = "scale";
        float scale;
        public ModelScale()
        { }

        public void ProcessLine(string[] data)
        {
            scale = (float)Convert.ToDouble(data[0]);
        } 

        public string Output()
        {
            string str = "";

            if (!Active)
                return str;

            str = String.Format("{0}{1}{2}",
                tag,
                LibFuncs.GetNewWhiteSpace(tag),
                scale);

            return str.CRL();
        }
    }
}
