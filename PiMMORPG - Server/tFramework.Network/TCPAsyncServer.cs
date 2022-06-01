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

    public class TCPAsyncServer : INetworkServer<TCPAsyncServer, TCPAsyncClient>
    {
        private ILogger _logger;
        private Socket _server;
        private readonly object _syncLock = new object();
        private List<TCPAsyncClient> _cliList;
        private Queue<Socket> _overClients;
        private Dictionary<uint, IResponse<TCPAsyncClient>> _responses;

        public event EventHandler<ServerEventArgs<TCPAsyncServer, TCPAsyncClient>> OnOpen, OnClose;
        public event EventHandler<ClientEventArgs<TCPAsyncClient>> OnConnected;

        public IPEndPoint EndPoint { get; set; }
        public TCPAsyncClient[] Clients { get { return _cliList.ToArray(); } }
        public uint ClientCount { get; private set; }
        public bool Opened { get; private set; }
        public uint MaximumClients { get; set; }
        public Type PacketType { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }
        public ClientReplaceMode ReplaceMode { get; set; }

        public TCPAsyncServer()
        {
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);
            ReplaceMode = ClientReplaceMode.MoveToQueue;

            _logger = LoggerFactory.GetLogger(this);
            _cliList = new List<TCPAsyncClient>();
            _responses = new Dictionary<uint, IResponse<TCPAsyncClient>>();
            _overClients = new Queue<Socket>();
        }

        public TCPAsyncServer(int port) : this(IPAddress.Any, port) { }
        public TCPAsyncServer(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPAsyncServer(IPEndPoint endPoint) { this.EndPoint = endPoint; }

        public bool Open()
        {
            try
            {
                _server = new Socket(EndPoint.AddressFamily, SocketType, Protocol);
                _server.Bind(EndPoint);
                _server.Listen(1);

                Opened = true;
                OnOpen.FireEvent(new ServerEventArgs<TCPAsyncServer, TCPAsyncClient>(this), this);
                BeginAccept();
                return true;
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                    _logger.LogFatal(ex);
                return false;
            }
        }

        public void RegisterResponse(IResponse<TCPAsyncClient> response)
        {
            _responses[response.ID] = response;
        }

        public void SendToAll(IDataPacket packet, Predicate<TCPAsyncClient> condition = null)
        {
            lock (_syncLock)
            {
                _cliList.ForEach(c => { if (condition == null || condition(c)) c.Send(packet); });
            }
        }

        public void SendToAll(IRequest<TCPAsyncClient> request, Predicate<TCPAsyncClient> condition = null)
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
                bool result = false;
                try
                {
                    if (_server != null)
                    {
                        DisconnectAll();
                        _server.Close();
                    }
                    else
                        _logger.LogWarning("TCPAsyncServer {0} has already closed!", EndPoint);
                    result = true;
                }
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                        _logger.LogFatal(ex);
                    result = _server == null;
                }

                if (result)
                {
                    Opened = false;
                    OnClose.FireEvent(new ServerEventArgs<TCPAsyncServer, TCPAsyncClient>(this), this);
                }
                return result;
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

        void BeginAccept()
        {
            _server.BeginAccept(EndAccept, null);
        }

        void EndAccept(IAsyncResult ar)
        {
            try
            {
                var socket = _server.EndAccept(ar);
                BeginAccept();

                if (!ar.IsCompleted)
                    _logger.LogWarning("EndAccept is not completed!");
                else if (socket == null)
                    _logger.LogWarning("EndAccept returns a null socket!");
                else if (!socket.Connected)
                    _logger.LogWarning("EndAccept returns a offline socket!");
                else if (ClientCount != 0 && ClientCount == MaximumClients)
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
                    HandleConnection(socket);
            }
            catch(Exception ex)
            {
                if(!(ex is ObjectDisposedException))
                    _logger.LogFatal(ex);
                Close();
            }
        }

        void HandleConnection(Socket socket)
        {
            lock (_syncLock)
            {
                ClientCount++;

                var client = new TCPAsyncClient(this, socket)
                {
                    PacketType = PacketType
                };
                _responses.Values.ForEach(r => client.RegisterResponse(r));

                foreach (var rClient in _cliList)
                    if (rClient == client)
                        rClient.Disconnect();

                _cliList.Add(client);
                client.Initalize();

                OnConnected.FireEvent(new ClientEventArgs<TCPAsyncClient>(client), this);
            }
        }

        public void FireDisconnected(TCPAsyncClient client)
        {
            lock (_syncLock)
            {
                if (_cliList.Remove(client))
                {
                    ClientCount--;
                }

                if (_overClients.Count > 0)
                {
                    var socket = _overClients.Dequeue();
                    HandleConnection(socket);
                }
            }
        }
    }
}
