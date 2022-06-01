using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.RPG.Responses
{
    using Client.RPG;
    using Manager;

    public class SetItemSlotResponse : PiRPGResponse
    {
        public override ushort ID => PacketID.SetItemSlot;

        Guid serial;
        uint slot;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            slot = packet.ReadUInt();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            ItemManager.SetItemSlot(Client, serial, slot);
        }
    }
}