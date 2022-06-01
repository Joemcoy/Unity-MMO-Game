//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using PiMMORPG;
//using PiMMORPG.Client;
//using PiMMORPG.Models;
//using Tree = PiMMORPG.Models.Tree;

//using UnityEngine;
//using tFramework.Network.Interfaces;

//namespace Scripts.Network.Responses.GameClient
//{
//    using Requests.GameClient;
//    using Local.Bundles;
//    using Local.Control;

//    public class SendTreesResponse : PiRPGResponse
//    {
//        public override ushort ID { get { return PacketID.SendTrees; } }

//        Tree[] trees;
//        public override bool Read(IDataPacket packet)
//        {
//            trees = packet.ReadWrappers<Tree>();
//            return true;
//        }

//        public override void Execute()
//        {
//            foreach(var tree in trees)
//                WorldControl.SpawnTree(tree);
//            Client.Socket.Send(new SendDropsRequest());
//        }
//    }
//}
        