using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Gate.Client;
using Game.Data.Models;
using Chat.Client;
using Base.Factories;
using Chat.Server.Writers;

namespace Chat.Server.GateResponses
{
    public class ServerMessagePacket : GTResponse
    {
        string Sender, Message;

        public override uint ID { get { return PacketID.ServerMessage; } }
        public override bool Read(ISocketPacket Packet)
        {
            Sender = Packet.ReadString();
            Message = Packet.ReadString();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new MessageRequest();

            Packet.Message = new MessageModel();
            Packet.Message.Content = this.Message;
            Packet.Message.SentTime = DateTime.Now;
            Packet.Message.Username = Sender;
            Packet.Message.Access = Game.Data.Enums.AccessLevel.Server;
            Packet.Message.Type = Game.Data.Enums.MessageType.System;

            foreach (ChatClient Client in ChatServer.Clients)
            {
                Client.Socket.Send(Packet);
            }
        }
    }
}