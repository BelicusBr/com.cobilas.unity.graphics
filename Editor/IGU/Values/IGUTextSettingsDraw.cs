using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUTextSettings))]
    [IGUCustomFieldDrawer("#IGUTextSettings")]
    public class IGUTextSettingsDraw : CPropertyDrawer {
        private IGUTextSettings settings = new IGUTextSettings();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_cursorColor = property.FindPropertyRelative("cursorColor");
            SerializedProperty prop_selectionColor = property.FindPropertyRelative("selectionColor");
            SerializedProperty prop_cursorFlashSpeed = property.FindPropertyRelative("cursorFlashSpeed");
            SerializedProperty prop_doubleClickSelectsWord = property.FindPropertyRelative("doubleClickSelectsWord");
            SerializedProperty prop_tripleClickSelectsLine = property.FindPropertyRelative("tripleClickSelectsLine");

            bool foldout = prop_foldout.boolValue;
            Color cursorColor = prop_cursorColor.colorValue;
            Color selectionColor = prop_selectionColor.colorValue;
            float cursorFlashSpeed = prop_cursorFlashSpeed.floatValue;
            bool doubleClickSelectsWord = prop_doubleClickSelectsWord.boolValue;
            bool tripleClickSelectsLine = prop_tripleClickSelectsLine.boolValue;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                cursorColor = EditorGUI.ColorField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("cursor color"), cursorColor);
                selectionColor = EditorGUI.ColorField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("selection color"), selectionColor);
                cursorFlashSpeed = EditorGUI.FloatField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("cursor flash speed"), cursorFlashSpeed);
                doubleClickSelectsWord = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("double click selects word"), doubleClickSelectsWord);
                tripleClickSelectsLine = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("triple click selects line"), tripleClickSelectsLine);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_cursorColor.colorValue = cursorColor;
                    prop_selectionColor.colorValue = selectionColor;
                    prop_cursorFlashSpeed.floatValue = cursorFlashSpeed;
                    prop_doubleClickSelectsWord.boolValue = doubleClickSelectsWord;
                    prop_tripleClickSelectsLine.boolValue = tripleClickSelectsLine;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return (SingleLineHeight * 6f) + (BlankSpace * 5f);
        }
    }
}
