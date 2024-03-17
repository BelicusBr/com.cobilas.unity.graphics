using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomEditor(typeof(IGUCanvasContainer))]
    public class IGUCanvasContainerDrawer : UEEditor {
        private SerializedProperty p_VolatileContainer;

        private void OnEnable() {
            p_VolatileContainer = serializedObject.FindProperty("Containers");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowList("Containers", p_VolatileContainer);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowList(string title, SerializedProperty list) {
            EditorGUILayout.LabelField(EditorGUIUtility.TrTextContent(title), EditorStyles.helpBox);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            for (int I = 0; I < list.arraySize; I++) {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                ++EditorGUI.indentLevel;
                SerializedProperty property = list.GetArrayElementAtIndex(I);
                SerializedProperty p_foldout = property.FindPropertyRelative("foldout");
                SerializedProperty p_name = property.FindPropertyRelative("name");
                SerializedProperty p_guid = property.FindPropertyRelative("guid");
                SerializedProperty p_status = property.FindPropertyRelative("status");

                EditorGUI.BeginChangeCheck();
                bool foldout = EditorGUILayout.Foldout(p_foldout.boolValue, EditorGUIUtility.TrTextContent(p_name.stringValue));
                if (EditorGUI.EndChangeCheck())
                    p_foldout.boolValue = foldout;
                if (foldout) {
                    EditorGUILayout.LabelField(EditorGUIUtility.TrTextContent($"GUID:{p_guid.stringValue}"), EditorStyles.helpBox);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(p_status, EditorGUIUtility.TrTextContent("Container Status"));
                    EditorGUI.EndDisabledGroup();
                    
                    ++EditorGUI.indentLevel;
                    SerializedProperty p_deeps = property.FindPropertyRelative("elements");
                    for (int J = 0; J < p_deeps.arraySize; J++) {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        property = p_deeps.GetArrayElementAtIndex(J);
                        EditorGUILayout.LabelField($"{property.objectReferenceValue.name}[{property.objectReferenceInstanceIDValue}]");
                        EditorGUILayout.EndVertical();
                    }
                    --EditorGUI.indentLevel;
                }
                --EditorGUI.indentLevel;
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}

