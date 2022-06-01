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

using Data.Server.Writers;

namespace Data.Server.Responses
{
    class SendCharacterItemsPacket : DCResponse
    {
        int CharacterID;

        public override uint ID { get { return PacketID.DataSendCharacterItems; } }
        public override bool Read(ISocketPacket Packet)
        {
            CharacterID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new CharacterItemsRequest();
            Packet.CharacterID = CharacterID;
            Packet.Items = CharacterItemManager.GetItemsByOwner(CharacterID);

            Client.Socket.Send(Packet);
        }
    }
}
