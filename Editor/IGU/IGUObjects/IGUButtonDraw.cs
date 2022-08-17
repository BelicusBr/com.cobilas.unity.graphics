using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUButton))]
    public class IGUButtonDraw : IGUTextObjectDraw {
        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized)
            => base.IGetPropertyHeight(serialized) + EditorGUI.GetPropertyHeight(serialized.FindProperty("onClick"));

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunEvents;
        }

        protected virtual void RunEvents(SerializedObject serialized) {
            SerializedProperty onClick = serialized.FindProperty("onClick");

            float onClickHeight = EditorGUI.GetPropertyHeight(onClick);

            EditorGUI.PropertyField(GetRect(), onClick);
            _ = MoveDown(onClickHeight);
        }
    }
}
