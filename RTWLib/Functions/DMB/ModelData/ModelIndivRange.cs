using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB.ModelData
{
    public class ModelIRange : IdmbOutput
    {
        string tag = "indiv_range";
        int range = 0;

        public ModelIRange()
        { }

        public void ProcessLine(string[] data)
        {
            range = Convert.ToInt32(data[0]);
        }

        public string Output()
        {
            string str = "";

            str = String.Format("{0}{1}{2}",
                tag,
                LibFuncs.GetNewWhiteSpace(tag),
                range);

            return str.CRL();
        }
    }
}
