using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Request
{
    public class SendTreesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendTrees; } }
        public int Server { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(Server);

            return true;
        }
    }
}
