﻿using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUCheckBox))]
    public class IGUCheckBoxDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUTextObject obj = serialized.targetObject as IGUTextObject;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_useTooltip = serialized.FindProperty("useTooltip");
                SerializedProperty prop_content = serialized.FindProperty("content");
                SerializedProperty prop__checked = serialized.FindProperty("_checked");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_checkBoxOn = serialized.FindProperty("checkBoxOn");
                SerializedProperty prop_checkBoxOff = serialized.FindProperty("checkBoxOff");
                SerializedProperty prop_onChecked = serialized.FindProperty("onChecked");

                prop_foldout.boolValue = EditorGUI.Foldout(position, prop_foldout.boolValue,
                    EditorGUIUtility.TrTempContent($"[{temp.GetType().Name}]{temp.name}"));

                if (prop_foldout.boolValue) {
                    DrawBackground(position = MoveDownWithBlankSpace(position), SingleRowHeightWithBlankSpace * 3f);
                    ++EditorGUI.indentLevel;
                    obj.name = EditorGUI.TextField(position, EditorGUIUtility.TrTempContent("Name"), obj.name);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(position = MoveDownWithBlankSpace(position), prop_parent, EditorGUIUtility.TrTempContent("parent"));
                    EditorGUI.LabelField(position = MoveDownWithBlankSpace(position),
                        EditorGUIUtility.TrTempContent($"Container: {prop_container.FindPropertyRelative("name").stringValue}"),
                        EditorStyles.helpBox);
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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 2f +
                        EditorGUI.GetPropertyHeight(prop_content) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    Rect postemp;
                    _ = EditorGUI.PropertyField(position, prop_useTooltip, EditorGUIUtility.TrTempContent("Use Tooltip"));
                    _ = EditorGUI.PropertyField(postemp = MoveDownWithBlankSpace(position), prop__checked, EditorGUIUtility.TrTempContent("Checked"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(postemp), prop_content, EditorGUIUtility.TrTempContent("Content"));
                    --EditorGUI.indentLevel;

                    _ = EditorGUI.PropertyField(position = MoveDown(position, pheight + BlankSpace), prop_onClick, EditorGUIUtility.TrTempContent("On Click"));
                    _ = EditorGUI.PropertyField(position = MoveDown(position, EditorGUI.GetPropertyHeight(prop_onClick) + BlankSpace), prop_onChecked, EditorGUIUtility.TrTempContent("On Checked"));
                    _ = EditorGUI.PropertyField(position = MoveDown(position, EditorGUI.GetPropertyHeight(prop_onChecked) + BlankSpace), prop_checkBoxOn, EditorGUIUtility.TrTempContent("CheckBox On"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_checkBoxOn) + BlankSpace), prop_checkBoxOff, EditorGUIUtility.TrTempContent("CheckBox Off"));
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
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_checkBoxOn = serialized.FindProperty("checkBoxOn");
                SerializedProperty prop_checkBoxOff = serialized.FindProperty("checkBoxOff");
                SerializedProperty prop_onChecked = serialized.FindProperty("onChecked");
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 6f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_content) +
                        EditorGUI.GetPropertyHeight(prop_onClick) +
                        EditorGUI.GetPropertyHeight(prop_checkBoxOn) +
                        EditorGUI.GetPropertyHeight(prop_checkBoxOff) +
                        EditorGUI.GetPropertyHeight(prop_onChecked) +
                        BlankSpace * 9f;
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
