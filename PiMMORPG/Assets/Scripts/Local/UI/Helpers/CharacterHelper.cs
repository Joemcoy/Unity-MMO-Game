using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MarkLight;
using UnityEngine;

using PiMMORPG.Models;
using Scripts.Local.Triggers;
using Scripts.Local.Inventory;
using Scripts.Local.Control;
using Scripts.Local.Locomotion;
using Devdog.InventoryPro;
using Devdog.General;

namespace Scripts.Local.UI.Helpers
{
    public class CharacterHelper : MonoBehaviour
    {
        public MorphEquipTrigger Current;
        public Vector3 Position, CameraPosition;
        public Quaternion CameraRotation;
        public _int HairIndex;
        new GameObject camera;

        Action callback;
        bool spawning = false;

        void Start()
        {
            HairIndex = new _int { Value = -1 };
            HairIndex.ValueSet += HairIndex_ValueSet;

            if(camera == null || camera.gameObject.activeInHierarchy)
            {
                camera = WorldControl.SpawnCreatorCamera();

                if (camera)
                {
                    WorldControl.SpawnClimate(camera);
                    camera.transform.position = CameraPosition;
                    camera.transform.rotation = CameraRotation;
                    camera.SetActive(true);
                }
            }
        }

        private void OnDestroy()
        {
            if (Current != null)
                Current.gameObject.SetActive(false);
        }

        private void HairIndex_ValueSet(object sender, EventArgs e)
        {
            if (Current != null && Current.Initalized)
                Current.Setter.SetHair(Convert.ToInt16(HairIndex.Value), () => { callback(); callback = null; });
        }

        public void UpdateHair()
        {
            HairIndex_ValueSet(this, EventArgs.Empty);
        }

        public void SetHair(int Index, Action callback)
        {
            this.callback = callback;
            HairIndex.Value = Index;
        }

        /*public void Spawn(Character Character, Action Callback = null)
        {
            StartCoroutine(AsyncSpawn(Character, Callback));
        }*/

        public void Spawn(bool IsFemale, Func<IEnumerator> Callback = null)
        {
            StartCoroutine(AsyncSpawn(IsFemale, Callback));
        }

        /*IEnumerator AsyncSpawn(Character Character, Action Callback)
        {
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(AsyncSpawn(Character.IsFemale, Callback));
            yield return new WaitForEndOfFrame();
        }*/

        IEnumerator AsyncSpawn(bool IsFemale, Func<IEnumerator> Callback = null)
        {
            yield return new WaitForEndOfFrame();

            /*var Overlay = FindObjectOfType<OverlayHelper>();
            Overlay.Show();
            while (!Overlay.End)
                yield return new WaitForEndOfFrame();*/

            spawning = false;
            yield return StartCoroutine(AsyncDespawn());
            spawning = true;

            var prefab = WorldControl.SpawnPlayer(IsFemale, Position, Quaternion.identity);
            Destroy(prefab.GetComponent<KeyboardLocomotor>());
            Destroy(prefab.GetComponent<InventoryPlayer>());
            Destroy(prefab.GetComponent<Player>());

            var trigger = prefab.GetComponentInChildren<PlayerTriggerHandler>();
            if (trigger != null)
                Destroy(trigger.gameObject);

            foreach (var eName in prefab.GetComponentsInChildren<EntityName>(true))
                Destroy(eName.gameObject);

            //prefab.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            Current = prefab.GetComponent<MorphEquipTrigger>();
            Current.Init(false);
            //Current.Reset();
            //Current.UnEquipAll();
            yield return null;
            //Current = Prefabs[Index];
            prefab.transform.rotation = Quaternion.identity;
            prefab.gameObject.SetActive(true);

            for(int i = 0; i < 15; i++) //wait for 15 frames
                yield return null;

            if (Callback != null)
                yield return StartCoroutine(Callback());
            spawning = false;

            /*Overlay.Hide();
            while (!Overlay.End)
                yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();*/
        }

        public void Despawn()
        {
            StartCoroutine(AsyncDespawn());
        }

        IEnumerator AsyncDespawn()
        {
            while (spawning)
                yield return new WaitForEndOfFrame();

            if (Current != null)
            {
                Current.gameObject.SetActive(false);
                Current.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);

                if (camera)
                {
                    camera.transform.position = CameraPosition;
                    camera.transform.rotation = CameraRotation;
                }
            }
        }
    }
}