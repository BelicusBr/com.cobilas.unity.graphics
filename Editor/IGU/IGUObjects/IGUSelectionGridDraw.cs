using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUSelectionGrid))]
    public class IGUSelectionGridDraw : IGUObjectDraw {
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
            reorderable = new ReorderableList(serialized, serialized.FindProperty("selectionGridToggles"));
            reorderable.elementHeight = SingleRowHeightWithBlankSpace;
            reorderable.drawElementCallback = DrawElement;
            reorderable.DoList(GetRect());
            _ = MoveDown(reorderable.GetHeight() + BlankSpace);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty property = reorderable.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty property_text = property.FindPropertyRelative("checkBox.content.text");

            rect.height = SingleLineHeight;
            EditorGUI.PropertyField(rect, property_text, GetGUIContent($"Item({index})"));
        }
    }
}
