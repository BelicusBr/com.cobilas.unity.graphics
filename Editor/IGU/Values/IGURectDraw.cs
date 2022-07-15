using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Editor.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomPropertyDrawer(typeof(IGURect))]
    [IGUCustomFieldDrawer("#IGURect")]
    public class IGURectDraw : CPropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //foldout
            position.height = SingleLineHeight;
            SerializedProperty prop_foldout = property.FindPropertyRelative("foldout");
            SerializedProperty prop_PosX = property.FindPropertyRelative("x");
            SerializedProperty prop_PosY = property.FindPropertyRelative("y");
            SerializedProperty prop_SizeX = property.FindPropertyRelative("width");
            SerializedProperty prop_SizeY = property.FindPropertyRelative("height");
            SerializedProperty prop_PivotX = property.FindPropertyRelative("pivotX");
            SerializedProperty prop_PivotY = property.FindPropertyRelative("pivotY");
            SerializedProperty prop_SFX = property.FindPropertyRelative("scaleFactorWidth");
            SerializedProperty prop_SFY = property.FindPropertyRelative("scaleFactorHeight");
            SerializedProperty prop_Rotation = property.FindPropertyRelative("rotation");

            bool foldout = prop_foldout.boolValue;
            float rotation = prop_Rotation.floatValue;
            Vector2 sf = new Vector2(prop_SFX.floatValue, prop_SFY.floatValue);
            Vector2 pos = new Vector2(prop_PosX.floatValue, prop_PosY.floatValue);
            Vector2 size = new Vector2(prop_SizeX.floatValue, prop_SizeY.floatValue);
            Vector2 pivot = new Vector2(prop_PivotX.floatValue, prop_PivotY.floatValue);
            if (foldout = EditorGUI.Foldout(position, foldout, label)) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                position = MoveDown(position);
                pos = DrawVector2Field(ref position, IGUTextObject.GetGUIContentTemp("position"), pos);
                size = DrawVector2Field(ref position, IGUTextObject.GetGUIContentTemp("size"), size);
                pivot = DrawVector2Field(ref position, IGUTextObject.GetGUIContentTemp("pivot"), pivot);
                sf = DrawVector2Field(ref position, IGUTextObject.GetGUIContentTemp("scale factor"), sf);
                rotation = EditorGUI.FloatField(position, IGUTextObject.GetGUIContentTemp("rotation"), rotation);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck()) {
                    prop_PosX.floatValue = pos.x;
                    prop_PosY.floatValue = pos.y;
                    prop_SizeX.floatValue = size.x;
                    prop_SizeY.floatValue = size.y;
                    prop_PivotX.floatValue = pivot.x;
                    prop_PivotY.floatValue = pivot.y;
                    prop_SFX.floatValue = sf.x;
                    prop_SFY.floatValue = sf.y;
                    prop_Rotation.floatValue = rotation;
                }
            }
            prop_foldout.boolValue = foldout;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (!property.FindPropertyRelative("foldout").boolValue) return SingleLineHeight;
            return GetVector2PropertyHeight(IGUTextObject.GetGUIContentTemp("position")) +
                GetVector2PropertyHeight(IGUTextObject.GetGUIContentTemp("size")) +
                GetVector2PropertyHeight(IGUTextObject.GetGUIContentTemp("pivot")) +
                GetVector2PropertyHeight(IGUTextObject.GetGUIContentTemp("scale factor")) +
                EditorGUI.GetPropertyHeight(SerializedPropertyType.Float, IGUTextObject.GetGUIContentTemp("rotation")) +
                SingleLineHeight + BlankSpace;
        }

        private float GetVector2PropertyHeight(GUIContent label)
            => EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, label) + BlankSpace;

        private Vector2 DrawVector2Field(ref Rect position, GUIContent label, Vector2 vector) {
            vector = EditorGUI.Vector2Field(position, label, vector);
            position = MoveDown(position, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, label) + BlankSpace);
            return vector;
        }
    }
}
