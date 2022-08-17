using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUSelectionGrid))]
    public class IGUSelectionGridDraw : IGUObjectDraw {
        private Dictionary<int, int> SelectIndex = new Dictionary<int, int>();

        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized) {
            IGUSelectionGrid box = serialized.targetObject as IGUSelectionGrid;
            IGUSelectionGridToggle[] boxBTs = box.SelectionGridToggles;

            ReorderableList reorderable = new ReorderableList(
                new List<IGUSelectionGridToggle>(boxBTs), boxBTs.GetType().GetElementType(),
                false, true, true, true
                );

            reorderable.elementHeight = (SingleLineHeight * 3f) + BlankSpace;

            return base.IGetPropertyHeight(serialized) + (SingleRowHeightWithBlankSpace * 3f) +
                (EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, GetGUIContent("Spacing")) + BlankSpace) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onSelectedIndex")) + reorderable.GetHeight();
        }

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);
        
        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunXCount;
            internalOnGUI += RunUseTooltip;
            internalOnGUI += RunSpacing;
            internalOnGUI += RunSelectedIndex;
            internalOnGUI += RunSelectionGridToggles;
            internalOnGUI += RunEvents;
        }

        protected void RunXCount(SerializedObject serialized) {
            SerializedProperty _xCount = serialized.FindProperty("_xCount");

            EditorGUI.PropertyField(GetRect(), _xCount, GetGUIContent("X count"));
            _ = MoveDown();
        }

        protected void RunUseTooltip(SerializedObject serialized) {
            SerializedProperty useTooltip = serialized.FindProperty("useTooltip");

            EditorGUI.PropertyField(GetRect(), useTooltip, GetGUIContent("Use tooltip"));
            _ = MoveDown();
        }

        protected void RunSpacing(SerializedObject serialized) {
            SerializedProperty spacing = serialized.FindProperty("spacing");

            EditorGUI.PropertyField(GetRect(), spacing, GetGUIContent("Spacing"));
            _ = MoveDown(EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, GetGUIContent("Spacing")) + BlankSpace);
        }

        protected void RunSelectedIndex(SerializedObject serialized) {
            SerializedProperty selectedIndex = serialized.FindProperty("selectedIndex");

            EditorGUI.PropertyField(GetRect(), selectedIndex, GetGUIContent("Selected index"));
            _ = MoveDown();
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onSelectedIndex = serialized.FindProperty("onSelectedIndex");

            float onSelectedIndexHeight = EditorGUI.GetPropertyHeight(onSelectedIndex);

            EditorGUI.PropertyField(GetRect(), onSelectedIndex);
            _ = MoveDown(onSelectedIndexHeight);
        }

        protected void RunSelectionGridToggles(SerializedObject serialized) {
            IGUSelectionGrid box = serialized.targetObject as IGUSelectionGrid;
            IGUSelectionGridToggle[] boxBTs = box.SelectionGridToggles;
            int BoxID = box.GetInstanceID();

            if (!SelectIndex.ContainsKey(BoxID))
                SelectIndex.Add(BoxID, -1);

            int SecIndex = SelectIndex[BoxID];

            ReorderableList reorderable = new ReorderableList(
                new List<IGUSelectionGridToggle>(boxBTs), boxBTs.GetType().GetElementType(),
                false, true, true, true
                );

            reorderable.elementHeight = (SingleLineHeight * 3f) + BlankSpace;
            reorderable.drawHeaderCallback = (r) => EditorGUI.LabelField(r, GetGUIContent("ComboBox buttons"));
            reorderable.onSelectCallback = (r) => SecIndex = r.index;

            reorderable.drawElementCallback = (r, i, a, f) => {
                IGUSelectionGridToggle item = reorderable.list[i] as IGUSelectionGridToggle;
                r.height = SingleLineHeight;
                item.Content.Text = EditorGUI.TextField(r, GetGUIContent("Text"), item.Content.Text);
                item.Content.Image = (Texture)EditorGUI.ObjectField(r = MoveDown(r), GetGUIContent("Image"), item.Content.Image, typeof(Texture), true);
                item.Content.Tooltip = EditorGUI.TextField(MoveDown(r), GetGUIContent("Tooltip"), item.Content.Tooltip);
            };
            if (SecIndex > -1) reorderable.index = SecIndex;
            reorderable.onRemoveCallback = (r) => {
                List<IGUSelectionGridToggle> temp = r.list as List<IGUSelectionGridToggle>;
                temp.RemoveAt(r.index);
                SelectIndex[BoxID] = SecIndex = r.index - 1;
                if (temp.Count > 0)
                {
                    List<IGUContent> contents = new List<IGUContent>(
                    Array.ConvertAll<IGUSelectionGridToggle, IGUContent>(temp.ToArray(), (t) => new IGUContent(t.Content))
                    );
                    box.SetSelectionGridToggleList(contents.ToArray());
                }
                else box.SetSelectionGridToggleList(new IGUContent[0]);
            };
            reorderable.onAddCallback = (r) => {
                List<IGUSelectionGridToggle> temp = r.list as List<IGUSelectionGridToggle>;
                List<IGUContent> contents = new List<IGUContent>(
                Array.ConvertAll<IGUSelectionGridToggle, IGUContent>(temp.ToArray(), (t) => new IGUContent(t.Content))
                );
                contents.Add(new IGUContent($"Item {contents.Count}"));
                box.SetSelectionGridToggleList(contents.ToArray());
            };
            reorderable.DoList(GetRect());
            SelectIndex[BoxID] = SecIndex;
            _ = MoveDown(reorderable.GetHeight() + BlankSpace);
            //reorderable = new ReorderableList(serialized, serialized.FindProperty("selectionGridToggles"));
            //reorderable.elementHeight = SingleRowHeightWithBlankSpace;
            //reorderable.drawElementCallback = DrawElement;
            //reorderable.DoList(GetRect());
            //_ = MoveDown(reorderable.GetHeight() + BlankSpace);
        }
    }
}
