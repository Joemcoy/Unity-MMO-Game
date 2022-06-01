using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Client;
using Network.Data;using Network.Data.Interfaces;
using Game.Data;
using Data.Client;
using Base.Factories;

namespace Game.Server.Responses
{
    public class DeleteCharacterPacket : GCResponse
    {
        int CID;

        public override uint ID { get { return PacketID.DeleteCharacter; } }
        public override bool Read(ISocketPacket Packet)
        {
            CID = Packet.ReadInt();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            DataClient Client = SingletonFactory.GetInstance<DataClient>();

            Client.SendDeleteCharacterRequest(this.Client, CID);
        }
    }
}
