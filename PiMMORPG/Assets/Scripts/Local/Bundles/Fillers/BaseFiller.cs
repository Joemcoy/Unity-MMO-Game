using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Bundles.Fillers
{
    public abstract class BaseFiller : MonoBehaviour
    {
        public string Path;

        public abstract void Load();
        private void Start()
        {
            Load();
        }
    }
}