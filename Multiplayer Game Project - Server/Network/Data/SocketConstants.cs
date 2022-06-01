using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Network.Data
{
    public static class SocketConstants
    {
        public const int ConnectTimeout = 3 * 1000;
        public const int SendTimeout = 10 * 1000;
        public const int ReceiveTimeout = 10 * 1000;

        public const int MaximumConnections = 10;
        public const int WriteQueueInterval = 50;
        public const int ReadQueueInterval = 50;
        public const int ExecuteQueueInterval = 50;
        public const int AcceptInterval = 100;
        public const int PingInterval = 2000;

        public const int MaximumPacketLength = 1024 * 1024 * 2;
        public const byte DisconnectFlag = 0x0;
        public const byte NonFlag = 0x1;
        public const byte PingFlag = 0x3;
        public const byte PacketFlag = 0x4;
        public const byte ChunkFlag = 0xFA;
        public const byte HandshakeFlag = 0xC4;
        public const int ChunkLength = 300 * 1024;
        public const int ReceiveBufferSize = ChunkLength;
        public const int SendBufferSize = ChunkLength;
        public const int LingerSeconds = 2;
        public const int Ttl = 42;
        public const int MinimumToCompress = 1024 * 120;

        private static Type[] SkExcept = new Type[]
        {
            typeof(IOException),
            typeof(ThreadAbortException),
            typeof(ThreadInterruptedException),
            typeof(SocketException)
        };

        public static bool SkipException(Exception ex)
        {
            return SkExcept.Any(E => E == ex.GetType());
        }
    }
}