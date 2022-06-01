using System.Linq;
using UnityEngine;
using UnityEditor;

public class TextureSizeUtil : EditorWindow
{
    [MenuItem("Tools/Texture Size Util")]
    static void ShowUtil()
    {
        var window = GetWindow<TextureSizeUtil>();
        window.Show();
    }

    int selected = 0;
    int[] values = new[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

    private void OnGUI()
    {
        var textures = Selection.objects.OfType<Texture>();
        selected = EditorGUILayout.IntPopup("Size", selected, values.Select(v => v.ToString()).ToArray(), values, GUILayout.ExpandWidth(true));

        GUI.enabled = textures.Any();
        if (GUILayout.Button("Apply", GUILayout.ExpandWidth(true)))
        {
            foreach (var texture in textures)
            {
                var path = AssetDatabase.GetAssetPath(texture);

                TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                tImporter.mipmapEnabled = true;
                tImporter.isReadable = true;
                tImporter.maxTextureSize = selected;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
        GUI.enabled = true;
    }
}