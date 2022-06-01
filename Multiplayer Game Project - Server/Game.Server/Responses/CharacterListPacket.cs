using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Data.Client;
using Base.Factories;


namespace Game.Server.Responses
{
    public class CharacterListPacket : GCResponse
    {
        string Username;
        int AccountID;

        public override uint ID { get { return PacketID.CharacterList; } }
        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            AccountID = Packet.ReadInt();

            LoggerFactory.GetLogger(this).LogInfo("{0} {1}", Username, AccountID);
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            DataClient Client = SingletonFactory.GetInstance<DataClient>();
            Client.SendAccountRequest(this.Client, AccountID);

            LoggerFactory.GetLogger(this).LogInfo("Sending account request...");
        }
    }
}
