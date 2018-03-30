using System.Collections.Generic;
using System.Threading;

namespace DLib
{
    public class ThreadQueue
    {
        Queue<string> queue = new Queue<string>();
        static int threadNumber = 0, calls = 0;

        public void Wait()
        {
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = (threadNumber++).ToString();
            if (!queue.Contains(Thread.CurrentThread.Name))
            {
                lock (queue)
                    queue.Enqueue(Thread.CurrentThread.Name);
                while (queue.Peek() != Thread.CurrentThread.Name) ;
            }
            else
                calls++;
        }

        public void Next()
        {
            if(calls == 0)
                lock (queue)
                    queue.Dequeue();
            else if (calls > 0)
                calls--;
        }
    }
}