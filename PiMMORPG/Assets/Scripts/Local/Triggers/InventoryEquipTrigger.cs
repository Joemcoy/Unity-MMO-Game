using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Devdog.InventoryPro;
using Devdog.General;

namespace Scripts.Local.Triggers
{
    using Inventory;
    using Interfaces;
    
    [Serializable]
    public class WeaponHolder
    {
        public GameObject Visual;
        public NetworkWeaponItem Weapon;

        public void Reset()
        {
            Visual = null;
            Weapon = null;
        }
    }

    public class InventoryEquipTrigger : NetworkTriggerBase
    {
        public Transform LeftHand, RightHand;
        public GameObject HairObject, Previous, Next;
        public bool RFlag = false;

        public bool LeftEquiped { get { return LeftHolder.Weapon != null; } }
        public bool RightEquiped { get { return RightHolder.Weapon != null; } }

        public WeaponHolder RightHolder, LeftHolder;
        public bool Helmet { get { return Equips != null && Equips.ContainsKey("Helmet"); } }

        ItemCollectionBase Inventory;
        public Animator animator;
        //public SwitchWeapon Switch;
        
        Dictionary<string, Transform> Bones;
        Dictionary<string, GameObject> Equips;

        public override void Init(bool IsLocal)
        {
            base.Init(IsLocal);

            animator = GetComponent<Animator>();
            RightHolder = new WeaponHolder();
            LeftHolder = new WeaponHolder();
            //Switch = animator.GetBehaviour<SwitchWeapon>();

            Equips = new Dictionary<string, GameObject>();
            if (IsLocal)
            {
                if (!IsLoaded && Player.isInitialized)
                    Player.characterUI.transform.parent.gameObject.SetActive(true);

                if (Player != null)
                {
                    Player.characterUI.OnAddedItem += CharacterCollection_OnAddedItem;
                    Player.characterUI.OnRemovedItem += CharacterUI_OnRemovedItem;

                    var lhb = Player.equipmentBinders.First(e => e.equippableSlot.equipmentTypes.Any(t => t.name.IndexOf("Left Hand") > -1));
                    var rhb = Player.equipmentBinders.First(e => e.equippableSlot.equipmentTypes.Any(t => t.name.IndexOf("Right Hand") > -1));

                    lhb.equipTransform = LeftHand;
                    rhb.equipTransform = RightHand;
                }
            }

            Reset();
        }

        void Reset()
        {
            animator.SetInteger("WeaponType", 0);
        }

        public override void UnloadEvents()
        {
            if(Equips != null)
            Equips.Clear();
            LeftHolder = null;
            RightHolder = null;

            if (Player && Player.characterUI)
            {
                Player.characterUI.OnAddedItem -= CharacterCollection_OnAddedItem;
                Player.characterUI.OnRemovedItem -= CharacterUI_OnRemovedItem;
            }
            base.UnloadEvents();
        }

        private void CharacterCollection_OnAddedItem(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
        {
            /*var Packet = new SetEquipStateRequest();

            foreach (var item in items)
            {
                if (item is NetworkEquippableItem)
                {
                    if (IsLoaded && Client != null)
                    {
                        Packet.Serial = (item as INetworkItem).Serial;
                        Packet.Equipped = true;
                        Client.Socket.Send(Packet);
                    }
                }
            }*/
        }

        private void CharacterUI_OnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            /*var Packet = new SetEquipStateRequest();

            if (item is NetworkEquippableItem)
            {
                if (IsLoaded)
                {
                    Packet.Serial = (item as INetworkItem).Serial;
                    Packet.Equipped = false;
                    Client.Socket.Send(Packet);
                }
            }*/
        }

        GameObject CreatePrefab(NetworkEquippableItem Equip)
        {
            if (Equip.equipmentType.name.IndexOf("Hand") > -1)
            {
                var Object = Instantiate(Equip).gameObject;
                Object.SetActive(false);
                Object.layer = gameObject.layer;

                Destroy(Object.GetComponent<Trigger>());
                Destroy(Object.GetComponent<ItemTriggerInputHandler>());

                var Rigid = Object.GetComponent<Rigidbody>();
                if (Rigid != null) Destroy(Rigid);

                foreach (var Col in Object.GetComponentsInChildren<Collider>())
                    Col.isTrigger = true;
                return Object;
            }
            return Equip.gameObject;
        }

        public virtual void Equip(NetworkEquippableItem Equip, GameObject visual = null)
        {
            /*if (Equips.ContainsKey(Equip.equipmentType.name) && !RFlag)
                UnEquip(Equip);
            Equips[Equip.equipmentType.name] = Item;*/

            var eq = Equip.equipmentType.name;
            if (eq.IndexOf("Hand") > -1)
            {
                var w = Equip as NetworkWeaponItem;
                /*if (eq.IndexOf("Right") > -1)
                    RightItem = w;
                else if (eq.IndexOf("Any") > -1)
                {
                    if (!RightEquiped)
                        RightItem = w;
                    else
                        LeftItem = w;
                }
                else
                    LeftItem = w;*/

                Debug.Log(w.serial);
                foreach (var item in Player.equipmentBinders.Where(b => b.currentItem != null).Select(b => b.currentItem.GetComponent<InventoryItemBase>()))
                    if (item is INetworkItem)
                        Debug.Log((item as INetworkItem).Serial);

                var slot = Player.equipmentBinders.First(b => b.equippableSlot.slot.item != null && (b.equippableSlot.slot.item as INetworkItem).Serial.Equals(w.Serial)).equippableSlot;
                if (slot.equipmentTypes[0].name.StartsWith("Left"))
                {
                    LeftHolder.Visual = visual ?? Equip.gameObject;
                    LeftHolder.Weapon = w;

                    var lt = LeftHolder.Visual.transform;
                    lt.transform.localPosition = w.LeftPosition;
                    lt.transform.localRotation = w.LeftRotation;
                }
                else// if (slot.equipmentTypes[0].name.StartsWith("Right"))
                {
                    RightHolder.Visual = visual ?? Equip.gameObject;
                    RightHolder.Weapon = w;
                }
                Helper.GameObjectHelper.SetLayerRecursive(visual ?? Equip.gameObject, LayerMask.NameToLayer("Ignore Raycast"));
                //Equip.gameObject.AddComponent<DamageTrigger>();

                var wt = w.WeaponType;
                Debug.Log(LeftEquiped);
                Debug.Log(RightEquiped);
                if (LeftEquiped && LeftHolder.Weapon.WeaponType == 1 && RightEquiped && RightHolder.Weapon.WeaponType == 1)
                    wt = 4;
                animator.SetInteger("WeaponType", wt);
            }
            /*if (!RFlag)
                Item.gameObject.SetActive(true);*/
        }

        public virtual void UnEquip(NetworkEquippableItem Item, bool Remove = true, bool NewEquip = false)
        {
            if (Item == null)
            {
                Debug.LogWarning("Null item?");
                return;
            }
            if (Item.categoryName == "Non-visual item")
                return;
            //GameObject Equip = null;
            //if (Equips.TryGetValue(Item.equipmentType.name, out Equip))
            {
                /*if (Item.equipmentType.name == "Right Hand" && NewEquip)
                {
                    if (!RFlag)
                    {
                        RFlag = true;
                        //Switch.Trigger = this;

                        Previous = Equip;
                        animator.SetTrigger("Put Back");
                    }
                    else
                        RFlag = false;
                }
                else

                var eq = Item.equipmentType.name;
                if (eq.IndexOf("Hand") > -1)
                    (eq.IndexOf("Right") > -1 ? RightHolder : LeftHolder).Reset();*/

                var w = Item as NetworkWeaponItem;                
                if (LeftHolder.Weapon != null && LeftHolder.Weapon.Serial.Equals(w.Serial))
                {
                    LeftHolder.Visual = null;
                    LeftHolder.Weapon = null;
                }
                else if (RightHolder.Weapon != null && RightHolder.Weapon.Serial.Equals(w.Serial))
                {
                    RightHolder.Visual = null;
                    RightHolder.Weapon = null;
                }

                if (!LeftEquiped && !RightEquiped)
                    animator.SetInteger("WeaponType", 0);
                else if(LeftEquiped && !RightEquiped)
                    animator.SetInteger("WeaponType", LeftHolder.Weapon.WeaponType);
                else
                    animator.SetInteger("WeaponType", RightHolder.Weapon.WeaponType);
            }
        }

        public void RemoveAll()
        {
            Equips.Clear();
        }

        public bool OnLeftHand(Predicate<NetworkEquippableItem> Condition)
        {
            return LeftHolder != null && Condition(LeftHolder.Weapon);
        }

        public bool OnRightHand(Predicate<NetworkEquippableItem> Condition)
        {
            return RightHolder != null && Condition(RightHolder.Weapon);
        }

        void Update()
        {
            if (HairObject != null)
                HairObject.SetActive(!Helmet);
        }
    }
}