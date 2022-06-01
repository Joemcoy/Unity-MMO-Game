using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Client;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Controller;
using Game.Data.Models;
using Game.Data.Results;
using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class RegisterPacket : DCResponse
    {
        string Username, Password, Nickname, Email;
        uint Server;

        public override uint ID { get { return PacketID.DataSendRegister; } }
        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            Password = Packet.ReadString();
            Nickname = Packet.ReadString();
            Email = Packet.ReadString();
            Server = Packet.ReadUInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new RegisterRequest();

            if (AccountManager.CheckAccount(Username, Nickname, Email, Server))
            {
                Packet.Result = RegisterResult.InvalidRegister;
            }
            else
            {
                AccountManager.Register(Username, Password, Nickname, Email, Server);
                Packet.Result = RegisterResult.Success;
            }

            Client.Socket.Send(Packet);
        }
    }
}