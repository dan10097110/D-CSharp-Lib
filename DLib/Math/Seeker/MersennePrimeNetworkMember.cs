using Mpir.NET;
using System;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker
{
    public class MPNetworkMember : IDisposable
    {
        bool threadRunning;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public int Port { get; private set; }
        public byte ThreadCount { get; set; }

        ~MPNetworkMember() => Dispose();

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
        }

        public void Start(int port, byte threadCount)
        {
            if (!Running && !Disposed)
                new Thread(() => {
                    Running = true;
                    threadRunning = true;
                    Port = port;
                    ThreadCount = threadCount;
                    var member = new Networking.MemberNew3(Port);
                    var lastExponent = int.MaxValue;
                    (ulong i, mpz_t s) start = (1, 3);
                    while (Running)
                    {
                        var message = member.SendRecieve("g").Split('|').Select(n => int.Parse(n)).ToArray();
                        if (message[0] < lastExponent)
                            start = (1, 3);
                        lastExponent = message[0] + message[1];
                        var mp = Enumerable.Range(message[0], message[1]).AsParallel().WithDegreeOfParallelism(ThreadCount).Where(exponent => Prime.Mersenne.Test((ulong)exponent, ref start.i, ref start.s)).ToArray();
                        if (mp.Length > 0)
                            member.Send(mp);
                    }
                    threadRunning = false;
                }).Start();
        }

        public void Stop()
        {
            if (!Disposed)
            {
                Running = false;
                while (threadRunning)
                    Thread.Sleep(5);
            }
        }
    }
}
