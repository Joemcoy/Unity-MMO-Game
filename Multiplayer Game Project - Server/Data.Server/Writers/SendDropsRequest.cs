using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class SendDropsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendDrops; } }
        public DropModel Drop { get; set; }
        public bool End { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteBool(End);
            if(!End)
                Drop.WritePacket(Packet);

            return true;
        }
    }
}
