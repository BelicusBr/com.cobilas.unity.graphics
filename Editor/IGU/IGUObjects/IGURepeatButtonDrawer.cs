using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGURepeatButton))]
    public class IGURepeatButtonDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUButton obj = serialized.targetObject as IGUButton;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");
                SerializedProperty prop_useTooltip = serialized.FindProperty("useTooltip");
                SerializedProperty prop_content = serialized.FindProperty("content");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_onRepeatClick = serialized.FindProperty("onRepeatClick");

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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace +
                        EditorGUI.GetPropertyHeight(prop_content) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_useTooltip, EditorGUIUtility.TrTempContent("Use Tooltip"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(position), prop_content, EditorGUIUtility.TrTempContent("Content"));
                    --EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(MoveDown(position, pheight + BlankSpace), prop_onClick, EditorGUIUtility.TrTempContent("On Click"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_onRepeatClick) + BlankSpace), prop_onRepeatClick, EditorGUIUtility.TrTempContent("On RepeatClick"));
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
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");
                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_content = serialized.FindProperty("content");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_onRepeatClick = serialized.FindProperty("onRepeatClick");
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 5f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_content) +
                        EditorGUI.GetPropertyHeight(prop_onClick) +
                        EditorGUI.GetPropertyHeight(prop_onRepeatClick) +
                        BlankSpace * 6f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private void DrawBackground(Rect position, float height)
        {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
