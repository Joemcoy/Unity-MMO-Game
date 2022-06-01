using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Chat.Client;
using Game.Data.Models;
using Data.Client;
using Base.Factories;
using Chat.Server.Writers;

namespace Chat.Server.ChatResponses
{
    public class SendMessagePacket : CCResponse
    {
        MessageModel Message;

        public override uint ID { get { return PacketID.SendMessage; } }
        public override bool Read(ISocketPacket Packet)
        {
            Message = new MessageModel();
            Message.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("[{0}]: {1}", Message.Username, Message.Content);

            DataClient Data = SingletonFactory.GetInstance<DataClient>();
            Data.SendMessage(Message);

            var Packet = new MessageRequest();
            Packet.Message = Message;

            foreach (ChatClient Client in ChatServer.Clients)
            {
                Client.Socket.Send(Packet);
            }
        }
    }
}
