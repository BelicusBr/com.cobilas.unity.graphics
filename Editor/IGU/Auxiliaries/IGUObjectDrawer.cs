using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUObject), true)]
    public class IGUObjectDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue == null)
                EditorGUI.PropertyField(position, property);
            else IGUPropertyDrawer.GetPropertyDrawer(property.objectReferenceValue.GetType()).OnGUI(position, property, label);
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;
            return IGUPropertyDrawer.GetPropertyDrawer(property.objectReferenceValue.GetType()).GetPropertyHeight(property, label);
        }
    }
}
