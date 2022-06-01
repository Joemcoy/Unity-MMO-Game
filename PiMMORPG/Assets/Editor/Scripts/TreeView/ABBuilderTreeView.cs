using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class ABBuilderTreeView : TreeView
{
    public ABBuilderTreeView(TreeViewState state, MultiColumnHeader header) : base(state, header)
    {
        rowHeight = 20;
        showAlternatingRowBackgrounds = true;
        showBorder = true;
        customFoldoutYOffset = (18f - EditorGUIUtility.singleLineHeight) * 0.5f;
        extraSpaceBeforeIconAndLabel = 18f;
        Reload();
    }

    public static MultiColumnHeader CreateDefaultHeader(ref MultiColumnHeader header)
    {
        if (header == null)
        {
            header = new MultiColumnHeader(new MultiColumnHeaderState(new MultiColumnHeaderState.Column[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 450,
                    minWidth = 60,
                    autoResize = true,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column { minWidth = 50, maxWidth = 50, width = 50, autoResize = true },
                new MultiColumnHeaderState.Column { minWidth = 50, maxWidth = 50, width = 50, autoResize = true },
                new MultiColumnHeaderState.Column { minWidth = 50, maxWidth = 50, width = 50, autoResize = true },
                new MultiColumnHeaderState.Column { minWidth = 150, maxWidth = 150, width = 150, autoResize = true },
            }));
        }
        return header;
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        //base.RowGUI(args);

        var item = args.item as ABTreeRow;
        for (int i = 0; i < args.GetNumVisibleColumns(); i++)
            CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
    }

    void CellGUI(Rect cellRect, ABTreeRow item, int column, ref RowGUIArgs args)
    {
        CenterRectUsingSingleLineHeight(ref cellRect);
        switch (column)
        {
            case 0:
                Rect toggleRect = cellRect;
                toggleRect.x += GetContentIndent(item);
                toggleRect.width = foldoutWidth;
                if (toggleRect.xMax < cellRect.xMax)
                    item.enabled = EditorGUI.Toggle(toggleRect, item.enabled);

                // Default icon and label
                args.rowRect = cellRect;
                base.RowGUI(args);
                break;
            case 1:
                GUI.enabled = item.enabled;
                if (GUI.Button(cellRect, "Build")) AssetBundleBuilder.BuildRow(item);
                GUI.enabled = true;
                break;
            case 2:
                GUI.enabled = item.enabled;
                if (GUI.Button(cellRect, "Test")) AssetBundleBuilder.TestAsset(item);
                GUI.enabled = true;
                break;
            case 3:
                GUI.enabled = item.enabled;
                if (GUI.Button(cellRect, "Load")) AssetBundleBuilder.LoadAsset(item);
                GUI.enabled = true;
                break;
            case 4:
                if (item.fullpath != null && GUI.Button(cellRect, "Select"))
                {
                    var assets = AssetDatabase.GetAssetPathsFromAssetBundle(item.fullpath);
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath(assets[0], typeof(UnityEngine.Object));
                }
                else if (item.fullpath == null && GUI.Button(cellRect, "Enable childrens"))
                    EnableChildrens(item);
                break;
        }
    }

    void EnableChildrens(ABTreeRow row)
    {
        row.enabled = true;
        if (row.hasChildren)
            foreach (var children in row.children.OfType<ABTreeRow>())
                EnableChildrens(children);
    }

    int counter;
    protected override TreeViewItem BuildRoot()
    {
        var root = rootItem as ABTreeRow ?? new ABTreeRow { id = counter = 0, depth = -1, displayName = "Bundles", children = new List<TreeViewItem>() };
        foreach (var asset in AssetDatabase.GetAllAssetBundleNames())
        {
            var node = root;

            foreach (var pnode in asset.Split('/').SelectMany(p => p.Split('.')))
                node = PrepareNode(node, pnode);
            node.fullpath = asset;
        }
        return root;
    }

    ABTreeRow PrepareNode(ABTreeRow parent, string val)
    {
        var node = parent.children.FirstOrDefault(t => t.displayName == val) as ABTreeRow;
        if (node == null)
            parent.AddChild(node = new ABTreeRow { id = ++counter, displayName = val, depth = parent.depth + 1, children = new List<TreeViewItem>() });
        return node;
    }
}