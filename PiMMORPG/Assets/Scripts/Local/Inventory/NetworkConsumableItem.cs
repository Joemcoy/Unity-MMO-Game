using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devdog.InventoryPro;

namespace Scripts.Local.Inventory
{
    using Interfaces;

    [Serializable]
    public class NetworkConsumableItem : ConsumableInventoryItem, INetworkItem
    {
        public string serial;
        public Guid Serial
        {
            get { return new Guid(serial); }
            set { serial = value.ToString("D"); }
        }
    }
}