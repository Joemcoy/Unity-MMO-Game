using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;

    public class DropItemResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.DropItem; } }
        Drop drop;

        public override bool Read(IDataPacket packet)
        {
            drop = packet.ReadWrapper<Drop>();
            return drop != null;
        }

        public override void Execute()
        {
            WorldControl.SpawnDrop(drop);
        }
    }
}
