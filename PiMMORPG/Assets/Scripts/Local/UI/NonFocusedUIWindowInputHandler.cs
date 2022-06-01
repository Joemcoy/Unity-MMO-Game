using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Devdog.General.UI;

namespace Scripts.Local.UI
{
    using Helper;
    public class NonFocusedUIWindowInputHandler : UIWindowInputHandler
    {
        EventSystem system;

        protected override void Update()
        {
            if (UIHelper.HasFieldFocused()) return;
            base.Update();
        }
    }
}