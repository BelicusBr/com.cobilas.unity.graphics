using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUComboBox))]
    public class IGUComboBoxDraw : IGUObjectDraw {
        private ReorderableList reorderable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

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
            reorderable = new ReorderableList(serialized, serialized.FindProperty("boxButtons"));
            reorderable.elementHeight = SingleRowHeightWithBlankSpace;
            reorderable.drawElementCallback = DrawElement;
            reorderable.DoList(GetRect());
            _ = MoveDown(reorderable.GetHeight() + BlankSpace);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty property = reorderable.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty property_text = property.FindPropertyRelative("button.content.text");

            rect.height = SingleLineHeight;
            EditorGUI.PropertyField(rect, property_text, GetGUIContent($"Item({index})"));
        }
    }
}
