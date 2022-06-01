using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chat.Client;
using Data.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data;using Network.Data.Interfaces;
using Game.Data.Enums;
using Base.Factories;

namespace Chat.Server.DataResponses
{
    public class SendAccountByIDPacket : DCResponse
    {
        AccountModel Account;

        public override uint ID { get { return PacketID.DataSendAccountByID; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Packet.ReadBool())
            {
                Account = new AccountModel();
                Account.ID = Packet.ReadInt();
                Account.Username = Packet.ReadString();
                Account.Password = Packet.ReadString();
                Account.Nickname = Packet.ReadString();
                Account.Cash = Packet.ReadInt();
                Account.Access = Packet.ReadEnum<AccessLevel>();
                Account.LoginCount = Packet.ReadInt();
                Account.RegisterDate = new DateTime(Packet.ReadLong());
                return true;
            }
            else
            {
                LoggerFactory.GetLogger(this).LogWarning("Failed to get account to client!");
                return false;
            }
        }

        public override void Execute(IClientSocket Socket)
        {
            ChatClient Client = this.Client.Dequeue<ChatClient>();
            int GatePort = SingletonFactory.GetInstance<ChatServer>().Server.EndPoint.Port;

            Client.Account = Account;
        }
    }
}