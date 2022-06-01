using UnityEditor;
using UnityEditor.SceneManagement;

public static class Tools
{
    [MenuItem("Scene/Enlarge scene camera draw distance %#X")]
    static void Enlarge()
    {
        SceneView.currentDrawingSceneView.camera.farClipPlane = 1000000;
    }
}