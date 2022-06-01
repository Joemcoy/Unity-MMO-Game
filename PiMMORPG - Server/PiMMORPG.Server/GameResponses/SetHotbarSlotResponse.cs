using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using Manager;

    public class SetHotbarSlotResponse : PiGameResponse
    {
        public override ushort ID => PacketID.SetHotbarSlot;

        Guid serial;
        int slot;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            slot = packet.ReadInt();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            ItemManager.SetItemHotbarSlot(Client, serial, slot);
        }
    }
}