using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SyncCharacterRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SyncCharacter; } }
        
        public Position Position { get; set; }
        public float Horizontal { get; set; }
        public float Vertical { get; set; }

        public override bool Write(IDataPacket Packet)
        {
            Position.WritePacket(Packet);
            Packet.WriteFloat(Horizontal);
            Packet.WriteFloat(Vertical);
            return true;
        }
    }
}