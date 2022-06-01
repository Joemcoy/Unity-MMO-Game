//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using tFramework.Network.Interfaces;
//namespace PiMMORPG.Server.GameResponses
//{
//    using Client;
//    using Models;
//    using Drivers;
//    using GameRequests;
//    using tFramework.Factories;

//    public class SendDropsResponse : PiGameResponse
//    {
//        public override ushort ID => PacketID.SendDrops;

//        public override bool Read(IDataPacket packet)
//        {
//            return true;
//        }

//        public override void Execute()
//        {
//            using (var ctx = new DropDriver())
//            {
//                var packet = new SendDropsRequest();
//                packet.Drops = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));

//                Socket.Send(packet);
//            }

//            var spawn = new SpawnCharacterRequest { Character = Client.Character };

//            /*var server = SingletonFactory.GetSingleton<PiServer>();
//            var gameServer = server.Channels.First(c => c.Channel.Port == Socket.Server.EndPoint.Port);
//            foreach (var client in gameServer.Clients)//.Where(c => c.CanSpawn(Client)))
//            {
//                var cs = client.CanSpawn(Client, false);
//                if(cs)
//                    client.Socket.Send(spawn);
//            }*/
//        }
//    }
//}