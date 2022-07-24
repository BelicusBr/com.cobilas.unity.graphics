using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGURepeatButton))]
    public class IGURepeatButtonDraw : IGUButtonDraw {
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

        protected override void BuildRun()
            => base.BuildRun();

        protected override void RunEvents(SerializedObject serialized) {
            base.RunEvents(serialized);
            SerializedProperty onRepeatClick = serialized.FindProperty("onRepeatClick");

            float onRepeatClickHeight = EditorGUI.GetPropertyHeight(onRepeatClick);

            EditorGUI.PropertyField(GetRect(), onRepeatClick);
            _ = MoveDown(onRepeatClickHeight);
        }
    }
}
