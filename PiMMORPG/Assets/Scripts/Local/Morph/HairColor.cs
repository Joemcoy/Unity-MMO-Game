using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MORPH3D.COSTUMING;
using tFramework.Extensions;
using Scripts.Local.Bundles;

namespace Scripts.Local.Morph
{
    [RequireComponent(typeof(CIhair))]
    public class HairColor : MonoBehaviour
    {
        public string HairName;
        public string[] Colors;
        public short Current = -1;
        private short Last = 0;

        SkinnedMeshRenderer[] Renderers;

        void Start()
        {
            Renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            Renderers.ForEach(R =>
            {
                var material = BundleLoader.LoadAsset<Material>("materials/hair/" + FormatName(Colors[0]));
                R.sharedMaterial = new Material(material);
            });
        }

        void Reset()
        {
            Current = -1;
            Update();
        }

        void Update()
        {
            if (Current != Last)
            {
                Last = Current;

                if (Current < Colors.Length && Current >= 0)
                {
                    var material = BundleLoader.LoadAsset<Material>("materials/hair/" + FormatName(Colors[Current]));
                    Renderers.ForEach(R => R.sharedMaterial = new Material(material));
                }
            }
        }

        string FormatName(string ColorName)
        {
            return string.Format("{0}/{1}", HairName, ColorName);
        }
    }
}
