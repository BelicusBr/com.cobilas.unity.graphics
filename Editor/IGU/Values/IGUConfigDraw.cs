using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUConfig))]
    public class IGUConfigDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_isVisible = property.FindPropertyRelative("isVisible");
            SerializedProperty prop_isEnabled = property.FindPropertyRelative("isEnabled");
            SerializedProperty prop_depth = property.FindPropertyRelative("depth");
            SerializedProperty prop_type = property.FindPropertyRelative("type");

            bool foldout = prop_foldout.boolValue;
            bool isVisible = prop_isVisible.boolValue;
            bool isEnabled = prop_isEnabled.boolValue;
            int mouseType = prop_type.enumValueIndex;
            int depth = prop_depth.intValue;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                isVisible = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("is visible"), isVisible);
                isEnabled = EditorGUI.Toggle(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("is enabled"), isEnabled);
                depth = EditorGUI.IntField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("depth"), depth);
                MouseButtonType buttonType = (MouseButtonType)EditorGUI.EnumPopup(MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("mouse type"), (MouseButtonType)mouseType);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_depth.intValue = depth;
                    prop_isVisible.boolValue = isVisible;
                    prop_isEnabled.boolValue = isEnabled;
                    prop_type.enumValueIndex = (int)buttonType;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return (SingleRowHeightWithBlankSpace * 5f);
        }
    }
}
