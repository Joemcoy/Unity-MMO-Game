using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

using tFramework.Interfaces;

namespace tFramework.Network
{
    using Enums;
    using Helper;
    using Factories;
    using EventArgs;
    using DataPacket;
    using Interfaces;

    public class TCPClient : IThread, INetworkClient<TCPClient>
    {
        private ILogger logger;
        private Socket socket;
        private IOQueue queue;
        private Dictionary<ushort, IResponse<TCPClient>> responseDict;
        private volatile object synclock = new object();

        public bool IOEnabled { get; set; }
        public IResponse<TCPClient>[] Responses { get { return responseDict.Values.ToArray(); } }
        public event EventHandler<ClientEventArgs<TCPClient>> OnConnect;
        public event EventHandler<DisconnectedEventArgs<TCPClient>> OnDisconnect;
        public event EventHandler<PacketEventArgs<TCPClient>> OnPacketRead, OnPacketReceive, OnPacketWrite, OnPacketSent;
        public event EventHandler<ResponseCallEventArgs<TCPClient>> OnResponseExecute;
        public event EventHandler<ClientErrorEventArgs<TCPClient>> OnError;
        public event EventHandler<RequestEventArgs<TCPClient>> OnRequestWrite;

        public bool Connected { get; private set; }
        public IPEndPoint EndPoint { get; set; }
        public NetworkStream Stream { get; private set; }
        public INetworkServer<TCPClient> Server { get; private set; }
        public Type PacketType { get; set; }
        public ProtocolType Protocol { get; set; }
        public SocketType SocketType { get; set; }

        public TCPClient()
        {
            IOEnabled = false;
            Protocol = ProtocolType.IP;
            SocketType = SocketType.Stream;
            PacketType = typeof(StreamPacket);

            logger = LoggerFactory.GetLogger(this);
            responseDict = new Dictionary<ushort, IResponse<TCPClient>>();
        }

        public TCPClient(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public TCPClient(IPEndPoint endPoint) : this()
        {
            this.EndPoint = endPoint;
        }

        internal TCPClient(TCPServer server, Socket client) : this((IPEndPoint)client.RemoteEndPoint)
        {
            this.Server = server;
            socket = client;
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
                if (CaughtException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                return false;
            }
        }

        public bool Resolve(string address)
        {
            IPAddress addr;
            var port = 0;

            var ps = address.IndexOf(':');
            if (ps > -1 && !int.TryParse(address.Substring(ps+1), out port))
                return false;
            else
            {
                if (ps > -1)
                    address = address.Substring(0, ps);

                if(!IPAddress.TryParse(address, out addr))
                {
                    var addresses = Dns.GetHostAddresses(address);
                    if (addresses == null || addresses.Length == 0)
                        return false;
                    else
                        addr = addresses[0];
                }
            }

            EndPoint = new IPEndPoint(addr, port);
            return true;
        }

        public void RegisterResponse(IResponse<TCPClient> response)
        {
            responseDict[response.ID] = response;
        }

        public void Send(IRequest<TCPClient> request)
        {
            var packet = CreatePacket(request.ID);
            if (request.Write(this, packet))
                Send(packet);
        }

        public void Send(IDataPacket packet)
        {
            OnPacketWrite.FireEvent(new PacketEventArgs<TCPClient>(this, packet), this);
            queue.EnqueueSend(packet);
        }

        public bool Disconnect()
        {
            if (Connected)
            {
                Close(DisconnectReason.Normal);
                return true;
            }
            return false;
        }

        internal void FirePacketReceived(IDataPacket packet)
        {
            OnPacketReceive.FireEvent(new PacketEventArgs<TCPClient>(this, packet), this);

            IResponse<TCPClient> response;
            if (responseDict.TryGetValue(packet.ID, out response))
            {
                var responseAction = new Action(() =>
                {
                    if (response.Read(this, packet))
                        response.Execute();
                });

                var args = new ResponseCallEventArgs<TCPClient>(this, response, responseAction);
                OnResponseExecute.FireEvent(args, this);

                if (!args.CancelCall)
                    responseAction();
            }
        }

        internal void FirePacketSent(IDataPacket packet)
        {
            OnPacketSent.FireEvent(new PacketEventArgs<TCPClient>(this, packet), this);
        }

        public IDataPacket CreatePacket(ushort id)
        {
            var packet = (IDataPacket)Activator.CreateInstance(PacketType);
            packet.ID = id;

            return packet;
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

            ThreadFactory.Start(this);
        }

        void IThread.Start()
        {
            Stream = new NetworkStream(socket);

            Connected = true;
            OnConnect.FireEvent(new ClientEventArgs<TCPClient>(this), this);
            ThreadFactory.Start(queue = new IOQueue(this));
        }

        void IThread.End()
        {
            logger.LogWarning("Client {0} thread has been stopped!", EndPoint);
            if(Connected)
                Close(DisconnectReason.EndOfStream);
        }

        bool IThread.Run()
        {
            try
            {
                IDataPacket packet = CreatePacket(0x0);

                byte[] header = new byte[packet.HeaderLength];
                if (!ReadBuffer(ref header, false) || header[0] != SocketConstants.HandshakeByte)
                    return false;

                int length = packet.LoadHeader(header);

                byte[] buffer = new byte[length];
                if (!ReadBuffer(ref buffer, true))
                    return false;

                packet.Buffer = buffer;
                packet.Reset();

                OnPacketRead.FireEvent(new PacketEventArgs<TCPClient>(this, packet), this);
                queue.EnqueueReceive(packet);
                return true;
            }
            catch (Exception ex)
            {
                if (CaughtException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                return false;
            }
        }

        byte[] chunk = new byte[SocketConstants.ChunkLength];
        private bool ReadBuffer(ref byte[] buffer, bool chunked)
        {
            try
            {
                if (chunked)
                {
                    int total = 0;
                    do
                    {
                        int received = Stream.Read(chunk, 0, chunk.Length);
                        if (received == 0)
                            return false;
                        else
                        {
                            Buffer.BlockCopy(chunk, 0, buffer, total, received > buffer.Length ? buffer.Length : received);
                            total += received;
                        }
                    } while (total < buffer.Length);
                    return total > 0;
                }
                else
                {
                    int received = Stream.Read(buffer, 0, buffer.Length);
                    return received == buffer.Length;
                }
            }
            catch (Exception ex)
            {
                if (CaughtException(ex))
                {
                    OnError.FireEvent(new ClientErrorEventArgs<TCPClient>(this, ex), this);
                    logger.LogFatal(ex);
                }
                return false;
            }
        }

        private bool CaughtException(Exception ex)
        {
            var type = ex.GetType();
            return type != typeof(ThreadAbortException) && type != typeof(ThreadInterruptedException);
        }

        private void Close(DisconnectReason reason)
        {
            lock (synclock)
            {
                if (Connected)
                {
                    Connected = false;

                    try
                    {
                        ThreadFactory.Stop(queue);
                        ThreadFactory.Stop(this);

                        Stream.Close();
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    catch (Exception ex)
                    {
                        if (CaughtException(ex))
                        {
                            OnError.FireEvent(new ClientErrorEventArgs<TCPClient>(this, ex), this);
                            logger.LogFatal(ex);
                        }
                    }
                    //catch (IOException) { }
                    //catch (SocketException) { }

                    
                    if(Server != null)
                        Server.FireDisconnected(this);
                    OnDisconnect.FireEvent(new DisconnectedEventArgs<TCPClient>(this, reason), this);
                }
            }
        }

        public bool Equals(TCPClient other)
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