using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Local.UI
{
    public class TabNavigator : MonoBehaviour
    {
        EventSystem System;

        void Update()
        {
            if(System == null)
            {
                System = EventSystem.current;
            }
            else if(Input.GetKeyDown(KeyCode.Tab) && System.currentSelectedGameObject != null)
            {
                var Actual = System.currentSelectedGameObject.GetComponent<Selectable>();

                if(Actual != null)
                {
                    var Next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? Actual.FindSelectableOnUp() : Actual.FindSelectableOnDown();

                    if (Next != null)
                    {
                        InputField Field = null;
                        if ((Field = Next.GetComponent<InputField>()) != null)
                            Field.OnPointerClick(new PointerEventData(System));
                        System.SetSelectedGameObject(Next.gameObject, new BaseEventData(System));
                    }
                }
            }
        }
    }
}
