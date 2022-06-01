using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chat.Client;

using Game.Data;
using Network.Data;using Network.Data.Interfaces;
using Data.Client;
using Base.Factories;

namespace Chat.Server.ChatResponses
{
    public class SendAccountPacket : CCResponse
    {
        int AccountID;

        public override uint ID { get { return PacketID.SendAccount; } }
        public override bool Read(ISocketPacket Packet)
        {
            AccountID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            DataClient Data = SingletonFactory.GetInstance<DataClient>();
            Data.SendAccountRequest(Client, AccountID);
        }
    }
}
