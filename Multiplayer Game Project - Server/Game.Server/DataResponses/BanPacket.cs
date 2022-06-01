using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

using Game.Client;
using Data.Client;
using Game.Server.Writers;
using Game.Data.Models;
using Game.Data.Enums;
using Game.Server.Manager;

namespace Game.Server.DataResponses
{
    public class BanPacket : DCResponse
    {
        bool Banned;
        public override uint ID { get { return PacketID.DataBan; } }

        public override bool Read(ISocketPacket Packet)
        {
            Banned = Packet.ReadBool();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameClient Client = this.Client.Dequeue<GameClient>();

            var Message = new MessageModel();
            Message.Type = MessageType.System;
            Message.Content = "LM:" + (Banned ? "Messages.PlayerBanned" : "Messages.PlayerNotFound");
            Message.Access = AccessLevel.Server;

            ChatManager.SendToClient(Client, Message);
        }
    }
}
