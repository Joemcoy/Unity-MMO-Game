using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class ClickMoveWriter : IRequest
    {
        public uint ID { get { return PacketID.ClickMove; } }
        public int CID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteFloat(X);
            Packet.WriteFloat(Y);
            Packet.WriteFloat(Z);

            return true;
        }
    }
}
