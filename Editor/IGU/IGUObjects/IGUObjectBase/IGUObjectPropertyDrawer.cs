using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    public class IGUObjectPropertyDrawer {
        private float height;
        private Rect curretPosition;

        protected float BlankSpace => CPropertyDrawer.BlankSpace;
        protected float SingleLineHeight => CPropertyDrawer.SingleLineHeight;
        protected float SingleRowHeightWithBlankSpace => CPropertyDrawer.SingleRowHeightWithBlankSpace;

        public void OnGUI(Rect position, SerializedObject serialized)
            => IOnGUI(curretPosition = position, serialized);

        public float GetPropertyHeight(SerializedObject serialized)
            => IGetPropertyHeight(serialized);

        protected virtual void IOnGUI(Rect position, SerializedObject serialized) { }

        protected virtual float IGetPropertyHeight(SerializedObject serialized) 
            => CPropertyDrawer.SingleRowHeightWithBlankSpace;

        protected Rect GetRect() => curretPosition;

        protected void Resize(Vector2 size) 
            => curretPosition.size = size;

        protected virtual Rect Spacing()
            => curretPosition = MoveDown(curretPosition, CPropertyDrawer.BlankSpace);

        protected virtual Rect MoveDown()
            => curretPosition = MoveDown(curretPosition, CPropertyDrawer.SingleRowHeightWithBlankSpace);

        protected virtual Rect MoveDown(float height)
            => curretPosition = MoveDown(curretPosition, height);

        protected virtual GUIContent GetGUIContent(string text)
            => IGUTextObject.GetGUIContentTemp(text);

        protected virtual void DrawBackgroundProperty(Rect position, GUIContent label, float height) {
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            position.height += height;
            GUI.Box(EditorGUI.IndentedRect(position), GetGUIContent(""), EditorStyles.helpBox);
        }

        protected virtual void DrawBackgroundProperty(Rect position, float height) {
            position.height = height;
            GUI.Box(EditorGUI.IndentedRect(position), GetGUIContent(""), EditorStyles.helpBox);
        }

        protected void AddHeight(float height) 
            => this.height += height;

        protected float GetHeight()
            => height;

        protected Rect MoveDown(Rect rect)
            => MoveDown(rect, SingleLineHeight);

        protected Rect MoveDown(Rect rect, float height) {
            rect.y += height;
            return rect;
        }
    }
}
