using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;

using PiMMORPG.Client;
using tFramework.Factories;

namespace Scripts.Local.Inventory
{
    using Triggers;
    using Interfaces;
    using Network.Requests.GameClient;

    [Serializable]
    public class NetworkEquippableItem : EquippableInventoryItem, INetworkItem
    {
        public string serial;

        public GameObject PlayerObject { get { return PlayerManager.instance.currentPlayer.gameObject; } }
        public Guid Serial
        {
            get { return new Guid(serial); }
            set { serial = value.ToString("D"); }
        }
        protected Animator Animator { get; set; }

        public override bool CanUse()
        {
            if (Animator == null)
                Animator = PlayerObject.GetComponent<Animator>();

            return base.CanUse();// && animator.GetBool("CanWalk");
        }

        public override bool IsInstanceObject()
        {
            return true;
        }

        public override void NotifyItemEquipped(EquippableSlot equipSlot, uint amountEquipped)
        {
            base.NotifyItemEquipped(equipSlot, amountEquipped);

            var client = PiBaseClient.Current;
            if (client != null && client.Socket.Connected)
            {
                var Packet = new SetEquipStateRequest();
                Packet.Equipped = true;
                Packet.Serial = Serial;

                client.Socket.Send(Packet);
            }
        }

        public override void NotifyItemEquippedVisually(CharacterEquipmentTypeBinder binder)
        {
            base.NotifyItemEquippedVisually(binder);

            var Trigger = PlayerManager.instance.currentPlayer.GetComponent<InventoryEquipTrigger>();
            Trigger.Equip(binder.currentItem.GetComponent<NetworkEquippableItem>());
        }

        public override void NotifyItemUnEquipped(ICharacterCollection equipTo, uint amountUnEquipped)
        {
            base.NotifyItemUnEquipped(equipTo, amountUnEquipped);

            var client = PiBaseClient.Current;
            if (client != null && client.Socket.Connected)
            {
                var Packet = new SetEquipStateRequest();
                Packet.Equipped = false;
                Packet.Serial = Serial;

                client.Socket.Send(Packet);
            }
        }

        public override void NotifyItemUnEquippedVisually(CharacterEquipmentTypeBinder binder)
        {
            base.NotifyItemUnEquippedVisually(binder);

            var Trigger = PlayerManager.instance.currentPlayer.GetComponent<InventoryEquipTrigger>();
            if (!Trigger.RFlag)
                Trigger.UnEquip(this);
        }
    }
}