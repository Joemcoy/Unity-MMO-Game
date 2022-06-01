using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class BanRequest : IRequest
    {
        public uint ID { get { return PacketID.DataBan; } }
        public bool Result { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteBool(Result);

            return true;
        }
    }
}
