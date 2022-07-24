using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGULabel))]
    public class IGULabelDraw : IGUTextObjectDraw {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunAutoSize;
            internalOnGUI += RunRichText;
        }

        protected void RunAutoSize(SerializedObject serialized) {
            SerializedProperty isInt = serialized.FindProperty("autoSize");

            EditorGUI.PropertyField(GetRect(), isInt, GetGUIContent("Auto size"));
            _ = MoveDown();
        }

        protected void RunRichText(SerializedObject serialized) {
            SerializedProperty isInt = serialized.FindProperty("richText");

            EditorGUI.PropertyField(GetRect(), isInt, GetGUIContent("Rich text"));
            _ = MoveDown();
        }
    }
}
