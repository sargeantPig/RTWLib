using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB.ModelData
{
    public class ModelTri : IdmbOutput
    {
        string tag = "model_tri";
        int val;
        float[] modelTri;

        public ModelTri()
        { }

        public void ProcessLine(string[] data)
        {
            modelTri = new float[3];
            val = Convert.ToInt32(data[0]);
            for(int i = 1; i < data.Count(); i++)
            {
                string trimmed = data[i].Trim('f');
                modelTri[i-1] =  (float)Convert.ToDouble(trimmed);
            }
        
        }

        public string Output()
        {
            string str = "";

            if (modelTri != null)
            {
                str = String.Format("{0}{1}{2}{3}",
                    tag,
                    LibFuncs.GetNewWhiteSpace(tag),
                    val.ToString() + ", ",
                    LibFuncs.ArrayToString(modelTri, false, false, true, 0, "f"));
            }
            return str.CarriageReturnNewLine();
        }
    }
}
