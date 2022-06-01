using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class PrivateMessageWriter : IRequest
    {
        public uint ID { get { return PacketID.PrivateMessage; } }

        public PMResult Result { get; set; }
        public MessageModel Message { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteEnum(Result);
            if (Result == PMResult.Sent)
            {
                Packet.WriteBool(true);
                Message.WritePacket(Packet);
            }
            return true;
        }
    }
}
