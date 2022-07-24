using UnityEngine;
using UnityEditor;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    public class IGUCheckBoxDraw : IGUTextObjectDraw {

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
            internalOnGUI += RunChecked;
            internalOnGUI += RunEvents;
        }

        protected void RunChecked(SerializedObject serialized) {
            SerializedProperty _checked = serialized.FindProperty("_checked");

            EditorGUI.PropertyField(GetRect(), _checked, GetGUIContent("Checked"));
            _ = MoveDown();
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onClick = serialized.FindProperty("onClick");
            SerializedProperty checkBoxOn = serialized.FindProperty("checkBoxOn");
            SerializedProperty checkBoxOff = serialized.FindProperty("checkBoxOff");
            SerializedProperty onChecked = serialized.FindProperty("onChecked");

            float onClickHeight = EditorGUI.GetPropertyHeight(onClick);
            float checkBoxOnHeight = EditorGUI.GetPropertyHeight(checkBoxOn);
            float checkBoxOffHeight = EditorGUI.GetPropertyHeight(checkBoxOff);
            float onCheckedHeight = EditorGUI.GetPropertyHeight(onChecked);

            EditorGUI.PropertyField(GetRect(), onClick);
            _ = MoveDown(onClickHeight);
            EditorGUI.PropertyField(GetRect(), checkBoxOn);
            _ = MoveDown(checkBoxOnHeight);
            EditorGUI.PropertyField(GetRect(), checkBoxOff);
            _ = MoveDown(checkBoxOffHeight);
            EditorGUI.PropertyField(GetRect(), onChecked);
            _ = MoveDown(onCheckedHeight);
        }
    }
}
