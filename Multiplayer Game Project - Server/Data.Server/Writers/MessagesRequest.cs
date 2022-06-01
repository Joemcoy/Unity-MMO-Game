using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class MessagesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendMessages; } }
        public MessageModel[] Messages { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(Messages.Length);
            foreach (var Message in Messages)
                Message.WritePacket(Packet);
            
            return true;
        }
    }
}
