using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.SceneManagement;
using UnityEditor;

public static class DisableRootSceneObjects
{
    [MenuItem("Scene/Disable root scene objects")]
    static void DisableObjects() { SetState(false); }

    [MenuItem("Scene/Enable root scene objects")]
    static void EnableObjects() { SetState(true); }

    static void SetState(bool state)
    {
        var scene = SceneManager.GetActiveScene();
        foreach (var obj in scene.GetRootGameObjects())
            if (state && !obj.activeInHierarchy || !state && obj.activeInHierarchy)
                obj.SetActive(state);
    }
}