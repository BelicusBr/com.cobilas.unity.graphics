using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUContent))]
    [IGUCustomFieldDrawer("#IGUContent")]
    public class IGUContentDraw : CPropertyDrawer {
        private IGUContent content = new IGUContent();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_hide_Tooltip = property.FindPropertyRelative("hide_Tooltip");
            SerializedProperty prop_text = property.FindPropertyRelative("text");
            SerializedProperty prop_image = property.FindPropertyRelative("image");
            SerializedProperty prop_tooltip = property.FindPropertyRelative("tooltip");

            bool foldout = prop_foldout.boolValue;
            bool hide_Tooltip = prop_hide_Tooltip.boolValue;
            string text = prop_text.stringValue;
            Object image = prop_image.objectReferenceValue;
            string tooltip = prop_tooltip.stringValue;

            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                text = EditorGUI.TextField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("text"), text);
                image = ObjectField<Texture>(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("image"), image as Texture);
                if (hide_Tooltip)
                    tooltip = EditorGUI.TextField(position = MoveDownWithBlankSpace(position), IGUTextObject.GetGUIContentTemp("tooltip"), tooltip);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_text.stringValue = text;
                    prop_image.objectReferenceValue = image;
                    prop_tooltip.stringValue = tooltip;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            if (!property.FindPropertyRelative("hide_Tooltip").boolValue)
                return (SingleLineHeight * 3f) + (BlankSpace * 2f);
            return (SingleLineHeight * 4f) + (BlankSpace * 3f);
        }

        private T ObjectField<T>(Rect position, GUIContent label, T value) where T : Object
            => (T)EditorGUI.ObjectField(position, label, value, typeof(T), true);
    }
}
