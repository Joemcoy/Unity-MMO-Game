using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.RPG.Responses
{
    using Client.RPG;
    using Manager;

    public class SetItemQuantityResponse : PiRPGResponse
    {
        public override ushort ID => PacketID.SetItemQuantity;

        Guid serial;
        uint quantity;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            quantity = packet.ReadUInt();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            ItemManager.SetItemQuantity(Client, serial, quantity);
        }
    }
}