﻿using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUTextField))]
    public class IGUTextFieldDraw : IGUTextFieldObjectDraw {

        protected override void IOnGUI(Rect position, SerializedObject serialized)
            => base.IOnGUI(position, serialized);

        protected override float IGetPropertyHeight(SerializedObject serialized) {

            return (base.IGetPropertyHeight(serialized) - SingleLineHeight) + 
                (SingleRowHeightWithBlankSpace * 3f) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onClick")) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onKeyDown")) +
                EditorGUI.GetPropertyHeight(serialized.FindProperty("onCharDown"));
        }

        protected override Rect MoveDown()
            => base.MoveDown();

        protected override Rect MoveDown(float height)
            => base.MoveDown(height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun() {
            ResetInternalOnGUI();
            internalOnGUI += RunName;
            internalOnGUI += RunFiliation;
            internalOnGUI += RunModifiers;
            internalOnGUI += RunIGUContent;
            internalOnGUI += RunIGUTextSettings;
            internalOnGUI += RunMaxLength;
            internalOnGUI += RunIsFocused;
            internalOnGUI += RunIsTextArea;
            internalOnGUI += RunOnEvents;
        }

        protected virtual void RunOnEvents(SerializedObject serialized) {
            SerializedProperty onClick = serialized.FindProperty("onClick");
            SerializedProperty onKeyDown = serialized.FindProperty("onKeyDown");
            SerializedProperty onCharDown = serialized.FindProperty("onCharDown");

            float onClickHeight = EditorGUI.GetPropertyHeight(onClick);
            float onKeyDownHeight = EditorGUI.GetPropertyHeight(onKeyDown);
            float onCharDownHeight = EditorGUI.GetPropertyHeight(onCharDown);

            EditorGUI.PropertyField(GetRect(), onClick);
            EditorGUI.PropertyField(MoveDown(onClickHeight), onKeyDown);
            EditorGUI.PropertyField(MoveDown(onKeyDownHeight), onCharDown);
            _ = MoveDown(onCharDownHeight);
        }

        protected void RunMaxLength(SerializedObject serialized) {
            SerializedProperty maxLength = serialized.FindProperty("maxLength");
            EditorGUI.PropertyField(GetRect(), maxLength, GetGUIContent("Max length"));
            _ = MoveDown();
        }

        protected void RunIsFocused(SerializedObject serialized) {
            SerializedProperty isFocused = serialized.FindProperty("isFocused");
            EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(GetRect(), isFocused, GetGUIContent("Is focused"));
            EditorGUI.EndDisabledGroup();
            _ = MoveDown();
        }

        protected void RunIsTextArea(SerializedObject serialized) {
            SerializedProperty isTextArea = serialized.FindProperty("isTextArea");
            EditorGUI.PropertyField(GetRect(), isTextArea, GetGUIContent("Is text area"));
            _ = MoveDown();
        }

        protected override void RunIGUContent(SerializedObject serialized) {
            SerializedProperty isTextArea = serialized.FindProperty("isTextArea");
            if (isTextArea.boolValue) RunIGUContentTA(serialized);
            else RunIGUContentTF(serialized);
        }

        protected override float GetHeightIGUContent(SerializedObject serialized) {
            SerializedProperty isTextArea = serialized.FindProperty("isTextArea");
            if (isTextArea.boolValue) return IGUPropertyDrawer.GetPropertyFieldDrawer($"#TA{nameof(IGUContent)}").GetPropertyHeight(serialized.FindProperty("content"), null);
            return IGUPropertyDrawer.GetPropertyFieldDrawer($"#TF{nameof(IGUContent)}").GetPropertyHeight(serialized.FindProperty("content"), null);
        }

        private void RunIGUContentTA(SerializedObject serialized) {
            SerializedProperty content = serialized.FindProperty("content");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#TA{nameof(IGUContent)}").GetPropertyHeight(content, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#TA{nameof(IGUContent)}").OnGUI(GetRect(), content, GetGUIContent("Content"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }

        private void RunIGUContentTF(SerializedObject serialized) {
            SerializedProperty content = serialized.FindProperty("content");

            float height = IGUPropertyDrawer.GetPropertyFieldDrawer($"#TF{nameof(IGUContent)}").GetPropertyHeight(content, null);
            DrawBackgroundProperty(GetRect(), height + BlankSpace);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer($"#TF{nameof(IGUContent)}").OnGUI(GetRect(), content, GetGUIContent("Content"));
            _ = MoveDown(height + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}
