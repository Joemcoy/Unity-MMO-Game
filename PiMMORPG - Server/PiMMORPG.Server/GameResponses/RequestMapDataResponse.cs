using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using PiMMORPG.Server.Drivers;
    using PiMMORPG.Server.GameRequests;

    public class RequestMapDataResponse : PiGameResponse
    {
        public override ushort ID => PacketID.RequestMapData;

        public override bool Read(IDataPacket packet)
        {
            return true;
        }

        public override void Execute()
        {
            Client.SwitchingMap = false;

            var server = SingletonFactory.GetSingleton<PiServer>();
            var gameServer = server.Channels.FirstOrDefault(s => s.Socket.EndPoint.Port == Client.Socket.Server.EndPoint.Port);

            var packet = new MapDataRequest();
            packet.Characters = gameServer.Clients.Where(c => Client.CanSpawn(c, true)).Select(c => c.Character).ToArray();
            using (var ctx = new DropDriver())
                packet.Drops = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));
            using (var ctx = new TreeDriver())
                packet.Trees = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));
            Socket.Send(packet);

            var request = new SpawnCharacterRequest { Character = Client.Character };
            gameServer.SendToAll(request, c => c.CanSpawn(Client, false));
        }
    }
}