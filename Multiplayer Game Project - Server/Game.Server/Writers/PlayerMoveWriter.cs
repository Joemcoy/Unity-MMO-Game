using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data.Enums;
using Game.Data;

using Network.Data.Interfaces;
using Game.Data.Models;

namespace Game.Server.Writers
{
    public class PlayerMoveWriter : IRequest
    {
        public uint ID { get { return PacketID.SetKeyState; } }

        public int CID { get; set; }
        public MoveDirection Direction { get; set; }
        public bool Pressed { get; set; }
        public PositionModel Position {get;set;}

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteEnum(Direction);
            Packet.WriteBool(Pressed);
            Position.WritePacket(Packet);

            return true;
        }
    }
}
