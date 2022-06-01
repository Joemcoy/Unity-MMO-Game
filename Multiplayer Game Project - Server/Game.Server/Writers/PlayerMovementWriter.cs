using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Server.Configuration;

using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class PlayerMovementWriter : IRequest
    {
        public int CID { get; set; }
        public byte Event { get; set; }
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public PositionModel Position { get; set; }
        public bool Running { get; set; }
        public bool Jump { get; set; }

        public uint ID { get { return PacketID.MovePlayer; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteByte(Event);

            switch(Event)
            {
                case 1:
                    Packet.WriteFloat(Horizontal);
                    Packet.WriteFloat(Vertical);
                    break;
                case 2:
                    Position.WritePacket(Packet);
                    break;
                case 3:
                    Packet.WriteBool(Running);
                    break;
                case 4:
                    Packet.WriteBool(Jump);
                    break;
            }

            return true;
        }
    }
}
