using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Client;
using Game.Data.Models;
using Base.Data.Abstracts;
using Base.Factories;
using Game.Data.Enums;
using Game.Server.Writers;

namespace Game.Server.Manager
{
    public class ChatManager : ASingleton<ChatManager>
    {
        GameServer Server;

        protected override void Created()
        {
            Server = SingletonFactory.GetInstance<GameServer>();
        }

        protected override void Destroyed()
        {

        }

        public static void SendToAllInMap(int MapID, MessageModel Message, params object[] Arguments)
        {
            SendToAll(Message, C => C.CurrentMap != null && C.CurrentMap.ID == MapID, Arguments);
        }

        public static void SendToAll(MessageModel Message, params object[] Arguments)
        {
            SendToAll(Message, null, Arguments);
        }

        public static void SendToAll(MessageModel Message, Func<GameClient, bool> Conditional, params object[] Arguments)
        {
            foreach (var Client in Instance.Server.Clients.Where(C => Conditional == null ? C.CurrentCharacter != null : Conditional(C)))
            {
                SendToClient(Client, Message, Arguments);
            }
        }

        public static void SendToClient(GameClient Client, MessageModel Message, params object[] Arguments)
        {
            var Packet = new MessageWriter();
            Packet.Message = Message;
            Packet.Message.Arguments = string.Join(";", Arguments.Select(O => O.ToString()).ToArray());

            if (Message.Username == null)
                Message.Username = "SERVER";

            Client.Socket.Send(Packet);
        }
    }
}