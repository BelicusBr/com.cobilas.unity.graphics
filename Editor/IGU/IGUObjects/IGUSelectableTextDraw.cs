using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU
{
    [IGUCustomDrawer(typeof(IGUSelectableText))]
    public class IGUSelectableTextDraw : IGUTextFieldObjectDraw {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override Rect MoveDown()
            => base.MoveDown();

        protected override Rect MoveDown(float height)
            => base.MoveDown(height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun() {
            ResetInternalOnGUI();
            internalOnGUI += RunName;
            internalOnGUI += RunFiliation;
            internalOnGUI += RunModifiers;
            internalOnGUI += RunIGUContent;
            internalOnGUI += RunIGUTextSettings;
            internalOnGUI += RunIsFocused;
            internalOnGUI += RunOnEvents;
        }

        protected virtual void RunOnEvents(SerializedObject serialized) {
            SerializedProperty onClick = serialized.FindProperty("onClick");

            float onClickHeight = EditorGUI.GetPropertyHeight(onClick);

            EditorGUI.PropertyField(GetRect(), onClick);
            _ = MoveDown(onClickHeight);
        }

        protected void RunIsFocused(SerializedObject serialized) {
            SerializedProperty isFocused = serialized.FindProperty("isFocused");
            using (GetDisabledScope(true))
                EditorGUI.PropertyField(GetRect(), isFocused, GetGUIContent("Is focused"));
            _ = MoveDown();
        }

        protected override void RunIGUContent(SerializedObject serialized) {
            SerializedProperty content = serialized.FindProperty("content");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#TA{nameof(IGUContent)}").GetPropertyHeight(content, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#TA{nameof(IGUContent)}").OnGUI(GetRect(), content, GetGUIContent("Content"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}
