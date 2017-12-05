using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DLib.Networking
{
    public class Server : IDisposable
    {
        ClientL1 server = new ClientL1();
        IPEndPoint client;
        List<(string s, IPEndPoint client)> recieved = new List<(string, IPEndPoint)>();

        public bool Running => server.Running;
        public uint Port => server.Port;

        public IPAddress LastClientIPAdress => client.Address;

        public Server() { }

        public void Start(uint port)
        {
            server.Start(port);
            client = new IPEndPoint(IPAddress.Any, 0);
            Communication();
        }

        public void Stop() => server.Stop();

        public void Dispose() => server.Dispose();

        /// <summary>
        /// sends s to endpoint from last receive
        /// </summary>
        /// <param name="s"></param>
        public void Send(string s) => server.Send(s, client);

        /// <summary>
        /// sends s to endPoint
        /// </summary>
        /// <param name="s"></param>
        /// <param name="endPoint"></param>
        public void Send(string s, IPEndPoint endPoint) => server.Send(s, endPoint);

        public string Receive() => Receive(ref client);

        /// <summary>
        /// recieve and get endpoint
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public string Receive(ref IPEndPoint endPoint)
        {
            while (recieved.Count == 0)
                Thread.Sleep(4);
            string s;
            lock (recieved)
            {
                s = recieved[0].s;
                endPoint = recieved[0].client;
                recieved.RemoveAt(0);
            }
            return s;
        }

        /// <summary>
        /// recieves from endPoint
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public string Receive(IPEndPoint endPoint)
        {
            for (int i = 0; ; i++)
            {
                while (i >= recieved.Count)
                    Thread.Sleep(4);
                if (recieved[i].client == endPoint)
                {
                    string s;
                    lock (recieved)
                    {
                        s = recieved[i].s;
                        client = recieved[i].client;
                        recieved.RemoveAt(i);
                    }
                    return s;
                }
            }
        }

        void Communication()
        {
            new Thread(() => {
                while (Running)
                {
                    while (server.Available)
                    {
                        try
                        {
                            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                            string s = server.ReceiveUntilAvailavle(ref endPoint);
                            if (s.Contains("||"))
                            {
                                server.Send("r" + s, endPoint);
                                s = s.Substring(0, s.IndexOf("||"));
                            }
                            recieved.Add((s, endPoint));
                        }
                        catch { }
                    }
                    Thread.Sleep(4);
                }
            }).Start();
        }
    }

    public class ServerNew
    {
        ClientNew client;
        List<IPEndPoint> members;

        public ServerNew(int port)
        {
            members = new List<IPEndPoint>();
            client = new ClientNew(port);
        }

        ~ServerNew() => Dispose();

        public void Dispose() => client.Dispose();

        public void Send(string s)
        {
            foreach (IPEndPoint member in members)
                client.Send(s, member);
        }

        public void Send(string s, IPEndPoint member)
        {
            client.SendAndWait(s, member);
        }

        public (string s, IPEndPoint member) Recieve()
        {
            string message = null;
            IPEndPoint endPoint = null;
            client.ReceiveUntilAvailable(ref message, ref endPoint);
            if (!members.Contains(endPoint))
                members.Add(endPoint);
            return (message, endPoint);
        }
    }

    public class ServerNew3
    {
        UdpClient server;

        public int Port { get; private set; }

        public ServerNew3(int port)
        {
            Port = port;
            server = new UdpClient(port);
        }

        ~ServerNew3() => Dispose();

        public void Dispose() => server.Dispose();

        public void Send<T>(IPEndPoint member, params T[] s)
        {
            var array = Encoding.ASCII.GetBytes(string.Join("|", s));
            server.Send(array, array.Length, member);
        }

        public string Recieve(ref IPEndPoint member)
        {
            return Encoding.ASCII.GetString(server.Receive(ref member));
        }
    }
}
