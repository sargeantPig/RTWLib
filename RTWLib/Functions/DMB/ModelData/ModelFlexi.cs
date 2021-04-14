using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Extensions;
namespace RTWLib.Functions.DMB.ModelData
{
    public enum FlexiTypes
    { 
        model_flexi,
        model_flexi_m
    }

    public class ModelFlexi : IdmbOutput
    {
        FlexiTypes type;
        string filepath;
        string valueStr;
        int valueInt = int.MaxValue;

        public ModelFlexi()
        { }

        public void ProcessLine(string[] data, FlexiTypes type)
        {
            this.type = type;
            filepath = data[0];
            int val;
            bool isInt;
            isInt = int.TryParse(data[1], out val);

            if (isInt)
                valueInt = val;
            else valueStr = data[1];
        }

        public string Output()
        {
            string str = "";
            string typeStr = type.ToString();
            if(valueStr != null)
                str = String.Format("{0}{1}{2}, {3}",
                type.ToString(),
                StrFormat.GetNewWhiteSpace(type.ToString()),
                filepath,
                valueStr); 
            else if(valueInt != int.MaxValue)
                str = String.Format("{0}{1}{2}, {3}",
                type.ToString(),
                StrFormat.GetNewWhiteSpace(type.ToString()),
                filepath,
                valueInt.ToString());

            return str.CRL();
        }

    }
}
