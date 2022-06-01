using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using tFramework.Extensions;

namespace Scripts.Local.Helper
{
    public static class TransformHelper
    {
        public static void SetParent(Transform parent, Transform child)
        {
            var colliders = child.GetComponentsInChildren<Collider>(true);
            var states = colliders.ToDictionary(c => c, c => c.enabled);

            colliders.ForEach(c => c.enabled = false);
            child.SetParent(parent);
            colliders.ForEach(c => c.enabled = states[c]);
        }
    }
}