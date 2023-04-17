using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(MaxMinSliderInt))]
    public class MaxMinSliderIntDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_min = property.FindPropertyRelative("min");
            SerializedProperty prop_max = property.FindPropertyRelative("max");
            bool foldout = prop_foldout.boolValue;
            int min = prop_min.intValue;
            int max = prop_max.intValue;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                min = EditorGUI.IntField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("min"), min);
                max = EditorGUI.IntField(MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("max"), max);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_min.intValue = min;
                    prop_max.intValue = max;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return (SingleLineHeight * 3f) + (BlankSpace * 2f);
        }
    }
}
