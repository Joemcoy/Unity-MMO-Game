using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Reflection;
using tFramework.Factories;

namespace tFramework.Network
{
    public static class SocketConstants
    {
        public const byte HandshakeByte = 0xAD;
        public const int ConnectTimeout = 5000;
        public const int SendTimeout = 10 * 1000;
        public const int ReceiveTimeout = 10 * 1000;
        public const int ChunkLength = 512;
        public const int ReceiveBufferSize = ChunkLength;
        public const int SendBufferSize = ChunkLength;
        public const int LingerSeconds = 2;
        public const int Ttl = 42;

        public static bool HandleException<TException>(TException exception) where TException : Exception
        {
            /*if (exception is ObjectDisposedException)
                return false;
            else if (exception is SocketException)
                return false;
            else*/
            if (exception is SocketException)
            {
                var se = exception as SocketException;
                if (se.SocketErrorCode == SocketError.ConnectionReset)
                    return false;
            }
            return true;
        }
    }
}