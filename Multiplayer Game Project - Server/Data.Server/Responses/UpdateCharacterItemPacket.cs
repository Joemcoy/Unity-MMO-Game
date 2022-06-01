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
    public class UpdateCharacterItemPacket : DCResponse
    {
        CharacterItemModel Item;

        public override uint ID { get { return PacketID.DataUpdateCharacterItem; } }
        public override bool Read(ISocketPacket Packet)
        {
            Item = new CharacterItemModel();
            Item.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            CharacterItemManager.UpdateItem(Item);
        }
    }
}
