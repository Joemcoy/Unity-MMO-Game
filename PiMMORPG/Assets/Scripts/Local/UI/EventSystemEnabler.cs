using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Local.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemEnabler : MonoBehaviour
    {
        EventSystem system;

        private void Update()
        {            
            if (system != null && !system.enabled && !FindObjectsOfType<EventSystem>().Where(e => e != system).Any())
                system.enabled = true;
            else
                system = GetComponent<EventSystem>();
        }
    }
}
