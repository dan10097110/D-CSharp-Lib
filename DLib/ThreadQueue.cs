using System.Collections.Generic;
using System.Threading;

namespace DLib
{
    public class ThreadQueue
    {
        Queue<string> queue = new Queue<string>();
        static int threadNumber = 0;

        public void Wait()
        {
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = (threadNumber++).ToString();
            lock (queue)
                queue.Enqueue(Thread.CurrentThread.Name);
            while (queue.Peek() != Thread.CurrentThread.Name) ;
        }

        public void Next()
        {
            lock (queue)
                queue.Dequeue();
        }
    }
}