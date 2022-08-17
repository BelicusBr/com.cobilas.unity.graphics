using System;
using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUObject), true)]
    public class IGUObjectDraw : IGUObjectPropertyDrawer {

        protected event Action<SerializedObject> internalOnGUI;

        protected override void IOnGUI(Rect position, SerializedObject serialized) {
            position.height = CPropertyDrawer.SingleLineHeight;
            Resize(position.size);
            BuildRun();
            try {
                if (Foldout(serialized))
                    internalOnGUI?.Invoke(serialized);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        protected override float IGetPropertyHeight(SerializedObject serialized) {
            SerializedProperty myRect = serialized.FindProperty("myRect");
            SerializedProperty myColor = serialized.FindProperty("myColor");
            SerializedProperty myConfg = serialized.FindProperty("myConfg");

            float myRectHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGURect").GetPropertyHeight(myRect, null);
            float myColorHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUColor").GetPropertyHeight(myColor, null);
            float myConfgHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUConfig").GetPropertyHeight(myConfg, null);
            return (SingleRowHeightWithBlankSpace * 7f) + (BlankSpace * 3f) + myRectHeight + myConfgHeight + myColorHeight;
        }

        private bool Foldout(SerializedObject serialized) {
            SerializedProperty foldout = serialized.FindProperty("foldout");
            string name = $"{serialized.targetObject.name}[type:{serialized.targetObject.GetType().Name}]";
            return foldout.boolValue = EditorGUI.Foldout(GetRect(), foldout.boolValue, GetGUIContent(name));
        }

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override Rect MoveDown()
            => base.MoveDown();

        protected override Rect MoveDown(float height)
            => base.MoveDown(height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override Rect Spacing()
            => base.Spacing();

        protected void ResetInternalOnGUI() => internalOnGUI = (Action<SerializedObject>)null;

        protected virtual void BuildRun() {
            ResetInternalOnGUI();
            internalOnGUI += RunName;
            internalOnGUI += RunFiliation;
            internalOnGUI += RunModifiers;
        }

        protected void RunName(SerializedObject serialized) {
            _ = EditorGUI.PropertyField(MoveDown(), serialized.FindProperty("subname"), GetGUIContent("Name"));
        }

        protected void RunFiliation(SerializedObject serialized) {
            DrawBackgroundProperty(MoveDown(), GetGUIContent("Filiation"), (SingleRowHeightWithBlankSpace * 3f) + BlankSpace);
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);
                _ = EditorGUI.IntField(MoveDown(), GetGUIContent("ID"), serialized.targetObject.GetInstanceID());
                _ = EditorGUI.PropertyField(MoveDown(), serialized.FindProperty("parent"), GetGUIContent("Parent"));
                _ = EditorGUI.PropertyField(MoveDown(), serialized.FindProperty("container"), GetGUIContent("Container"));
            EditorGUI.EndDisabledGroup();
            _ = Spacing();
            EditorGUI.indentLevel--;
        }

        protected void RunModifiers(SerializedObject serialized) {
            SerializedProperty myRect = serialized.FindProperty("myRect");
            SerializedProperty myColor = serialized.FindProperty("myColor");
            SerializedProperty myConfg = serialized.FindProperty("myConfg");

            float myRectHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGURect").GetPropertyHeight(myRect, null);
            float myColorHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUColor").GetPropertyHeight(myColor, null);
            float myConfgHeight = IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUConfig").GetPropertyHeight(myConfg, null);

            DrawBackgroundProperty(MoveDown(), GetGUIContent("Modifiers"), myRectHeight + myColorHeight + myConfgHeight + BlankSpace * 2f);
            EditorGUI.indentLevel++;
            IGUPropertyDrawer.GetPropertyFieldDrawer("#IGURect").OnGUI(MoveDown(), myRect, GetGUIContent("Rect"));
            IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUColor").OnGUI(MoveDown(myRectHeight), myColor, GetGUIContent("Main color"));
            IGUPropertyDrawer.GetPropertyFieldDrawer("#IGUConfig").OnGUI(MoveDown(myColorHeight), myConfg, GetGUIContent("Confg"));
            _ = MoveDown(myConfgHeight + BlankSpace);
            _ = Spacing();
            EditorGUI.indentLevel--;
        }
    }
}
