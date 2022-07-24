using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUHorizontalSlider))]
    public class IGUHorizontalSliderDraw : IGUSliderObjectDraw {

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
            internalOnGUI += RunEvents;
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onModifiedSlider = serialized.FindProperty("onModifiedSlider");
            SerializedProperty onModifiedSliderInt = serialized.FindProperty("onModifiedSliderInt");

            float onModifiedSliderHeight = EditorGUI.GetPropertyHeight(onModifiedSlider);
            float onModifiedSliderIntHeight = EditorGUI.GetPropertyHeight(onModifiedSliderInt);

            EditorGUI.PropertyField(GetRect(), onModifiedSlider);
            _ = MoveDown(onModifiedSliderHeight);
            EditorGUI.PropertyField(GetRect(), onModifiedSliderInt);
            _ = MoveDown(onModifiedSliderIntHeight);
        }
    }
}
