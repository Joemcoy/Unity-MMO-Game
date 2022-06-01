using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Triggers
{
    public class TriggerInitalizer : MonoBehaviour
    {
        public bool IsLocal = false;

        private void Awake()
        {
            foreach (var trigger in GetComponents<NetworkTriggerBase>())
            {
                trigger.Init(IsLocal);
                //trigger.LoadEvents();
            }
        }
    }
}