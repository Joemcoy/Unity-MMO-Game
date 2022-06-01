using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;

using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;
    public class CreateCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.CreateCharacter; } }

        bool Result = false;
        Character[] characters;

        public override bool Read(IDataPacket packet)
        {
            Result = packet.ReadBool();

            if (Result)
            {
                characters = new Character[packet.ReadInt()];
                for (int i = 0; i < characters.Length; i++)
                {
                    (characters[i] = new Character()).ReadPacket(packet);
                }
            }
            return true;
        }

        public override void Execute()
        {
            var view = GameObject.FindObjectOfType<CreateCharacterView>();
            view.CanBack.Value = true;
            view.CanCreate.Value = true;

            if (Result)
            {
                WorldControl.RemovePlayer(0);
                Client.Characters = characters;

                view.Message.Value = "Personagem criado, carregando lista de personagens...";
                var select = view.transform.parent.gameObject.GetComponentInChildren<CharacterSelectionView>(true);
                select.UpdateCharacters(characters, 100);
            }
            else
                view.Message.Value = "O nome do personagem já está sendo utilizado!";
        }
    }
}