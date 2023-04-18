using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUNumericBox))]
    public class IGUNumericBoxDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUNumericBox obj = serialized.targetObject as IGUNumericBox;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");
                SerializedProperty prop_value = serialized.FindProperty("value");
                SerializedProperty prop_additionValue = serialized.FindProperty("additionValue");
                SerializedProperty prop_maxMinSlider = serialized.FindProperty("maxMinSlider");
                SerializedProperty prop_numberOfDecimalPlaces = serialized.FindProperty("numberOfDecimalPlaces");
                SerializedProperty prop_onClick1 = GetOnClick(serialized.FindProperty("buttonLeft"));
                SerializedProperty prop_onClick2 = GetOnClick(serialized.FindProperty("buttonRight"));

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

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = SingleRowHeightWithBlankSpace * 5f +
                        EditorGUI.GetPropertyHeight(prop_maxMinSlider) + BlankSpace);
                    ++EditorGUI.indentLevel;
                    Rect recttemp;
                    obj.UseTooltip = EditorGUI.Toggle(position, EditorGUIUtility.TrTempContent("Use Tooltip"), obj.UseTooltip);
                    EditorGUI.BeginDisabledGroup(!obj.UseTooltip);
                    obj.Tooltip = EditorGUI.TextField(recttemp = MoveDownWithBlankSpace(position), EditorGUIUtility.TrTempContent("Tooltip"), obj.Tooltip);
                    EditorGUI.EndDisabledGroup();
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_value, EditorGUIUtility.TrTempContent("Value"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_additionValue, EditorGUIUtility.TrTempContent("Addition Value"));
                    _ = EditorGUI.PropertyField(recttemp = MoveDownWithBlankSpace(recttemp), prop_numberOfDecimalPlaces, EditorGUIUtility.TrTempContent("Number Of Decimal Places"));
                    _ = EditorGUI.PropertyField(MoveDownWithBlankSpace(recttemp), prop_maxMinSlider, EditorGUIUtility.TrTempContent("maxMinSlider"));
                    --EditorGUI.indentLevel;

                    prop_onClick1.serializedObject.Update();
                    prop_onClick2.serializedObject.Update();
                    _ = EditorGUI.PropertyField(position = MoveDown(position, pheight + BlankSpace), prop_onClick1, EditorGUIUtility.TrTempContent("On Click Left"));
                    _ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_onClick1) + BlankSpace), prop_onClick2, EditorGUIUtility.TrTempContent("On Click Right"));
                    _ = prop_onClick1.serializedObject.ApplyModifiedProperties();
                    _ = prop_onClick2.serializedObject.ApplyModifiedProperties();
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
                SerializedProperty prop_maxMinSlider = serialized.FindProperty("maxMinSlider");
                SerializedProperty prop_onClick1 = GetOnClick(serialized.FindProperty("buttonLeft"));
                SerializedProperty prop_onClick2 = GetOnClick(serialized.FindProperty("buttonRight"));
                if (prop_foldout.boolValue)
                    return SingleRowHeightWithBlankSpace * 9f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(prop_maxMinSlider) +
                        EditorGUI.GetPropertyHeight(prop_onClick1) +
                        EditorGUI.GetPropertyHeight(prop_onClick2) +
                        BlankSpace * 7f;
            }
            return SingleRowHeightWithBlankSpace;
        }

        private SerializedProperty GetOnClick(SerializedProperty property) {
            SerializedObject serialized = new SerializedObject(property.objectReferenceValue);
            return serialized.FindProperty("onClick");
        }

        private void DrawBackground(Rect position, float height) {
            position.height = height;
            EditorGUI.HelpBox(position, string.Empty, MessageType.None);
        }
    }
}
