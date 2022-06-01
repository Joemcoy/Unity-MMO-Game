using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Devdog.InventoryPro;

namespace Scripts.Local.Inventory
{
    using Interfaces;
    public class NetworkCollectionPopulator : CollectionPopulator
    {
        protected new void Awake()
        {
            for(int i = 0; i < items.Length; i++)
            {
                var old = items[i];
                var item = Instantiate(old.item);
                (item as INetworkItem).Serial = Guid.NewGuid();

                items[i] = new ItemAmountRow(item, old.amount);
            }
            base.Awake();
        }
    }
}