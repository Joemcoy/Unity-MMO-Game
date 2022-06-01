using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Helper
{
    public static class GameObjectHelper
    {
        public static GameObject FindObjectWithLayer(string layer, string tag = null)
        {
            var mask = LayerMask.NameToLayer(layer);
            var objects = tag == null ? GameObject.FindObjectsOfType<GameObject>() : GameObject.FindGameObjectsWithTag(tag);
            foreach (var @object in objects)
                if (@object.layer == mask)
                    return @object;
            return null;
        }

        public static void SetLayerRecursive(GameObject root, int layer)
        {
            root.layer = layer;
            if (root.transform.childCount > 0)
                foreach (Transform children in root.transform)
                    if (children != null)
                        SetLayerRecursive(children.gameObject, layer);
        }
    }
}