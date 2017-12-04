using Mpir.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker
{
    public class MPNetworkMember : IDisposable
    {
        List<ulong> primes = new List<ulong>();
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
                                foreach (ulong exponent in Extra.SplitString(member.SendRecieve("g"), '|', s => Convert.ToUInt64(s)).Cast<ulong>().ToArray())
                                {
                                    testTime.Restart();
                                    if (exponent < lastExponent)
                                        start = (1, 3);
                                    lastExponent = exponent;
                                    if (Prime.Mersenne.Test(exponent, ref start.i, ref start.s, primes))
                                        member.Send(exponent.ToString() + "|" + DateTime.Now + "|" + testTime.Elapsed);
                                }
                            runningThreadCount--;
                        }).Start();
                    }
                }
            }
        }

        ~MPNetworkMember() => Dispose();

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
            primes.Clear();
        }

        public void Start(int port, byte threadCount)
        {
            if (!Disposed)
            {
                Running = true;
                Port = port;
                primes.Add(3);
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
                primes.Clear();
                while (runningThreadCount != threadCount)
                    Thread.Sleep(5);
            }
        }
    }

    public class MPNetworkMember2 : IDisposable
    {
        List<ulong> primes = new List<ulong>();
        bool threadRunning;
        ulong primesUntil;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public int Port { get; private set; }
        public byte ThreadCount { get; set; }

        ~MPNetworkMember2() => Dispose();

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
            primes.Clear();
        }

        public void Start(int port, byte threadCount)
        {
            if (!Disposed)
            {
                Running = true;
                Port = port;
                primes.Add(2);
                primesUntil = 2;
                ThreadCount = threadCount;
                new Thread(() => {
                    threadRunning = true;
                    var member = new Networking.MemberNew3(Port);
                    (ulong i, mpz_t s) start = (1, 3);
                    var lastExponent = int.MaxValue;
                    while (Running)
                    {
                        var message = member.SendRecieve("g");
                        int startExponent = int.Parse(message.Remove(message.IndexOf("|"))), count = int.Parse(message.Remove(0, message.IndexOf("|") + 1));
                        if (startExponent < lastExponent)
                        {
                            start = (1, 3);
                            primes.Clear();
                            primes.Add(2);
                            primesUntil = 2;
                        }
                        lastExponent = startExponent + count;
                        primes.AddRange(Enumerable.Range((int)primesUntil, startExponent - (int)primesUntil).Where(n => Prime.Test.Probabilistic.TrialDivision((ulong)n, primes)).Cast<ulong>());
                        primesUntil = (ulong)lastExponent;
                        member.Send(string.Join("|", Enumerable.Range(startExponent, count).AsParallel().WithDegreeOfParallelism(ThreadCount).Where(exponent => Prime.Mersenne.Test((ulong)exponent, ref start.i, ref start.s, primes))));
                    }
                    threadRunning = false;
                }).Start();
            }
        }

        public void Stop()
        {
            if (!Disposed)
            {
                Running = false;
                primes.Clear();
                while (threadRunning)
                    Thread.Sleep(5);
            }
        }
    }
}
