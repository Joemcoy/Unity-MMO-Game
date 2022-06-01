using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Chat.Server.Writers
{
    public class MessageRequest : IRequest
    {
        public uint ID { get { return PacketID.SendMessage; } }
        public MessageModel Message { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Message.WritePacket(Packet);

            return true;
        }
    }
}
