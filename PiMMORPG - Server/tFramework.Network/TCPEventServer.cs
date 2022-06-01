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

    public class TCPEventServer : INetworkServer<TCPEventServer, TCPEventClient>
    {
        private ILogger _logger;
        private Socket _server;
        private readonly object _syncLock = new object();
        private List<TCPEventClient> _cliList;
        private Queue<Socket> _overClients;
        private Dictionary<uint, IResponse<TCPEventClient>> _responses;

        public event EventHandler<ServerEventArgs<TCPEventServer, TCPEventClient>> OnOpen, OnClose;
        public event EventHandler<ClientEventArgs<TCPEventClient>> OnConnected;

        public IPEndPoint EndPoint { get; set; }
        public TCPEventClient[] Clients { get { return _cliList.ToArray(); } }
        public uint ClientCount { get; private set; }
        public bool Opened { get; private set; }
        public uint MaximumClients { get; set; }
        public Type PacketType { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }
        public ClientReplaceMode ReplaceMode { get; set; }

        public TCPEventServer()
        {
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);
            ReplaceMode = ClientReplaceMode.MoveToQueue;

            _logger = LoggerFactory.GetLogger(this);
            _cliList = new List<TCPEventClient>();
            _responses = new Dictionary<uint, IResponse<TCPEventClient>>();
            _overClients = new Queue<Socket>();
        }

        public TCPEventServer(int port) : this(IPAddress.Any, port) { }
        public TCPEventServer(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPEventServer(IPEndPoint endPoint) { this.EndPoint = endPoint; }

        public bool Open()
        {
            try
            {
                _server = new Socket(EndPoint.AddressFamily, SocketType, Protocol);
                _server.Bind(EndPoint);
                _server.Listen(1);

                Opened = true;
                OnOpen.FireEvent(new ServerEventArgs<TCPEventServer, TCPEventClient>(this), this);
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

        public void RegisterResponse(IResponse<TCPEventClient> response)
        {
            _responses[response.ID] = response;
        }

        public void SendToAll(IDataPacket packet, Predicate<TCPEventClient> condition = null)
        {
            lock (_syncLock)
            {
                _cliList.ForEach(c => { if (condition == null || condition(c)) c.Send(packet); });
            }
        }

        public void SendToAll(IRequest<TCPEventClient> request, Predicate<TCPEventClient> condition = null)
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
                    OnClose.FireEvent(new ServerEventArgs<TCPEventServer, TCPEventClient>(this), this);
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
            var args = new SocketAsyncEventArgs();
            args.Completed += (s,e) => AcceptCall(e);

            if (!_server.AcceptAsync(args))
                AcceptCall(args);
        }

        void AcceptCall(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                try
                {
                    var socket = e.AcceptSocket;
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
                }
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                        _logger.LogFatal(ex);
                }
                BeginAccept();
            }
            else
                Close();
        }

        void HandleConnection(Socket socket)
        {
            lock (_syncLock)
            {
                ClientCount++;
                
                var client = new TCPEventClient(this, socket)
                {
                    PacketType = PacketType
                };
                _responses.Values.ForEach(r => client.RegisterResponse(r));
                OnConnected.FireEvent(new ClientEventArgs<TCPEventClient>(client), this);

                foreach (var rClient in _cliList)
                    if (rClient == client)
                        rClient.Disconnect();

                _cliList.Add(client);
                client.Initalize();
            }
        }

        public void FireDisconnected(TCPEventClient client)
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
