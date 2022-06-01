using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Client.Writers
{
    public class BanRequest : IRequest
    {
        public uint ID { get { return PacketID.DataBan; } }
        public int Type { get; set; }
        public string Name { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(Type);
            Packet.WriteString(Name);

            return true;
        }
    }
}
