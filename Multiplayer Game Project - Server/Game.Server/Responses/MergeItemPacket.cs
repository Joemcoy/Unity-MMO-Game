using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Game.Data;
using Game.Data.Models;

using Game.Client;
using Game.Server.Manager;

using Network.Data.Interfaces;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class MergeItemPacket : GCResponse
    {
        public override uint ID { get { return PacketID.MergeItem; } }

        uint Amount;
        Guid From, To;

        public override bool Read(ISocketPacket Packet)
        {
            From = Packet.ReadGuid();
            To = Packet.ReadGuid();
            Amount = Packet.ReadUInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();
            Manager.MergeItem(Client.CurrentCharacter.ID, From, To, Amount);
        }
    }
}