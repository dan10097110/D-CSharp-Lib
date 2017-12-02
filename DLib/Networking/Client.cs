using System;
using System.Collections.Generic;
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

    public class ClientNew
    {
        UdpClient client;
        List<(string message, IPEndPoint endPoint)> received = new List<(string, IPEndPoint)>(), toSend = new List<(string, IPEndPoint)>(), toConfirm = new List<(string, IPEndPoint)>();

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

        ~ClientNew() => Dispose();

        public void Dispose()
        {
            Disposed = true;
            while (receiving || sending)
                Thread.Sleep(1);
            client.Dispose();
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
                        var endPoint = new IPEndPoint(IPAddress.Any, 0);
                        var s = Encoding.ASCII.GetString(client.Receive(ref endPoint));
                        if (s.StartsWith("c"))
                            lock (toConfirm)
                                toConfirm.Remove((s.Remove(0, 1), endPoint));
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

        public void Send(string message, IPEndPoint endPoint) => toSend.Add((message, endPoint));

        public void SendAndWait(string message, IPEndPoint endPoint)
        {
            toSend.Add((message, endPoint));
            toConfirm.Add((message, endPoint));
            while (toConfirm.Contains((message, endPoint)))
                Thread.Sleep(1);
        }

        public void ReceiveIfAvailable(ref string message, ref IPEndPoint endPoint)
        {
            if (received.Count == 0)
                return;
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