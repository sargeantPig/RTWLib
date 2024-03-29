﻿using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB.ModelData
{
    public class ModelType : IdmbOutput
    {
        string tag = "type";
        public string type { get; set; }

        public ModelType()
        { }

        public void ProcessLine(string[] data)
        {
            type = data[0];
        }

        public string Output()
        {
            string str = "";
            str = String.Format("{0}{1}{2}", 
                tag,
                StrFo.GetNewWhiteSpace(tag),
                type);

            return str.CRL();
        }

    }
}
