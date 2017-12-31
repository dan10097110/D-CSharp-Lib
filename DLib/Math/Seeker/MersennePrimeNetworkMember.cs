using Mpir.NET;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker
{
    public class MPNetworkMember2 : IDisposable
    {
        byte threadCount, runningThreadCount;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public int Port { get; private set; }
        public byte ThreadCount
        {
            get => threadCount;
            private set
            {
                if (Running)
                {
                    threadCount = value;
                    for (byte b = runningThreadCount; b < threadCount; b++)
                    {
                        byte id = b;
                        new Thread(() => {
                            runningThreadCount++;
                            var member = new Networking.MemberNew3(Port);
                            (ulong i, mpz_t s) start = (1, 3);
                            ulong lastExponent = 0;
                            var testTime = new Stopwatch();
                            while (Running && id < ThreadCount)
                                foreach (ulong exponent in member.SendRecieve("g").Split('|').Select(s => ulong.Parse(s)).ToArray())
                                {
                                    testTime.Restart();
                                    if (exponent < lastExponent)
                                        start = (1, 3);
                                    lastExponent = exponent;
                                    if (Prime.Mersenne.Test(exponent, ref start.i, ref start.s))
                                        member.Send(exponent.ToString(), DateTime.Now.ToString(), testTime.Elapsed.ToString());
                                }
                            runningThreadCount--;
                        }).Start();
                    }
                }
            }
        }

        ~MPNetworkMember2() => Dispose();

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
        }

        public void Start(int port, byte threadCount)
        {
            if (!Disposed)
            {
                Running = true;
                Port = port;
                ThreadCount = threadCount;
                while (runningThreadCount != threadCount)
                    Thread.Sleep(5);
            }
        }

        public void Stop()
        {
            if (!Disposed)
            {
                Running = false;
                while (runningThreadCount != threadCount)
                    Thread.Sleep(5);
            }
        }
    }

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
                        {
                            start = (1, 3);
                        }
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
