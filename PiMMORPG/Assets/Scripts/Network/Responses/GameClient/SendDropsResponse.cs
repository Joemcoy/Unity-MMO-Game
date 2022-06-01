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
//    using Local.Control;

//    public class SendDropsResponse : PiRPGResponse
//    {
//        public override ushort ID { get { return PacketID.SendDrops; } }

//        Drop[] drops;
//        public override bool Read(IDataPacket packet)
//        {
//            drops = packet.ReadWrappers<Drop>();
//            return true;
//        }

//        public override void Execute()
//        {
//            foreach (var drop in drops)
//                WorldControl.SpawnDrop(drop);
//            Client.Socket.Send(new SendPlayersRequest());
//        }
//    }
//}
