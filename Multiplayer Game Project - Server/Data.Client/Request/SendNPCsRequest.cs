using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Request
{
    public class SendNPCsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendNPCs; } }
        public int Server { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(Server);

            return true;
        }
    }
}
