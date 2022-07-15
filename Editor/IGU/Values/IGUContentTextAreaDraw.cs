using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomFieldDrawer("#TAIGUContent")]
    public class IGUContentTextAreaDraw : CPropertyDrawer {

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
                position.height = SingleLineHeight * 5f;
                text = EditorGUI.TextArea(position, text);
                position.height = SingleLineHeight;
                EditorGUI.indentLevel--;

                if (EditorGUI.EndChangeCheck())
                    prop_text.stringValue = text;
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.isExpanded) return SingleLineHeight;
            return (SingleLineHeight * 6f) + (BlankSpace * 2f);
        }
    }
}
