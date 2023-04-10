using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IGUStyleCustom))]
    public class IGUStyleCustomEditor : UnityEditor.Editor {
        private SerializedProperty prop_name;
        private SerializedProperty prop_fixedWidth;
        private SerializedProperty prop_fixedHeight;
        private SerializedProperty prop_stretchWidth;
        private SerializedProperty prop_stretchHeight;
        private SerializedProperty prop_font;
        private SerializedProperty prop_fontSize;
        private SerializedProperty prop_fontStyle;
        private SerializedProperty prop_wordWrap;
        private SerializedProperty prop_richText;
        private SerializedProperty prop_clipOffset;
        private SerializedProperty prop_alignment;
        private SerializedProperty prop_imagePosition;
        private SerializedProperty prop_clipping;
        private SerializedProperty prop_contentOffset;
        private SerializedProperty prop_normal;
        private SerializedProperty prop_hover;
        private SerializedProperty prop_active;
        private SerializedProperty prop_focused;
        private SerializedProperty prop_onNormal;
        private SerializedProperty prop_onHover;
        private SerializedProperty prop_onActive;
        private SerializedProperty prop_onFocused;
        private SerializedProperty prop_border;
        private SerializedProperty prop_margin;
        private SerializedProperty prop_padding;
        private SerializedProperty prop_overflow;

        private void OnEnable() {
            SerializedProperty prop_style = serializedObject.FindProperty("style");
            prop_name = prop_style.FindPropertyRelative("name");
            prop_fixedWidth = prop_style.FindPropertyRelative("fixedWidth");
            prop_fixedHeight = prop_style.FindPropertyRelative("fixedHeight");
            prop_stretchWidth = prop_style.FindPropertyRelative("stretchWidth");
            prop_stretchHeight = prop_style.FindPropertyRelative("stretchHeight");
            prop_font = prop_style.FindPropertyRelative("font");
            prop_fontSize = prop_style.FindPropertyRelative("fontSize");
            prop_fontStyle = prop_style.FindPropertyRelative("fontStyle");
            prop_wordWrap = prop_style.FindPropertyRelative("wordWrap");
            prop_richText = prop_style.FindPropertyRelative("richText");
            prop_clipOffset = prop_style.FindPropertyRelative("clipOffset");
            prop_alignment = prop_style.FindPropertyRelative("alignment");
            prop_imagePosition = prop_style.FindPropertyRelative("imagePosition");
            prop_clipping = prop_style.FindPropertyRelative("clipping");
            prop_contentOffset = prop_style.FindPropertyRelative("contentOffset");
            prop_normal = prop_style.FindPropertyRelative("normal");
            prop_hover = prop_style.FindPropertyRelative("hover");
            prop_active = prop_style.FindPropertyRelative("active");
            prop_focused = prop_style.FindPropertyRelative("focused");
            prop_onNormal = prop_style.FindPropertyRelative("onNormal");
            prop_onHover = prop_style.FindPropertyRelative("onHover");
            prop_onActive = prop_style.FindPropertyRelative("onActive");
            prop_onFocused = prop_style.FindPropertyRelative("onFocused");
            prop_border = prop_style.FindPropertyRelative("border");
            prop_margin = prop_style.FindPropertyRelative("margin");
            prop_padding = prop_style.FindPropertyRelative("padding");
            prop_overflow = prop_style.FindPropertyRelative("overflow");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent("Name"), EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            _ = EditorGUILayout.PropertyField(prop_name, GUIContent.none);
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();

            DrawGUIStyleStatus("Normal", prop_normal);
            DrawGUIStyleStatus("Hover", prop_hover);
            DrawGUIStyleStatus("Active", prop_active);
            DrawGUIStyleStatus("Focused", prop_focused);
            DrawGUIStyleStatus("OnNormal", prop_onNormal);
            DrawGUIStyleStatus("OnHover", prop_onHover);
            DrawGUIStyleStatus("OnActive", prop_onActive);
            DrawGUIStyleStatus("OnFocused", prop_onFocused);

            DrawRectOffset("Border", prop_border);
            DrawRectOffset("Margin", prop_margin);
            DrawRectOffset("Padding", prop_padding);
            DrawRectOffset("Overflow", prop_overflow);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent("Others"), EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            EditorGUILayout.ObjectField(prop_font, EditorGUIUtility.TrTempContent("Font"));
            _ = EditorGUILayout.PropertyField(prop_fontSize, EditorGUIUtility.TrTempContent("Font Size"));
            _ = EditorGUILayout.PropertyField(prop_fontStyle, EditorGUIUtility.TrTempContent("Font Style"));
            _ = EditorGUILayout.PropertyField(prop_alignment, EditorGUIUtility.TrTempContent("Alignment"));
            _ = EditorGUILayout.PropertyField(prop_wordWrap, EditorGUIUtility.TrTempContent("Word Wrap"));
            _ = EditorGUILayout.PropertyField(prop_richText, EditorGUIUtility.TrTempContent("Rich Text"));
            _ = EditorGUILayout.PropertyField(prop_clipping, EditorGUIUtility.TrTempContent("Text Clipping"));
            _ = EditorGUILayout.PropertyField(prop_imagePosition, EditorGUIUtility.TrTempContent("Image Position"));
            _ = EditorGUILayout.PropertyField(prop_contentOffset, EditorGUIUtility.TrTempContent("Content Offset"));
            _ = EditorGUILayout.PropertyField(prop_fixedWidth, EditorGUIUtility.TrTempContent("Fixed Width"));
            _ = EditorGUILayout.PropertyField(prop_fixedHeight, EditorGUIUtility.TrTempContent("Fixed Height"));
            _ = EditorGUILayout.PropertyField(prop_stretchWidth, EditorGUIUtility.TrTempContent("Stretch Width"));
            _ = EditorGUILayout.PropertyField(prop_stretchHeight, EditorGUIUtility.TrTempContent("Stretch Height"));
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();

            if (serializedObject.ApplyModifiedProperties())
                EditorUtility.SetDirty(target);
                //Debug.Log("ApplyModifiedProperties");
        }

        private void DrawRectOffset(string name, SerializedProperty prop) {
            SerializedProperty prop_foldout = prop.FindPropertyRelative("foldout");
            SerializedProperty prop_rectOffSet_xy = prop.FindPropertyRelative("rectOffSet_xy");
            SerializedProperty prop_rectOffSet_zw = prop.FindPropertyRelative("rectOffSet_zw");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            ++EditorGUI.indentLevel;
            if (prop_foldout.boolValue = EditorGUILayout.Foldout(prop_foldout.boolValue, name)) {
                Vector2Int rectOffSet_xy = prop_rectOffSet_xy.vector2IntValue;
                Vector2Int rectOffSet_zw = prop_rectOffSet_zw.vector2IntValue;
                ++EditorGUI.indentLevel;
                EditorGUI.BeginChangeCheck();
                rectOffSet_xy.x = EditorGUILayout.IntField(EditorGUIUtility.TrTempContent("Left"), rectOffSet_xy.x);
                rectOffSet_xy.y = EditorGUILayout.IntField(EditorGUIUtility.TrTempContent("Right"), rectOffSet_xy.y);
                rectOffSet_zw.x = EditorGUILayout.IntField(EditorGUIUtility.TrTempContent("Top"), rectOffSet_zw.x);
                rectOffSet_zw.y = EditorGUILayout.IntField(EditorGUIUtility.TrTempContent("Bottom"), rectOffSet_zw.y);
                if (EditorGUI.EndChangeCheck()) {
                    prop_rectOffSet_xy.vector2IntValue = rectOffSet_xy;
                    prop_rectOffSet_zw.vector2IntValue = rectOffSet_zw;
                }
                --EditorGUI.indentLevel;
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }

        private void DrawGUIStyleStatus(string text, SerializedProperty prop) {
            SerializedProperty prop_foldout = prop.FindPropertyRelative("foldout");
            SerializedProperty prop_background = prop.FindPropertyRelative("background");
            SerializedProperty prop_textColor = prop.FindPropertyRelative("textColor");
            SerializedProperty prop_scaledBackgrounds = prop.FindPropertyRelative("scaledBackgrounds");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent(text), EditorStyles.boldLabel);

            ++EditorGUI.indentLevel;
            _ = EditorGUILayout.PropertyField(prop_background, EditorGUIUtility.TrTextContent("Background"), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            _ = EditorGUILayout.PropertyField(prop_textColor, EditorGUIUtility.TrTextContent("Text color"));
            int size = prop_scaledBackgrounds.arraySize;
            if (prop_foldout.boolValue = EditorGUILayout.Foldout(prop_foldout.boolValue, "Scaled Backgrounds")) {
                ++EditorGUI.indentLevel;
                EditorGUI.BeginChangeCheck();
                size = EditorGUILayout.IntField(EditorGUIUtility.TrTempContent("Size"), size);
                size = size < 0 ? 0 : size;
                if (EditorGUI.EndChangeCheck())
                    prop_scaledBackgrounds.arraySize = size;
                for (int I = 0; I < size; I++) {
                    SerializedProperty temp = prop_scaledBackgrounds.GetArrayElementAtIndex(I);
                    EditorGUILayout.ObjectField(temp, GUIContent.none, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
                --EditorGUI.indentLevel;
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }
    }
}