using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUNumericBoxInt))]
    public class IGUNumericBoxIntDraw : IGUObjectDraw {
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
            internalOnGUI += IGUNBIBuildRun;
        }

        protected virtual void IGUNBIBuildRun(SerializedObject serialized) {
            RunValue(serialized);
            RunAdditionValue(serialized);
            RunMinMax(serialized);
        }

        protected void RunValue(SerializedObject serialized) {
            SerializedProperty value = serialized.FindProperty("value");

            EditorGUI.PropertyField(GetRect(), value, GetGUIContent("Value"));
            _ = MoveDown();
        }

        protected void RunAdditionValue(SerializedObject serialized) {
            SerializedProperty value = serialized.FindProperty("additionValue");

            EditorGUI.PropertyField(GetRect(), value, GetGUIContent("Addition value"));
            _ = MoveDown();
        }

        protected virtual void RunMinMax(SerializedObject serialized) {
            SerializedProperty maxMinSlider = serialized.FindProperty("maxMinSlider");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(MaxMinSliderInt)}").GetPropertyHeight(maxMinSlider, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(MaxMinSliderInt)}").OnGUI(GetRect(), maxMinSlider, GetGUIContent("Max Min"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}
