using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUSliderObject), true)]
    public class IGUSliderObjectDraw : IGUObjectDraw {

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
            internalOnGUI += RunIsInt;
            internalOnGUI += RunValue;
            internalOnGUI += RunMaxMinSlider;
        }

        protected void RunIsInt(SerializedObject serialized) {
            SerializedProperty isInt = serialized.FindProperty("isInt");

            EditorGUI.PropertyField(GetRect(), isInt, GetGUIContent("Is int"));
            _ = MoveDown();
        }

        protected void RunValue(SerializedObject serialized) {
            SerializedProperty value = serialized.FindProperty("value");

            EditorGUI.PropertyField(GetRect(), value, GetGUIContent("Value"));
            _ = MoveDown();
        }

        protected void RunMaxMinSlider(SerializedObject serialized) {
            SerializedProperty maxMinSlider = serialized.FindProperty("maxMinSlider");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(MaxMinSlider)}").GetPropertyHeight(maxMinSlider, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(MaxMinSlider)}").OnGUI(GetRect(), maxMinSlider, GetGUIContent("Max Min"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}