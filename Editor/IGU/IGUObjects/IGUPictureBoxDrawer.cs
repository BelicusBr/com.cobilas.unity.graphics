using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGUPictureBox))]
    public class IGUPictureBoxDrawer : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.height = SingleLineHeight;
            Object temp = property.objectReferenceValue;
            if (temp == null) EditorGUI.LabelField(position, label);
            else {
                SerializedObject serialized = new SerializedObject(temp);
                serialized.Update();
                IGUObject obj = serialized.targetObject as IGUObject;
                Undo.RecordObject(obj, "change_name");

                SerializedProperty prop_foldout = serialized.FindProperty("foldout");
                SerializedProperty prop_parent = serialized.FindProperty("parent");
                SerializedProperty prop_container = serialized.FindProperty("container");
                SerializedProperty prop_myRect = serialized.FindProperty("myRect");
                SerializedProperty prop_myColor = serialized.FindProperty("myColor");
                SerializedProperty prop_myConfg = serialized.FindProperty("myConfg");

                SerializedProperty prop_texture = serialized.FindProperty("texture");
                SerializedProperty prop_alphaBlend = serialized.FindProperty("alphaBlend");
                SerializedProperty prop_imageAspect = serialized.FindProperty("imageAspect");
                SerializedProperty prop_scaleMode = serialized.FindProperty("scaleMode");
                SerializedProperty prop_borderWidths = serialized.FindProperty("borderWidths");
                SerializedProperty prop_borderRadiuses = serialized.FindProperty("borderRadiuses");
                /*
                         [SerializeField] protected Texture texture;
        [SerializeField] protected bool alphaBlend;
        [SerializeField] protected float imageAspect;
        [SerializeField] protected ScaleMode scaleMode;
        [SerializeField] protected Vector4 borderWidths;
        [SerializeField] protected Vector4 borderRadiuses;
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

                    //position.y += EditorGUIUtility.standardVerticalSpacing;
                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), pheight = EditorGUI.GetPropertyHeight(prop_myConfg));
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_myConfg, EditorGUIUtility.TrTempContent("Config"));
                    --EditorGUI.indentLevel;

                    DrawBackground(position = MoveDown(position, pheight + BlankSpace), SingleRowHeightWithBlankSpace * 4f +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, EditorGUIUtility.TrTempContent("Border Widths")) +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, EditorGUIUtility.TrTempContent("Border Radiuses")) +
                        BlankSpace * 2f);
                    ++EditorGUI.indentLevel;
                    _ = EditorGUI.PropertyField(position, prop_texture, EditorGUIUtility.TrTempContent("Texture"));
                    _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_alphaBlend, EditorGUIUtility.TrTempContent("Alpha Blend"));
                    _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_imageAspect, EditorGUIUtility.TrTempContent("Image Aspect"));
                    _ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_scaleMode, EditorGUIUtility.TrTempContent("Scale Mode"));
                    Vector4 borderWidths = prop_borderWidths.vector4Value;
                    Vector4 borderRadiuses = prop_borderRadiuses.vector4Value;
                    EditorGUI.BeginChangeCheck();
                    borderWidths = EditorGUI.Vector4Field(position = MoveDownWithBlankSpace(position), EditorGUIUtility.TrTempContent("Border Widths"), borderWidths);
                    borderRadiuses = EditorGUI.Vector4Field(MoveDown(position, 
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, EditorGUIUtility.TrTempContent("Border Widths")) + BlankSpace),
                        EditorGUIUtility.TrTempContent("Border Radiuses"), borderRadiuses);

                    if (EditorGUI.EndChangeCheck()) {
                        prop_borderWidths.vector4Value = borderWidths;
                        prop_borderRadiuses.vector4Value = borderRadiuses;
                    }

                    //_ = EditorGUI.PropertyField(position = MoveDownWithBlankSpace(position), prop_borderWidths, EditorGUIUtility.TrTempContent("border Widths"));
                    //_ = EditorGUI.PropertyField(MoveDown(position, EditorGUI.GetPropertyHeight(prop_borderWidths)), prop_borderRadiuses, EditorGUIUtility.TrTempContent("Border Radiuses"));
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
                    return SingleRowHeightWithBlankSpace * 8f +
                        EditorGUI.GetPropertyHeight(prop_myRect) +
                        EditorGUI.GetPropertyHeight(prop_myColor) +
                        EditorGUI.GetPropertyHeight(prop_myConfg) +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, EditorGUIUtility.TrTempContent("Border Widths")) +
                        EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, EditorGUIUtility.TrTempContent("Border Radiuses")) +
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
