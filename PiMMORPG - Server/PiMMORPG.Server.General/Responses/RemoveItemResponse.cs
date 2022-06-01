using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.General.Responses
{
    using Client;

    public abstract class RemoveItemResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.RemoveItem;

        protected Guid serial;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            return serial != Guid.Empty;
        }
    }
}