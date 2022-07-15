using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using CBAspectRatio = Cobilas.Unity.Graphics.Resolutions.AspectRatio;

namespace Cobilas.Unity.Editor.Graphics.Resolutions {
    [CustomPropertyDrawer(typeof(CBAspectRatio))]
    public class AspectRatioDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty serialized_width = property.FindPropertyRelative("width");
            SerializedProperty serialized_height = property.FindPropertyRelative("height");
            int width = serialized_width.intValue;
            int height = serialized_height.intValue;

            EditorGUI.BeginChangeCheck();
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label);
            position.y += 3;
            EditorGUI.indentLevel++;
            width = EditorGUI.IntField(position = MoveDown(position), "width", width);
            position.y += 3;
            height = EditorGUI.IntField(MoveDown(position), "height", height);
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck()) {
                serialized_width.intValue = width;
                serialized_height.intValue = height;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => (EditorGUIUtility.singleLineHeight * 3) + 6;
    }
}
