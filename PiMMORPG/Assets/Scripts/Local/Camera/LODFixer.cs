using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
//namespace Scripts.Local.Camera
//{
    [RequireComponent(typeof(LODGroup))]
    [ExecuteInEditMode]
    public class LODFixer : MonoBehaviour
    {
        LODGroup ld;

        private void Start()
        {
            ld = GetComponent<LODGroup>();
        }

        private void Update()
        {
            foreach(var lod in ld.GetLODs())
            {
                var renderer = lod.renderers.First();
                //if (renderer.isVisible && !renderer.enabled)
                    renderer.enabled = true;
                //else if (!renderer.isVisible)
                    //renderer.enabled = false;
            }
        }
    }
//}