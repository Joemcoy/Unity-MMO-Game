using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class SpawnTreeWriter : IRequest
    {
        public uint ID { get { return PacketID.SpawnTree; } }

        public bool End { get; set; }
        public TreeModel Tree { get; set; }
        public TreePositionModel[] Spawns { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteBool(End);
            //Tree.WritePacket(Packet);

            if (!End)
            {
                Packet.WriteInt(Spawns.Length);
                foreach (var Spawn in Spawns)
                    Spawn.WritePacket(Packet);
            }
            return true;
        }
    }
}