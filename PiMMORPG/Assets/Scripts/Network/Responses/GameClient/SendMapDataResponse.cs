using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;

using tFramework.Extensions;
using tFramework.Network.Interfaces;

using Devdog.InventoryPro;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Inventory;
    using Local.Control;
    using Local.UI;

    public class SendMapDataResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.RequestMapData; } }

        Character[] characters;
        Drop[] drops;
        Tree[] trees;

        public override bool Read(IDataPacket packet)
        {
            var t = packet.ReadInt();

            characters = new Character[t];
            for (var i = 0; i < t; i++)
            {
                var c = characters[i] = packet.ReadWrapper<Character>();
                c.Items = packet.ReadWrappers<CharacterItem>();
            }

            drops = packet.ReadWrappers<Drop>();
            trees = packet.ReadWrappers<Tree>();
            return true;
        }

        public override void Execute()
        {
            foreach (var character in characters)
                WorldControl.SpawnPlayer(character, false);

            drops.ForEach(WorldControl.SpawnDrop);
            trees.ForEach(WorldControl.SpawnTree);

            WorldControl.SpawnPlayer(Client.Character, true);
            var helper = InventoryHelper.Instance;

            LoadingScreen.Instance.Dimiss();
        }
    }
}