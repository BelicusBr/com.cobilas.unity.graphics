using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUSelectableText))]
    public class IGUSelectableTextDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUSelectableText obj = serialized.targetObject as IGUSelectableText;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_useTooltip = serialized.FindProperty("useTooltip");
                SerializedProperty prop_content = serialized.FindProperty("content");
                SerializedProperty prop_settings = serialized.FindProperty("settings");

                SerializedProperty prop_isFocused = serialized.FindProperty("isFocused");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");

                prop_foldout.boolValue = EditorGUI.Foldout(position, prop_foldout.boolValue,
                    EditorGUIUtility.TrTempContent($"[{temp.GetType().Name}]{temp.name}"));

                if (prop_foldout.boolValue) {
                    DrawBackground(position = MoveDownWithBlankSpace(position), SingleRowHeightWithBlankSpace * 3f);
                    ++EditorGUI.indentLevel;
                    obj.name = EditorGUI.TextField(position, EditorGUIUtility.TrTempContent("Name"), obj.name);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(position = MoveDownWithBlankSpace(position), prop_parent, EditorGUIUtility.TrTempContent("parent"));
                    _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_container, EditorGUIUtility.TrTempContent("Container"));
                    EditorGUI.EndDisabledGroup();
                    --EditorGUI.indentLevel;

                    position.y += BlankSpace;
                    float pheight;
                    DrawBackground(position = MoveDownWithBlankSpace(position), pheight = EditorGUI.GetPropertyHeight(prop_myRect));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myRect, EditorGUIUtility.TrTempContent("Rect"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_myColor));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myColor, EditorGUIUtility.TrTempContent("Color"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_myConfg));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myConfg, EditorGUIUtility.TrTempContent("Config"));
                    --EditorGUI.indentLevel;

                    float cheight = prop_content.FindPropertyRelative("foldout").boolValue ? SingleRowHeightWithBlankSpace * 3f : SingleRowHeightWithBlankSpace;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace +
                        cheight + BlankSpace);
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_useTooltip, EditorGUIUtility.TrTempContent("Use Tooltip"));
                    DrawIGUContent(prop_content, MoveDownWithBlankSpace(position));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_settings));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_settings, EditorGUIUtility.TrTempContent("Settings"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace);
                    ++EditorGUI.indentLevel;
                    EditorGUI.BeginDisabledGroup(true);
                    _ = EditorGUI.PropertyField(position, prop_isFocused, EditorGUIUtility.TrTempContent("Is Focused"));
                    EditorGUI.EndDisabledGroup();
                    --EditorGUI.indentLevel;

                    _ = EditorGUI.PropertyField(MoveDown(position, pheight + BlankSpace), prop_onClick, EditorGUIUtility.TrTempContent("On Click"));
                }
                serialized.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Object temp = property.objectReferenceValue;
            if (temp != null) {
                SerializedObject serialized = new SerializedObject(temp);
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_content = serialized.FindProperty("content");
                float cheight = prop_content.FindPropertyRelative("foldout").boolValue ? SingleRowHeightWithBlankSpace * 3f : SingleRowHeightWithBlankSpace;
                SerializedProperty prop_settings = serialized.FindProperty("settings");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 6f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_settings) +
                        EditorGUI.GetPropertyHeight(prop_onClick) +
                        cheight + BlankSpace * 8f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private void DrawIGUContent(SerializedProperty property, Rect position) {
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_text = property.FindPropertyRelative("text");
            SerializedProperty prop_tooltip = property.FindPropertyRelative("tooltip");

            if (prop_foldout.boolValue = EditorGUI.Foldout(position, prop_foldout.boolValue, EditorGUIUtility.TrTempContent("Content"))) {
                _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_text, EditorGUIUtility.TrTempContent("Text"));
                _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(position), prop_tooltip, EditorGUIUtility.TrTempContent("Tooltip"));
            }
        }

        private void DrawBackground(Rect position, float height)
        {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
