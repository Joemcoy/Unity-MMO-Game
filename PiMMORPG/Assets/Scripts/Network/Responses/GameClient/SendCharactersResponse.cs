using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

using UnityEngine;

namespace Scripts.Network.Responses.GameClient
{
    using Requests.GameClient;
    using Scripts.Local.UI;

    public class SendCharactersResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.SendCharacters; } }

        Character[] characters;
        public override bool Read(IDataPacket packet)
        {
            var t = packet.ReadInt();

            characters = new Character[t];
            for (int i = 0; i < t; i++)
            {
                var c = characters[i] = packet.ReadWrapper<Character>();
                c.Items = packet.ReadWrappers<CharacterItem>();
            }
            return true;
        }

        public override void Execute()
        {
            Client.Characters = characters;

            var menu = GameObject.FindObjectOfType<MainMenu>();
            var sel = menu.transform.parent.gameObject.GetComponentInChildren<CharacterSelectionView>(true);

            sel.UpdateCharacters(characters, 5);
        }
    }
}