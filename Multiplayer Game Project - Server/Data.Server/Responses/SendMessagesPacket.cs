using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Results;
using Game.Data.Models;
using Game.Controller;
using Data.Client;
using Base.Factories;

using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class SendMessagesPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataSendMessages; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new MessagesRequest();
            Packet.Messages = ChatLogManager.GetLastMessages();

            Client.Socket.Send(Packet);
        }
    }
}