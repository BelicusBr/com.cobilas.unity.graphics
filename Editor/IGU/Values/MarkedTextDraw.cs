using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(MarkedText))]
    [IGUCustomFieldDrawer("#MarkedText")]
    public class MarkedTextDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_text = property.FindPropertyRelative("text");
            SerializedProperty prop_textColor = property.FindPropertyRelative("textColor");
            SerializedProperty prop_unchangeText = property.FindPropertyRelative("unchangeText");
            SerializedProperty prop_fontStyle = property.FindPropertyRelative("fontStyle");

            bool foldout = prop_foldout.boolValue;
            string text = prop_text.stringValue;
            Color textColor = prop_textColor.colorValue;
            bool unchangeText = prop_unchangeText.boolValue;
            FontStyle fontStyle = (FontStyle)prop_fontStyle.enumValueIndex;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                text = EditorGUI.TextField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("text"), text);
                textColor = EditorGUI.ColorField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("text color"), textColor);
                unchangeText = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("unchange text"), unchangeText);
                fontStyle = (FontStyle)EditorGUI.EnumPopup(MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("font style"), fontStyle);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_text.stringValue = text;
                    prop_textColor.colorValue = textColor;
                    prop_unchangeText.boolValue = unchangeText;
                    prop_fontStyle.enumValueIndex = (int)fontStyle;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return (SingleLineHeight * 5f) + (BlankSpace * 4f);
        }
    }
}
