using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;
using uObject = UnityEngine.GameObject;

using UnityEditor;
using tFramework.Extensions;
using Scripts.Local;
using System.Collections;
using Scripts.Local.Bundles;
using UnityEditor.IMGUI.Controls;


public class AssetBundleBuilder : EditorWindow
{
    public const BuildAssetBundleOptions Options = BuildAssetBundleOptions.StrictMode | BuildAssetBundleOptions.UncompressedAssetBundle;

    [MenuItem("Assets/Convert variant to bundles")]
    public static void Convert()
    {
        var bundles = AssetDatabase.GetAllAssetBundleNames();
        foreach (var bundle in bundles)
        {
            if (bundle.StartsWith("materials/hair"))
            {
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPathsFromAssetBundle(bundle)[0]);
                if (!string.IsNullOrEmpty(importer.assetBundleVariant))
                    importer.SetAssetBundleNameAndVariant(string.Format("{0}/{1}", importer.assetBundleName, importer.assetBundleVariant), null);
            }
        }
    }

    [MenuItem("Assets/Build Asset Bundles")]
    public static void Build()
    {
        var Window = GetWindow<AssetBundleBuilder>("Asset Bundle Builder");
        Window.Show();
    }

    Vector2 ScrollPosition = Vector2.zero;
    //Dictionary<string, bool> States;
    ABBuilderTreeView tree;
    TreeViewState state;
    MultiColumnHeader header;

    private void OnEnable()
    {
        if (state == null)
            state = new TreeViewState();
        tree = new ABBuilderTreeView(state, ABBuilderTreeView.CreateDefaultHeader(ref header));
    }

    internal static ABTreeRow GetRow(string name, ABTreeRow parent = null)
    {
        var first = name.Split('/').FirstOrDefault();

        if (first == null)
            return parent;

        foreach (var row in (parent == null ? GetWindow<AssetBundleBuilder>().tree.GetRows() : parent.children).OfType<ABTreeRow>())
        {
            if (row.displayName == first && row.enabled)
                return GetRow(name.Substring(first.Length + 1), row);
        }
        return null;
    }

    internal static void TestAsset(string name)
    {
        var row = GetRow(name);
        if (row != null)
            TestAsset(row);
    }

    internal static void TestAsset(ABTreeRow row) { AsyncInvoker.Create(() => ATestAsset(row)); }
    internal static IEnumerator ATestAsset(ABTreeRow row)
    {
        var Target = Path.Combine(BundleLoader.GetTargetPath(), "Bundles");
        if (row.enabled)
        {
            var Real = Path.Combine(Target, row.fullpath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(Real))
            {
                var request = AssetBundle.LoadFromFileAsync(Real);
                yield return request;

                var bundle = request.assetBundle;
                if (bundle.isStreamedSceneAssetBundle)
                {
                    foreach (var Scene in bundle.GetAllScenePaths())
                        Debug.LogFormat("Scene: {0}", Scene);
                }
                else
                {
                    foreach (var Asset in bundle.LoadAllAssets())
                        Debug.LogFormat("Asset: {0}", Asset);
                }
                bundle.Unload(true);
            }
            else
                Debug.LogWarningFormat("The bundle {0} not exists on path {1}!", row.fullpath, Real);
        }
    }

    internal static void LoadAsset(ABTreeRow row) { AsyncInvoker.Create(() => ALoadAsset(row)); }
    internal static IEnumerator ALoadAsset(ABTreeRow row)
    {
        var Target = Path.Combine(BundleLoader.GetTargetPath(), "Bundles");
        if (row.enabled)
        {
            var Real = Path.Combine(Target, row.fullpath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(Real))
            {
                var request = AssetBundle.LoadFromFileAsync(Real);
                yield return request;

                var bundle = request.assetBundle;
                if (bundle.isStreamedSceneAssetBundle)
                    Debug.LogWarningFormat("Bundle {0} is an scene bundle!", bundle.name);
                else
                {
                    var aRequest = bundle.LoadAllAssetsAsync();
                    yield return aRequest;

                    var asset = aRequest.allAssets[0];
                    if (asset as GameObject)
                    {
                        GameObject.Instantiate(asset, Vector3.zero, Quaternion.identity);
                        bundle.Unload(false);
                    }
                    else
                    {
                        Debug.Log(asset);
                        bundle.Unload(true);
                    }
                }
            }
            else
                Debug.LogWarningFormat("The bundle {0} not exists on path {1}!", row.fullpath, Real);
        }
    }

    void SetRowsState(bool state, ABTreeRow parent = null)
    {
        foreach (var row in (parent == null ? tree.GetRows() : parent.children).OfType<ABTreeRow>())
        {
            row.enabled = state;

            if (row.hasChildren)
                SetRowsState(state, row);
        }
    }

    internal static void BuildRow(ABTreeRow row)
    {
        if (row.hasChildren)
        {
            foreach (var children in row.children.OfType<ABTreeRow>())
                if (children.enabled)
                    BuildRow(children);
        }
        else if(row.enabled)
        {
            var target = Path.Combine(BundleLoader.GetTargetPath(), "Bundles");
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);

            var assets = AssetDatabase.GetAssetPathsFromAssetBundle(row.fullpath);

            if (assets.Length > 0)
            {
                BuildPipeline.BuildAssetBundles(target, GenerateBuildData(row.fullpath, assets[0]), Options, EditorUserBuildSettings.activeBuildTarget);

                File.Delete(Path.Combine(target, "Bundles"));
                foreach (var Manifest in Directory.GetFiles(target, "*.manifest", SearchOption.AllDirectories))
                    File.Delete(Manifest);
            }
        }
    }

    void BuildAll()
    {
        foreach (var row in tree.GetRows().OfType<ABTreeRow>())
        {
            if (row.enabled)
                BuildRow(row);
        }
    }

    void OnGUI()
    {
        var Target = Path.Combine(BundleLoader.GetTargetPath(), "Bundles");

        if (!Directory.Exists(Target))
            Directory.CreateDirectory(Target);

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("Total of a {0} bundles!", AssetDatabase.GetAllAssetBundleNames().Length), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build selected bundles", GUILayout.ExpandWidth(true)))
        {
            BuildAll();
        }

        if (GUILayout.Button("Test selected bundles", GUILayout.ExpandWidth(true)))
        {
            foreach (var asset in AssetDatabase.GetAllAssetBundleNames())
                TestAsset(asset);
        }

        var loaded = AssetBundle.GetAllLoadedAssetBundles();

        GUI.enabled = loaded.Any();
        if (GUILayout.Button("Unload all", GUILayout.ExpandWidth(true)))
        {
            foreach (var bundle in loaded)
                bundle.Unload(true);
            Resources.UnloadUnusedAssets();
            GC.Collect(GC.MaxGeneration);
        }
        GUI.enabled = true;
        if (GUILayout.Button("Force unload all", GUILayout.ExpandWidth(true)))
        {
            AssetBundle.UnloadAllAssetBundles(true);

            Resources.UnloadUnusedAssets();
            GC.Collect(GC.MaxGeneration);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select all"))
            SetRowsState(true);

        if (GUILayout.Button("Deselect all"))
            SetRowsState(false);

        if (GUILayout.Button("Reload tree"))
            tree.Reload();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        var rect = GUILayoutUtility.GetLastRect();

        if (state == null) state = new TreeViewState();
        if (tree == null) tree = new ABBuilderTreeView(state, ABBuilderTreeView.CreateDefaultHeader(ref header));
        tree.OnGUI(new Rect(rect.x, rect.y, position.width, position.height - rect.y));
    }

    internal static AssetBundleBuild[] GenerateBuildData(string name, string path)
    {
        var lastDot = name.LastIndexOf('.');
        var realName = lastDot > -1 ? name.Substring(0, lastDot) : name;
        var variant = lastDot > -1 ? name.Substring(lastDot + 1) : string.Empty;

        return new AssetBundleBuild[1]
        {
            new AssetBundleBuild
            {
                assetBundleName = realName,
                assetBundleVariant = variant,
                assetNames = new[] { path }
            }
        };
    }
}