using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUColor))]
    public class IGUColorDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_color = property.FindPropertyRelative("color");
            SerializedProperty prop_textColor = property.FindPropertyRelative("textColor");
            SerializedProperty prop_backgroundColor = property.FindPropertyRelative("backgroundColor");

            bool foldout = prop_foldout.boolValue;
            Color color = prop_color.colorValue;
            Color textColor = prop_textColor.colorValue;
            Color backgroundColor = prop_backgroundColor.colorValue;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                color = EditorGUI.ColorField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("color"), color);
                textColor = EditorGUI.ColorField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("text color"), textColor);
                backgroundColor = EditorGUI.ColorField(MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("background color"), backgroundColor);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_color.colorValue = color;
                    prop_textColor.colorValue = textColor;
                    prop_backgroundColor.colorValue = backgroundColor;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return (SingleLineHeight * 4f) + (BlankSpace * 3f);
        }
    }
}
