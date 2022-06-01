//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PiMMORPG.Server.General.Requests
//{
//    using Client;
//    using Models;
//    using tFramework.Network.Interfaces;

//    public class SendDropsRequest : PiBaseRequest
//    {
//        public override ushort ID => PacketID.SendDrops;
//        public Drop[] Drops { get; set; }

//        public override bool Write(IDataPacket packet)
//        {
//            packet.WriteWrappers(Drops);
//            return true;
//        }
//    }
//}