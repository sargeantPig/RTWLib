using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.DMB
{
    public partial class DMB
    {
        public static string IdmbArrayToString(IdmbOutput[] idmbs)
        {
            string str = "";

            if (idmbs == null)
                return "";
            foreach (var dmb in idmbs)
                str += dmb.Output();

            return str;
        }


    }        
}
