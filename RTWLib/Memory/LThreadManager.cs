using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTWLib.Memory
{
    public class LThreadManager
    {
        public int controlThread { get; } = 0;
        public List<LThreadWrapper> threads = new List<LThreadWrapper>();

        public LThreadManager()
        { 
            
        }

        public void CreateThread(Action func)
        {
            threads.Add(new LThreadWrapper());
            threads.Last().SetFunc(func);
        }

        public int GetCurrentlyRunning()
        {
            int count = 0;
            for (int i = 0; i < threads.Count; i++)
            {
                if (i == 0)
                    continue;
                if (threads[i].thread != null)
                    if (threads[i].thread.ThreadState == ThreadState.Running || threads[i].thread.ThreadState == ThreadState.WaitSleepJoin || threads[i].thread.IsAlive)
                        count++;
            }

            return count;
        }

        public void Start(int index = -1)
        {
            if (index < 0)
            {
                for (int i = 0; i < threads.Count; i++)
                {
                    if (threads[i].thread != null)
                        continue;
                    threads[i].StartThread(i.ToString());
                }
            }
            else threads[index].StartThread(index.ToString());
        }


        public void Wait(int sleep = 1000)
        {
            Thread.Sleep(sleep);

            bool runningFound = true;
            while (GetCurrentlyRunning() > 0)
            {
                runningFound = false;
                for (int i = 0; i < threads.Count(); i++)
                {
                    if (i == controlThread)
                        continue;

                    if (threads[i].thread != null)
                    {
                        if (threads[i].thread.ThreadState == ThreadState.Running)
                        {
                            runningFound = true;
                            Console.WriteLine("Waiting for thread: {0}", i);
                            threads[i].thread.Join();
                            break;
                        }
                    }
                }
            }
        }

        public void ClearThreads()
        {
            bool running = true;

            while (running)
            {
                running = false;
                for (int i = 0; i < threads.Count(); i++)
                {
                    if (threads[i].thread != null)
                    {
                        if ((threads[i].thread.ThreadState & ThreadState.Stopped) == ThreadState.Stopped)
                        {
                            threads.RemoveAt(i);
                            running = true;
                            break;
                        }
                    }

                    else { threads.RemoveAt(i); running = true; break; }
                }
            }
        }
    }
}
