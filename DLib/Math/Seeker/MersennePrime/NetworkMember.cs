using System;
using System.Linq;
using System.Threading;

namespace DLib.Math.Seeker.MersennePrime
{
    public class NetworkMember : IDisposable
    {
        bool threadRunning;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public int Port { get; private set; }
        public byte ThreadCount { get; set; }

        ~NetworkMember() => Dispose();

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
                    while (Running)
                    {
                        var message = member.SendRecieve("g").Split('|').Select(n => int.Parse(n)).ToArray();
                        var mp = Enumerable.Range(message[0], message[1]).AsParallel().WithDegreeOfParallelism(ThreadCount).Where(exponent => new Number.MersenneNumber(exponent).IsPrime).ToArray();
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