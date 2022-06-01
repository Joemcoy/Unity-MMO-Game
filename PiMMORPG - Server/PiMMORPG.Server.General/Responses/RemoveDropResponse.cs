using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Models;
    using General;
    using General.Drivers;
    using Requests;

    public class RemoveDropResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.RemoveDrop;
        protected Drop drop;
        Guid serial;

        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            drop = null;
            using (var ctx = new DropDriver())
            {
                var packet = new RemoveDropRequest { Serial = serial };

                var server = ServerControl.GetServer(Socket.Server.EndPoint.Port);
                foreach (var client in server.Clients.Where(c => !c.SwitchingMap && c.Character != null && c.Character.Map.ID == Client.Character.Map.ID))
                    client.Socket.Send(packet);

                drop = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                ctx.RemoveModel(drop);
            }
        }
    }
}
