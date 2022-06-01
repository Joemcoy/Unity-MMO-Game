using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Extensions;

namespace tFramework.Network
{
    using tFramework.Network.Interfaces;

    public class AsyncState
    {
        public IDataPacket Packet { get; set; }
        public byte[] Header { get; set; }
        public byte[] Buffer { get; set; }
        public byte[] Chunk { get; set; }
        public int Received { get; set; }

        public AsyncState()
        {
            
        }

        public void Clear()
        {
            Packet = null;
            Header = null;
            Buffer = null;
            Chunk = new byte[SocketConstants.ChunkLength];
            Received = 0;
        }

        public AsyncState Clone()
        {
            return new AsyncState
            {
                Packet = Packet,
                Header = Header,
                Buffer = Buffer,
                Chunk = Chunk,
                Received = Received
            };
        }

        public void LoadHeader()
        {
            if (Header == null)
            {
                var header = new byte[Packet.HeaderLength];
                Packet.CopyHeader(ref header);
                Header = header;
            }
            else
                Header.For((i, b) => Header[i] = 0);
        }
    }
}
