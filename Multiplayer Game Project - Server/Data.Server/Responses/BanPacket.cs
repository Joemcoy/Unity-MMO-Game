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
    public class BackPacket : DCResponse
    {
        string Name;
        int Type;

        public override uint ID { get { return PacketID.DataBan; } }
        public override bool Read(ISocketPacket Packet)
        {
            Name = Packet.ReadString();
            Type = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new BanRequest();
            Packet.Result = false;

            AccountModel Target = null;

            if (Type < 3)
            {
                Target = AccountManager.GetAccount(Condition(Type));
            }
            else if(Type == 4)
            {
                var C = CharacterManager.GetCharacterByName(Name);
                if (C != null)
                    Target = AccountManager.GetAccountByID(C.AID);
            }

            if(Target != null)
            {
                Target.IsBanned = true;
                AccountManager.UpdateAccount(Target);

                Packet.Result = true;
            }
            Client.Socket.Send(Packet);
        }

        Predicate<AccountModel> Condition(int Type)
        {
            switch(Type)
            {
                case 0:
                    return A => A.Username == Name;
                case 1:
                    return A => A.Nickname == Name;
                case 2:
                    return A => A.Email == Name;
            }
            return null;
        }
    }
}