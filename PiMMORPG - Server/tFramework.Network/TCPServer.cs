using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using tFramework.Interfaces;
namespace tFramework.Network
{
    using Interfaces;
    using Factories;
    using DataPacket;
    using EventArgs;
    using Extensions;
    using Helper;
    using Enums;

    public class TCPServer : INetworkServer<TCPServer, TCPClient>, IThread
    {
        private ILogger _logger;
        private Socket _server;
        private readonly object _syncLock = new object();
        private List<TCPClient> _cliList;
        private Queue<Socket> _overClients;
        private Dictionary<uint, IResponse<TCPClient>> _responses;

        public event EventHandler<ServerEventArgs<TCPServer, TCPClient>> OnOpen, OnClose;
        public event EventHandler<ClientEventArgs<TCPClient>> OnConnected;
        
        public IPEndPoint EndPoint { get; set; }
        public TCPClient[] Clients { get { return _cliList.ToArray(); } }
        public uint ClientCount { get; private set; }
        public bool Opened { get; private set; }
        public uint MaximumClients { get; set; }
        public Type PacketType { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }
        public ClientReplaceMode ReplaceMode { get; set; }

        public TCPServer()
        {
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);
            ReplaceMode = ClientReplaceMode.MoveToQueue;

            _logger = LoggerFactory.GetLogger(this);
            _cliList = new List<TCPClient>();
            _responses = new Dictionary<uint, IResponse<TCPClient>>();
            _overClients = new Queue<Socket>();
        }
        public TCPServer(int port) : this(IPAddress.Any, port) { }
        public TCPServer(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPServer(IPEndPoint endPoint) { this.EndPoint = endPoint; }

        public bool Open()
        {
            try
            {
                _server = new Socket(EndPoint.AddressFamily, SocketType, Protocol);
                _server.Bind(EndPoint);
                _server.Listen(1);

                ThreadFactory.Start(this);
                return true;
            }
            catch(Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                    _logger.LogFatal(ex);
                return false;
            }
        }

        public void RegisterResponse(IResponse< TCPClient> response)
        {
            _responses[response.ID] = response;
        }

        public void SendToAll(IDataPacket packet, Predicate<TCPClient> condition = null)
        {
            lock (_syncLock)
            {
                _cliList.ForEach(c => { if (condition == null || condition(c)) c.Send(packet); });
            }
        }

        public void SendToAll(IRequest<TCPClient> request, Predicate<TCPClient> condition = null)
        {
            lock (_syncLock)
            {
                _cliList.ForEach(c => { if (condition == null || condition(c)) c.Send(request); });
            }
        }

        public bool Close()
        {
            lock (_syncLock)
            {
                try
                {
                    if (_server != null)
                    {
                        ThreadFactory.Stop(this);
                        _cliList.ForEach(c => c.Disconnect());
                        _server.Close();
                    }
                    else
                        _logger.LogWarning("TCPServer {0} has already closed!", EndPoint);
                    return true;
                }
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                        _logger.LogFatal(ex);
                    return _server == null;
                }
            }
        }

        public void DisconnectAll()
        {
            var temp = _cliList.ToArray();
            foreach (var client in temp)
                client.Disconnect();

            while (_overClients.Count > 0)
            {
                var client = _overClients.Dequeue();
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        public void FireDisconnected(TCPClient client)
        {
            if (_cliList.Remove(client))
            {
                ClientCount--;
                if(_overClients.Count > 0)
                {
                    var socket = _overClients.Dequeue();
                    HandleConnection(socket);
                }
            }
        }

        void IThread.Start()
        {
            ClientCount = 0;
            Opened = true;
            OnOpen.FireEvent(new ServerEventArgs<TCPServer, TCPClient>(this), this);
        }

        void IThread.End()
        {
            Opened = false;
            OnClose.FireEvent(new ServerEventArgs<TCPServer, TCPClient>(this), this);
        }

        bool IThread.Run()
        {
            try
            {
                var socket = _server.Accept();
                if (socket != null && socket.Connected)
                {
                    if (ClientCount != 0 && ClientCount == MaximumClients)
                    {
                        var endPoint = socket.RemoteEndPoint as IPEndPoint;
                        var already = _cliList.FirstOrDefault(c => c.EndPoint.Address == endPoint.Address && c.EndPoint.Port == endPoint.Port);

                        if (already != null)
                        {
                            already.Disconnect();
                            HandleConnection(socket);
                        }
                        else if (ReplaceMode == ClientReplaceMode.DisconnectLast)
                        {
                            _cliList.Last().Disconnect();
                            HandleConnection(socket);
                        }
                        else if (ReplaceMode == ClientReplaceMode.DisconnectFirst)
                        {
                            _cliList.First().Disconnect();
                            HandleConnection(socket);
                        }
                        else
                        {
                            _logger.LogWarning("Client {0} moved to over queue!", socket.RemoteEndPoint);
                            _overClients.Enqueue(socket);
                        }
                    }
                    else
                    {
                        HandleConnection(socket);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                    _logger.LogFatal(ex);
                return false;
            }
        }

        void HandleConnection(Socket socket)
        {
            lock (_syncLock)
            {
                ClientCount++;

                var client = new TCPClient(this, socket)
                {
                    PacketType = PacketType
                };
                _responses.Values.ForEach(r => client.RegisterResponse(r));
                OnConnected.FireEvent(new ClientEventArgs<TCPClient>(client), this);

                foreach (var rClient in _cliList)
                    if (rClient == client)
                        rClient.Disconnect();

                _cliList.Add(client);
                client.Initalize();
            }
        }
    }
}
