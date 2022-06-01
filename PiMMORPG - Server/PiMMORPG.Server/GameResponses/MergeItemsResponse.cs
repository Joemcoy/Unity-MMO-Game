using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using Manager;

    public class MergeItemsResponse : PiGameResponse
    {
        public override ushort ID => PacketID.MergeItems;

        Guid from, to;
        uint quantity;
        public override bool Read(IDataPacket packet)
        {
            from = packet.ReadGuid();
            to = packet.ReadGuid();
            quantity = packet.ReadUInt();
            return from != Guid.Empty && to != Guid.Empty;
        }

        public override void Execute()
        {
            ItemManager.MergeItems(Client, from, to, quantity);
        }
    }
}