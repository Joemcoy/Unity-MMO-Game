using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Data.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Responses
{
    public class AddItemPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataAddItem; } }

        CharacterItemModel Item;
        public override bool Read(ISocketPacket Packet)
        {
            Item = new CharacterItemModel();
            Item.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Base = ControllerFactory.GetBaseController("character_items");
            Base.AddModel(Item);
        }
    }
}
