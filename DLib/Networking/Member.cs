using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DLib.Networking
{
    /// <summary>
    /// nicht Threadsafe. Folglich für jeden Thread der kommunizieren soll einen eigenen client
    /// </summary>
    public class Member : IDisposable
    {
        ClientL1 client;
        IPEndPoint server;
        List<string> recieved = new List<string>(), confirmation = new List<string>();
        bool communication;
        ulong messageNumber;
        Stopwatch sw = new Stopwatch();

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public uint Port => (uint)server.Port;

        public Member() { }

        public Member(uint port) => Start(port);

        ~Member() => Dispose();

        public void Start(uint port)
        {
            if (!Disposed)
            {
                Running = true;
                messageNumber = 0;
                client = new ClientL1();
                server = new IPEndPoint(IPAddress.Broadcast, (int)port);
                Communication();
            }
        }

        public void Stop()
        {
            if (!Disposed)
            {
                Running = false;
                while (communication)
                    Thread.Sleep(10);
                client.Stop();
            }
        }

        public void Dispose()
        {
            if (Running)
                Stop();
            Disposed = true;
        }

        public void Send(string s)
        {
            if (!Disposed)
                client.Send(s, server);
        }

        public void SendSave(string s)
        {
            if (!Disposed)
            {
                while (true)
                {
                    g:;
                    sw.Restart();
                    ulong number = messageNumber++ - 1;
                    client.Send(s + "||" + number, server);
                    for (int i = 0; sw.ElapsedMilliseconds < 100; i++)
                    {
                        while (i >= confirmation.Count)
                        {
                            if (sw.ElapsedMilliseconds > 100)
                                goto g;
                            Thread.Sleep(5);
                        }
                        if (confirmation[i] == "r" + s + "||" + number)
                        {
                            lock (confirmation)
                                confirmation.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        public string Recieve()
        {
            if (Disposed)
                return null;
            while (recieved.Count == 0)
                Thread.Sleep(5);
            string s;
            lock (recieved)
            {
                s = recieved[0];
                recieved.RemoveAt(0);
            }
            return s;
        }

        public string SendRecieve(string s)
        {
            if (Disposed)
                return null;
            SendSave(s);
            return Recieve();
        }

        public string SendRecieveSave(string s)
        {
            if (Disposed)
                return null;
            while (true)
            {
                client.Send(s, server);
                for (int i = 0; i < 25; i++)
                {
                    if (recieved.Count > 0)
                    {
                        string t;
                        lock (recieved)
                        {
                            t = recieved[0];
                            recieved.RemoveAt(0);
                        }
                        return t;
                    }
                    Thread.Sleep(4);
                }
            }
        }

        void Communication()
        {
            new Thread(() => {
                communication = true;
                while (Running)
                {
                    while (client.Available)
                    {
                        try
                        {
                            string s = client.ReceiveUntilAvailavle(ref server);
                            if (s.StartsWith("r"))
                                lock (confirmation)
                                    confirmation.Add(s);
                            else
                                lock (recieved)
                                    recieved.Add(s);
                        }
                        catch { }
                    }
                    Thread.Sleep(4);
                }
                communication = false;
            }).Start();
        }
    }

    //ip adressen im client stimmen nicht überein weshalb der im send feststeckt
    public class MemberNew
    {
        ClientNew client;
        IPEndPoint server;

        public MemberNew(int port)
        {
            server = new IPEndPoint(IPAddress.Broadcast, port);
            client = new ClientNew();
        }

        ~MemberNew() => Dispose();

        public void Connect(string serverName)
        {

        }

        public void Dispose() => client.Dispose();

        public void Send(string s)
        {
            client.SendAndWait(s, server);
        }

        public string Recieve()
        {
            string message = null;
            IPEndPoint endPoint = null;
            client.ReceiveUntilAvailable(ref message, ref endPoint);
            return message;
        }

        public string SendRecieve(string s)
        {
            Send(s);
            return Recieve();
        }
    }


    public class MemberNew2
    {
        ClientNew client;
        IPEndPoint server;

        public MemberNew2(int port)
        {
            server = new IPEndPoint(IPAddress.Broadcast, port);
            client = new ClientNew();
        }

        ~MemberNew2() => Dispose();

        public void Connect(string serverName)
        {

        }

        public void Dispose() => client.Dispose();

        public void Send(string s)
        {
            client.SendAndWait(s, server);
        }

        public string Recieve()
        {
            string message = null;
            IPEndPoint endPoint = null;
            client.ReceiveUntilAvailable(ref message, ref endPoint);
            return message;
        }

        public string SendRecieve(string s)
        {
            ClientNew client = new ClientNew();
            string message = null;
            IPEndPoint endPoint = null;
            client.SendAndWait(s, server);
            client.ReceiveUntilAvailable(ref message, ref endPoint);
            client.Dispose();
            return message;
        }
    }

    public class MemberNew3
    {
        UdpClient member;
        IPEndPoint server;

        public MemberNew3(int port)
        {
            member = new UdpClient();
            server = new IPEndPoint(IPAddress.Broadcast, port);
        }

        ~MemberNew3() => Dispose();

        public void Dispose() => member.Dispose();

        public string SendRecieve<T>(params T[] s)
        {
            var client = new UdpClient();
            string m = (s.Length > 0 ? string.Join("|", s) : "");
            var array = Encoding.ASCII.GetBytes(m);
            client.Send(array, array.Length, server);
            IPEndPoint endPoint = null;
            m = Encoding.ASCII.GetString(client.Receive(ref endPoint));
            client.Dispose();
            return m;
        }

        public void Send<T>(params T[] s)
        {
            string m = (s.Length > 0 ? string.Join("|", s) : "");
            var array = Encoding.ASCII.GetBytes(m);
            member.Send(array, array.Length, server);
        }
    }
}
