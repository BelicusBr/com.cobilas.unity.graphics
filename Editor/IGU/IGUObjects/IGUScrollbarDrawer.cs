using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUHorizontalScrollbar))]
    [CustomPropertyDrawer(typeof(IGUVerticalScrollbar))]
    public class IGUScrollbarDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUSliderObject obj = serialized.targetObject as IGUSliderObject;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");

                SerializedProperty prop_value = serialized.FindProperty("value");
                SerializedProperty prop_isInt = serialized.FindProperty("isInt");
                SerializedProperty prop_maxMinSlider = serialized.FindProperty("maxMinSlider");

                SerializedProperty prop_scrollbarThumbSize = serialized.FindProperty("scrollbarThumbSize");
                SerializedProperty prop_onModifiedScrollbar = serialized.FindProperty("onModifiedScrollbar");
                SerializedProperty prop_onModifiedScrollbarInt = serialized.FindProperty("onModifiedScrollbarInt");

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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 3f 
                        + EditorGUI.GetPropertyHeight(prop_maxMinSlider) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    Rect recttemp;
                    _ = EditorGUI.PropertyField(position, prop_value, EditorGUIUtility.TrTempContent("Value"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(position), prop_isInt, EditorGUIUtility.TrTempContent("Is int"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_scrollbarThumbSize, EditorGUIUtility.TrTempContent("Size"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(recttemp), prop_maxMinSlider, EditorGUIUtility.TrTempContent("Min max"));
                    --EditorGUI.indentLevel;

                    _ = EditorGUI.PropertyField(position = MoveDown(position, pheight + BlankSpace), prop_onModifiedScrollbar, EditorGUIUtility.TrTempContent("On Modified Scrollbar"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_onModifiedScrollbar) + BlankSpace), prop_onModifiedScrollbarInt, EditorGUIUtility.TrTempContent("On Modified Scrollbar Int"));
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
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfig");
                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_maxMinSlider = serialized.FindProperty("maxMinSlider");
                SerializedProperty prop_onModifiedScrollbar = serialized.FindProperty("onModifiedScrollbar");
                SerializedProperty prop_onModifiedScrollbarInt = serialized.FindProperty("onModifiedScrollbarInt");
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 7f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_maxMinSlider) +
                        EditorGUI.GetPropertyHeight(prop_onModifiedScrollbar) +
                        EditorGUI.GetPropertyHeight(prop_onModifiedScrollbarInt) +
                        BlankSpace * 7f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private void DrawBackground(Rect position, float height) {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
