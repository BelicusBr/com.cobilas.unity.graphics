using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [IGUCustomDrawer(typeof(IGUPictureBox))]
    public class IGUPictureBoxDraw : IGUObjectDraw {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => base.OnGUI(position, property, label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => base.GetPropertyHeight(property, label);

        protected override void DrawBackgroundProperty(Rect position, float height)
            => base.DrawBackgroundProperty(position, height);

        protected override void DrawBackgroundProperty(Rect position, GUIContent label, float height)
            => base.DrawBackgroundProperty(position, label, height);

        protected override GUIContent GetGUIContent(string text)
            => base.GetGUIContent(text);

        protected override void BuildRun() {
            base.BuildRun();
            internalOnGUI += RunTexture;
            internalOnGUI += RunAlphaBlend;
            internalOnGUI += RunImageAspect;
            internalOnGUI += RunScaleMode;
            internalOnGUI += RunBorderWidths;
            internalOnGUI += RunBorderRadiuses;
        }
        
        protected void RunTexture(SerializedObject serialized) {
            SerializedProperty texture = serialized.FindProperty("texture");

            EditorGUI.PropertyField(GetRect(), texture, GetGUIContent("Texture"));
            _ = MoveDown();
        }

        protected void RunAlphaBlend(SerializedObject serialized) {
            SerializedProperty alphaBlend = serialized.FindProperty("alphaBlend");

            EditorGUI.PropertyField(GetRect(), alphaBlend, GetGUIContent("Alpha blend"));
            _ = MoveDown();
        }

        protected void RunImageAspect(SerializedObject serialized) {
            SerializedProperty imageAspect = serialized.FindProperty("imageAspect");

            EditorGUI.PropertyField(GetRect(), imageAspect, GetGUIContent("Image aspect"));
            _ = MoveDown();
        }

        protected void RunScaleMode(SerializedObject serialized) {
            SerializedProperty scaleMode = serialized.FindProperty("scaleMode");

            EditorGUI.PropertyField(GetRect(), scaleMode, GetGUIContent("Scale mode"));
            _ = MoveDown();
        }

        protected void RunBorderWidths(SerializedObject serialized) {
            SerializedProperty borderWidths = serialized.FindProperty("borderWidths");
            Vector4 vec4 = borderWidths.vector4Value;
            Rect rect = new Rect(vec4.x, vec4.y, vec4.z, vec4.w);

            EditorGUI.BeginChangeCheck();
            rect = EditorGUI.RectField(GetRect(), GetGUIContent("Border widths"), rect);
            vec4.x = rect.x;
            vec4.y = rect.y;
            vec4.z = rect.width;
            vec4.w = rect.height;
            if (EditorGUI.EndChangeCheck())
                borderWidths.vector4Value = vec4;

            _ = MoveDown(EditorGUI.GetPropertyHeight(SerializedPropertyType.Rect, GetGUIContent("Border widths")) + BlankSpace);
        }

        protected void RunBorderRadiuses(SerializedObject serialized) {
            SerializedProperty borderRadiuses = serialized.FindProperty("borderRadiuses");
            Vector4 vec4 = borderRadiuses.vector4Value;
            Rect rect = new Rect(vec4.x, vec4.y, vec4.z, vec4.w);

            EditorGUI.BeginChangeCheck();
            rect = EditorGUI.RectField(GetRect(), GetGUIContent("Border radiuses"), rect);
            vec4.x = rect.x;
            vec4.y = rect.y;
            vec4.z = rect.width;
            vec4.w = rect.height;
            if (EditorGUI.EndChangeCheck())
                borderRadiuses.vector4Value = vec4;

            _ = MoveDown(EditorGUI.GetPropertyHeight(SerializedPropertyType.Rect, GetGUIContent("Border radiuses")) + BlankSpace);
        }
    }
}
