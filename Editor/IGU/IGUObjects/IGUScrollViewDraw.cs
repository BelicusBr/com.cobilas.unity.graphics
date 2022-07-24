using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUScrollView))]
    public class IGUScrollViewDraw : IGUObjectDraw {

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
            internalOnGUI += RunViewRect;
            internalOnGUI += RunAlwaysShowVertical;
            internalOnGUI += RunAlwaysShowHorizontal;
            internalOnGUI += RunEvents;
        }

        protected void RunViewRect(SerializedObject serialized) {
            SerializedProperty viewRect = serialized.FindProperty("viewRect");

            GUIContent content = GetGUIContent("View rect");

            EditorGUI.PropertyField(GetRect(), viewRect, content);
            _ = MoveDown(EditorGUI.GetPropertyHeight(SerializedPropertyType.Rect, content) + BlankSpace);
        }

        protected void RunAlwaysShowVertical(SerializedObject serialized) {
            SerializedProperty alwaysShowVertical = serialized.FindProperty("alwaysShowVertical");

            EditorGUI.PropertyField(GetRect(), alwaysShowVertical, GetGUIContent("Always show vertical"));
            _ = MoveDown();
        }

        protected void RunAlwaysShowHorizontal(SerializedObject serialized) {
            SerializedProperty alwaysShowHorizontal = serialized.FindProperty("alwaysShowHorizontal");

            EditorGUI.PropertyField(GetRect(), alwaysShowHorizontal, GetGUIContent("Always show horizontal"));
            _ = MoveDown();
        }

        protected void RunEvents(SerializedObject serialized) {
            SerializedProperty onScrollView = serialized.FindProperty("onScrollView");

            float onScrollViewHeight = EditorGUI.GetPropertyHeight(onScrollView);

            EditorGUI.PropertyField(GetRect(), onScrollView);
            _ = MoveDown(onScrollViewHeight);
        }
    }
}
