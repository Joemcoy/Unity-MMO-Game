using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
namespace Scripts.Local.Inventory
{
    using Triggers;

    public class NetworkWeaponItem : NetworkEquippableItem
    {
        public int WeaponType = 1;
        public Vector3 LeftPosition;
        public Quaternion LeftRotation;

        int remoteMask;
        bool on = false;
        InventoryEquipTrigger trigger;

        private void Start()
        {
            remoteMask = LayerMask.NameToLayer("Remote");
            trigger = GetComponentInParent<InventoryEquipTrigger>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == remoteMask && !on && trigger.RightEquiped)
            {
                on = true;
                Debug.Log("Hit");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            on = false;
        }
    }
}