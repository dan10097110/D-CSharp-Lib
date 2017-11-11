using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DLib.Networking
{
    class ClientL1 : IDisposable
    {
        UdpClient client;

        public bool Running { get; private set; }
        public bool Disposed { get; private set; }
        public uint Port { get; private set; }
        public bool Available => Running && client.Available > 0;

        public void Start(uint port)
        {
            if (!Disposed && !Running)
            {
                Running = true;
                Port = port;
                client = new UdpClient((int)Port);
            }
        }

        ~ClientL1()
        {
            Dispose();
        }

        public void Stop()
        {
            if (!Disposed && Running)
            {
                client.Close();
                Running = false;
            }
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                client.Dispose();
            }
        }

        public void Send(string s, IPEndPoint endPoint)
        {
            if (Running)
            {
                var bytes = Encoding.ASCII.GetBytes(s);
                client.Send(bytes, bytes.Length, endPoint);
            }
        }

        public string ReceiveUntilAvailavle(ref IPEndPoint endPoint) => Running ? Encoding.ASCII.GetString(client.Receive(ref endPoint)) : null;

        public string ReceiveIfAvailable(ref IPEndPoint endPoint) => Running && Available ? Encoding.ASCII.GetString(client.Receive(ref endPoint)) : null;
    }

    class ClientL2
    {
        ClientL1 client = new ClientL1();
        List<(string message, IPEndPoint endPoint)> toSend = new List<(string s, IPEndPoint endPoint)>(), recieved = new List<(string s, IPEndPoint endPoint)>();

        public bool Running => client.Running;
        public bool Disposed => client.Disposed;
        public uint Port => client.Port;
        public bool Available => client.Available;

        public void Start(uint port) => client.Start(port);

        public void Stop() => client.Stop();

        public void Dispose() => client.Dispose();

        public void Send(string s, IPEndPoint endPoint)
        {
            lock (toSend)
                toSend.Add((s, endPoint));
        }

        public string ReceiveIfAvailable(ref IPEndPoint endPoint)
        {
            if (recieved.Count > 1)
                return null;
            lock (recieved)
            {
                string s = recieved[0].message;
                endPoint = recieved[0].endPoint;
                recieved.RemoveAt(0);
                return s;
            }
        }

        public string ReceiveUntilAvailable(ref IPEndPoint endPoint)
        {
            while (recieved.Count < 1)
                Thread.Sleep(1);
            lock (recieved)
            {
                string s = recieved[0].message;
                endPoint = recieved[0].endPoint;
                recieved.RemoveAt(0);
                return s;
            }
        }

        public string Recieve(ref IPEndPoint endPoint, ulong timeOutInMs)
        {
            while (timeOutInMs-- > 0 && recieved.Count < 1)
                Thread.Sleep(1);
            return ReceiveIfAvailable(ref endPoint);
        }

        void Communication()
        {
            new Thread(() => {
                while (Running)
                {
                    lock (recieved)
                        if (client.Available)
                        {
                            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                            recieved.Add((client.ReceiveIfAvailable(ref endPoint), endPoint));
                        }
                    lock (toSend)
                        while (toSend.Count > 0)
                        {
                            client.Send(toSend[0].message, toSend[0].endPoint);
                            toSend.RemoveAt(0);
                        }
                    Thread.Sleep(1);
                }
            }).Start();
        }
    }

    class ClientL3
    {
        ClientL2 client;
        List<(string message, IPEndPoint endPoint)> recieved = new List<(string message, IPEndPoint endPoint)>(), confirmed = new List<(string message, IPEndPoint endPoint)>();

        public bool Running => client.Running;
        public bool Disposed => client.Disposed;
        public uint Port => client.Port;
        public bool Available => client.Available;
        public bool Connected { get; private set; }

        public void Start(uint port) => client.Start(port);

        public void Stop() => client.Stop();

        public void Dispose() => client.Dispose();

        public void Send(string s, IPEndPoint endPoint)
        {
            int i = 0;
            while (!(i < confirmed.Count && s == confirmed[i].message && endPoint == confirmed[i].endPoint))
            {
                client.Send(s, endPoint);
                for (int j = 0; j < 50 && (s != confirmed[i].message || endPoint != confirmed[i].endPoint); i++)
                    for (; i >= confirmed.Count; j++)
                        Thread.Sleep(1);
            }
            lock (confirmed)
                confirmed.RemoveAt(i);
        }

        public void Send(string s, IPEndPoint endPoint, ulong timeOutInMs)
        {
            int i = 0;
            for (int k = 0; !(i < confirmed.Count && s == confirmed[i].message && endPoint == confirmed[i].endPoint) && k < (int)timeOutInMs;)
            {
                client.Send(s, endPoint);
                for (int j = 0; j < 50 && (s != confirmed[i].message || endPoint != confirmed[i].endPoint) && k < (int)timeOutInMs; i++)
                    for (; i >= confirmed.Count && k < (int)timeOutInMs; j++, k++)
                        Thread.Sleep(1);
            }
            if ((i < confirmed.Count && s == confirmed[i].message && endPoint == confirmed[i].endPoint))
                lock (confirmed)
                    confirmed.RemoveAt(i);
            else
                Connected = false;
        }

        public string ReceiveIfAvailable(ref IPEndPoint endPoint)
        {
            if (recieved.Count > 1)
                return null;
            lock (recieved)
            {
                string s = recieved[0].message;
                endPoint = recieved[0].endPoint;
                recieved.RemoveAt(0);
                return s;
            }
        }

        public string ReceiveUntilAvailable(ref IPEndPoint endPoint)
        {
            while (recieved.Count < 1)
                Thread.Sleep(1);
            lock (recieved)
            {
                string s = recieved[0].message;
                endPoint = recieved[0].endPoint;
                recieved.RemoveAt(0);
                return s;
            }
        }

        public string Recieve(ref IPEndPoint endPoint, ulong timeOutInMs)
        {
            while (timeOutInMs-- > 0 && recieved.Count < 1)
                Thread.Sleep(1);
            return ReceiveIfAvailable(ref endPoint);
        }

        void Communication()
        {
            new Thread(() => {
                while (Running)
                {
                    lock (recieved)
                        if (client.Available)
                        {
                            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                            string s = client.ReceiveIfAvailable(ref endPoint);
                            if (s.StartsWith("c"))
                                confirmed.Add((s.Remove(0, 1), endPoint));
                            else
                                recieved.Add((s, endPoint));
                        }
                    Thread.Sleep(1);
                }
            }).Start();
        }
    }

    public class ClientNew
    {
        UdpClient client;
        List<(string message, IPEndPoint endPoint)> received = new List<(string, IPEndPoint)>(), toSend = new List<(string message, IPEndPoint endPoint)>();
        List<(string message, string endPoint)> toConfirm = new List<(string message, string endPoint)>();

        public bool Disposed { get; private set; }
        bool receiving, sending;

        public ClientNew(int port)
        {
            client = new UdpClient(port);
            Communication();
        }

        public ClientNew()
        {
            client = new UdpClient();
            Communication();
        }

        void Communication()
        {
            new Thread(() =>
            {
                receiving = true;
                while (!Disposed)
                {
                    while (client.Available > 0)
                    {
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                        string s = Encoding.ASCII.GetString(client.Receive(ref endPoint));
                        if (s.StartsWith("c"))
                            lock (toConfirm)
                                toConfirm.Remove((s.Remove(0, 1), endPoint.ToString()));
                        else if (s != "")
                        {
                            lock (received)
                                received.Add((s, endPoint));
                            var bytes = Encoding.ASCII.GetBytes("c" + s);
                            client.Send(bytes, bytes.Length, endPoint);
                        }
                    }
                    Thread.Sleep(1);
                }
                receiving = false;
            }).Start();
            new Thread(() =>
            {
                sending = true;
                while (!Disposed)
                {
                    while (toSend.Count > 0)
                    {
                        var bytes = Encoding.ASCII.GetBytes(toSend[0].message);
                        client.Send(bytes, bytes.Length, toSend[0].endPoint);
                        toSend.RemoveAt(0);
                    }
                    Thread.Sleep(1);
                }
                sending = false;
            }).Start();
        }

        ~ClientNew() => Dispose();

        public void Dispose()
        {
            Disposed = true;
            while (receiving || sending)
                Thread.Sleep(1);
            client.Dispose();
        }

        public void Send(string message, IPEndPoint endPoint)
        {
            toSend.Add((message, endPoint));
        }

        public void SendAndWait(string message, IPEndPoint endPoint)
        {
            toSend.Add((message, endPoint));
            toConfirm.Add((message, endPoint.ToString()));
            while (toConfirm.Contains((message, endPoint.ToString())))
                Thread.Sleep(1);
        }

        public void ReceiveIfAvailable(ref string message, ref IPEndPoint endPoint)
        {
            if (received.Count > 0)
                lock (received)
                {
                    message = received[0].message;
                    endPoint = received[0].endPoint;
                    received.RemoveAt(0);
                }
        }

        public void ReceiveUntilAvailable(ref string message, ref IPEndPoint endPoint)
        {
            while (received.Count == 0)
                Thread.Sleep(1);
            lock (received)
            {
                message = received[0].message;
                endPoint = received[0].endPoint;
                received.RemoveAt(0);
            }
        }
    }
}
