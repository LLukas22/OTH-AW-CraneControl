using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CraneControl.Server;

namespace Cranecontrol.Server
{
    public class TCPServer
    {
        private TcpClient client;
        private NetworkStream networkStream;
        private readonly Payload payload;

        public event EventHandler<client_connected> ClientConnectedEvent;

        public TCPServer(Payload payload, int port)
        {
            this.payload = payload;
            this.port = port;
            serverOn = true;
            active = true;
            Start();
        }

        public int port { get; set; }
        public bool active { get; set; }
        public bool serverOn { get; set; }

        public void Start()
        {
            Task.Run(() =>
            {
                var server = new TcpListener(IPAddress.Any, port);
                server.Start();
                client = server.AcceptTcpClient();
                networkStream = client.GetStream();
                while (true)
                {
                    if (!serverOn)
                    {
                        networkStream.Close();
                        client.Close();
                        server.Stop();
                        OnClientConnectedEvent(new client_connected(client.Connected));
                        return;
                    }
                    if (client.Connected)
                    {
                        OnClientConnectedEvent(new client_connected(client.Connected));
                        while (serverOn)
                        {
                            var msg = new byte[1024];
                            try
                            { 
                                if (active) 
                                { 
                                    networkStream.Read(msg, 0, msg.Length); 
                                    networkStream.Write(payload.ServerData, 0, payload.ServerData.Length);
                                }
                            }
                            catch (SystemException exception)
                            {
                                OnClientConnectedEvent(new client_connected(client.Connected));
                                networkStream.Close();
                                client.Close();
                                server.Stop();
                                server = new TcpListener(IPAddress.Any, port);
                                server.Start();
                                client = server.AcceptTcpClient();
                                networkStream = client.GetStream();
                                break;
                            }
                        }
                    }
                        
                }
            });
        }

        protected virtual void OnClientConnectedEvent(client_connected e)
        {
            ClientConnectedEvent?.Invoke(this, e);
        }
    }
}