using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class ShieldStartWriter : IRequest
    {
        public uint ID { get { return PacketID.ShieldStart; } }
        public int CID { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);

            return true;
        }
    }
}
