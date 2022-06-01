using Data.Client;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Data.Server.Responses
{
    public class UpdateAccountPacket : DCResponse
    {
        AccountModel Account;

        public override uint ID { get { return PacketID.DataUpdateAccount; } }
        public override bool Read(ISocketPacket Packet)
        {
            Account = new AccountModel();
            Account.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            AccountManager.UpdateAccount(Account);
        }
    }
}
