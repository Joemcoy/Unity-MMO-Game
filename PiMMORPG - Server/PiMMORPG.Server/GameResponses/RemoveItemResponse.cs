using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using Manager;

    public class RemoveItemResponse : PiGameResponse
    {
        public override ushort ID => PacketID.RemoveItem;

        Guid serial;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            ItemManager.RemoveItem(Client, serial);
        }
    }
}