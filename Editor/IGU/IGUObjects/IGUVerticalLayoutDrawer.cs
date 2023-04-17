using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Layouts;

namespace Cobilas.Unity.Editor.Graphics.IGU
{
    [CustomPropertyDrawer(typeof(IGUVerticalLayout))]
    public class IGUVerticalLayoutDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUVerticalLayout obj = serialized.targetObject as IGUVerticalLayout;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");

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

                    //position.y += EditorGUIUtility.standardVerticalSpacing;
                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), EditorGUI.GetPropertyHeight(prop_myConfg));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myConfg, EditorGUIUtility.TrTempContent("Config"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), SingleRowHeightWithBlankSpace * 2f +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, EditorGUIUtility.TrTempContent("Cell Size")) +
                        BlankSpace);
                    ++EditorGUI.indentLevel;
                    obj.Spacing = EditorGUI.FloatField(position, EditorGUIUtility.TrTempContent("Spacing"), obj.Spacing);
                    obj.CellSize = EditorGUI.Vector2Field(position = MoveDownWithBlankSpace(position), EditorGUIUtility.TrTempContent("Cell Size"), obj.CellSize);
                    obj.UseCellSize = EditorGUI.Toggle(MoveDown(position, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, EditorGUIUtility.TrTempContent("Cell Size")) + BlankSpace),
                        EditorGUIUtility.TrTempContent("Use Cell Size"), obj.UseCellSize);
                    --EditorGUI.indentLevel;
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
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 6f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, EditorGUIUtility.TrTempContent("Cell Size")) +
                        BlankSpace * 4f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private void DrawBackground(Rect position, float height) {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
