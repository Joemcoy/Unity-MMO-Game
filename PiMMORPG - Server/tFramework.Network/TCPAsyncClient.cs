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

    public class TCPAsyncClient : INetworkClient<TCPAsyncClient>
    {
        private Dictionary<ushort, IResponse<TCPAsyncClient>> _responseDict;
        private ILogger logger;
        private Socket socket;
        private NetworkStream stream;
        private AutoResetEvent sendEvent;
        //private AsyncState sendState, receiveState;

        public IResponse<TCPAsyncClient>[] Responses { get { return _responseDict.Values.ToArray(); } }
        public bool IOEnabled { get; set; }
        public bool UseStream { get; set; }
        public bool Connected { get; private set; }
        public Type PacketType { get; set; }
        public INetworkServer<TCPAsyncClient> Server { get; private set; }
        public IPEndPoint EndPoint { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }

        public event EventHandler<ClientEventArgs<TCPAsyncClient>> OnConnect;
        public event EventHandler<DisconnectedEventArgs<TCPAsyncClient>> OnDisconnect;
        public event EventHandler<PacketEventArgs<TCPAsyncClient>> OnPacketRead, OnPacketReceive, OnPacketWrite, OnPacketSent;
        public event EventHandler<ClientErrorEventArgs<TCPAsyncClient>> OnError;
        public event EventHandler<ResponseCallEventArgs<TCPAsyncClient>> OnResponseExecute;
        public event EventHandler<RequestEventArgs<TCPAsyncClient>> OnRequestWrite;

        public TCPAsyncClient()
        {
            IOEnabled = false;
            UseStream = false;
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);

            logger = LoggerFactory.GetLogger(this);
            _responseDict = new Dictionary<ushort, IResponse<TCPAsyncClient>>();
        }

        public TCPAsyncClient(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPAsyncClient(IPEndPoint endPoint) : this()
        {
            EndPoint = endPoint;
        }

        internal TCPAsyncClient(TCPAsyncServer server, Socket client) : this((IPEndPoint)client.RemoteEndPoint)
        {
            Server = server as INetworkServer<TCPAsyncClient>;
            socket = client;
        }

        internal void Initalize()
        {
            /*socket.NoDelay = true;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            socket.LingerState = new LingerOption(true, SocketConstants.LingerSeconds);
            socket.ReceiveBufferSize = SocketConstants.ReceiveBufferSize;
            socket.ReceiveTimeout = SocketConstants.ReceiveTimeout;
            socket.SendBufferSize = SocketConstants.SendBufferSize;
            socket.SendTimeout = SocketConstants.SendTimeout;
            socket.Ttl = SocketConstants.Ttl;*/

            sendEvent = new AutoResetEvent(true);
            if (UseStream)
                stream = new NetworkStream(socket, true);

            Connected = true;
            OnConnect.FireEvent(new ClientEventArgs<TCPAsyncClient>(this), this);
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
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
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
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                    {
                        OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                        logger.LogFatal(ex);
                    }
                }
                return socket == null || !socket.Connected;
            }
            else
                return false;
        }

        public void RegisterResponse(IResponse<TCPAsyncClient> response)
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

        public void Send(IRequest<TCPAsyncClient> request)
        {
            var packet = CreatePacket(request.ID);
            OnRequestWrite.FireEvent(new RequestEventArgs<TCPAsyncClient>(this, request), this);

            if (request.Write(this, packet))
                Send(packet);
        }

        public void Send(IDataPacket packet)
        {
            try
            {
                sendEvent.WaitOne();

                OnPacketWrite.FireEvent(new PacketEventArgs<TCPAsyncClient>(this, packet), this);
                
                var state = new AsyncState();

                byte[] header = new byte[packet.HeaderLength];
                packet.CopyHeader(ref header);

                state.Packet = packet;
                state.Buffer = header.Concat(packet.Buffer).ToArray();

                if (UseStream)
                    stream.BeginWrite(state.Buffer, 0, state.Buffer.Length, EndSend, state);
                else
                    socket.BeginSend(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, EndSend, state);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void EndSend(IAsyncResult ar)
        {
            try
            {
                if (UseStream)
                    stream.EndWrite(ar);
                else
                    socket.EndSend(ar);
                sendEvent.Set();

                var state = ar.AsyncState as AsyncState;
                OnPacketSent.FireEvent(new PacketEventArgs<TCPAsyncClient>(this, state.Packet), this);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
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

        void BeginRead()
        {
            try
            {
                var state = new AsyncState();
                state.Packet = CreatePacket(0x0);
                state.LoadHeader();

                if (UseStream)
                    stream.BeginRead(state.Header, 0, state.Packet.HeaderLength, EndReadHeader, state);
                else
                    socket.BeginReceive(state.Header, 0, state.Packet.HeaderLength, SocketFlags.None, EndReadHeader, state);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        int tries = 0;
        void EndReadHeader(IAsyncResult ar)
        {
            try
            {
                var received = UseStream ? stream.EndRead(ar) : socket.EndReceive(ar);
                var state = ar.AsyncState as AsyncState;

                if (received > 0)
                {
                    if (tries > 0) tries = 0;

                    if (state.Received == 0 && state.Header[0] != SocketConstants.HandshakeByte)
                    {
                        Close(DisconnectReason.EndOfStream);
                    }
                    else if(state.Received + received != state.Header.Length)
                    {
                        state.Received += received;

                        if(state.Packet.HeaderLength - state.Received < 0)
                        {
                            logger.LogWarning("Packet Header corruption!");
                            Close(DisconnectReason.Error);
                            return;
                        }
                        
                        if (UseStream)
                            stream.BeginRead(state.Header, state.Received, state.Packet.HeaderLength - state.Received, EndReadHeader, state);
                        else
                            socket.BeginReceive(state.Header, state.Received, state.Packet.HeaderLength - state.Received, SocketFlags.None, EndReadHeader, state);
                    }
                    else
                    {
                        state.Received = 0;

                        int length = state.Packet.LoadHeader(state.Header);
                        var packet = state.Packet;

                        if (length == 0)
                        {
                            BeginRead();
                            FirePacketReceive(packet);
                        }
                        else
                        {
                            state.Buffer = new byte[length];
                            BeginReadBuffer(state);
                        }
                    }
                }
                //else if (e.SocketError == SocketError.Interrupted || e.SocketError == SocketError.ConnectionReset)
                //BeginRead();
                else if (tries++ > 5)
                    Close(DisconnectReason.EndOfStream);
                else
                {
                    if (UseStream)
                        stream.BeginRead(state.Header, state.Received, state.Packet.HeaderLength - state.Received, EndReadHeader, state);
                    else
                        socket.BeginReceive(state.Header, state.Received, state.Packet.HeaderLength - state.Received, SocketFlags.None, EndReadHeader, state);
                }
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void BeginReadBuffer(AsyncState state)
        {
            try
            {
                if (UseStream)
                    stream.BeginRead(state.Buffer, state.Received, state.Buffer.Length - state.Received, EndReadBuffer, state);
                else
                    socket.BeginReceive(state.Buffer, state.Received, state.Buffer.Length - state.Received, SocketFlags.None, EndReadBuffer, state);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void EndReadBuffer(IAsyncResult ar)
        {
            try
            {
                var received = UseStream ? stream.EndRead(ar) : socket.EndReceive(ar);
                var state = ar.AsyncState as AsyncState;

                if (received > 0)
                {
                    if (tries > 0) tries = 0;
                    state.Received += received;

                    if (state.Received < state.Buffer.Length)
                        BeginReadBuffer(state);
                    else
                    {
                        var packet = state.Packet;
                        packet.Buffer = state.Buffer;
                        packet.Reset();
                        BeginRead();

                        FirePacketReceive(packet);
                    }
                }
                else if (tries++ > 5)
                    Close(DisconnectReason.EndOfStream);
                else
                    BeginReadBuffer(state);
            }
            catch (Exception ex)
            {
                if (SocketConstants.HandleException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                Close(DisconnectReason.Error);
            }
        }

        void FirePacketReceive(IDataPacket packet)
        {
            OnPacketRead.FireEvent(new PacketEventArgs<TCPAsyncClient>(this, packet), this);

            IResponse<TCPAsyncClient> response;
            if (_responseDict.TryGetValue(packet.ID, out response))
            {
                var responseAction = new Action(() =>
                {
                    if (response.Read(this, packet))
                        response.Execute();
                });

                var args = new ResponseCallEventArgs<TCPAsyncClient>(this, response, responseAction);
                OnResponseExecute.FireEvent(args, this);

                if (!args.CancelCall)
                    responseAction();
            }

            OnPacketReceive.FireEvent(new PacketEventArgs<TCPAsyncClient>(this, packet), this);
        }

        void Close(DisconnectReason reason)
        {
            if (Connected)
            {
                try
                {
                    Connected = false;
                    sendEvent.Close();

                    if (socket.Connected)
                        socket.Shutdown(SocketShutdown.Both);

                    if (stream != null)
                        stream.Close();
                    socket.Close();
                }
                catch (Exception ex)
                {
                    if (SocketConstants.HandleException(ex))
                    {
                        OnError.FireEvent(new ClientErrorEventArgs<TCPAsyncClient>(this, ex), this);
                        logger.LogFatal(ex);
                    }
                }
                //catch (IOException) { }
                //catch (SocketException) { }

                if (Server != null)
                    Server.FireDisconnected(this);
                OnDisconnect.FireEvent(new DisconnectedEventArgs<TCPAsyncClient>(this, reason), this);
            }
        }

        public bool Equals(TCPAsyncClient other)
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