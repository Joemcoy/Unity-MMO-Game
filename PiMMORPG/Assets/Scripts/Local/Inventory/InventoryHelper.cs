using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;

using tFramework.Extensions;

namespace Scripts.Local.Inventory
{
    using Bundles;

    public class InventoryHelper : SingletonBehaviour<InventoryHelper>
    {
        InventoryPlayer player;
        public GameObject DropPouch;

        public bool Enabled
        {
            get { return gameObject.activeInHierarchy; }
            set { gameObject.SetActive(value); }
        }

        public void Clear()
        {
            player.characterUI.items.Select(i => i.item).OfType<EquippableInventoryItem>().Where(i => i.isEquipped).ForEach(i => player.characterCollection.UnEquipItem(i, false));
            player.skillbarCollection.items.ForEach(i => player.skillbarCollection.RemoveItem(i.item));
            foreach (var inventory in player.inventoryCollections)
                inventory.items.ForEach(i => inventory.RemoveItem(i.item));
        }

        public override void Created()
        {
            base.Created();
            DropPouch = BundleLoader.LoadPrefab("prefabs/drop_pouch");

            var t = typeof(ManagerBase<>);
            var p = t.GetProperty("instance", BindingFlags.Public | BindingFlags.Static);
            foreach (var manager in transform.GetChild(0).GetComponents(t))
            {
                p.SetValue(manager, manager, null);
            }
            player = GetComponent<InventoryPlayer>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void CopyTo(NetworkInventoryPlayer player)
        {
            player.characterUI = this.player.characterUI;
            player.equipmentHandler = this.player.equipmentHandler;
            player.skillbarCollection = this.player.skillbarCollection;
            player.inventoryCollections = this.player.inventoryCollections;
            player.equipmentBinders = this.player.equipmentBinders;
            player.equipmentHandler.Init(player.characterUI);
        }
    }
}