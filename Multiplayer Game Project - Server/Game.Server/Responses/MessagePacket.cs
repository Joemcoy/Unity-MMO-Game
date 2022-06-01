using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Game.Data.Enums;
using Game.Data.Models;
using Game.Server.Manager;
using Server.Configuration;
using Base.Factories;

namespace Game.Server.Responses
{
    public class MessagePacket : GCResponse
    {
        public override uint ID { get { return PacketID.SendMessage; } }
        MessageModel Message;

        public override bool Read(ISocketPacket Packet)
        {
            Message = new MessageModel();
            Message.ReadPacket(Packet);

            LoggerFactory.GetLogger(this).LogInfo("User {0} with access {1} on {2} sends a {3} message: {4}", Message.Username, Message.Access, Message.SentTime, Message.Type, Message.Content);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            int Total = 0;
            if (Client.CanSendMessage(Message.Type, ref Total))
            {
                Client.LastChatTime = DateTime.Now;
                switch (Message.Type)
                {
                    case MessageType.Chat:
                        ChatManager.SendToAll(Message, DistanceControl);
                        break;
                    case MessageType.Shout:
                        ChatManager.SendToAll(Message);
                        break;
                }
            }
            else
            {
                var Message = new MessageModel();
                Message.Type = MessageType.System;
                Message.Content = "LM:Messages.Chat Interval";

                ChatManager.SendToClient(Client, Message, Total);
            }
        }

        bool DistanceControl(GameClient Client)
        {
            return
                Client.CurrentMap != null &&
                Client.CurrentMap.ID == this.Client.CurrentMap.ID &&
                Client.CurrentCharacter.Position.Distance(this.Client.CurrentCharacter.Position) < GameConfiguration.ChatDistance;
        }
    }
}
