using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using General;
    using Requests;
    using Models;

    public class SetEquipStateResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.SetEquipState;

        Guid serial;
        bool equipped;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            equipped = packet.ReadBool();
            return true;
        }

        protected CharacterItem item;
        public override void Execute()
        {
            item = Client.Character.Items.First(i => i.Serial == serial);
            item.Equipped = equipped;

            LoggerFactory.GetLogger(this).LogSuccess("Player {0} {1} item {2}", Client.Character.Name, item.Equipped ? "equipped" : "unequipped", item.ID);
            var server = ServerControl.GetServer(Socket.Server.EndPoint.Port);

            var packet = new SetEquipStateRequest
            {
                Equip = item,
                OwnerID = Client.Character.ID
            };
            foreach (var client in server.Clients.Where(c => c.CanSpawn(Client, true)))// || c.Equals(Client)))
            {
                packet.ToOwner = client.Equals(Client);
                client.Socket.Send(packet);
            }
        }
    }
}