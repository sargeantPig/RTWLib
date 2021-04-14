using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTWLib.Memory
{


    public class LThreadWrapper
    {
        public Thread thread { get; set; }
        public ThreadStart threadStart { get; set; }

        public void StartThread(string threadNO)
        {
            thread = new Thread(threadStart);
            thread.Name = threadNO;
            thread.Start();
        }

        public void SetFunc(Action func)
        {
            threadStart = delegate { func(); };
        }
    }
}
