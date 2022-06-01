using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using Models;
    using Manager;
    using General.Drivers;

    public class RemoveDropResponse : PiGameResponse
    {
        public override ushort ID => PacketID.RemoveDrop;
        Guid serial;

        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            Drop drop = null;
            using (var ctx = new DropDriver())
            {
                var packet = new RemoveDropRequest { Serial = serial };
                var channels = SingletonFactory.GetSingleton<PiServer>().Channels;
                var gameServer = channels.First(s => s.Channel.Port == Socket.Server.EndPoint.Port);

                foreach (var client in gameServer.Clients.Where(c => !c.SwitchingMap && c.Character != null && c.Character.Map.ID == Client.Character.Map.ID))
                    client.Socket.Send(packet);

                drop = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                ctx.RemoveModel(drop);
            }

            ItemManager.AddItem(Client, drop.InventoryID, drop.Serial, drop.Quantity);
        }
    }
}
