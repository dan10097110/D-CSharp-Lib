using Mpir.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        ~MPNetworkMember()
        {
            Dispose();
        }

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
        bool threadRunning;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public uint Port { get; private set; }
        public byte ThreadCount { get; private set; }

        ~MPNetworkMember2()
        {
            Dispose();
        }

        public void Start(uint port, byte threadCount)
        {
            if (!Disposed)
            {
                Running = true;
                Port = port;
                ThreadCount = threadCount;
                new Thread(ThreadWork).Start();
            }
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

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
        }

        void ThreadWork()
        {
            threadRunning = true;
            List<ulong> primes = new List<ulong>() { 3 };
            Networking.Member client = new Networking.Member(Port);
            ulong startI;
            mpz_t startS;
            while (Running)
            {
                var exponents = Extra.SplitString(client.SendRecieveSave("g"), '|', ulong.Parse);
                startI = (ulong)(exponents.First() / 1.667);// Math.Floor(BigInteger.Log((BigInteger.One << (int)exponents.First()) - 1, 3));
                startS = mpz_t.Three.Power((int)startI);
                var options = new ParallelOptions { MaxDegreeOfParallelism = ThreadCount };
                Parallel.ForEach(Enumerable.Range((int)primes.Last() + 2, (int)exponents.Last()).Where(x => (x & 1) == 1), options, n => {
                    if (IsPrime((ulong)n))
                        primes.Add((ulong)n);///////squential not parrallel
                });
                Parallel.ForEach(exponents, options, exponent => {
                    Stopwatch testTime = new Stopwatch();
                    testTime.Start();
                    if (IsMersennePrime())
                        client.Send(exponent.ToString() + "|" + DateTime.Now + "|" + testTime.Elapsed);

                    bool IsMersennePrime()
                    {
                        if (!IsPrime(exponent) || ((exponent & 3) == 3 && IsPrime((exponent << 1) + 1)))
                            return false;
                        mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
                        return Prime.Mersenne.TrialDivision(exponent, mersenneNumber, primes) && LucasLehmer();

                        bool LucasLehmer()
                        {
                            mpz_t s = startS.Clone();
                            /*ulong m = (exponent - startI) & 1;
                            if (m == 1)
                                mpir.mpz_powm_ui(s, s, 2, mersenneNumber);*/
                            for (ulong i = startI/* + m*/; i < exponent; i++)
                                mpir.mpz_powm_ui(s, s, 2, mersenneNumber);
                            return s + 3 == mersenneNumber;
                        }
                    }
                });

                bool IsPrime(ulong n)
                {
                    ulong sqrt = (ulong)System.Math.Sqrt(n);
                    for (int i = 0; primes[i] <= sqrt; i++)
                        if (n % primes[i] == 0)
                            return false;
                    return true;
                }
            }
            client.Dispose();
            primes.Clear();
            threadRunning = false;
        }
    }
}
