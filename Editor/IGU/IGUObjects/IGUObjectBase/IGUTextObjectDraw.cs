using System;
using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUTextObject), true)]
    public class IGUTextObjectDraw : IGUObjectDraw {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override Rect MoveDown()
            => base.MoveDown();

        protected override Rect MoveDown(float height)
            => base.MoveDown(height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunUseTooltip;
            internalOnGUI += RunIGUContent;
        }

        protected void RunUseTooltip(SerializedObject serialized) {
            SerializedProperty useTooltip = serialized.FindProperty("useTooltip");
            SerializedProperty hide_Tooltip = serialized.FindProperty("content.hide_Tooltip");

            _ = EditorGUI.PropertyField(GetRect(), useTooltip, GetGUIContent("Use tooltip"));
            hide_Tooltip.boolValue = useTooltip.boolValue;
        }

        protected virtual void RunIGUContent(SerializedObject serialized) {
            SerializedProperty content = serialized.FindProperty("content");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(IGUContent)}").GetPropertyHeight(content, null);
            DrawBackgroundProperty(MoveDown(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#{nameof(IGUContent)}").OnGUI(GetRect(), content, GetGUIContent("Content"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}
