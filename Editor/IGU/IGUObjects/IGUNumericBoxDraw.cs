using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUNumericBox))]
    public class IGUNumericBoxDraw : IGUNumericBoxIntDraw {

        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized) {
            return base.IGetPropertyHeight(serialized) + SingleRowHeightWithBlankSpace;
        }

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun()
            => base.BuildRun();

        protected override void IGUNBIBuildRun(SerializedObject serialized) {
            RunValue(serialized);
            RunAdditionValue(serialized);
            RunNumberOfDecimalPlaces(serialized);
            RunMinMax(serialized);
        }

        protected void RunNumberOfDecimalPlaces(SerializedObject serialized) {
            SerializedProperty numberOfDecimalPlaces = serialized.FindProperty("numberOfDecimalPlaces");

            EditorGUI.PropertyField(GetRect(), numberOfDecimalPlaces, GetGUIContent("Number of decimal places"));
            _ = MoveDown();
        }

        protected override float GetHeightMaxMinSliderValue(SerializedObject serialized)
            => IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(MaxMinSlider)}").GetPropertyHeight(serialized.FindProperty("maxMinSlider"), null);

        protected override void RunMinMax(SerializedObject serialized) {
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
