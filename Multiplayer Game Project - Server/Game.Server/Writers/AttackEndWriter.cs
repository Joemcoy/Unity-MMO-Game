using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class AttackEndWriter : IRequest
    {
        public uint ID { get { return PacketID.AttackEnd; } }
        public int CID { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);

            return true;
        }
    }
}
