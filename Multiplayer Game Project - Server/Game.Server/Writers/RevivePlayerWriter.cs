using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Models;

namespace Game.Server.Writers
{
    public class RevivePlayerWriter : IRequest
    {
        public uint ID { get { return PacketID.Revive; } }
        public int CID { get; set; }
        public PositionModel Position { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Position.WritePacket(Packet);

            return true;
        }
    }
}
