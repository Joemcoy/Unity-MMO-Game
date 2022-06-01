using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Data.Client;
using Base.Factories;

using Game.Data.Information;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Data.Models;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class SetWeaponPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SetWeapon; } }
        int Index;

        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Index = Packet.ReadInt();
                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            Client.CurrentCharacter.CurrentSlot = Index;

            foreach (GameClient RemoteClient in WorldManager.GetPlayersInMap(Client.CurrentMap.ID))
            {
                SetWeaponWriter Packet = new SetWeaponWriter();
                Packet.CID = Client.CurrentCharacter.ID;
                Packet.Index = Index;

                RemoteClient.Socket.Send(Packet);
            }
        }
    }
}
