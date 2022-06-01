using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Factory;

using Base.Data;
using Base.Data.Interfaces;

using Socket.Data;
using Socket.Data.Interfaces;
using Game.Data.Models;
using Server.Data.Interfaces;
using Game.Client;
using Game.Manager.Writers;
using Data.Client;
using Gate.Client.Packets.Writers;

namespace Game.Manager
{
    public class ItemCacheManager : ISingleton, IUpdater, IComponent
    {
        private Dictionary<int, ItemModel> Items;
        private Dictionary<int, List<WorldItemModel>> WorldItems, AddedItems;
        private IGameServer Server;
        private DataClient Data;

        object syncLock;

        public int Interval { get { return 15 * 60 * 1000; } }        
        public void Initalize(params object[] Args)
        {
            syncLock = new object();
            Items = new Dictionary<int, ItemModel>();
            WorldItems = new Dictionary<int, List<WorldItemModel>>();
            AddedItems = new Dictionary<int, List<WorldItemModel>>();

            Server = SingletonFactory.GetSingleton<IGameServer>();
            Data = SingletonFactory.GetSingleton<DataClient>();
        }

        public void Destroy()
        {
            Items.Clear();
        }

        public void PutItem(ItemModel Model)
        {
            if (!Items.ContainsKey(Model.ID))
            {
                Items[Model.ID] = Model;
                LoggerFactory.GetLogger().LogInfo($"Creating cache for item {Model.ID}...");
            }
        }

        public void PutWorldItem(WorldItemModel Model)
        {
            if(!WorldItems.ContainsKey(Model.Position.MapID))
            {
                WorldItems[Model.Position.MapID] = new List<WorldItemModel>();
                LoggerFactory.GetLogger().LogInfo($"Creating cache for map {Model.Position.MapID}...");
            }

            WorldItems[Model.Position.MapID].Add(Model);
            LoggerFactory.GetLogger().LogInfo($"Creating cache for world item {Model.Item.Name} on map {Model.Position.MapID}...");
        }

        public void AddWorldItem(WorldItemModel Model)
        {
            if (!AddedItems.ContainsKey(Model.Position.MapID))
            {
                AddedItems[Model.Position.MapID] = new List<WorldItemModel>();
                LoggerFactory.GetLogger().LogInfo($"Creating cache for map {Model.Position.MapID}...");
            }

            AddedItems[Model.Position.MapID].Add(Model);
            LoggerFactory.GetLogger().LogInfo($"Creating cache for world item {Model.Item.Name} on map {Model.Position.MapID}...");
        }

        public ItemModel[] GetItems()
        {
            return Items.Values.ToArray();
        }

        public ItemModel GetItem(int ItemID)
        {
            return Items[ItemID];
        }

        public WorldItemModel[] GetWorldItems(int MapID)
        {
            lock (syncLock)
            {
                List<WorldItemModel> DBItems = new List<WorldItemModel>();
                if (WorldItems.ContainsKey(MapID))
                    DBItems.AddRange(WorldItems[MapID]);

                if (AddedItems.ContainsKey(MapID))
                    DBItems.AddRange(AddedItems[MapID]);
                return DBItems.ToArray();
            }
        }

        public void Update()
        {
            lock (syncLock)
            {
                foreach(int AddedItemID in AddedItems.Keys)
                {
                    List<WorldItemModel> Items = AddedItems[AddedItemID];
                    PutItemInWorldWriter Packet = new PutItemInWorldWriter();

                    foreach (WorldItemModel Item in Items)
                    {
                        Packet.Item = Item;

                        Data.Socket.Send(Packet);
                    }

                    if (!WorldItems.ContainsKey(AddedItemID))
                        WorldItems[AddedItemID] = new List<WorldItemModel>();
                    WorldItems[AddedItemID].AddRange(Items);
                }
                AddedItems.Clear();

                SendServerMessageWriter msgPacket = new SendServerMessageWriter();
                msgPacket.Message = $"Salvando o mundo, próxima ação agendada para {new TimeSpan(0, 0, 0, 0, Interval)}!";

                IChatGateClient Chat = SingletonFactory.GetSingleton<IChatGateClient>();
                Chat.Socket.Send(msgPacket);
            }
        }

        public void End()
        {
            Update();
        }

        public bool Enable()
        {
            try
            {
                UpdaterFactory.Start(this);
                return true;
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger().LogFatal(ex);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                UpdaterFactory.Stop(this);
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger().LogFatal(ex);
                return false;
            }
        }
    }
}
