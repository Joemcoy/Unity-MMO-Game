using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class SetPlayerPositionWriter : IRequest
    {
        public uint ID { get { return PacketID.SetPlayerPosition; } }

        public int CharacterID { get; set; }
        public PositionModel Position { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CharacterID);
            Position.WritePacket(Packet);

            return true;
        }
    }
}
