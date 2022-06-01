using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Factories;

using Base.Data;
using Base.Data.Interfaces;

using Network.Data;
using Network.Data.Interfaces;
using Network.Data.EventArgs;

using Game.Data.Models;

using Game.Client;
using Data.Client;
using Gate.Client.Responses.Writers;
using Game.Server.Writers;

namespace Game.Server.Manager
{
    public class ItemCacheManager : ISingleton, IUpdater, IComponent
    {
        private Dictionary<int, ItemModel> Items;
        private Dictionary<int, List<WorldItemModel>> WorldItems, AddedItems;
        private Dictionary<int, List<CharacterItemModel>> CharacterItems;
        private Dictionary<int, List<CharacterItemModel>> RemovedItems;

        private ILogger Logger;

        private GameServer Server;
        private DataClient Data;

        object syncLock;

        public int Interval { get { return 5 * 60 * 1000; } }

        void ISingleton.Destroy()
        {
            Items.Clear();
        }

        void ISingleton.Create()
        {
            Logger = LoggerFactory.GetLogger(this);

            syncLock = new object();
            Items = new Dictionary<int, ItemModel>();

            CharacterItems = new Dictionary<int, List<CharacterItemModel>>();
            WorldItems = new Dictionary<int, List<WorldItemModel>>();
            AddedItems = new Dictionary<int, List<WorldItemModel>>();
            RemovedItems = new Dictionary<int, List<CharacterItemModel>>();

            Server = SingletonFactory.GetInstance<GameServer>();
            Data = SingletonFactory.GetInstance<DataClient>();            
        }

        public void SaveClient(GameClient Client)
        {
            Logger.LogWarning("Client: {0}", Client == null ? "NULL" : Client.Socket == null ? "Non connected" : Client.CurrentCharacter == null ? "Non logged" : Client.CurrentCharacter.Name);

            if (Client != null && Client.CurrentCharacter != null)
            {
                List<CharacterItemModel> Items = null;

                if (RemovedItems.TryGetValue(Client.CurrentCharacter.ID, out Items))
                {
                    foreach (var Item in Items)
                    {
                        Data.SendRemoveItem(Item);
                    }
                    Items.Clear();
                }
                else
                    Logger.LogWarning("Failed to get removed items of character {0}", Client.CurrentCharacter.ID);

                if (CharacterItems.TryGetValue(Client.CurrentCharacter.ID, out Items))
                {
                    foreach (var Item in Items)
                    {
                        Item.IsDirty = false;
                        Data.SendUpdateCharacterItem(Item);
                    }
                }
                else
                    Logger.LogWarning("Failed to get items of character {0}", Client.CurrentCharacter.ID);
            }
        }

        private CharacterItemModel GetCharacterItem(int CharacterID, uint InventoryID)
        {
            if (CharacterItems.ContainsKey(CharacterID))
            {
                var Item = CharacterItems[CharacterID].FirstOrDefault(I => I.ItemID == Items[I.ItemID].UniqueID);
                if (Item != null)
                    return Item;
            }

            return null;
        }

        private CharacterItemModel GetCharacterItem(int CharacterID, Guid Serial)
        {
            if(CharacterItems.ContainsKey(CharacterID))
            {
                var Item = CharacterItems[CharacterID].FirstOrDefault(I => I.Serial == Serial);
                if (Item != null)
                    return Item;
            }

            return null;
        }

        private CharacterItemModel GetCharacterItemInHotbar(int CharacterID, short HotbarSlot)
        {
            if (CharacterItems.ContainsKey(CharacterID))
            {
                var Item = CharacterItems[CharacterID].FirstOrDefault(I => I.HotbarSlot == HotbarSlot);
                if (Item != null)
                    return Item;
            }

            return null;
        }

        private CharacterItemModel GetCharacterItemBySlot(int CharacterID, uint Slot)
        {
            return CharacterItems[CharacterID].First(I => Slot == I.Slot);
        }

        public void SetItemState(int CharacterID, Guid Serial, bool State)
        {
            var Item = GetCharacterItem(CharacterID, Serial);
            Item.Equiped = State;

            var Packet = new SetEquipStateWriter();
            Packet.InventoryID = (uint)Items[Item.ItemID].UniqueID;
            Packet.OwnerID = CharacterID;
            Packet.State = State;

            foreach (var Client in Server.Clients.Where(C => C.CurrentCharacter != null && C.CurrentCharacter.ID != CharacterID))
            {
                Client.Socket.Send(Packet);
            }
        }

        public void SetItemSlot(int CharacterID, Guid Serial, uint Slot)
        {
            if (CharacterItems.ContainsKey(CharacterID))
            {
                GetCharacterItem(CharacterID, Serial).Slot = Slot;
            }
            else
                Logger.LogWarning("Failed to set item {0} to slot {1} of character {2}", Serial, Slot, CharacterID);
        }

        public void SetHotbarItem(int CharacterID, Guid Serial, bool Insert, short Slot)
        {
            var RealSlot = Convert.ToInt16(Insert ? Slot : -1);
            GetCharacterItem(CharacterID, Serial).HotbarSlot = RealSlot;
        }

        public void SetItemAmount(int CharacterID, Guid Serial, uint Amount, bool Add = false)
        {
            lock (syncLock)
            {
                if (Amount == 0)
                    RemoveItem(CharacterID, Serial);
                else
                {
                    Logger.LogWarning("Set item on slot {0} ammount to {1}", Serial, Amount);
                    var Item = GetCharacterItem(CharacterID, Serial);

                    if (Add)
                        Item.Amount += Amount;
                    else
                        Item.Amount = Amount;
                }
            }
        }

        public void RemoveItem(int CharacterID, uint Slot)
        {
            lock (syncLock)
            {
                Predicate<CharacterItemModel> C = I => I.Slot == Slot;
                CharacterItemModel Item;

                if (RemoveItem(CharacterItems, CharacterID, C, out Item))
                {
                    Logger.LogWarning("Item {0} find on character items!", Item.Serial);

                    if (!Item.IsDirty)
                    {
                        if (!RemovedItems.ContainsKey(CharacterID))
                            RemovedItems[CharacterID] = new List<CharacterItemModel>();
                        RemovedItems[CharacterID].Add(Item);
                    }
                }
                else
                    Logger.LogWarning("Item {0} find on added items!", Item.Serial);
            }
        }

        public void RemoveItem(int CharacterID, Guid Serial)
        {
            lock (syncLock)
            {
                Predicate<CharacterItemModel> C = I => I.Serial == Serial;
                CharacterItemModel Item;

                if (RemoveItem(CharacterItems, CharacterID, C, out Item))
                {
                    Logger.LogWarning("Item {0} find on character items!", Item.Serial);

                    if (!RemovedItems.ContainsKey(CharacterID))
                        RemovedItems[CharacterID] = new List<CharacterItemModel>();
                    RemovedItems[CharacterID].Add(Item);
                }
                else
                    Logger.LogWarning("Failed to remove item {0}:{1}!", CharacterID, Serial);

                /*var Item = GetCharacterItem(CharacterID, Serial);
                if(Item != null)
                {
                    if (!RemovedItems.ContainsKey(CharacterID))
                        RemovedItems[CharacterID] = new List<CharacterItemModel>();
                    RemovedItems[CharacterID].Add(Item);

                    Logger.LogWarning("Item {0} found!", Serial);
                }
                else
                    Logger.LogWarning("Failed to remove item {0}:{1}!", CharacterID, Serial);*/
            }
        }

        bool RemoveItem(Dictionary<int, List<CharacterItemModel>> Items, int CharacterID, 
            Predicate<CharacterItemModel> Condition, out CharacterItemModel Item)
        {
            var Server = SingletonFactory.GetInstance<GameServer>();
            List<CharacterItemModel> ItemList;
            Item = null;

            if(Items.TryGetValue(CharacterID, out ItemList))
            {
                Item = ItemList.FirstOrDefault(I => Condition(I));
                if(Item != null)
                {
                    ItemList.Remove(Item);
                    Server.OnlineClients.First(C => C.CurrentCharacter.ID == CharacterID).RemoveItem(Item);

                    return true;
                }
            }
            return false;
        }

        public void UnstackItem(int CharacterID, uint Start, uint End, uint Amount, Guid FromSerial, Guid ToSerial)
        {
            Logger.LogWarning("Unstacking item {4} of {0}:-{1} to {2}:{3} on {5}", Start, Amount, End, Amount, FromSerial, ToSerial);
            var Item = GetCharacterItem(CharacterID, FromSerial);

            SetItemSlot(CharacterID, FromSerial, Start);
            SetItemAmount(CharacterID, FromSerial, Item.Amount - Amount);

            var New = new CharacterItemModel();
            Item.CopyTo(New);

            New.ID = -1;
            New.Slot = End;
            New.Amount = Amount;
            New.HotbarSlot = -1;
            New.Serial = ToSerial;
            New.Equiped = false;

            AddCharacterItem(CharacterID, New, true);
        }

        public void MergeItem(int CharacterID, Guid From, Guid To, uint Amount)
        {
            Logger.LogInfo("Try to merge slot {0}:{2} to item {1}!", From, To, Amount);

            RemoveItem(CharacterID, From);
            SetItemAmount(CharacterID, To, Amount);
        }

        public void PutItem(ItemModel Model)
        {
            if (!Items.ContainsKey(Model.ID))
            {
                //Logger.LogInfo("Creating cache for item {0}...", Model.ID);
                Items[Model.ID] = Model;
            }
        }

        public void PutWorldItem(WorldItemModel Model)
        {
            if (!WorldItems.ContainsKey(Model.Position.MapID))
            {
                WorldItems[Model.Position.MapID] = new List<WorldItemModel>();
                Logger.LogInfo("Creating cache for map {0}...", Model.Position.MapID);
            }

            WorldItems[Model.Position.MapID].Add(Model);
            //Logger.LogInfo("Creating cache for world item {Model.Item.Name} on map {Model.Position.MapID}...");
        }

        public void AddWorldItem(WorldItemModel Model)
        {
            if (!AddedItems.ContainsKey(Model.Position.MapID))
            {
                AddedItems[Model.Position.MapID] = new List<WorldItemModel>();
                Logger.LogInfo("Creating cache for map {0}...", Model.Position.MapID);
            }

            AddedItems[Model.Position.MapID].Add(Model);
            //Logger.LogInfo("Creating cache for world item {Model.Item.Name} on map {Model.Position.MapID}...");
        }

        public ItemModel[] GetItems()
        {
            return Items.Values.ToArray();
        }

        public ItemModel GetItem(int ItemID)
        {
            Logger.LogInfo("Try to get item {0}...", ItemID);

            ItemModel Item;
            return Items.TryGetValue(ItemID, out Item) ? Item : null;
        }

        public ItemModel GetItemByUID(int UniqueID)
        {
            return Items.Values.FirstOrDefault(I => I.UniqueID == UniqueID);
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

        public void AddCharacterItems(int CharacterID, CharacterItemModel[] Items)
        {
            foreach (var Item in Items)
                AddCharacterItem(CharacterID, Item);

            Logger.LogWarning("Added {0} items of character {1} to cache...", Items.Length, CharacterID);
        }

        public void AddCharacterItem(int CharacterID, CharacterItemModel Item, bool ToAdd = false)
        {
            if (ToAdd)
            {
                Item.IsDirty = true;
                Logger.LogWarning("Adding the item {0} to character {1} on cache!", Item.ItemID, CharacterID);
            }

            var Client = Server.OnlineClients.First(C => C.CurrentCharacter.ID == CharacterID);
            Client.AddItem(Item);

            var RItem = Items[Item.ItemID];
            if (RItem.CanEquip())
                Client.EquipItem(RItem.Type, Item);


            if (!CharacterItems.ContainsKey(CharacterID))
            {
                CharacterItems.Add(CharacterID, new List<CharacterItemModel>());
            }            

            CharacterItems[CharacterID].Add(Item);
        }

        public CharacterItemModel[] GetCharacterItems(int CharacterID)
        {
            List<CharacterItemModel> Items;

            if (CharacterItems.TryGetValue(CharacterID, out Items))
                return Items.ToArray();
            return new CharacterItemModel[0];
        }

        void IUpdater.Start()
        {

        }

        void IUpdater.Loop()
        {
            lock (syncLock)
            {
                foreach (int AddedItemID in AddedItems.Keys)
                {
                    List<WorldItemModel> Items = AddedItems[AddedItemID];

                    foreach (WorldItemModel Item in Items)
                    {
                        Data.SendPutItemInWorld(null, Item.Group.ClaimLeader, Item.ItemID, Item.Group, Item.Position);
                    }

                    if (!WorldItems.ContainsKey(AddedItemID))
                        WorldItems[AddedItemID] = new List<WorldItemModel>();
                    WorldItems[AddedItemID].AddRange(Items);
                }
                AddedItems.Clear();

                foreach (var Item in RemovedItems.SelectMany(I => I.Value))
                {
                    Data.SendRemoveItem(Item);
                    Logger.LogWarning("Removing item {0}!", Item.Serial);
                }
                RemovedItems.Clear();

                foreach(var Item in CharacterItems.SelectMany(I => I.Value).Where(I => I.IsDirty))
                {
                    Data.SendUpdateCharacterItem(Item);
                    Item.IsDirty = false;
                }
            }
        }

        void IUpdater.End()
        {

        }

        bool IComponent.Enable()
        {
            try
            {
                UpdaterFactory.Start(this);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogFatal(ex);
                return false;
            }
        }

        bool IComponent.Disable()
        {
            try
            {
                UpdaterFactory.Stop(this);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogFatal(ex);
                return false;
            }
        }
    }
}