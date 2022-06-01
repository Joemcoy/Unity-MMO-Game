using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using uObject = UnityEngine.Object;

namespace Scripts.Local.Bundles
{
    using Helper;
    using UI;
    using Control;
    using System.Linq;

    public class SceneTag : MonoBehaviour
    {
        public string Tag;
    }

    public class BundleLoader : SingletonBehaviour<BundleLoader>
    {
        //Dictionary<string, GameObject> Prefabs;
        Dictionary<string, string> scenes;
        Dictionary<string, ObjectHolder> assets;
        List<AssetBundle> toUnload;

        static AssetBundle sceneBundle;
        public static string CurrentBScene { get; private set; }

        public override void Created()
        {
            base.Created();
            //Prefabs = new Dictionary<string, GameObject>();
            toUnload = new List<AssetBundle>();
            assets = new Dictionary<string, ObjectHolder>();
            scenes = new Dictionary<string, string>();
        }

        private void OnDestroy()
        {
            Debug.Log("Unloading bundles");

            /*if (Prefabs != null)
                Prefabs.Clear();*/

            if (assets != null)
            {
                foreach (var holder in assets.Values)
                {
                    Destroy(holder);
                }
                assets.Clear();
            }

            if (scenes != null)
                scenes.Clear();

            foreach (var bundle in toUnload)
                bundle.Unload(false);
            toUnload.Clear();

            Resources.UnloadUnusedAssets();
            Caching.ClearCache();
        }

        public static string GetTargetPath()
        {
#if UNITY_EDITOR
            var Target = Path.Combine(Environment.CurrentDirectory, "Build");
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneLinuxUniversal:
                case UnityEditor.BuildTarget.StandaloneLinux:
                    Target = Path.Combine(Target, "Linux");
                    break;
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    Target = Path.Combine(Target, "Linux64");
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows:
                    Target = Path.Combine(Target, "Windows");
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    Target = Path.Combine(Target, "Windows64");
                    break;
                case UnityEditor.BuildTarget.StandaloneOSXIntel:
                    Target = Path.Combine(Target, "OSXIntel");
                    break;
                case UnityEditor.BuildTarget.StandaloneOSXIntel64:
                    Target = Path.Combine(Target, "OSXIntel64");
                    break;
                case UnityEditor.BuildTarget.StandaloneOSX:
                    Target = Path.Combine(Target, "OSX");
                    break;
            }
#else
                var Target = Environment.CurrentDirectory;
#endif
            //Target = Path.Combine(Target, "Bundles");
            return Target;
        }

        /*static IEnumerator Register(string FilePath)
        {
            yield return null;

            var Result = AssetBundle.LoadFromFileAsync(FilePath);
            yield return Result;

            var bundle = Result.assetBundle;
            if (bundle.isStreamedSceneAssetBundle)
            {
                Instance.Scenes[Result.assetBundle.name] = FilePath;
                Result.assetBundle.Unload(true);
            }
            else
                Instance.Bundles[Result.assetBundle.name] = bundle;
            //

            yield return null;
        }

        public static IEnumerator RegisterBundle(string FilePath)
        {
            var Ext = Path.GetExtension(FilePath);
            if (Ext != ".hash" && Ext != ".manifest" && Path.GetFileName(FilePath) != "Bundles")
                yield return Instance.StartCoroutine(Register(FilePath));
        }*/
        
        /*static IEnumerator load(string bundle, string name)
        {
            var op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!op.isDone)
                yield return null;

            yield return null;
            var scene = SceneManager.GetSceneByName(name);

            var objs = Instance.scenes[bundle] = new SceneTag[scene.rootCount];
            int i = 0;
            foreach (var obj in scene.GetRootGameObjects())
                (objs[i++] = obj.AddComponent<SceneTag>()).Tag = name;
            yield return null;
        }*/

        public static bool RegisterBundle(string path, AssetBundle bundle)
        {
            if (bundle != null)
            {
                if (bundle.isStreamedSceneAssetBundle)
                {
                    if (Application.KeepScenes)
                    {
                        var name = Path.GetFileNameWithoutExtension(bundle.GetAllScenePaths()[0]);
                        //Instance.StartCoroutine(load(bundle.name, name));
                        //Instance.scenes[bundle.name] = path;
                        Instance.scenes[bundle.name] = name;
                        Instance.toUnload.Add(bundle);
                    }
                    else
                    {
                        Instance.scenes[bundle.name] = path;
                        bundle.Unload(true);
                    }
                }
                else
                {
                    var asset = bundle.LoadAsset(bundle.GetAllAssetNames().First());
                    RegisterAsset(bundle.name, asset);

                    if (!(asset as GameObject))
                        bundle.Unload(false);
                    else
                        Instance.toUnload.Add(bundle);
                }
                return true;
            }
            return false;
        }

        public static void RegisterAsset(string name, uObject asset)
        {            
            var o = new GameObject(string.Format("Holder of ({0})", asset.name));
            o.SetActive(false);
            o.transform.SetParent(Instance.transform);
            var h = o.AddComponent<ObjectHolder>();

            if (asset is AudioClip)
            {
                var clip = asset as AudioClip;
                if (clip.loadState == AudioDataLoadState.Unloaded || clip.loadState == AudioDataLoadState.Failed)
                    clip.LoadAudioData();

                var real = AudioClip.Create("Clone of " + clip.name, clip.samples, clip.channels, clip.frequency, false);
                var samples = new float[clip.samples * clip.channels];
                clip.GetData(samples, 0);
                real.SetData(samples, 0);

                asset = real;
            }
            else if (asset is GameObject)
            {
                /*var prefab = Instantiate(asset) as GameObject;
                prefab.transform.SetParent(o.transform);
                //yield return new WaitForFixedUpdate();
                prefab.SetActive(false);
                prefab.name = prefab.name.Replace("(Clone)", string.Empty);

                prefab.SetActive(false);
                asset = prefab;*/
                (asset as GameObject).SetActive(false);
            }
            h.Object = asset;

            Instance.assets[name] = h;
        }

        static bool IsBlacklisted(uObject asset)
        {
            var list = new[] { typeof(AudioClip), typeof(Material), typeof(Texture), typeof(Shader) };
            return !list.Any(t => asset.GetType().Equals(t));
        }

        public static GameObject LoadPrefab(string Name)
        {
            //GameObject prefab = null;
            //return Instance.Prefabs.TryGetValue(Name, out prefab) ? prefab : null;

            return LoadAsset(Name) as GameObject;
        }

        public static uObject LoadAsset(string Name)
        {
            ObjectHolder holder = null;
            return Instance.assets.TryGetValue(Name, out holder) ? holder.Object : null;
        }

        public static TAsset LoadAsset<TAsset>(string Name) where TAsset : class
        {
            return LoadAsset(Name) as TAsset;
        }

        public static uObject[] LoadAssetsOf(string path)
        {
            return Instance.assets.Keys.Where(k => k.StartsWith(path)).Select(k => Instance.assets[k].Object).ToArray();
        }

        public static TAsset[] LoadAssetsOf<TAsset>(string path) where TAsset : uObject
        {
            var objects =  LoadAssetsOf(path);
            return objects.Select(o => o is GameObject ? (o as GameObject).GetComponent<TAsset>() : o as TAsset).ToArray();
        }

        public static string[] GetChildrens(string path)
        {
            return Instance.assets.Keys.Where(k => k.StartsWith(path)).ToArray();
        }

        static IEnumerator Load(string FilePath, Action Callback, bool DimissLoading = true)
        {
            yield return null;
            if(sceneBundle != null)
            {
                sceneBundle.Unload(true);
                sceneBundle = null;
            }

            var Result = AssetBundle.LoadFromFileAsync(Path.Combine(GetTargetPath(), FilePath));
            yield return Result;

            sceneBundle = Result.assetBundle;
            SceneHelper.LoadScene(sceneBundle, Callback, DimissLoading);
            yield return null;
        }

        public static void LoadScene(string sceneName, Action callback = null, bool dimiss = true)
        {
            //Instance.StartCoroutine(LoadAsync(sceneName, callback, dimiss));

            var key = string.Format("scenes/{0}", sceneName);
            string name = null;
            if (Instance.scenes.TryGetValue(key, out name))
                if (Application.KeepScenes)
                    SceneHelper.LoadScene(name, callback, dimiss);
                else
                    Instance.StartCoroutine(Load(name, callback, dimiss));
            else
                Debug.LogErrorFormat("Scene {0} hast found!", sceneName);
        }

        static IEnumerator LoadAsync(string sceneName, Action callback, bool dimiss)
        {
            var key = string.Format("scenes/{0}", sceneName);
            string FilePath;

            if (Instance.scenes.TryGetValue(key, out FilePath))
            {
                CurrentBScene = sceneName;
                yield return Instance.StartCoroutine(Load(FilePath, callback, dimiss));
            }
            else
                Debug.LogErrorFormat("Scene {0} hasn't found!", sceneName);

            //var loading = GameObject.FindObjectOfType<LoadingScreen>();
            //if (loading == null)
            //    yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            //loading = GameObject.FindObjectOfType<LoadingScreen>();

            //WorldControl.RemoveTrees();
            //WorldControl.RemoveDrops();
            //WorldControl.RemovePlayers();

            //if (!string.IsNullOrEmpty(CurrentBScene))
            //{
            //    Debug.LogFormat("Disabling {0}!", CurrentBScene);
            //    foreach (var tag in Instance.scenes[CurrentBScene])
            //        tag.gameObject.SetActive(false);
            //}

            //foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects())
            //    if (obj.GetComponent<SceneTag>() == null)
            //        Destroy(obj);
            //CurrentBScene = key;

            //SceneTag[] tags = null;

            //if (Instance.scenes.TryGetValue(key, out tags))
            //{
            //    loading.Maximum = tags.Length;
            //    var name = tags[0].Tag;

            //    if (!SceneManager.SetActiveScene(SceneManager.GetSceneByName(name)))
            //        Debug.LogWarning("Failed to set the current scene!");

            //    foreach (var tag in tags)
            //    {
            //        tag.gameObject.SetActive(true);
            //        tag.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
            //        //tag.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
            //        loading.Progress++;
            //    }
            //}
            //else
            //    loading.InfoText = "Failed to load the scene!";

            //if (callback != null)
            //    callback();
            //if (dimiss)
            //    LoadingScreen.Instance.Dimiss();
        }
    }
}