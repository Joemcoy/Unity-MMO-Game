using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;
using Scripts.Local.UI.Helpers;

[CustomEditor(typeof(CharacterHelper))]
public class CharacterHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        var helper = target as CharacterHelper;

        GUI.enabled = camera != null;
        if (GUILayout.Button("Get from main camera"))
        {
            helper.CameraPosition = camera.transform.position;
            helper.CameraRotation = camera.transform.rotation;
        }
        GUI.enabled = true;
        base.OnInspectorGUI();
    }
}