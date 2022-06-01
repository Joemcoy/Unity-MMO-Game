using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class CharacterListRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendCharacters; } }
        public CharacterModel[] Characters { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            if (Characters.Length > 0)
            {
                Packet.WriteBool(true);
                Packet.WriteInt(Characters.Length);

                foreach (var Character in Characters)
                    Character.WritePacket(Packet);
            }
            else
                Packet.WriteBool(false);

            return true;
        }
    }
}
