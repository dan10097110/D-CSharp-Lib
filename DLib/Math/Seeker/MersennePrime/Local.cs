using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker.MersennePrime
{
    public class Local
    {
        public const string version = "180101";

        int threadCount, pausedThreadCount, runningThreadCount;
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
        List<(int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)> mersennePrimes = new List<(int, DateTime, TimeSpan, TimeSpan)>();
        Stopwatch totalTime = new Stopwatch();
        TimeSpan timeOffset = new TimeSpan();

        public (int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)[] MersennePrimes => mersennePrimes.ToArray();
        public int[] MersennePrimeExponents => mersennePrimes.Select(i => i.exponent).ToArray();
        public int MersennePrimeCount => mersennePrimes.Count();
        public int NextExponent { get; private set; }
        public TimeSpan Time => totalTime.Elapsed.Add(timeOffset);
        public bool Started => totalTime.ElapsedTicks > 0;
        public bool Running { get; private set; }
        public bool Paused => !manualResetEvent.Wait(0);
        public int ThreadCount
        {
            get => threadCount;
            set
            {
                if (Running)
                {
                    threadCount = value;
                    for (int b = runningThreadCount; b < threadCount; b++)
                    {
                        int id = b;
                        new Thread(() => {
                            runningThreadCount++;
                            var testTime = new Stopwatch();
                            while (id < threadCount && Running)
                            {
                                pausedThreadCount++;
                                manualResetEvent.Wait();
                                pausedThreadCount--;
                                testTime.Restart();
                                int exponent = (NextExponent += 2) - 2;
                                if (new Number.MersenneNumber(exponent).IsPrime)
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

        public void Start(int startExponent, int threadCount) => Start(startExponent, threadCount, new List<(int, DateTime, TimeSpan, TimeSpan)>(), new TimeSpan());

        public void Start(string path, int threadCount)
        {
            if (!File.Exists(path))
                throw new Exception("File does not exist.");
            var lines = File.ReadAllLines(path);
            var mersennePrimes = new List<(int, DateTime, TimeSpan, TimeSpan)>();
            for (int i = 2; i < lines.Length; i += 4)
                mersennePrimes.Add((int.Parse(lines[i]), DateTime.Parse(lines[i + 1]), TimeSpan.Parse(lines[i + 2]), TimeSpan.Parse(lines[i + 3])));
            Start(System.Math.Max(5, int.Parse(lines[1]) + ((int.Parse(lines[1]) + 1) & 1)), threadCount, mersennePrimes, TimeSpan.Parse(lines[0]));
        }

        void Start(int startExponent, int threadCount, List<(int, DateTime, TimeSpan, TimeSpan)> mersennePrimes, TimeSpan timeOffset)
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

    public class Local2
    {
        public const string version = "180101";

        int threadCount, pausedThreadCount, runningThreadCount;
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
        List<(int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)> mersennePrimes = new List<(int, DateTime, TimeSpan, TimeSpan)>();
        Stopwatch totalTime = new Stopwatch();
        TimeSpan timeOffset = new TimeSpan();

        public (int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)[] MersennePrimes => mersennePrimes.ToArray();
        public int[] MersennePrimeExponents => mersennePrimes.Select(i => i.exponent).ToArray();
        public int MersennePrimeCount => mersennePrimes.Count();
        public int NextExponent { get; private set; }
        public TimeSpan Time => totalTime.Elapsed.Add(timeOffset);
        public bool Started => totalTime.ElapsedTicks > 0;
        public bool Running { get; private set; }
        public bool Paused => !manualResetEvent.Wait(0);
        public int ThreadCount
        {
            get => threadCount;
            set
            {
                if (Running)
                {
                    threadCount = value;
                    for (int b = runningThreadCount; b < threadCount; b++)
                    {
                        int id = b;
                        new RepeatedThread(() => {
                            pausedThreadCount++;
                            manualResetEvent.Wait();
                            pausedThreadCount--;
                            var testTime = new Stopwatch();
                            testTime.Restart();
                            int exponent = (NextExponent += 2) - 2;
                            if (new Number.MersenneNumber(exponent).IsPrime)
                                lock (mersennePrimes)
                                {
                                    int i = mersennePrimes.Count;
                                    for (; i > 0 && mersennePrimes[i - 1].exponent > exponent; i--) ;
                                    mersennePrimes.Insert(i, (exponent, DateTime.Now, Time, testTime.Elapsed));
                                }
                        }, () => id < threadCount && Running).Start();
                    }
                }
            }
        }

        public void Start(int startExponent, int threadCount) => Start(startExponent, threadCount, new List<(int, DateTime, TimeSpan, TimeSpan)>(), new TimeSpan());

        public void Start(string path, int threadCount)
        {
            if (!File.Exists(path))
                throw new Exception("File does not exist.");
            var lines = File.ReadAllLines(path);
            var mersennePrimes = new List<(int, DateTime, TimeSpan, TimeSpan)>();
            for (int i = 2; i < lines.Length; i += 4)
                mersennePrimes.Add((int.Parse(lines[i]), DateTime.Parse(lines[i + 1]), TimeSpan.Parse(lines[i + 2]), TimeSpan.Parse(lines[i + 3])));
            Start(System.Math.Max(5, int.Parse(lines[1]) + ((int.Parse(lines[1]) + 1) & 1)), threadCount, mersennePrimes, TimeSpan.Parse(lines[0]));
        }

        void Start(int startExponent, int threadCount, List<(int, DateTime, TimeSpan, TimeSpan)> mersennePrimes, TimeSpan timeOffset)
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

    public class Manager
    {
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(true);
        List<(int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)> mersennePrimes = new List<(int, DateTime, TimeSpan, TimeSpan)>();
        Stopwatch totalTime = new Stopwatch();
        TimeSpan timeOffset = new TimeSpan();

        public (int exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime)[] MersennePrimes => mersennePrimes.ToArray();
        public int[] MersennePrimeExponents => mersennePrimes.Select(i => i.exponent).ToArray();
        public int MersennePrimeCount => mersennePrimes.Count();
        public int NextExponent { get; private set; }
        public TimeSpan Time => totalTime.Elapsed.Add(timeOffset);
        public bool Paused => !manualResetEvent.Wait(0);

        public Manager(int startExponent)
        {
            NextExponent = System.Math.Max(5, startExponent + ((startExponent + 1) & 1));
            totalTime.Start();
        } 

        public Manager(string path)
        {
            if (!File.Exists(path))
                throw new Exception("File does not exist.");
            var lines = File.ReadAllLines(path);
            for (int i = 2; i < lines.Length; i += 4)
                mersennePrimes.Add((int.Parse(lines[i]), DateTime.Parse(lines[i + 1]), TimeSpan.Parse(lines[i + 2]), TimeSpan.Parse(lines[i + 3])));
            timeOffset = TimeSpan.Parse(lines[0]);
            int startExponent = System.Math.Max(5, int.Parse(lines[1]) + ((int.Parse(lines[1]) + 1) & 1));
            NextExponent = System.Math.Max(5, startExponent + ((startExponent + 1) & 1));
            totalTime.Start();
        }

        public void Continue()
        {
            if (Paused)
            {
                totalTime.Start();
                manualResetEvent.Set();
            }
        }

        public void Pause()
        {
            if (!Paused)
            {
                manualResetEvent.Reset();
                totalTime.Stop();
            }
        }

        public bool Save(string name, string path)
        {
            if (!name.Contains(":"))
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

        public int Get()
        {
            manualResetEvent.Wait();
            return (NextExponent += 2) - 2;
        }

        public void Confirm(int n, TimeSpan testTime)
        {
            lock (mersennePrimes)
            {
                int i = mersennePrimes.Count;
                for (; i > 0 && mersennePrimes[i - 1].exponent > n; i--) ;
                mersennePrimes.Insert(i, (n, DateTime.Now, Time, testTime));
            }
        }
    }

    public class Worker
    {
        Manager manager;
        
        public bool Running { get; private set; }

        public Worker(Manager manager) => this.manager = manager;

        public void Start()
        {
            if (!Running)
            {
                Running = true;
                new Thread(() => 
                {
                    var testTime = new Stopwatch();
                    while (Running)
                    {
                        testTime.Restart();
                        int exponent = manager.Get();
                        if (new Number.MersenneNumber(exponent).IsPrime)
                            manager.Confirm(exponent, testTime.Elapsed);
                    }
                }).Start();
            }
        }

        public void Stop()
        {
            if (Running)
                Running = false;
        }
    }
}