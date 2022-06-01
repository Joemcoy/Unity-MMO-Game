using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Locomotion
{
    using Triggers;
    public class CombatControl : NetworkTriggerBase
    {
        Animator animator;
        InventoryEquipTrigger trigger;

        public override void Init(bool IsLocal)
        {
            base.Init(IsLocal);
            animator = GetComponent<Animator>();
            trigger = GetComponent<InventoryEquipTrigger>();
            Debug.Log(trigger);
        }

        private void Update()
        {
            if(IsLoaded && Input.GetButtonDown("Fire1") && (trigger.RightEquiped || trigger.LeftEquiped))
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}