using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Local.UI
{
    [ExecuteInEditMode]
    public class RealScaler : MonoBehaviour
    {
        CanvasScaler scaler;
        public float Scalar = 0;

        private void Update()
        {
            if (scaler == null)
            {
                scaler = GetComponent<CanvasScaler>();
                return;
            }

            var res = scaler.referenceResolution;
            var t = new Vector2(Screen.width, Screen.height) * Scalar;
            if (res.x != t.x) res.x = Mathf.RoundToInt(t.x);
            if (res.y != t.y) res.y = Mathf.RoundToInt(t.y);
            scaler.referenceResolution = res;
        }
    }
}