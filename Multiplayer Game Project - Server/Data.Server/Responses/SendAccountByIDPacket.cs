using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Data.Client;
using Game.Data.Models;
using Game.Controller;
using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class SendAccountByIDPacket : DCResponse
    {
        int AID;
        public override uint ID { get { return PacketID.DataSendAccountByID; } }
        public override bool Read(ISocketPacket Packet)
        {
            AID = Packet.ReadInt();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new AccountRequest();
            Packet.Account = AccountManager.GetAccountByID(AID);

            Client.Socket.Send(Packet);
        }
    }
}