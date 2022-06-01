using System;
using System.IO;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Local.Helper
{
    using UI;
    using Control;

    public class SceneHelper
    {
        static IEnumerator LoadScene(Func<AsyncOperation> Caller, Action Callback, bool DimissLoading = true)
        {
            yield return null;
            /*var loading = GameObject.FindObjectOfType<LoadingScreen>();
            if (loading == null)
                yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);*/
            if (SceneManager.GetActiveScene().buildIndex != 1)
                yield return SceneManager.LoadSceneAsync(1);
            var loading = GameObject.FindObjectOfType<LoadingScreen>();
            loading.Maximum = 1f;

            WorldControl.RemoveTrees();
            WorldControl.RemoveDrops();
            WorldControl.RemovePlayers();
                        
            var operation = Caller();

            operation.allowSceneActivation = false;
            while(!operation.isDone && operation.progress < 0.9f)
            {
                loading.Progress = operation.progress;
                yield return null;
            }
            
            operation.allowSceneActivation = true;
            while (!operation.isDone)
            {
                loading.Progress = operation.progress;
                yield return null;
            }
            loading.Progress = 1f;

            var scene = SceneManager.GetActiveScene();
            foreach (var obj in scene.GetRootGameObjects()) obj.SetActive(true);

            yield return null;
            try
            {
                if (Callback != null)
                    Callback();
            }
            catch(Exception ex) { Debug.LogException(ex); }
            yield return null;

            if (DimissLoading)
            {
                loading.Dimiss();
                yield return null;
            }
        }

        public static void LoadScene(int ID, Action Callback = null, bool DimissLoading = true)
        {            
            AsyncInvoker.Create(() => LoadScene(() => SceneManager.LoadSceneAsync(ID), Callback, DimissLoading));
        }

        public static void LoadScene(AssetBundle bundle, Action callback = null, bool DimissLoading = true)
        {
            AsyncInvoker.Create(() => LoadFromBundle(bundle, callback, DimissLoading));
        }

        static IEnumerator LoadFromBundle(AssetBundle bundle, Action callback = null, bool DimissLoading = true)
        {
            var path = Path.GetFileNameWithoutExtension(bundle.GetAllScenePaths()[0]);
            yield return LoadScene(() => SceneManager.LoadSceneAsync(path), callback, DimissLoading);
            bundle.Unload(false);
        }

        public static void LoadScene(string Name, Action Callback = null, bool DimissLoading = true)
        {
            AsyncInvoker.Create(() => LoadScene(() => SceneManager.LoadSceneAsync(Name), Callback, DimissLoading));
        }
    }
}