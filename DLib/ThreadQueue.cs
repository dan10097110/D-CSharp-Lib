using System.Collections.Generic;
using System.Threading;

namespace DLib
{
    public class ThreadQueue
    {
        Queue<string> queue = new Queue<string>();

        public void Wait()
        {
            queue.Enqueue(Thread.CurrentThread.Name);
            while (queue.Peek() != Thread.CurrentThread.Name)
                Thread.Sleep(1);
        }

        public void Finished()
        {
            if (Thread.CurrentThread.Name == queue.Peek())
                queue.Dequeue();
        }
    }
}