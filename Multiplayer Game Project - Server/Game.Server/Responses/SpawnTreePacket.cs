using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;
using Game.Client;
using Game.Data;
using Game.Data.Models;
using Game.Server.Writers;
using Game.Server.Manager;

namespace Game.Server.Responses
{
    public class SpawnTreesPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SpawnTree; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new SpawnTreeWriter();
            var Spawns = WorldManager.GetTreeSpawns(Client.CurrentMap.ID);

            int Total = Spawns.Length;
            for(int i = 0; i < Total; i += 100)
            {
                Packet.Spawns = Spawns.Skip(i).Take(100).ToArray();
                Packet.End = false;
                Socket.Send(Packet);
            }

            Packet.End = true;
            Socket.Send(Packet);
        }
    }
}