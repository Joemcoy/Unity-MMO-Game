using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devdog.InventoryPro;

using UnityEngine;

using PiMMORPG.Client;
using PiMMORPG.Models;

namespace Scripts.Local.Triggers
{
    using Interfaces;
    using Network.Requests.GameClient;

    public class InventoryActionTrigger : NetworkTriggerBase
    {
        public GameObject PouchItem;
        private Animator animator;

        public override void Init(bool IsLocal)
        {
            base.Init(IsLocal);

            if (IsLocal)
            {
                animator = GetComponent<Animator>();
                foreach (var Inventory in Player.inventoryCollections)
                {
                    if (Inventory.collectionName != "Skill" && Inventory.collectionName != "Motion")
                    {
                        Inventory.OnRemovedItem += Inventory_OnRemovedItem;
                        Inventory.OnUsedItem += Inventory_OnUsedItem;
                        Inventory.OnUnstackedItem += Inventory_OnUnstackedItem;
                        Inventory.OnDroppedItem += Inventory_OnDroppedItem;
                        Inventory.OnMergedSlots += Inventory_OnMergedSlots;
                    }
                    Inventory.OnSetItem += Inventory_OnSetItem;
                }

                //Player.skillbarCollection.OnSetItem += SkillbarCollection_OnSetItem;
                Player.skillbarCollection.OnUsedItem += SkillbarCollection_OnUsedItem;
                Player.skillbarCollection.OnAddedItem += SkillbarCollection_OnAddedItem;
                Player.skillbarCollection.OnRemovedItem += SkillbarCollection_OnRemovedItem;
            }
        }

        private void SkillbarCollection_OnAddedItem(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
        {
            if (!Client.Socket.Connected)
                return;

            var Packet = new SetHotbarSlotRequest();
            var item = items.First();
            var Item = item as INetworkItem;

            if (Item != null)
            {
                Packet.Slot = Convert.ToInt16(item.index);
                Packet.Serial = Item.Serial;
            }

            Client.Socket.Send(Packet);
        }

        private void SkillbarCollection_OnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            var Item = item as INetworkItem;
            if (!Client.Socket.Connected || Item == null)
                return;

            var Packet = new SetHotbarSlotRequest();

            Packet.Slot = -1;
            Packet.Serial = Item.Serial;

            Client.Socket.Send(Packet);
        }

        public override void UnloadEvents()
        {
            if (Player != null && Player.inventoryCollections != null)
            {
                foreach (var Inventory in Player.inventoryCollections)
                {
                    if (Inventory.collectionName != "Skill" && Inventory.collectionName != "Motion")
                    {
                        Inventory.OnRemovedItem -= Inventory_OnRemovedItem;
                        Inventory.OnUsedItem -= Inventory_OnUsedItem;
                        Inventory.OnUnstackedItem -= Inventory_OnUnstackedItem;
                        Inventory.OnDroppedItem -= Inventory_OnDroppedItem;
                        Inventory.OnMergedSlots -= Inventory_OnMergedSlots;
                    }
                    Inventory.OnSetItem -= Inventory_OnSetItem;
                }

                if (Player.skillbarCollection != null)
                {
                    Player.skillbarCollection.OnAddedItem -= SkillbarCollection_OnAddedItem;
                    Player.skillbarCollection.OnRemovedItem -= SkillbarCollection_OnRemovedItem;
                    Player.skillbarCollection.OnUsedItem -= SkillbarCollection_OnUsedItem;
                }
            }
            base.UnloadEvents();
        }

        private void Inventory_OnMergedSlots(ItemCollectionBase fromCollection, uint fromSlot, ItemCollectionBase toCollection, uint toSlot)
        {
            if (IsLocal)
            {
                var toItem = toCollection[toSlot].item;
                var fromItem = fromCollection[fromSlot].item;
                var Size = fromItem.currentStackSize + toItem.currentStackSize;

                var ToItem = toItem as INetworkItem;
                var FromItem = fromItem as INetworkItem;

                if (IsLoaded && ToItem != null && FromItem != null)
                {
                    var Packet = new MergeItemRequest();
                    Packet.From = FromItem.Serial;
                    Packet.To = ToItem.Serial;
                    Packet.Quantity = Size;

                    Client.Socket.Send(Packet);
                }
            }
        }

        private void Inventory_OnUnstackedItem(ItemCollectionBase fromCollection, uint startSlot, ItemCollectionBase toCollection, uint endSlot, uint amount)
        {
            var FromItem = fromCollection[startSlot].item as INetworkItem;
            var ToItem = toCollection[endSlot].item as INetworkItem;
            Debug.LogFormat("Unstacking item {0}:{1} to {2}:{3}!", startSlot, FromItem.Serial, endSlot, ToItem.Serial);

            if (IsLoaded && FromItem != null && ToItem != null)
            {
                var Packet = new UnstackItemRequest();
                Packet.FromSlot = startSlot;
                Packet.ToSlot = endSlot;
                Packet.Quantity = amount;
                Packet.From = FromItem.Serial;
                Packet.To = ToItem.Serial = Guid.NewGuid();

                Client.Socket.Send(Packet);
            }
        }

        private void Inventory_OnDroppedItem(InventoryItemBase item, uint slot, GameObject droppedObj)
        {
            var Item = item as INetworkItem;           

            if (IsLocal && IsLoaded && Item != null)
            {
                Debug.LogFormat("Dropped object {0} a serial {1}!", droppedObj.name, Item.Serial);
                //if(droppedObj.is)
                //Destroy(droppedObj);

                var Packet = new DropItemRequest();
                Packet.Drop = new Drop();
                Packet.Drop.Serial = Item.Serial;
                Packet.Drop.Vector = droppedObj.transform.position;
                Packet.Drop.Quaternion = droppedObj.transform.rotation;
                Packet.Drop.InventoryID = item.ID;
                Packet.Drop.Quantity = item.currentStackSize;

                Client.Socket.Send(Packet);

                //var Motion = new PlayMotionWriter();
                //Motion.TriggerName = "Throw Item";

                //Client.Socket.Send(Motion);
            }
            else if (IsLocal)
            {
                var Pouch = Instantiate(this.PouchItem);
                Pouch.GetComponent<ItemTrigger>().itemPrefab = item;
                Pouch.GetComponent<Light>().color = item.rarity.color;
                Pouch.transform.position = droppedObj.transform.position;
                Pouch.transform.rotation = droppedObj.transform.rotation;

                Destroy(droppedObj);
            }
        }

        private void Inventory_OnUsedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            if (item.maxStackSize > 1 && IsLoaded)
            {
                var Item = item == null ? null : item.itemCollection[slot] as INetworkItem;

                PiBaseRequest Packet = null;
                if (Item != null)
                {
                    Packet = new SetItemQuantityRequest
                    {
                        Quantity = item.itemCollection[slot].item.currentStackSize,
                        Serial = Item.Serial
                    };
                }
                else
                {
                    Packet = new RemoveItemRequest
                    {
                        Serial = Item.Serial
                    };
                }

                Client.Socket.Send(Packet);
            }

            UseAnimation(item);
        }

        private void Inventory_OnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            if (item is ConsumableInventoryItem)
            {
                Debug.LogFormat("{0} on slot {1} to ammount {2} removed!", itemID, slot, amount);
            }
        }

        private void SkillbarCollection_OnUsedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            UseAnimation(item);
        }

        private void UseAnimation(InventoryItemBase item)
        {
            if (item is ConsumableInventoryItem && !animator.GetBool("Walking"))
            {
                if (IsLoaded)
                {
                    /*var Packet = new PlayMotionWriter();

                    switch (item.categoryName)
                    {
                        case "Food":
                            Packet.TriggerName = "Eat";
                            break;
                        case "Potion":
                            Packet.TriggerName = "Drink Potion";
                            break;
                        case "Drink":
                            Packet.TriggerName = "Drink";
                            break;
                    }

                    Client.Socket.Send(Packet);*/
                }
                else
                {
                    string TriggerName = string.Empty;
                    switch (item.categoryName)
                    {
                        case "Food":
                            TriggerName = "Eat";
                            break;
                        case "Potion":
                            TriggerName = "Drink Potion";
                            break;
                        case "Drink":
                            TriggerName = "Drink";
                            break;
                    }
                    GetComponent<Animator>().SetTrigger(TriggerName);
                }
            }
        }

        public void LoadEvents(InventoryPlayer Player)
        {
            IsLoaded = true;
        }

        private void Inventory_OnSetItem(uint slot, InventoryItemBase item)
        {
            var Item = item as INetworkItem;
            if (Item != null && IsLocal && IsLoaded && Client != null)
            {
                var Packet = new SetItemSlotRequest();
                Packet.Serial = Item.Serial;
                Packet.Slot = slot;

                Client.Socket.Send(Packet);
            }
        }
    }
}
