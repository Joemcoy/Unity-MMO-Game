using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class PacketEventArgs : BaseClientEventArgs
    {
        public ISocketPacket Packet { get; private set; }

        public PacketEventArgs(IClientSocket Client, ISocketPacket Packet)
            : base(Client)
        {
            this.Packet = Packet;
        }
    }
}
