using Mpir.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker
{
    public class MersennePrime
    {
        public const string version = "171230";

        byte threadCount, pausedThreadCount, runningThreadCount;
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
        List<(ulong exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)> mersennePrimes = new List<(ulong, DateTime, TimeSpan, TimeSpan)>();
        Stopwatch totalTime = new Stopwatch();
        TimeSpan timeOffset = new TimeSpan();

        public (ulong exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)[] MersennePrimes => mersennePrimes.ToArray();
        public ulong[] MersennePrimeExponents => mersennePrimes.Select(i => i.exponent).ToArray();
        public byte MersennePrimeCount => (byte)mersennePrimes.Count();
        public ulong NextExponent { get; private set; }
        public TimeSpan Time => totalTime.Elapsed.Add(timeOffset);
        public bool Started => totalTime.ElapsedTicks > 0;
        public bool Running { get; private set; }
        public bool Paused => !manualResetEvent.Wait(0);
        public byte ThreadCount
        {
            get => threadCount;
            set
            {
                if (Running)
                {
                    threadCount = value;
                    for (byte b = runningThreadCount; b < threadCount; b++)
                    {
                        byte id = b;
                        new Thread(() => {
                            runningThreadCount++;
                            (ulong i, mpz_t s) start = (1, 3);
                            var testTime = new Stopwatch();
                            while (id < threadCount && Running)
                            {
                                pausedThreadCount++;
                                manualResetEvent.Wait();
                                pausedThreadCount--;
                                testTime.Restart();
                                ulong exponent = (NextExponent += 2) - 2;
                                if (Prime.Mersenne.Test(exponent, ref start.i, ref start.s))
                                    lock (mersennePrimes)
                                    {
                                        int i = mersennePrimes.Count;
                                        for (; i > 0 && mersennePrimes[i - 1].exponent > exponent; i--) ;
                                        mersennePrimes.Insert(i, (exponent, DateTime.Now, Time, testTime.Elapsed));
                                    }
                            }
                            runningThreadCount--;
                        }).Start();
                    }
                }
            }
        }

        public void Start(ulong startExponent, byte threadCount) => Start(startExponent, threadCount, new List<(ulong, DateTime, TimeSpan, TimeSpan)>(), new TimeSpan());

        public void Start(string path, byte threadCount)
        {
            if (!File.Exists(path))
                throw new Exception("File does not exist.");
            var lines = File.ReadAllLines(path);
            var mersennePrimes = new List<(ulong, DateTime, TimeSpan, TimeSpan)>();
            for (int i = 2; i < lines.Length; i += 4)
                mersennePrimes.Add((uint.Parse(lines[i]), DateTime.Parse(lines[i + 1]), TimeSpan.Parse(lines[i + 2]), TimeSpan.Parse(lines[i + 3])));
            Start(System.Math.Max(5, ulong.Parse(lines[1]) + ((ulong.Parse(lines[1]) + 1) & 1)), threadCount, mersennePrimes, TimeSpan.Parse(lines[0]));
        }

        void Start(ulong startExponent, byte threadCount, List<(ulong, DateTime, TimeSpan, TimeSpan)> mersennePrimes, TimeSpan timeOffset)
        {
            if (!Running)
            {
                Running = true;
                NextExponent = System.Math.Max(5, startExponent + ((startExponent + 1) & 1));
                this.timeOffset = timeOffset;
                this.mersennePrimes = mersennePrimes;
                manualResetEvent.Reset();
                ThreadCount = threadCount;
                while (runningThreadCount != threadCount)
                    Thread.Sleep(1);
                totalTime.Restart();
                manualResetEvent.Set();
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                manualResetEvent.Set();
                while (runningThreadCount != 0)
                    Thread.Sleep(1);
            }
        }

        public void Continue()
        {
            if (Running && Paused)
            {
                totalTime.Start();
                manualResetEvent.Set();
            }
        }

        public void Pause()
        {
            if (Running && !Paused)
            {
                manualResetEvent.Reset();
                while (pausedThreadCount != threadCount)
                    Thread.Sleep(1);
                totalTime.Stop();
            }
        }

        public bool Save(string name, string path)
        {
            if (Started && !name.Contains(":"))
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string content = Time.ToString() + Environment.NewLine + NextExponent + Environment.NewLine;
                foreach (var mersennePrime in MersennePrimes)
                    content += mersennePrime.exponent + Environment.NewLine + mersennePrime.explorationDate + Environment.NewLine + mersennePrime.totalTime.ToString() + Environment.NewLine + mersennePrime.testTime.ToString() + Environment.NewLine;
                File.WriteAllText(path + @"\" + name, content);
                return true;
            }
            else
                return false;
        }
    }
}
