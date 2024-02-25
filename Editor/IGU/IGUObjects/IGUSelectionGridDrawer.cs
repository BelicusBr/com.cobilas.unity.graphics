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
    [CustomPropertyDrawer(typeof(IGUSelectionGrid))]
    public class IGUSelectionGridDrawer : CPropertyDrawer {

        private readonly Dictionary<int, int> SelectIndex = new Dictionary<int, int>();
        private ReorderableList reorderableList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            UEObject temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUSelectionGrid obj = serialized.targetObject as IGUSelectionGrid;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");

                SerializedProperty prop_spacing = serialized.FindProperty("spacing");
                SerializedProperty prop_useTooltip = serialized.FindProperty("useTooltip");
                SerializedProperty prop_selectedIndex = serialized.FindProperty("selectedIndex");
                SerializedProperty prop_onSelectedIndex = serialized.FindProperty("onSelectedIndex");

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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 3f +
                        EditorGUI.GetPropertyHeight(prop_spacing) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    Rect recttemp;
                    obj.xCount = EditorGUI.IntField(position, EditorGUIUtility.TrTempContent("XCount"), obj.xCount);
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(position), prop_selectedIndex, EditorGUIUtility.TrTempContent("Selected Index"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_useTooltip, EditorGUIUtility.TrTempContent("Use Tooltip"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(recttemp), prop_spacing, EditorGUIUtility.TrTempContent("Spacing"));
                    --EditorGUI.indentLevel;

                    reorderableList = GetReorderableList(obj, position = MoveDown(position, pheight + BlankSpace));

                    _ = EditorGUI.PropertyField(MoveDown(position, reorderableList.GetHeight() + BlankSpace), prop_onSelectedIndex, EditorGUIUtility.TrTempContent("On Selected Index"));
                }
                serialized.ApplyModifiedProperties();
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
                SerializedProperty prop_spacing = serialized.FindProperty("spacing");
                SerializedProperty prop_onSelectedIndex = serialized.FindProperty("onSelectedIndex");
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 7f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_spacing) +
                        GetReorderableListHeight() +
                        EditorGUI.GetPropertyHeight(prop_onSelectedIndex) +
                        BlankSpace * 6f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private ReorderableList GetReorderableList(IGUSelectionGrid comboBox, Rect position)
        {
            ReorderableList reorderable = new ReorderableList(
                    new List<IGUSelectionGridToggle>(comboBox), typeof(IGUSelectionGridToggle),
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
                IGUSelectionGridToggle item = reorderable.list[i] as IGUSelectionGridToggle;
                r.height = SingleLineHeight;
                item.Text = EditorGUI.TextField(r, EditorGUIUtility.TrTempContent("Text"), item.Text);
                item.Image = (Texture)EditorGUI.ObjectField(r = MoveDown(r), EditorGUIUtility.TrTempContent("Image"), item.Image, typeof(Texture), true);
                item.ToolTip = EditorGUI.TextField(MoveDown(r), EditorGUIUtility.TrTempContent("Tooltip"), item.ToolTip);
            };

            reorderable.onRemoveCallback = (r) => {
                List<IGUSelectionGridToggle> temp = r.list as List<IGUSelectionGridToggle>;
                temp.RemoveAt(r.index);
                comboBox.Remove(r.index);
                SelectIndex[BoxID] = SecIndex = r.index - 1;
            };

            reorderable.onAddCallback = (r) => {
                List<IGUSelectionGridToggle> temp = r.list as List<IGUSelectionGridToggle>;
                comboBox.Add($"Item[{comboBox.ToggleCount}]");
                temp.Add(comboBox[comboBox.ToggleCount - 1]);
            };
            reorderable.DoList(position);
            SelectIndex[BoxID] = SecIndex;

            return reorderable;
        }


        private float GetReorderableListHeight()
            => reorderableList == null ? 0f : reorderableList.GetHeight();

        private void DrawBackground(Rect position, float height) {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
