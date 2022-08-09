using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.Resolutions;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Graphics.Resolutions {
    [CustomEditor(typeof(CobilasResolutions))]
    public class CobilasResolutionsInspector : UEEditor {

        private SerializedProperty p_resolutions;
        private SerializedProperty p_aspectRatios;
        private SerializedProperty p_frequencys;

        private void OnEnable() {
            p_resolutions = serializedObject.FindProperty("resolutions");
            p_aspectRatios = serializedObject.FindProperty("aspectRatios");
            p_frequencys = serializedObject.FindProperty("frequencys");
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Resolutions", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int I = 0; I < p_resolutions.arraySize; I++) {
                int w = p_resolutions.GetArrayElementAtIndex(I).FindPropertyRelative("width").intValue;
                int h = p_resolutions.GetArrayElementAtIndex(I).FindPropertyRelative("height").intValue;
                EditorGUILayout.LabelField($"Resolution[{w}x{h}]");
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Aspect ratios", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int I = 0; I < p_aspectRatios.arraySize; I++) {
                int w = p_aspectRatios.GetArrayElementAtIndex(I).FindPropertyRelative("width").intValue;
                int h = p_aspectRatios.GetArrayElementAtIndex(I).FindPropertyRelative("height").intValue;
                EditorGUILayout.LabelField($"Aspect ratios[{w}:{h}]");
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Frequencys", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int I = 0; I < p_frequencys.arraySize; I++) {
                int w = p_frequencys.GetArrayElementAtIndex(I).intValue;
                EditorGUILayout.LabelField($"@{w}");
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}