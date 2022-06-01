//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PiMMORPG.Server.General.Requests
//{
//    using Client;
//    using Models;
//    using tFramework.Network.Interfaces;

//    public class SendTreesRequest : PiBaseRequest
//    {
//        public Tree[] Trees { get; set; }
//        public override ushort ID { get { return PacketID.SendTrees; } }

//        public override bool Write(IDataPacket packet)
//        {
//            packet.WriteWrappers(Trees);
//            return true;
//        }
//    }
//}