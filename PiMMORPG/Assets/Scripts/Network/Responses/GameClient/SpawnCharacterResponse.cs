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
    using Local.Locomotion;
    using Scripts.Local.Triggers;
    using Scripts.Local.UI;

    public class SpawnCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.SpawnCharacter; } }

        Character Player;
        public override bool Read(IDataPacket Packet)
        {
            Player = Packet.ReadWrapper<Character>();
            Player.Items = Packet.ReadWrappers<CharacterItem>();

            return true;
        }

        public override void Execute()
        {
            WorldControl.SpawnPlayer(Player, false);
        }
    }
}