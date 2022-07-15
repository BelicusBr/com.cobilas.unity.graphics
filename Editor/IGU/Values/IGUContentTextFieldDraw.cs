using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomFieldDrawer("#TFIGUContent")]
    public class IGUContentTextFieldDraw : CPropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_text = property.FindPropertyRelative("text");
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");

            string text = prop_text.stringValue;
            bool foldout = prop_foldout.boolValue;

            if (property.isExpanded = foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();

                EditorGUI.indentLevel++;
                position = MoveDownWithBlankSpace(position);
                text = EditorGUI.TextField(position, IGUTextObject.GetGUIContentTemp("Text"), text);
                EditorGUI.indentLevel--;

                if (EditorGUI.EndChangeCheck())
                    prop_text.stringValue = text;
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.isExpanded) return SingleLineHeight;
            return SingleRowHeightWithBlankSpace * 2f;
        }
    }
}
