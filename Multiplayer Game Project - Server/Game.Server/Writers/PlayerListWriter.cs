using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Server.Configuration;

using Network.Data;
using Network.Data.Interfaces;
using Game.Server.Manager;
using Base.Factories;

namespace Game.Server.Writers
{
    public class PlayerListWriter : IRequest
    {
        public MapModel Map { get; set; }
        public PositionModel Spawn { get; set; }
        public WorldItemModel[] Items { get; set; }

        public int CID { get; set; }

        public uint ID { get { return PacketID.PlayerList; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            if (Map == null)
            {
                Packet.WriteBool(false);
            }
            else
            {
                Packet.WriteBool(true);
                Map.WritePacket(Packet);

                CharacterModel[] CharactersInMap = WorldManager.GetPlayersInMap(Map.ID).Select(C => C.CurrentCharacter).OrderBy(C => C.ID != CID).ToArray();

                Packet.WriteInt(CharactersInMap.Length);
                foreach (CharacterModel Character in CharactersInMap)
                {
                    Character.WritePacket(Packet);
                }

                Packet.WriteInt(Items.Length);
                foreach (WorldItemModel WorldItem in Items)
                {
                    WorldItem.WritePacket(Packet);
                }

                /*Packet.WriteInt(Mobs.Length);
                foreach (var Mob in Mobs)
                    Mob.WritePacket(Packet);

                Packet.WriteInt(Spawns.Length);
                foreach (var Spawn in Spawns)
                    Spawn.WritePacket(Packet);*/                    
            }
            return true;
        }
    }
}