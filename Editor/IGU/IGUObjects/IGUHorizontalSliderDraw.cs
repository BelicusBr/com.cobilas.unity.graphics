using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUHorizontalSlider))]
    public class IGUHorizontalSliderDraw : IGUSliderObjectDraw {

        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized) {
            return base.IGetPropertyHeight(serialized) + 
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onModifiedSlider")) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onModifiedSliderInt"));
        }

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
