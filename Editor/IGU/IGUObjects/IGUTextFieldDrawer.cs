using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUTextField))]
    public class IGUTextFieldDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUTextField obj = serialized.targetObject as IGUTextField;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");
                SerializedProperty prop_useTooltip = serialized.FindProperty("useTooltip");
                SerializedProperty prop_content = serialized.FindProperty("content");
                SerializedProperty prop_settings = serialized.FindProperty("settings");

                SerializedProperty prop_maxLength = serialized.FindProperty("maxLength");
                SerializedProperty prop_isTextArea = serialized.FindProperty("isTextArea");
                SerializedProperty prop_isFocused = serialized.FindProperty("isFocused");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_onKeyDown = serialized.FindProperty("onKeyDown");
                SerializedProperty prop_onCharDown = serialized.FindProperty("onCharDown");

                /*
        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;

                        [SerializeField] protected int maxLength;
        [SerializeField, HideInInspector] 
        protected bool isFocused;
        [SerializeField] protected bool isTextArea;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUTextFieldKeyCodeEvent onKeyDown;
        [SerializeField] protected IGUTextFieldKeyCharEvent onCharDown;
                 */

                prop_foldout.boolValue = EditorGUI.Foldout(position, prop_foldout.boolValue,
                    EditorGUIUtility.TrTempContent($"[{temp.GetType().Name}]{temp.name}"));

                if (prop_foldout.boolValue) {
                    DrawBackground(position = MoveDownWithBlankSpace(position), SingleRowHeightWithBlankSpace * 3f);
                    ++EditorGUI.indentLevel;
                    obj.name = EditorGUI.TextField(position, EditorGUIUtility.TrTempContent("Name"), obj.name);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(position = MoveDownWithBlankSpace(position), prop_parent, EditorGUIUtility.TrTempContent("parent"));
                    _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_container, EditorGUIUtility.TrTempContent("Container"));
                    EditorGUI.EndDisabledGroup();
                    --EditorGUI.indentLevel;

                    position.y += BlankSpace;
                    float pheight;
                    DrawBackground(position = MoveDownWithBlankSpace(position), pheight = EditorGUI.GetPropertyHeight(prop_myRect));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myRect, EditorGUIUtility.TrTempContent("Rect"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_myColor));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myColor, EditorGUIUtility.TrTempContent("Color"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_myConfg));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myConfg, EditorGUIUtility.TrTempContent("Config"));
                    --EditorGUI.indentLevel;

                    float cheight = prop_content.FindPropertyRelative("foldout").boolValue ? SingleRowHeightWithBlankSpace * 3f : SingleRowHeightWithBlankSpace;
                    bool isTextArea = prop_isTextArea.boolValue;
                    cheight += isTextArea ? SingleLineHeight * 2f : 0f;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace +
                        cheight + BlankSpace);
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_useTooltip, EditorGUIUtility.TrTempContent("Use Tooltip"));
                    DrawIGUContent(prop_content, MoveDownWithBlankSpace(position), isTextArea);
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_settings));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_settings, EditorGUIUtility.TrTempContent("Settings"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 3f);
                    ++EditorGUI.indentLevel;
                    Rect recttemp;
                    _ = EditorGUI.PropertyField(position, prop_isTextArea, EditorGUIUtility.TrTempContent("Is Text Area"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(position), prop_maxLength, EditorGUIUtility.TrTempContent("Max length"));
                    EditorGUI.BeginDisabledGroup(true);
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(recttemp), prop_isFocused, EditorGUIUtility.TrTempContent("Is Focused"));
                    EditorGUI.EndDisabledGroup();
                    --EditorGUI.indentLevel;

                    _ = EditorGUI.PropertyField(position = MoveDown(position, pheight + BlankSpace), prop_onClick, EditorGUIUtility.TrTempContent("On Click"));
                    _ = EditorGUI.PropertyField(position = MoveDown(position, EditorGUI.GetPropertyHeight(prop_onClick) + BlankSpace), prop_onKeyDown, EditorGUIUtility.TrTempContent("On Key Down"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_onKeyDown) + BlankSpace), prop_onCharDown, EditorGUIUtility.TrTempContent("On Char Down"));
                }
                serialized.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Object temp = property.objectReferenceValue;
            if (temp != null) {
                SerializedObject serialized = new SerializedObject(temp);
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");
                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_content = serialized.FindProperty("content");
                float cheight = prop_content.FindPropertyRelative("foldout").boolValue ? SingleRowHeightWithBlankSpace * 3f : SingleRowHeightWithBlankSpace;
                SerializedProperty prop_settings = serialized.FindProperty("settings");
                SerializedProperty prop_onClick = serialized.FindProperty("onClick");
                SerializedProperty prop_isTextArea = serialized.FindProperty("isTextArea");
                SerializedProperty prop_onKeyDown = serialized.FindProperty("onKeyDown");
                SerializedProperty prop_onCharDown = serialized.FindProperty("onCharDown");

                bool isTextArea = prop_isTextArea.boolValue;
                cheight += isTextArea ? SingleLineHeight * 2f : 0f;

                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 8f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_settings) +
                        EditorGUI.GetPropertyHeight(prop_onClick) +
                        EditorGUI.GetPropertyHeight(prop_onKeyDown) +
                        EditorGUI.GetPropertyHeight(prop_onCharDown) +
                        cheight + BlankSpace * 10f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private void DrawIGUContent(SerializedProperty property, Rect position, bool isTextArea) {
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_text = property.FindPropertyRelative("text");
            SerializedProperty prop_tooltip = property.FindPropertyRelative("tooltip");

            if (prop_foldout.boolValue = EditorGUI.Foldout(position, prop_foldout.boolValue, EditorGUIUtility.TrTempContent("Content"))) {
                string txt = prop_text.stringValue;
                float height = SingleRowHeightWithBlankSpace;
                EditorGUI.BeginChangeCheck();
                if (isTextArea) {
                    position.height = height = SingleLineHeight * 3f;
                    height += BlankSpace;
                    txt = EditorGUI.TextArea(position = MoveDownWithBlankSpace(position), txt);
                    position.height = SingleLineHeight;
                } else txt = EditorGUI.TextField(position = MoveDownWithBlankSpace(position), EditorGUIUtility.TrTempContent("Text"), txt);
                if (EditorGUI.EndChangeCheck())
                    prop_text.stringValue = txt;
                //_ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_text, EditorGUIUtility.TrTempContent("Text"));
                _ = EditorGUI.PropertyField(MoveDown(position, height), prop_tooltip, EditorGUIUtility.TrTempContent("Tooltip"));
            }
        }

        private void DrawBackground(Rect position, float height)
        {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
