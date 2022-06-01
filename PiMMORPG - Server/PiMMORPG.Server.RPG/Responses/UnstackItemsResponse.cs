using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.RPG.Responses
{
    using Client.RPG;
    using Models;
    using Manager;

    public class UnstackItemsResponse : PiRPGResponse
    {
        public override ushort ID => PacketID.UnstackItems;

        Guid from, to;
        uint fromSlot, toSlot, quantity;
        public override bool Read(IDataPacket packet)
        {
            from = packet.ReadGuid();
            fromSlot = packet.ReadUInt();
            to = packet.ReadGuid();
            toSlot = packet.ReadUInt();
            quantity = packet.ReadUInt();
            return true;
        }

        public override void Execute()
        {
            ItemManager.UnstackItem(Client, from, fromSlot, to, toSlot, quantity);
        }
    }
}