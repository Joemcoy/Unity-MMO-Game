using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Devdog.InventoryPro;

namespace Scripts.Local.Inventory
{
    [CreateAssetMenu(menuName = InventoryPro.CreateAssetMenuPath + "Null equipment handler")]
    public class NullEquipmentHandler : ItemEquipmentHandlerBase
    {
        public override EquippableInventoryItem Equip(EquippableInventoryItem item, CharacterEquipmentTypeBinder binder, bool createCopy)
        {
            return item;
        }

        public override void UnEquip(CharacterEquipmentTypeBinder binder, bool deleteItem)
        {
            
        }
    }
}