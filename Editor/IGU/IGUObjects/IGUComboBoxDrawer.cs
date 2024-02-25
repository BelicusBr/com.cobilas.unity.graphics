using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

using UEObject = UnityEngine.Object;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUComboBox))]
    public class IGUComboBoxDrawer : CPropertyDrawer {

        private readonly Dictionary<int, int> SelectIndex = new Dictionary<int, int>();
        private ReorderableList reorderableList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            UEObject temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUComboBox obj = serialized.targetObject as IGUComboBox;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_onSelectedIndex = serialized.FindProperty("onSelectedIndex");
                SerializedProperty prop_onActivatedComboBox = serialized.FindProperty("onActivatedComboBox");
                //SerializedProperty prop_scrollViewHeight = serialized.FindProperty("scrollViewHeight");
                SerializedProperty prop_comboBoxButtonHeight = serialized.FindProperty("comboBoxButtonHeight");
                SerializedProperty prop_content = GetIGUContent(serialized.FindProperty("cbx_button"));

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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 6f +
                        EditorGUI.GetPropertyHeight(prop_content) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    Rect recttemp;
                    prop_content.serializedObject.Update();
                    obj.Index = EditorGUI.IntField(position, EditorGUIUtility.TrTempContent("Index"), obj.Index);
                    obj.UseTooltip = EditorGUI.Toggle(recttemp = MoveDownWithBlankSpace(position), EditorGUIUtility.TrTempContent("Use Tooltip"), obj.UseTooltip);
                    EditorGUI.BeginDisabledGroup(!obj.UseTooltip);
                    obj.ToolTip = EditorGUI.TextField(recttemp = MoveDownWithBlankSpace(recttemp), EditorGUIUtility.TrTempContent("Tooltip"), obj.ToolTip);
                    EditorGUI.EndDisabledGroup();
                    obj.AdjustComboBoxView = EditorGUI.Toggle(recttemp = MoveDownWithBlankSpace(recttemp), EditorGUIUtility.TrTempContent("Adjust ComboBoxView"), obj.AdjustComboBoxView);
                    obj.ScrollViewHeight = EditorGUI.FloatField(recttemp = MoveDownWithBlankSpace(recttemp), EditorGUIUtility.TrTempContent("ScrollView height"), obj.ScrollViewHeight);
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_comboBoxButtonHeight, EditorGUIUtility.TrTempContent("ComboBoxButton height"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(recttemp), prop_content, EditorGUIUtility.TrTempContent("Content"));
                    _ = prop_content.serializedObject.ApplyModifiedProperties();
                    --EditorGUI.indentLevel;

                    reorderableList = GetReorderableList(obj, position = MoveDown(position, pheight + BlankSpace));

                    _ = EditorGUI.PropertyField(position = MoveDown(position, reorderableList.GetHeight() + BlankSpace), prop_onClick, EditorGUIUtility.TrTempContent("On Click"));
                    _ = EditorGUI.PropertyField(position = MoveDown(position, EditorGUI.GetPropertyHeight(prop_onClick) + BlankSpace), prop_onSelectedIndex, EditorGUIUtility.TrTempContent("On Selected Index"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_onSelectedIndex) + BlankSpace), prop_onActivatedComboBox, EditorGUIUtility.TrTempContent("On Activated ComboBox"));
                }
                _ = serialized.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            UEObject temp = property.objectReferenceValue;
            if (temp != null) {
                SerializedObject serialized = new SerializedObject(temp);
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_onSelectedIndex = serialized.FindProperty("onSelectedIndex");
                SerializedProperty prop_onActivatedComboBox = serialized.FindProperty("onActivatedComboBox");
                SerializedProperty prop_content = GetIGUContent(serialized.FindProperty("cbx_button"));
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 10f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_onClick) +
                        EditorGUI.GetPropertyHeight(prop_content) +
                        EditorGUI.GetPropertyHeight(prop_onSelectedIndex) +
                        EditorGUI.GetPropertyHeight(prop_onActivatedComboBox) +
                        GetReorderableListHeight() +
                        BlankSpace * 9f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private ReorderableList GetReorderableList(IGUComboBox comboBox, Rect position) {
            ReorderableList reorderable = new ReorderableList(
                    new List<IGUComboBoxButton>(comboBox), typeof(IGUComboBoxButton),
                    false, true, true, true
                );

            int BoxID = comboBox.GetInstanceID();

            if (!SelectIndex.ContainsKey(BoxID))
                SelectIndex.Add(BoxID, -1);

            int SecIndex = reorderable.index = SelectIndex[BoxID];

            reorderable.elementHeight = (SingleLineHeight * 3f) + BlankSpace;
            reorderable.drawHeaderCallback = (r) => EditorGUI.LabelField(r, EditorGUIUtility.TrTempContent("ComboBox buttons"));
            reorderable.onSelectCallback = (r) => SecIndex = r.index;

            reorderable.drawElementCallback = (r, i, a, f) => {
                IGUComboBoxButton item = reorderable.list[i] as IGUComboBoxButton;
                r.height = SingleLineHeight;
                item.Text = EditorGUI.TextField(r, EditorGUIUtility.TrTempContent("Text"), item.Text);
                item.Image = (Texture)EditorGUI.ObjectField(r = MoveDown(r), EditorGUIUtility.TrTempContent("Image"), item.Image, typeof(Texture), true);
                item.ToolTip = EditorGUI.TextField(MoveDown(r), EditorGUIUtility.TrTempContent("Tooltip"), item.ToolTip);
            };

            reorderable.onRemoveCallback = (r) => {
                List<IGUComboBoxButton> temp = r.list as List<IGUComboBoxButton>;
                temp.RemoveAt(r.index);
                comboBox.Remove(r.index);
                SelectIndex[BoxID] = SecIndex = r.index - 1;
            };

            reorderable.onAddCallback = (r) => {
                List<IGUComboBoxButton> temp = r.list as List<IGUComboBoxButton>;
                comboBox.Add($"Item[{comboBox.ButtonCount}]");
                temp.Add(comboBox[comboBox.ButtonCount - 1]);
                r.index = comboBox.ButtonCount - 1;
            };
            reorderable.DoList(position);
            SelectIndex[BoxID] = SecIndex;

            return reorderable;
        }

        private float GetReorderableListHeight()
            => reorderableList == null ? 0f : reorderableList.GetHeight();

        private SerializedProperty GetIGUContent(SerializedProperty property) {
            SerializedObject serialized = new SerializedObject(property.objectReferenceValue);
            return serialized.FindProperty("content");
        }

        private void DrawBackground(Rect position, float height) {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
