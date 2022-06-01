using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class SendTreesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendTrees; } }
        public int Server { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            var Trees = TreeManager.GetAllTrees();

            Packet.WriteInt(Trees.Length);
            foreach (var Tree in Trees)
            {
                Tree.WritePacket(Packet);

                var Spawns = TreeManager.GetSpawnsByTree(Tree.ID, Server);

                Packet.WriteInt(Spawns.Length);
                foreach (var Spawn in Spawns)
                    Spawn.WritePacket(Packet);
            }

            return true;
        }
    }
}
