using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Client;
using Data.Client;

using Game.Data;
using Network.Data;using Network.Data.Interfaces;
using Base.Factories;

namespace Game.Server.Responses
{
    public class SelectCharacterPacket : GCResponse
    {
        int CID;

        public override uint ID { get { return PacketID.SelectCharacter; } }
        public override bool Read(ISocketPacket Packet)
        {
            CID = Packet.ReadInt();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            Client.CurrentCharacter = Client.Characters.First(C => C.ID == CID);
            Client.Account.LastCharacter = CID;

            DataClient Data = SingletonFactory.GetInstance<DataClient>();
            Data.SendUpdateAccount(Client.Account);
            Data.SendMapRequest(Client, Client.CurrentCharacter.Position.MapID, true);

            LoggerFactory.GetLogger(this).LogInfo("Sending a map request for client {0}!", Socket.EndPoint);
        }
    }
}
