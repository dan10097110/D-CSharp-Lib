using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DLib
{
    public class Threads
    {
        Action action;
        int threadCount;
        public int ThreadCount
        {
            get => threadCount;
            set
            {
                for (int b = threadCount; b < threadCount; b++)
                    new Thread(() => action()).Start();
                threadCount = value;
            }
        }
        List<Thread> threads = new List<Thread>();

        public Threads(Action action, int count)
        {
            this.action = action;
            threadCount = count;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public void Continue()
        {

        }

        public void Pause()
        {

        }
    }

    public class RepeatedThread
    {
        Thread thread;
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
        public bool Running { get; private set; }
        bool stopped = false;
        
        public RepeatedThread(Action action, Func<bool> cond)
        {
            thread = new Thread(() => {
                Running = true;
                while (!stopped && cond())
                {
                    manualResetEvent.Wait();
                    action();
                }
                Running = false;
            });
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            stopped = true;
        }

        public void Continue()
        {
            manualResetEvent.Set();
        }

        public void Pause()
        {
            manualResetEvent.Reset();
        }
    }
}
