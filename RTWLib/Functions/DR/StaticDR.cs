using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    public partial class Descr_Region
    {
        static int regionVARS = 7;
        static int currentVar = 0;
        static void ParseSimpleLine<T>(string[] data, ref T obj)
        {
            if (typeof(T) == typeof(int))
            {
                int val = Convert.ToInt32(data[0]);
                obj = (T)(object)val;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = (T)(object)data[0];
            }
        }

        static void ParseList<T>(string[] data, ref T obj)
        {
            if (typeof(T) == typeof(int[]))
            {
                if (data.Count() == 1)
                {
                    int[] conv = data[0].StringToArray(' ').ConvertToIntArray();
                    obj = (T)(object)conv;
                }

                else
                {
                    int[] conv = data.ConvertToIntArray();
                    obj = (T)(object)conv;
                }
            }
            else if (typeof(T) == typeof(string[]))
            {
                if (data.Count() == 1)
                {
                    string[] conv = data[0].StringToArray(' ');
                    obj = (T)(object)conv;
                }

                else
                {
                    obj = (T)(object)data;
                }
            }
        }
    }
}
