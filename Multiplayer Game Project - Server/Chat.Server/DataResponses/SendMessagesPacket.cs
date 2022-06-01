using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chat.Client;
using Data.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Enums;
using Base.Factories;

namespace Chat.Server.DataResponses
{
    public class SendMessagesPacket : DCResponse
    {
        ISocketPacket Packet;

        public override uint ID { get { return PacketID.DataSendMessages; } }
        public override bool Read(ISocketPacket Packet)
        {
            this.Packet = Packet;
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            ChatClient Client = this.Client.Dequeue<ChatClient>();

            Packet.ID = PacketID.SendMessages;
            Client.Socket.Send(Packet);
        }
    }
}
