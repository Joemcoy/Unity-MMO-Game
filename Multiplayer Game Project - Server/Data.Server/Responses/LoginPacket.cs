using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Data.Results;

using Game.Controller;
using Data.Client;
using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class LoginPacket : DCResponse
    {
        string Username, Password;
        uint Server;

        public override uint ID { get { return PacketID.DataSendLogin; } }
        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            Password = Packet.ReadString();
            Server = Packet.ReadUInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new LoginRequest();

            AccountModel Account = AccountManager.Login(Username, Password, Server);
            if (Account == null)
                Packet.Result = LoginResult.InvalidUsername;
            else if (Account.Password != Password)
                Packet.Result = LoginResult.InvalidPassword;
            else if (Account.IsBanned)
                Packet.Result = LoginResult.AccountBanned;
            else
            {
                Packet.Result = LoginResult.Success;
                Packet.Account = Account;

                AccountManager.IncrementLoginCount(Account.ID);
            }

            Client.Socket.Send(Packet);
        }
    }
}