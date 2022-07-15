using System;
using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUTextFieldObject), true)]
    public class IGUTextFieldObjectDraw : IGUTextObjectDraw {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override Rect MoveDown()
            => base.MoveDown();

        protected override Rect MoveDown(float height)
            => base.MoveDown(height);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunIGUTextSettings;
        }

        protected override void RunIGUContent(SerializedObject serialized)
            => base.RunIGUContent(serialized);

        protected void RunIGUTextSettings(SerializedObject serialized) {
            SerializedProperty settings = serialized.FindProperty("settings");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(IGUTextSettings)}").GetPropertyHeight(settings, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(IGUTextSettings)}").OnGUI(GetRect(), settings, GetGUIContent("Cursor settings"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}