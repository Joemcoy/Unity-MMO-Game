using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class MapRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendMapByID; } }
        public MapModel Map { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteBool(Map != null);
            if (Map != null)
                Map.WritePacket(Packet);

            return true;
        }
    }
}
