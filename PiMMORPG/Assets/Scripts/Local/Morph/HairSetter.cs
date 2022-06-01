using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MORPH3D;
using Scripts.Local.Bundles;
using MORPH3D.COSTUMING;

namespace Scripts.Local.Morph
{
    [RequireComponent(typeof(M3DCharacterManager))]
    public class HairSetter : MonoBehaviour
    {
        public string[] Hairs;

        public short CurrentIndex = -1;
        public HairColor Current { get; private set; }

        private M3DCharacterManager Manager;
        private short Last = 0;
        private Action callback;
        private Dictionary<string, CIhair> prefabs;

        public void SetHair(short Index, Action Callback = null)
        {
            CurrentIndex = Index;
            callback = Callback;
        }

        void Start()
        {
            Manager = GetComponent<M3DCharacterManager>();
            prefabs = new Dictionary<string, CIhair>();
        }

        void Reset()
        {
            CurrentIndex = -1;
            Update();
        }

        void Update()
        {
            if (CurrentIndex != Last || CurrentIndex > -1 && (!Manager.GetVisibleHair().Any() || Manager.GetVisibleHair().First().ID != prefabs[Hairs[CurrentIndex]].ID))
            {
                Last = CurrentIndex;

                try
                {
                    foreach (var hair in Manager.GetAllHair())
                        if (hair.isVisible)
                            hair.SetVisibility(false);
                }
                finally
                {
                    if (CurrentIndex > -1 && CurrentIndex < Hairs.Length)
                    {
                        var hName = Hairs[CurrentIndex];

                        CIhair hair;
                        if (!prefabs.TryGetValue(hName, out hair))
                        {
                            var prefab = BundleLoader.LoadPrefab("models/characters/hair/" + hName);
                            hair = prefabs[hName] = prefab.GetComponent<CIhair>();
                        }

                        var attached = Manager.GetHairByID(hair.ID);
                        if (attached == null)
                        {
                            Manager.AddContentPack(new ContentPack(hair.gameObject));
                            attached = Manager.GetHairByID(hair.ID);
                        }
                        attached.SetVisibility(true);
                        if (!attached.gameObject.activeInHierarchy)
                            attached.gameObject.SetActive(true);

                        Current = attached.GetComponent<HairColor>();
                        if (callback != null)
                        {
                            callback();
                            callback = null;
                        }
                    }
                }
            }
        }
    }
}