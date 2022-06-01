using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Bundles.Fillers
{
    public class PrefabFromBundle : BaseFiller
    {
        public override void Load()
        {
            var prefab = BundleLoader.LoadPrefab("prefabs/" + Path);
            var instance = Instantiate(prefab, transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.SetActive(true);
        }
    }
}