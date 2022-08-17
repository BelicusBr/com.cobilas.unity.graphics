using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUComboBox))]
    public class IGUComboBoxDraw : IGUObjectDraw {
        private Dictionary<int, int> SelectIndex = new Dictionary<int, int>();

        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized) {
            IGUComboBox box = serialized.targetObject as IGUComboBox;
            IGUComboBoxButton[] boxBTs = box.BoxButtons;
            ReorderableList reorderable = new ReorderableList(
                new List<IGUComboBoxButton>(boxBTs), boxBTs.GetType().GetElementType(),
                false, true, true, true
                );

            reorderable.elementHeight = (SingleLineHeight * 3f) + BlankSpace;
            return base.IGetPropertyHeight(serialized) + (SingleRowHeightWithBlankSpace * 6f) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onClick")) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onActivatedComboBox")) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onSelectedIndex")) +
                reorderable.GetHeight();
        }

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunIndex;
            internalOnGUI += RunActivatedComboBox;
            internalOnGUI += RunScrollViewHeight;
            internalOnGUI += RunComboBoxButtonHeight;
            internalOnGUI += RunCloseOnClickComboBoxViewButton;
            internalOnGUI += RunAdjustComboBoxViewAccordingToTheButtonsPresent;
            internalOnGUI += RunBoxButtons;
            internalOnGUI += RunEvents;
        }

        protected void RunIndex(SerializedObject serialized) {
            SerializedProperty index = serialized.FindProperty("index");

            EditorGUI.PropertyField(GetRect(), index, GetGUIContent("Index"));
            _ = MoveDown();
        }

        protected void RunActivatedComboBox(SerializedObject serialized) {
            SerializedProperty activatedComboBox = serialized.FindProperty("activatedComboBox");

            EditorGUI.PropertyField(GetRect(), activatedComboBox, GetGUIContent("Activated comboBox"));
            _ = MoveDown();
        }

        protected void RunScrollViewHeight(SerializedObject serialized) {
            SerializedProperty scrollViewHeight = serialized.FindProperty("scrollViewHeight");

            EditorGUI.PropertyField(GetRect(), scrollViewHeight, GetGUIContent("Scroll view height"));
            _ = MoveDown();
        }

        protected void RunComboBoxButtonHeight(SerializedObject serialized) {
            SerializedProperty comboBoxButtonHeight = serialized.FindProperty("comboBoxButtonHeight");

            EditorGUI.PropertyField(GetRect(), comboBoxButtonHeight, GetGUIContent("ComboBox button height"));
            _ = MoveDown();
        }

        protected void RunCloseOnClickComboBoxViewButton(SerializedObject serialized) {
            SerializedProperty closeOnClickComboBoxViewButton = serialized.FindProperty("closeOnClickComboBoxViewButton");

            EditorGUI.PropertyField(GetRect(), closeOnClickComboBoxViewButton, GetGUIContent("Close ComboBoxView"));
            _ = MoveDown();
        }

        protected void RunAdjustComboBoxViewAccordingToTheButtonsPresent(SerializedObject serialized) {
            SerializedProperty adjustComboBoxViewAccordingToTheButtonsPresent = serialized.FindProperty("adjustComboBoxViewAccordingToTheButtonsPresent");

            EditorGUI.PropertyField(GetRect(), adjustComboBoxViewAccordingToTheButtonsPresent, GetGUIContent("Adjust ComboBoxView"));
            _ = MoveDown();
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onClick = serialized.FindProperty("onClick");
            SerializedProperty onActivatedComboBox = serialized.FindProperty("onActivatedComboBox");
            SerializedProperty onSelectedIndex = serialized.FindProperty("onSelectedIndex");

            float onClickHeight = EditorGUI.GetPropertyHeight(onClick);
            float onActivatedComboBoxHeight = EditorGUI.GetPropertyHeight(onActivatedComboBox);
            float onSelectedIndexHeight = EditorGUI.GetPropertyHeight(onSelectedIndex);

            EditorGUI.PropertyField(GetRect(), onClick);
            _ = MoveDown(onClickHeight);
            EditorGUI.PropertyField(GetRect(), onActivatedComboBox);
            _ = MoveDown(onActivatedComboBoxHeight);
            EditorGUI.PropertyField(GetRect(), onSelectedIndex);
            _ = MoveDown(onSelectedIndexHeight);
        }

        protected void RunBoxButtons(SerializedObject serialized) {
            IGUComboBox box = serialized.targetObject as IGUComboBox;
            IGUComboBoxButton[] boxBTs = box.BoxButtons;
            int BoxID = box.GetInstanceID();

            if (!SelectIndex.ContainsKey(BoxID))
                SelectIndex.Add(BoxID, -1);

            int SecIndex = SelectIndex[BoxID];

            ReorderableList reorderable = new ReorderableList(
                new List<IGUComboBoxButton>(boxBTs), boxBTs.GetType().GetElementType(),
                false, true, true, true
                );

            reorderable.elementHeight = (SingleLineHeight * 3f) + BlankSpace;
            reorderable.drawHeaderCallback = (r) => EditorGUI.LabelField(r, GetGUIContent("ComboBox buttons"));
            reorderable.onSelectCallback = (r) => SecIndex = r.index;

            reorderable.drawElementCallback = (r, i, a, f) => {
                IGUComboBoxButton item = reorderable.list[i] as IGUComboBoxButton;
                r.height = SingleLineHeight;
                item.Content.Text = EditorGUI.TextField(r, GetGUIContent("Text"), item.Content.Text);
                item.Content.Image = (Texture)EditorGUI.ObjectField(r = MoveDown(r), GetGUIContent("Image"), item.Content.Image, typeof(Texture), true);
                item.Content.Tooltip = EditorGUI.TextField(MoveDown(r), GetGUIContent("Tooltip"), item.Content.Tooltip);
            };
            if (SecIndex > -1) reorderable.index = SecIndex;
            reorderable.onRemoveCallback = (r) => {
                List<IGUComboBoxButton> temp = r.list as List<IGUComboBoxButton>;
                temp.RemoveAt(r.index);
                SelectIndex[BoxID] = SecIndex = r.index - 1;
                if (temp.Count > 0) {
                    List<IGUContent> contents = new List<IGUContent>(
                    Array.ConvertAll<IGUComboBoxButton, IGUContent>(temp.ToArray(), (t) => new IGUContent(t.Content))
                    );
                    box.SetIGUComboBoxButtonList(contents.ToArray());
                } else box.SetIGUComboBoxButtonList(new IGUContent[0]);
            };
            reorderable.onAddCallback = (r) => {
                List<IGUComboBoxButton> temp = r.list as List<IGUComboBoxButton>;
                List<IGUContent> contents = new List<IGUContent>(
                Array.ConvertAll<IGUComboBoxButton, IGUContent>(temp.ToArray(), (t) => new IGUContent(t.Content))
                );
                contents.Add(new IGUContent($"Item {contents.Count}"));
                box.SetIGUComboBoxButtonList(contents.ToArray());
            };
            reorderable.DoList(GetRect());
            SelectIndex[BoxID] = SecIndex;
            _ = MoveDown(reorderable.GetHeight() + BlankSpace);
        }
    }
}
