using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Client;
using Data.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data;using Network.Data.Interfaces;
using Game.Data.Enums;
using Base.Factories;

namespace Game.Server.DataResponses
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
                Account.ReadPacket(Packet);
            }
            else
            {
                LoggerFactory.GetLogger(this).LogWarning("Failed to get account to client!");
                return false;
            }
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogInfo("Account {0} from {1} has been received!", Account.Nickname, Account.Username);

            GameClient Client = this.Client.Dequeue<GameClient>();

            Client.Account = Account;
            this.Client.SendCharacterListRequest(Client, Client.Account.ID);
        }
    }
}