using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

namespace Scripts.Local.Helper
{
    public static class EditorHelper
    {
        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return UnityEngine.Application.isEditor;
#endif
            }
        }

        public static bool IsPlaying
        {
            get
            {
#if UNITY_EDITOR
                return EditorApplication.isPlaying;// && EditorWindow.focusedWindow.titleContent.text == "Game";
#else
                return true;
#endif
            }
        }
    }
}