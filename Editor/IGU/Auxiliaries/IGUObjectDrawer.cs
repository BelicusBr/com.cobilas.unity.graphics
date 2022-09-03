using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUObject), true)]
    public class IGUObjectDrawer : PropertyDrawer {

        private Dictionary<int, SerializedObject> pairs = new Dictionary<int, SerializedObject>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue == null)
                EditorGUI.PropertyField(position, property);
            else {
                SerializedObject serialized = GetSerializedObject(property.objectReferenceValue);
                serialized.Update();
                IGUPropertyDrawer.GetPropertyDrawer(property.objectReferenceValue.GetType()).OnGUI(position, serialized);
                serialized.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (property.objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;
            SerializedObject serialized = GetSerializedObject(property.objectReferenceValue);
            if (serialized.FindProperty("foldout").boolValue)
                return IGUPropertyDrawer.GetPropertyDrawer(property.objectReferenceValue.GetType()).GetPropertyHeight(serialized);
            return EditorGUIUtility.singleLineHeight;
        }

        private SerializedObject GetSerializedObject(Object target) {
            if (!pairs.ContainsKey(target.GetInstanceID()))
                pairs.Add(target.GetInstanceID(), new SerializedObject(target));
            return pairs[target.GetInstanceID()];
        }
    }
}
