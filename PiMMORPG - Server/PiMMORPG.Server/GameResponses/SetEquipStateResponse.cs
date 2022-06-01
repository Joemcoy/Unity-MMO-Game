using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using GameRequests;

    public class SetEquipStateResponse : PiRPGResponse
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

        public override void Execute()
        {
            var item = Client.Character.Items.First(i => i.Serial == serial);
            item.Equipped = equipped;

            LoggerFactory.GetLogger(this).LogSuccess("Player {0} {1} item {2}", Client.Character.Name, item.Equipped ? "equipped" : "unequipped", item.ID);
            var channels = SingletonFactory.GetSingleton<PiServer>().Channels;
            var gameServer = channels.First(s => s.Channel.Port == Socket.Server.EndPoint.Port);

            var packet = new SetEquipStateRequest
            {
                Equip = item,
                OwnerID = Client.Character.ID
            };
            foreach (var client in gameServer.Clients.Where(c => c.CanSpawn(Client, true)))// || c.Equals(Client)))
            {
                packet.ToOwner = client.Equals(Client);
                client.Socket.Send(packet);
            }
        }
    }
}