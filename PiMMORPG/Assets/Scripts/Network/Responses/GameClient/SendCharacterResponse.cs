using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;

using UnityEngine;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.UI.Helpers;
    using Local.Bundles;
    using Requests.GameClient;

    public class SendCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.SendCharacter; } }

        bool result;
        Character player;

        public override bool Read(IDataPacket Packet)
        {
            result = Packet.ReadBool();

            if (result)
            {
                player = new Character();
                player.ReadPacket(Packet);
                player.Items = Packet.ReadWrappers<CharacterItem>();
            }
            return true;
        }

        public override void Execute()
        {
            if (result)
            {
                Client.Character = player;

                GameObject.FindObjectOfType<CharacterHelper>().Despawn();
                BundleLoader.LoadScene(player.Map.SceneName, () => Socket.Send(new SendMapDataRequest()), false);
            }
            else
            {
                var view = GameObject.FindObjectOfType<CharacterSelectionView>();
                view.Message.Value = "O personagem selecionado já está online!";
            }
        }
    }
}