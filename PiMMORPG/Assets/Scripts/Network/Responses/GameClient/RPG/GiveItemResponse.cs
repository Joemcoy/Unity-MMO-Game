using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client.RPG;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;
    using Local.Triggers;

    public class GiveItemResponse : PiRPGResponse
    {
        public override ushort ID { get { return PacketID.GiveItem; } }
        CharacterItem item;

        public override bool Read(IDataPacket packet)
        {
            item = packet.ReadWrapper<CharacterItem>();
            return item != null;
        }

        public override void Execute()
        {
            var player = WorldControl.GetPlayer(Client.Character);
            var morph = player.GetComponent<MorphEquipTrigger>();

            WorldControl.AddItem(morph, item, true);
        }
    }
}
