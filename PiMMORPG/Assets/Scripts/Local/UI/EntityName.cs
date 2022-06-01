using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

using UnityEngine;

namespace Scripts.Local.UI
{
    using Camera = UnityEngine.Camera;

    [RequireComponent(typeof(Canvas))]
    public class EntityName : MonoBehaviour
    {
        TextMeshProUGUI text;

        public Camera Target;
        public string Name = "Entity";

        void Start()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Reset()
        {
            Destroy(gameObject);
        }

        void Update()
        {
            if (text != null)
            {
                if (text.text != Name)
                    text.text = Name;

                if (Target != null)
                {
                    transform.LookAt(Target.transform);
                    transform.Rotate(Vector3.up, 180);
                }
                else
                {
                    Target = Camera.main;
                }
            }
            else
                enabled = false;
        }
    }
}
