using System.Collections.Generic;
using System.Threading;

namespace DLib
{

    public class ThreadQueue
    {
        Queue<Thread> queue = new Queue<Thread>();

        public void Wait()
        {
            queue.Enqueue(Thread.CurrentThread);
            while (queue.Peek() != Thread.CurrentThread)
                Thread.Sleep(1);
        }

        public void Finished()
        {
            if (Thread.CurrentThread == queue.Peek())
                queue.Dequeue();
        }
    }
}