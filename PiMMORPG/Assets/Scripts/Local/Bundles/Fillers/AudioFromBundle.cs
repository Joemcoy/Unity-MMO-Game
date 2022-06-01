using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Bundles.Fillers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioFromBundle : BaseFiller
    {
        bool flag = false;

        public override void Load()
        {
            var source = GetComponent<AudioSource>();
            var state = gameObject.activeInHierarchy;

            flag = true;
            gameObject.SetActive(false);
            source.enabled = false;
            source.clip = null;
            source.clip = BundleLoader.LoadAsset<AudioClip>("musics/" + Path);
            source.enabled = true;
            gameObject.SetActive(state);
            flag = false;
        }

        private void OnEnable()
        {
            if(!flag)
                Load();
        }
    }
}