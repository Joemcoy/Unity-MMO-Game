using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.ImageEffects;

using PiMMORPG.Client;
using PiMMORPG.Models;

using Devdog.InventoryPro;
using Tree = PiMMORPG.Models.Tree;

using tFramework.Factories;
using tFramework.Extensions;

namespace Scripts.Local.Control
{
    using UI;
    using Bundles;
    using Locomotion;
    using Local.Triggers;
    using Network.Requests.GameClient;

    using Interfaces;
    using Inventory;
    using Climate;
    using Helper;

    public class WorldControl : SingletonBehaviour<WorldControl>
    {
        class Pool
        {
            public GameObject prefab;
            public bool isfree = true;
            public int id = 0;
            public int position = 0;
        }

        public const string PlayerNameFormat = "Player - {0}";
        Pool[] allPools;
        Dictionary<string, Pool[]> poolReference;

        GameObject treeContainer, dropContainer, poolContainer;

        public override void Created()
        {
            base.Created();
            (treeContainer = new GameObject("Tree Container")).transform.SetParent(transform);
            (dropContainer = new GameObject("Drop Container")).transform.SetParent(transform);
            (poolContainer = new GameObject("Pool Container")).transform.SetParent(transform);
        }

        public override void Destroyed()
        {
            base.Destroyed();
            /*if (Players != null)
            {
                foreach (var Player in Players.Values)
                    Destroy(Player);
                Players.Clear();
            }*/
            Debug.Log("Destroying prefabs!");
            if (poolReference != null)
            {
                allPools.ForEach(p => Destroy(p.prefab));
                poolReference.Clear();
            }
        }

        public static void DisablePools()
        {
            Instance.allPools.ForEach(m =>
            {
                foreach (var Trigger in m.prefab.GetComponents<NetworkTriggerBase>())
                    Trigger.UnloadEvents();

                m.prefab.SetActive(false);
                m.isfree = true;
                m.id = m.position;
            });
        }

        public static void PreparePool(int poolSize)
        {
            poolSize += 1;

            if (Instance.poolReference == null)
                Instance.poolReference = new Dictionary<string, Pool[]>();
            else
            {
                if (poolSize <= Instance.poolReference.Count)
                    return;
            }

            var childrens = BundleLoader.GetChildrens("models/characters/online");
            Instance.allPools = new Pool[childrens.Length * poolSize];

            int n = 0;
            foreach (var ppath in childrens)
            {
                var prefab = BundleLoader.LoadPrefab(ppath);
                var pools = new Pool[poolSize];
                for (int i = 0; i < poolSize; i++)
                {
                    var pool = pools[i] = Instance.allPools[n] = new Pool();
                    pool.position = n++;

                    var instance = pool.prefab = Instantiate(prefab) as GameObject;
                    instance.SetActive(false);
                    TransformHelper.SetParent(Instance.poolContainer.transform, instance.transform);
                    instance.transform.position = Vector3.one * -9999;
                }
                Instance.poolReference[ppath] = pools;
            }
        }

        static Pool GetFreePool(string path, Character player)
        {
            foreach (var pool in Instance.poolReference[path].Skip(1))
            {
                if (pool.isfree)
                {
                    pool.isfree = false;
                    pool.id = player != null ? Convert.ToInt32(player.ID) : 0;
                    //pool.id = player == null ? -1 : Convert.ToInt32(player.ID);
                    return pool;
                }
            }
            return null;
        }

        public static GameObject SpawnPlayer(bool IsFemale, Vector3 position, Quaternion rotation, Character player = null)
        {
            var path = "models/characters/online/human/" + (IsFemale ? "female" : "male");
            var state = player == null ? Instance.poolReference[path][0] : GetFreePool(path, player);
            if (state != null)
            {
                //state.isfree = false;
                var instance = state.prefab;

                instance.name = string.Format(PlayerNameFormat, path + "/" + Convert.ToString(state.id));
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                //instance.transform.SetParent(Instance.playerContainer.transform);
                instance.SetActive(true);
                return instance as GameObject;
            }
            return null;
        }

        public static void SpawnPlayer(Character player, bool isLocal)
        {
            var instance = SpawnPlayer(player.IsFemale, player.Position.Vector, player.Position.Quaternion, player);

            instance.tag = "Player";
            instance.layer = LayerMask.NameToLayer(isLocal ? "Local" : "Remote");
            instance.transform.position = player.Position.Vector;
            instance.transform.rotation = player.Position.Quaternion;

            var Locomotor = instance.GetComponent<KeyboardLocomotor>();
            Locomotor.Online = isLocal;

            var nPrefab = Instantiate(BundleLoader.LoadPrefab("prefabs/name"));
            var name = nPrefab.GetComponentInChildren<EntityName>(true);
            name.Name = player.Name;

            var nRect = nPrefab.GetComponent<RectTransform>();
            TransformHelper.SetParent(instance.transform, nPrefab.transform);
            nRect.localPosition = new Vector3(0, 1.5f, 0);

            var iPlayer = isLocal ? instance.GetComponent<NetworkInventoryPlayer>() ?? instance.AddComponent<NetworkInventoryPlayer>() : null;
            var morph = instance.GetComponent<MorphEquipTrigger>();
            morph.CopyFrom(player.Style);

            if (isLocal)
            {
                var camera = SpawnCamera();
                camera.SetActive(true);
                SpawnClimate(camera, instance);
                SpawnClock();

                var helper = InventoryHelper.Instance;
                helper.Enabled = true;

                iPlayer.dontDestroyOnLoad = false;
                helper.CopyTo(iPlayer);

                iPlayer.Init();
                iPlayer.stats.Get("Default", "Gender").SetCurrentValueRaw(player.IsFemale ? 1 : 0);
            }
            foreach (var Trigger in instance.GetComponents<NetworkTriggerBase>())
            {
                Trigger.Init(isLocal);
                Trigger.LoadEvents();
            }

            foreach (var item in player.Items)
                AddItem(morph, item, isLocal);
            if (isLocal)
            {
                PiBaseClient.IsLoaded = true;
            }

            instance.SetActive(true);
            nPrefab.SetActive(true);
        }

        public static void AddItem(MorphEquipTrigger morph, CharacterItem item, bool isLocal)
        {
            var ritem = ItemManager.database.items.FirstOrDefault(i => i.ID == item.Info.InventoryID);
            if (ritem != null)
            {
                /*if (isLocal)
                    (ritem as INetworkItem).Serial = item.Serial;

                if (item.Info.IsEquip && item.Equipped)
                {
                    var equip = ritem as NetworkEquippableItem;
                    if (isLocal)
                    {
                        var slot = morph.Player.equipmentBinders.First(i => i.equippableSlot.equipmentTypes.Contains(equip.equipmentType)).equippableSlot;
                        morph.Player.characterCollection.EquipItem(slot, equip);
                    }
                    else
                        morph.Equip(equip);
                }
                else if (isLocal)
                {
                    //if (ritem.maxStackSize > 1)
                        ritem = Instantiate(ritem);
                    (ritem as INetworkItem).Serial = item.Serial;
                    ritem.index = item.Slot;
                    ritem.currentStackSize = item.Quantity;

                    if (item.HotbarSlot > -1)
                        morph.Player.skillbarCollection.SetItem(Convert.ToUInt32(item.HotbarSlot), ritem, true);
                    else
                    {
                        var inventory = morph.Player.inventoryCollections[item.Info.Type.InventoryID];
                        /*if (inventory.CanSetItem(item.Slot, ritem))
                            inventory.SetItem(item.Slot, ritem, true);
                        else
                        {
                            var slot = Convert.ToUInt32(inventory.FindFirstEmptySlot());
                            inventory.SetItem(slot, ritem, true);

                            var packet = new SetItemSlotRequest { Serial = item.Serial, Slot = slot };
                            SingletonFactory.GetSingleton<PiBaseClient>().Socket.Send(packet);
                        }*/

                /*if (inventory.CanAddItem(ritem))
                {
                    inventory.AddItem(ritem);

                    var aitem = inventory.Select(i => i.item).FirstOrDefault(i => i is INetworkItem && (i as INetworkItem).Serial == item.Serial);
                    if (aitem != null)
                        inventory.MoveItem(aitem, aitem.index, inventory, item.Slot, true);
                    else
                        Debug.LogErrorFormat("Item {0} not found!", item.Serial);
                }
            }
        }*/

                if (isLocal)
                {
                    if (ritem.maxStackSize > 1)
                        ritem = Instantiate(ritem);
                    (ritem as INetworkItem).Serial = item.Serial;
                    ritem.index = item.Slot;
                    ritem.currentStackSize = item.Quantity;

                    if (item.Equipped && item.Info.IsEquip)
                    {
                        var equip = ritem as NetworkEquippableItem;
                        var slot = morph.Player.equipmentBinders.First(i => i.equippableSlot.equipmentTypes.Contains(equip.equipmentType)).equippableSlot;
                        morph.Player.characterCollection.EquipItem(slot, equip);
                    }
                    else if (item.HotbarSlot > -1)
                    {
                        morph.Player.skillbarCollection.SetItem(Convert.ToUInt32(item.HotbarSlot), ritem, true);
                    }
                    else
                    {
                        var inventory = morph.Player.inventoryCollections[item.Info.Type.InventoryID];
                        inventory.AddItem(ritem);

                        var aitem = inventory.Select(i => i.item).FirstOrDefault(i => i is INetworkItem && (i as INetworkItem).Serial == item.Serial);
                        if (aitem != null)
                        {
                            if (!inventory.MoveItem(aitem, aitem.index, inventory, item.Slot, true))
                                Debug.LogErrorFormat("Failed to move item {0} from slot {1} to {2}!", aitem.name, aitem.index, item.Slot);
                        }
                        else
                            Debug.LogErrorFormat("Item {0} not found!", item.Serial);
                    }
                }
                else
                    morph.Equip(ritem as NetworkEquippableItem);
            }
            else
                LoggerFactory.GetLogger<WorldControl>().LogWarning("Failed to add item {0}!", item.ID);
        }

        public static GameObject GetPlayer(Character player)
        {
            var id = player == null ? -1 : Convert.ToInt32(player.ID);
            return GetPlayer(id);
        }

        public static GameObject GetPlayer(uint id) { return GetPlayer(Convert.ToInt32(id)); }
        public static GameObject GetPlayer(int id)
        {
            Pool state = null;
            return GetPlayer(id, ref state);
        }

        static GameObject GetPlayer(int id, ref Pool refPool)
        {
            if (Instance.allPools != null)
            {
                foreach (var pool in Instance.allPools)
                {
                    if (!pool.isfree && pool.id == id)
                    {
                        refPool = pool;
                        return pool.prefab;
                    }
                }
            }
            return null;
        }

        public static void RemovePlayer(int ID)
        {
            Pool state = null;
            var player = GetPlayer(ID, ref state);
            if (player)
            {
                player.SendMessage("Reset");
                foreach (var Trigger in player.GetComponents<NetworkTriggerBase>())
                    Trigger.UnloadEvents();

                var name = player.GetComponentInChildren<EntityName>();
                if (name)
                    Destroy(name.gameObject);

                player.SetActive(false);
                state.id = state.position;
                state.isfree = true;
            }
            else
                Debug.LogFormat("Failed to destroy the character {0}!", ID);
        }

        public static void SpawnTree(Tree tree)
        {
            var prefab = BundleLoader.LoadAsset<GameObject>(tree.BundleName);
            if(prefab == null)
            {
                Debug.LogErrorFormat("Failed to spawn the tree {0}!", tree.BundleName);
                return;
            }
            var instance = GameObject.Instantiate(prefab);
            TransformHelper.SetParent(Instance.treeContainer.transform, instance.transform);
            instance.transform.position = new Vector3(tree.PositionX, tree.PositionY, tree.PositionZ);
            instance.SetActive(true);
        }

        public static void SpawnDrop(Drop drop)
        {
            InventorySettingsManager.instance.settings.itemTriggerOnPlayerCollision = false;

            var item = ItemManager.database.items.First(I => I.ID == drop.InventoryID);
            item.currentStackSize = drop.Quantity;
            (item as INetworkItem).Serial = drop.Serial;

            var pouch = Instantiate(InventoryHelper.Instance.DropPouch);
            TransformHelper.SetParent(Instance.dropContainer.transform, pouch.transform);
            pouch.transform.rotation = drop.Quaternion;

            var terrain = GameObject.FindGameObjectWithTag("MainTerrain");
            if (terrain)
            {
                var data = terrain.GetComponent<Terrain>().terrainData;
                var pos = drop.Vector;
                var add = data.GetHeight((int)pos.x, (int)pos.z);

                pos.y = add + pos.y + pouch.transform.localScale.y / 2;
                drop.Vector = pos;
            }
            pouch.transform.position = drop.Vector;

            pouch.GetComponent<ItemTrigger>().itemPrefab = item;
            pouch.GetComponent<DropInfo>().DropSerial = drop.Serial;
            pouch.GetComponent<Light>().color = item.rarity.color;
            pouch.SetActive(true);
        }

        public static GameObject SpawnCamera()
        {
            var prefab = BundleLoader.LoadPrefab("prefabs/camera");
            if (prefab)
            {
                var c = Instantiate(prefab);
                var sun = GameObject.FindGameObjectWithTag("Sun");
                if (sun)
                    c.GetComponent<SunShafts>().sunTransform = sun.transform;

                LoadEffects(c);
                c.SetActive(true);
                return c;
            }
            return null;
        }

        public static GameObject SpawnCreatorCamera()
        {
            var prefab = BundleLoader.LoadPrefab("prefabs/creator_camera");
            if (prefab)
            {
                var c = Instantiate(prefab);
                LoadEffects(c);
                c.SetActive(true);
                return c;
            }
            return null;
        }

        public static void LoadEffects(GameObject cameraObject)
        {
            var behaviour = cameraObject.GetComponent<PostProcessingBehaviour>();
            if (behaviour)
            {
                var graphics = Application.Configuration.Graphics;
                var profile = behaviour.profile;
                profile.antialiasing.enabled = graphics.AntiAliasing;
                profile.bloom.enabled = graphics.Bloom;
                profile.fog.enabled = graphics.Fog;
                profile.ambientOcclusion.enabled = graphics.AmbientOcclusion;
                profile.depthOfField.enabled = graphics.DepthOfField;
                profile.motionBlur.enabled = graphics.MotionBlur;
                profile.colorGrading.enabled = graphics.ColorGrading;
                profile.chromaticAberration.enabled = graphics.ChromaticAberration;
                profile.userLut.enabled = graphics.UserLut;
                profile.eyeAdaptation.enabled = graphics.EyeAdaption;
                profile.screenSpaceReflection.enabled = graphics.ScreenSpaceReflection;
            }
        }

        public static void SpawnClimate(GameObject cameraObject, GameObject player = null)
        {
            var climate = GameObject.FindGameObjectWithTag("Climate");
            if (climate != null)
            {
                var unistorm = climate.GetComponentInChildren<UniStormWeatherSystem_C>();
                var helper = Instantiate(BundleLoader.LoadPrefab("prefabs/climate")).GetComponent<UnistormHelper>();

                helper.transform.SetParent((player == null ? climate : player).transform, false);                
                helper.CopyTo(unistorm, cameraObject);
                helper.gameObject.SetActive(true);
                unistorm.enabled = true;
            }
        }

        public static void SpawnClock()
        {
            Instantiate(BundleLoader.LoadPrefab("prefabs/clock")).SetActive(true);
        }

        public static void RemoveTrees()
        {
            for (int i = 0; i < Instance.treeContainer.transform.childCount; i++)
                Destroy(Instance.treeContainer.transform.GetChild(i).gameObject);
        }

        public static void RemoveDrops()
        {
            for (int i = 0; i < Instance.dropContainer.transform.childCount; i++)
                Destroy(Instance.dropContainer.transform.GetChild(i).gameObject);
        }

        public static void RemovePlayers()
        {
            if(Instance.allPools != null)
            Instance.allPools.ForEach(a =>
            {
                a.isfree = true;
                a.id = a.position;
                a.prefab.SetActive(false);
            });
        }
    }
}