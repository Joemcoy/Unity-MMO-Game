using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Gate.Client;
using Game.Data.Models;
using Game.Server.Manager;

namespace Game.Server.GateResponses
{
    public class GlobalMessagePacket : GTResponse
    {

        public override uint ID { get { return PacketID.AuthClient; } }

        string Sender, Content;
        public override bool Read(ISocketPacket Packet)
        {
            Sender = Packet.ReadString();
            Content = Packet.ReadString();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Message = new MessageModel();
            Message.Username = Sender;
            Message.Content = Content;

            ChatManager.SendToAll(Message, C => C.CurrentCharacter != null);
        }
    }
}
