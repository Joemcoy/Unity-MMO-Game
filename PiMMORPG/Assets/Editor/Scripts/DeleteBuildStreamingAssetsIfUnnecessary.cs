using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;

public static class DeleteBuildStreamingAssetsIfUnnecessary
{
    [PostProcessBuild(100)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string streamingAssetsPath = null;

        switch (target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
                {
                    // windows and linux use "_Data" folder
                    string root = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), Path.GetFileNameWithoutExtension(pathToBuiltProject) + "_Data");
                    streamingAssetsPath = Path.Combine(root, "StreamingAssets");
                }
                break;
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                {
                    streamingAssetsPath = Path.Combine(pathToBuiltProject, "Contents");
                    streamingAssetsPath = Path.Combine(streamingAssetsPath, "Resources");
                    streamingAssetsPath = Path.Combine(streamingAssetsPath, "Data");
                    streamingAssetsPath = Path.Combine(streamingAssetsPath, "StreamingAssets");
                }
                break;
        }

        if (streamingAssetsPath == null || !Directory.Exists(streamingAssetsPath))
            return;

        Debug.LogFormat("Deleting the directory {0}!", streamingAssetsPath);
        Directory.Delete(streamingAssetsPath, true);
    }
}