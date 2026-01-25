using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace Nanodogs.Nanobox.Mod.Editor
{
    public static class NbExtEditorUtils
    {
        public static string TextFieldWithLabel(string label, string text)
        {
            EditorGUILayout.LabelField(label); // Draws the label first (above)
            return EditorGUILayout.TextField(text); // Draws the field below
        }
    }
}
