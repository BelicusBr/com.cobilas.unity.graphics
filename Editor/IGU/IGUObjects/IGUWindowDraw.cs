using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUWindow))]
    public class IGUWindowDraw : IGUTextObjectDraw {

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
            internalOnGUI += RunDragFlap;
            internalOnGUI += RunEvents;
        }

        protected void RunDragFlap(SerializedObject serialized) {
            SerializedProperty dragFlap = serialized.FindProperty("dragFlap");

            EditorGUI.PropertyField(GetRect(), dragFlap, GetGUIContent("Drag flap"));
            _ = MoveDown(EditorGUI.GetPropertyHeight(SerializedPropertyType.Rect, GetGUIContent("Drag flap")) + BlankSpace);
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onMovingWindow = serialized.FindProperty("onMovingWindow");

            float onMovingWindowHeight = EditorGUI.GetPropertyHeight(onMovingWindow);

            EditorGUI.PropertyField(GetRect(), onMovingWindow);
            _ = MoveDown(onMovingWindowHeight);
        }
    }
}
