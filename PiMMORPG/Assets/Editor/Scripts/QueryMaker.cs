using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using uObject = UnityEngine.Object;
using UnityEngine;
using UnityEditor;
using Devdog.InventoryPro;

public class QueryMaker : EditorWindow
{
    [MenuItem("Tools/QueryMaker")]
    static void ShowWindow()
    {
        GetWindow<QueryMaker>("Query Maker").Show();
    }

    interface ITool
    {
        string Title { get; }
        string Generate();
        void DrawFields();
    }

    public class ItemQuery : ITool
    {
        public string Title { get { return "Items"; } }

        public string Generate()
        {
            var baseQuery = "INSERT INTO items (InventoryID) VALUES ({0});";
            var query = "";

            foreach (var item in ItemManager.database.items)
                query += string.Format(baseQuery, item.ID) + Environment.NewLine;
            return query;
        }

        public void DrawFields() { }
    }

    public class TreeQuery : ITool
    {
        public string Title { get { return "Trees"; } }

        Terrain current;
        string mapid;

        public string Generate()
        {
            if (current == null)
                return "Set a target terrain!";

            var baseQuery = "INSERT INTO trees (BundleName, Map, PositionX, PositionY, PositionZ, Serial) VALUES ('{0}', {1}, {2}, {3}, {4}, '{5}');";
            var query = "";

            var data = current.terrainData;
            foreach (var tree in data.treeInstances)
            {
                var prototype = data.treePrototypes[tree.prototypeIndex];
                var name = AssetDatabase.GetImplicitAssetBundleName(AssetDatabase.GetAssetPath(prototype.prefab));
                var position = new Vector3(tree.position.x * data.size.x, tree.position.y * data.size.y, tree.position.z * data.size.z) + current.transform.position;
                query += string.Format(baseQuery, name, mapid, position.x, position.y, position.z, Guid.NewGuid()) + Environment.NewLine;
            }
            return query;
        }

        public void DrawFields()
        {
            current = EditorGUILayout.ObjectField("Terrain", current, typeof(Terrain), true) as Terrain;
            mapid = EditorGUILayout.TextField("Map ID", mapid);
        }
    }

    public class PositionQuery : ITool
    {
        public string Title { get { return "Position"; } }

        string table, mapid;

        public string Generate()
        {
            var baseQuery = "INSERT INTO {0} (MapID, PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ, RotationW) VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});";
            var query = "";

            if (Selection.transforms.Length == 0) return "Select an object of scene!";
            foreach(var transform in Selection.transforms)
            {
                var p = transform.localPosition;
                var r = transform.localRotation;
                query += string.Format(baseQuery, table, mapid, p.x, p.y, p.z, r.x, r.y, r.z, r.w) + Environment.NewLine;
            }
            return query;
        }

        public void DrawFields()
        {
            table = EditorGUILayout.TextField("Table", table);
            mapid = EditorGUILayout.TextField("Map ID", mapid);
        }
    }

    List<ITool> tools;
    int current = 0;
    string content = "";
    Vector2 scroll = Vector2.zero;

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        if (tools == null || GUILayout.Button("Refresh tools", GUILayout.ExpandWidth(true)))
        {
            current = 0;
            tools = new List<ITool>();
            foreach(var type in typeof(QueryMaker).Assembly.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof(ITool).IsAssignableFrom(t)))
            {
                var tool = Activator.CreateInstance(type) as ITool;
                tools.Add(tool);
            }
        }

        if (current < 0 || current >= tools.Count)
            current = 0;

        GUILayout.BeginHorizontal();
        for(int i = 0; i < tools.Count; i++)
        {
            GUI.enabled = i != current;
            var tool = tools[i];
            if (GUILayout.Button(tool.Title))
                current = i;
            GUI.enabled = true;
        }
        GUILayout.EndHorizontal();

        var ct = current > tools.Count ? null : tools[current];
        if (ct != null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate", GUILayout.ExpandWidth(true)))
                content = ct.Generate();

            if (GUILayout.Button("Copy to clipboard", GUILayout.ExpandWidth(true)))
            {
                var te = new TextEditor();
                te.text = content;
                te.SelectAll();
                te.Copy();
            }

            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true)))
                content = string.Empty;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            ct.DrawFields();
            GUILayout.EndHorizontal();
        }

        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.TextArea(content, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}