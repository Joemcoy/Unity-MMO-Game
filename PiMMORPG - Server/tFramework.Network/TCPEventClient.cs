using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using tFramework.Interfaces;
namespace tFramework.Network
{
    using Enums;
    using Helper;
    using Interfaces;
    using DataPacket;
    using Factories;
    using EventArgs;

    public class TCPEventClient : INetworkClient<TCPEventClient>
    {
        private Dictionary<ushort, IResponse<TCPEventClient>> _responseDict;
        private ILogger logger;
        private Socket socket;
        private object sendLock = new object();
        private AsyncState sendState, receiveState;

        public IResponse<TCPEventClient>[] Responses { get { return _responseDict.Values.ToArray(); } }
        public bool IOEnabled { get; set; }
        public bool Connected { get; private set; }
        public Type PacketType { get; set; }
        public INetworkServer<TCPEventClient> Server { get; private set; }
        public IPEndPoint EndPoint { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }

        public event EventHandler<ClientEventArgs<TCPEventClient>> OnConnect;
        public event EventHandler<DisconnectedEventArgs<TCPEventClient>> OnDisconnect;
        public event EventHandler<PacketEventArgs<TCPEventClient>> OnPacketRead, OnPacketReceive, OnPacketWrite, OnPacketSent;
        public event EventHandler<ClientErrorEventArgs<TCPEventClient>> OnError;
        public event EventHandler<ResponseCallEventArgs<TCPEventClient>> OnResponseExecute;
        public event EventHandler<RequestEventArgs<TCPEventClient>> OnRequestWrite;

        public TCPEventClient()
        {
            IOEnabled = false;
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);

            logger = LoggerFactory.GetLogger(this);
            sendState = new AsyncState();
            receiveState = new AsyncState();
            _responseDict = new Dictionary<ushort, IResponse<TCPEventClient>>();
        }

        public TCPEventClient(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPEventClient(IPEndPoint endPoint) : this()
        {
            this.EndPoint = endPoint;
        }

        internal TCPEventClient(TCPEventServer server, Socket client) : this((IPEndPoint)client.RemoteEndPoint)
        {
            this.Server = server;
            socket = client;
        }

        internal void Initalize()
        {
            socket.NoDelay = true;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            socket.LingerState = new LingerOption(true, SocketConstants.LingerSeconds);
            socket.ReceiveBufferSize = SocketConstants.ReceiveBufferSize;
            socket.ReceiveTimeout = SocketConstants.ReceiveTimeout;
            socket.SendBufferSize = SocketConstants.SendBufferSize;
            socket.SendTimeout = SocketConstants.SendTimeout;
            socket.Ttl = SocketConstants.Ttl;

            Connected = true;
            OnConnect.FireEvent(new ClientEventArgs<TCPEventClient>(this), this);
            BeginRead();
        }

        public bool Connect()
        {
            try
            {
                if (Connected)
                    return true;

                socket = new Socket(EndPoint.AddressFamily, SocketType, Protocol);
                var state = socket.BeginConnect(EndPoint, null, null);
                var result = state.AsyncWaitHandle.WaitOne(SocketConstants.ConnectTimeout, true);

                if (result)
                {
                    socket.EndConnect(state);
                    Initalize();
                    return true;
                }
                else
                {
                    socket.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                return false;
            }
        }

        public bool Disconnect()
        {
            if (Connected)
            {
                try
                {
                    Close(DisconnectReason.Normal);
                }
                catch(Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                    {
                        OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                        logger.LogFatal(ex);
                    }
                }
                return socket == null || !socket.Connected;
            }
            else
                return false;
        }

        public void RegisterResponse(IResponse<TCPEventClient> response)
        {
            _responseDict[response.ID] = response;
        }

        public bool Resolve(string address)
        {
            IPAddress addr;
            var port = 0;

            var ps = address.IndexOf(':');
            if (ps > -1 && !int.TryParse(address.Substring(ps + 1), out port))
            {
                logger.LogWarning("DNS_ERROR_1");
                return false;
            }
            else
            {
                if (ps > -1)
                    address = address.Substring(0, ps);

                if (!IPAddress.TryParse(address, out addr))
                {
                    var addresses = Dns.GetHostAddresses(address);
                    if (addresses == null || addresses.Length == 0)
                    {
                        logger.LogWarning("DNS_ERROR_2");
                        return false;
                    }
                    else
                        addr = addresses[0];
                }
            }

            EndPoint = new IPEndPoint(addr, port);
            return true;
        }

        public void Send(IRequest<TCPEventClient> request)
        {
            var packet = CreatePacket(request.ID);
            OnRequestWrite.FireEvent(new RequestEventArgs<TCPEventClient>(this, request), this);

            if (request.Write(this, packet))
                Send(packet);
        }

        public void Send(IDataPacket packet)
        {
            try
            {
                OnPacketWrite.FireEvent(new PacketEventArgs<TCPEventClient>(this, packet), this);

                byte[] header = null;
                packet.CopyHeader(ref header);

                sendState.Clear();
                sendState.Header = header;
                sendState.Buffer = packet.Buffer;

                var args = CreateArgs(SendHeader, sendState);
                args.SetBuffer(sendState.Header, 0, sendState.Header.Length);

                if (!socket.SendAsync(args))
                    SendHeader(args);
            }
            catch(Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void SendHeader(SocketAsyncEventArgs e)
        {
            try
            {
                var state = (AsyncState)e.UserToken;
                if (e.SocketError == SocketError.Success)
                {
                    var args = CreateArgs(SendBuffer, sendState);
                    args.SetBuffer(sendState.Buffer, 0, sendState.Buffer.Length);

                    if (!socket.SendAsync(args))
                        SendBuffer(args);
                }
                //else if (e.SocketError == SocketError.Interrupted)
                    //Send(state.Packet);
                else
                    throw new SocketException((int)e.SocketError);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void SendBuffer(SocketAsyncEventArgs e)
        {
            try
            {
                var state = (AsyncState)e.UserToken;
                if (e.SocketError == SocketError.Success)
                    OnPacketSent.FireEvent(new PacketEventArgs<TCPEventClient>(this, state.Packet), this);
                //else if (e.SocketError == SocketError.Interrupted)
                //Send(state.Packet);
                else
                    throw new SocketException((int)e.SocketError);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        public IDataPacket CreatePacket(ushort id)
        {
            var packet = (IDataPacket)Activator.CreateInstance(PacketType);
            packet.ID = id;

            return packet;
        }

        SocketAsyncEventArgs CreateArgs(Action<SocketAsyncEventArgs> callback, object state = null)
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += (s, e) => callback(e);
            args.UserToken = state;
            return args;
        }

        void BeginRead()
        {
            try
            {
                receiveState.Clear();
                receiveState.Packet = CreatePacket(0x0);
                receiveState.LoadHeader();

                var args = CreateArgs(ReadHeader, receiveState);
                args.SetBuffer(receiveState.Header, 0, receiveState.Header.Length);

                if (!socket.ReceiveAsync(args))
                    ReadHeader(args);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        int tries = 0;
        void ReadHeader(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && e.BytesTransferred != 0)
                {
                    if (tries > 0) tries = 0;
                    if (receiveState.Header[0] != SocketConstants.HandshakeByte)
                    {
                        Close(DisconnectReason.EndOfStream);
                    }
                    else
                    {
                        int length = receiveState.Packet.LoadHeader(receiveState.Header);

                        if (length == 0)
                        {
                            var packet = receiveState.Packet;

                            BeginRead();
                            FirePacketReceive(packet);
                        }
                        else
                        {
                            receiveState.Buffer = new byte[length];
                            var args = CreateArgs(ReadBuffer, receiveState);
                            args.SetBuffer(receiveState.Chunk, 0, receiveState.Chunk.Length);

                            if (!socket.ReceiveAsync(args))
                                ReadBuffer(args);
                        }
                    }
                }
                //else if (e.SocketError == SocketError.Interrupted || e.SocketError == SocketError.ConnectionReset)
                    //BeginRead();
                else if (e.SocketError != SocketError.Success)
                    throw new SocketException((int)e.SocketError);
                else if(tries++ > 5)
                    Close(DisconnectReason.EndOfStream);
                else
                {
                    var args = CreateArgs(ReadHeader, receiveState);
                    args.SetBuffer(receiveState.Header, 0, receiveState.Header.Length);

                    if (!socket.ReceiveAsync(args))
                        ReadHeader(args);
                }
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void ReadBuffer(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && e.BytesTransferred != 0)
                {
                    if (tries > 0) tries = 0;

                    var f = receiveState.Buffer.Length - receiveState.Received;
                    var total = e.BytesTransferred > f ? e.BytesTransferred - f : e.BytesTransferred;
                    Buffer.BlockCopy(receiveState.Chunk, 0, receiveState.Buffer, receiveState.Received, total);
                    receiveState.Received += e.BytesTransferred;

                    if (receiveState.Received < receiveState.Buffer.Length)
                    {
                        var args = CreateArgs(ReadBuffer, receiveState);
                        args.SetBuffer(receiveState.Chunk, 0, receiveState.Chunk.Length);

                        if (!socket.ReceiveAsync(args))
                            ReadBuffer(args);
                    }
                    else
                    {
                        var packet = receiveState.Packet;
                        packet.Buffer = receiveState.Buffer;
                        packet.Reset();
                        BeginRead();

                        FirePacketReceive(packet);
                    }
                }
                else if (e.SocketError != SocketError.Success)
                    throw new SocketException((int)e.SocketError);
                else if (tries++ > 5)
                    Close(DisconnectReason.EndOfStream);
                else
                {
                    var args = CreateArgs(ReadBuffer, receiveState);
                    args.SetBuffer(receiveState.Chunk, 0, receiveState.Chunk.Length);

                    if (!socket.ReceiveAsync(args))
                        ReadBuffer(args);
                }
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void FirePacketReceive(IDataPacket packet)
        {
            OnPacketRead.FireEvent(new PacketEventArgs<TCPEventClient>(this, packet), this);

            IResponse<TCPEventClient> response;
            if (_responseDict.TryGetValue(packet.ID, out response))
            {
                var responseAction = new Action(() =>
                {
                    if (response.Read(this, packet))
                        response.Execute();
                });

                var args = new ResponseCallEventArgs<TCPEventClient>(this, response, responseAction);
                OnResponseExecute.FireEvent(args, this);

                if (!args.CancelCall)
                    responseAction();
            }

            OnPacketReceive.FireEvent(new PacketEventArgs<TCPEventClient>(this, packet), this);
        }

        void Close(DisconnectReason reason)
        {
            if (Connected)
            {
                try
                {
                    Connected = false;

                    if(socket.Connected)
                        socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                    {
                        OnError.FireEvent(new ClientErrorEventArgs<TCPEventClient>(this, ex), this);
                        logger.LogFatal(ex);
                    }
                }
                //catch (IOException) { }
                //catch (SocketException) { }

                if (Server != null)
                    Server.FireDisconnected(this);
                OnDisconnect.FireEvent(new DisconnectedEventArgs<TCPEventClient>(this, reason), this);
            }
        }

        public bool Equals(TCPEventClient other)
        {
            if (other == null || other.EndPoint == null)
                return false;
            return other.EndPoint.Address.Equals(EndPoint.Address) && other.EndPoint.Port == EndPoint.Port;
        }

        public override int GetHashCode()
        {
            return EndPoint.GetHashCode();
        }
    }
}