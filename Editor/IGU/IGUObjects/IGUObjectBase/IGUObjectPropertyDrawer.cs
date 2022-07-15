using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    public class IGUObjectPropertyDrawer : CPropertyDrawer {

        private float myHeight;
        private Rect curretPosition;
        private bool isInitPosition;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected void InitPosition(Rect position) {
            if (isInitPosition) throw new UnityException("InitPosition not finished!!!");
            isInitPosition = true;
            curretPosition = position;
            myHeight = 0;
        }

        protected Rect GetRect() => curretPosition;

        protected virtual Rect Spacing() {
            AddHeight(BlankSpace);
            return curretPosition = MoveDown(curretPosition, BlankSpace);
        }

        protected virtual Rect MoveDown() {
            AddHeight(SingleRowHeightWithBlankSpace);
            return curretPosition = MoveDownWithBlankSpace(curretPosition);
        }

        protected virtual Rect MoveDown(float height) {
            AddHeight(height);
            return curretPosition = MoveDown(curretPosition, height);
        }

        protected void AddHeight(float height) => myHeight += height;

        protected void FinishPosition() {
            if (!isInitPosition) throw new UnityException("InitPosition not started!!!");
            isInitPosition = false;
            //curretPosition = MoveDownWithBlankSpace(curretPosition);
        }

        protected float GetHeight() => myHeight;

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
    }
}
