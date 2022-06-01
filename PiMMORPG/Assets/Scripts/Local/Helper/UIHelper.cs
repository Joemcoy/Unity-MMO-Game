using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Scripts.Local.Helper
{
    public class UIHelper
    {
        public static bool HasFieldFocused()
        {
            var system = EventSystem.current;
            if (system != null && system.currentSelectedGameObject != null)
            {
                var field = system.currentSelectedGameObject.GetComponent<InputField>();
                if (field != null && field.isFocused)
                    return true;
            }
            return false;
        }
    }
}