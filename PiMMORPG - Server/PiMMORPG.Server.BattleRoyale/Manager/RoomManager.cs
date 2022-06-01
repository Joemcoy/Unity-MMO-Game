using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.BattleRoyale.Manager
{
    using Client.BattleRoyale;
    using Client.BattleRoyale.Enums;

    public class RoomManager : ISingleton
    {
        Dictionary<Guid, Room> rooms;
        void ISingleton.Created()
        {
            rooms = new Dictionary<Guid, Room>();
        }

        void ISingleton.Destroyed()
        {

        }

        public static Room GetFreeRoom(PiBRClient client)
        {
            var manager = SingletonFactory.GetSingleton<RoomManager>();

            Room free = null;
            foreach(var room in manager.rooms.Values)
            {
                if (room.State == RoomState.WaitingForPlayer || room.State == RoomState.WaitingTimeout)
                {
                    free = room;
                    break;
                }
            }

            if (free == null)
            {
                free = new Room(client);
                manager.rooms.Add(free.ID, free);
            }
            else
                free.AddClient(client);

            return free;
        }

        public static Room GetRoomByID(Guid ID)
        {
            var manager = SingletonFactory.GetSingleton<RoomManager>();
            Room room = null;

            return manager.rooms.TryGetValue(ID, out room) ? room : null;
        }

        public static void RemoveRoom(Room room)
        {
            var manager = SingletonFactory.GetSingleton<RoomManager>();
            if (manager.rooms.ContainsKey(room.ID))
            {
                ThreadFactory.Stop(room);
                manager.rooms.Remove(room.ID);
            }
        }
    }
}