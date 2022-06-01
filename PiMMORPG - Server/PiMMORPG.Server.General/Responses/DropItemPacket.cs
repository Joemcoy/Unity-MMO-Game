using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Models;

    public abstract class DropItemPacket : PiBaseResponse
    {
        public override ushort ID => PacketID.DropItem;

        protected Drop drop;
        public override bool Read(IDataPacket packet)
        {
            drop = packet.ReadWrapper<Drop>();
            drop.Map = Client.Character.Map;
            return true;
        }
    }
}