using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace DLib.Math.Seeker
{
    public class MPNetworkServer : IDisposable
    {
        Networking.ServerNew3 server;
        List<(uint exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime, IPAddress ipAdresse)> mersennePrimes = new List<(uint, DateTime, TimeSpan, TimeSpan, IPAddress)>();
        ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(true);
        Stopwatch totalTime = new Stopwatch();

        public bool Started => totalTime.ElapsedTicks > 0;
        public bool Running { get; private set; }
        public bool Paused => !manualResetEvent.Wait(0);
        public uint NextExponent { get; private set; }
        public (uint exponent, DateTime explorationDate, TimeSpan totalTime, TimeSpan testTime, IPAddress ipAdresse)[] MersennePrimes => mersennePrimes.ToArray();
        public uint[] MersennePrimeExponents => mersennePrimes.Select(i => i.exponent).ToArray();
        public TimeSpan ServerWorkTime => totalTime.Elapsed;

        public void Start(int port, uint startExponent)
        {
            if (!Running && !Started)
            {
                Running = true;
                server = new Networking.ServerNew3(port);
                mersennePrimes.Clear();
                manualResetEvent.Set();
                NextExponent = System.Math.Max(5, startExponent + ((startExponent + 1) & 1));
                totalTime.Restart();
                new Thread(() => {
                    while (Running)
                    {
                        manualResetEvent.Wait();
                        IPEndPoint member = null;
                        string message = server.Recieve(ref member);
                        if (message == "g")
                        {
                            const int count = 1024;
                            server.Send(NextExponent + "|" + count, member);
                            NextExponent += count;
                        }
                        else
                        {
                            var array = Extra.SplitString(message, '|', n => n).ToArray();
                            InsertMersennPrime((uint.Parse(array[0]), DateTime.Parse(array[1]), ServerWorkTime, TimeSpan.Parse(array[2]), member.Address));

                            void InsertMersennPrime((uint, DateTime, TimeSpan, TimeSpan, IPAddress) mersennePrime)
                            {
                                Console.WriteLine("exponent: {0};\texploration date: {1};\ttotal time: {2};\ttest time: {3};\tclient ip: {4}", mersennePrime.Item1, mersennePrime.Item2, mersennePrime.Item3, mersennePrime.Item4, mersennePrime.Item5);
                                int i = mersennePrimes.Count;
                                for (; i > 0 && mersennePrimes[i - 1].exponent > mersennePrime.Item1; i--) ;
                                mersennePrimes.Insert(i, mersennePrime);
                            }
                        }
                    }
                }).Start();
            }
        }

        public void Stop()
        {
            if (Running)
            {
                manualResetEvent.Set();
                Running = false;
                totalTime.Stop();
            }
        }

        public void Dispose()
        {
            if (Running)
                Stop();
            manualResetEvent.Dispose();
            server.Dispose();
        }

        public void Pause()
        {
            if (Running && !Paused)
            {
                manualResetEvent.Reset();
                totalTime.Stop();
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
    }
}
