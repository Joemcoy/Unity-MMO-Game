//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using tFramework.Network.Interfaces;
//namespace PiMMORPG.Server.GameResponses
//{
//    using Client;
//    using Drivers;
//    using GameRequests;

//    public class SendTreesResponse : PiGameResponse
//    {
//        public override ushort ID => PacketID.SendTrees;

//        public override bool Read(IDataPacket packet) { return true; }
//        public override void Execute()
//        {
//            var packet = new SendTreesRequest();
//            using (var ctx = new TreeDriver())
//                packet.Trees = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));
//            Socket.Send(packet);
//        }
//    }
//}
