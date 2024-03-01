using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomEditor(typeof(IGUCanvasContainer))]
    public class IGUCanvasContainerDrawer : UEEditor {
        private SerializedProperty p_VolatileContainer;
        private SerializedProperty p_PermanentContainer;

        private void OnEnable() {
            p_VolatileContainer = serializedObject.FindProperty("VolatileContainer");
            p_PermanentContainer = serializedObject.FindProperty("PermanentContainer");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowList("Volatile Container", p_VolatileContainer);
            ShowList("Permanent Container", p_PermanentContainer);
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
                SerializedProperty p_loadWhenSceneActivates = property.FindPropertyRelative("loadWhenSceneActivates");

                EditorGUI.BeginChangeCheck();
                bool foldout = EditorGUILayout.Foldout(p_foldout.boolValue, EditorGUIUtility.TrTextContent(p_name.stringValue));
                if (EditorGUI.EndChangeCheck())
                    p_foldout.boolValue = foldout;
                if (foldout) {
                    EditorGUILayout.LabelField(EditorGUIUtility.TrTextContent($"GUID:{p_guid.stringValue}"), EditorStyles.helpBox);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ToggleLeft(EditorGUIUtility.TrTextContent("Load When Scene Activates"),
                        p_loadWhenSceneActivates.boolValue);
                    EditorGUI.EndDisabledGroup();
                    
                    ++EditorGUI.indentLevel;
                    SerializedProperty p_deeps = property.FindPropertyRelative("deeps");
                    for (int J = 0; J < p_deeps.arraySize; J++) {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        property = p_deeps.GetArrayElementAtIndex(J);
                        SerializedProperty p_foldout_deeps = property.FindPropertyRelative("foldout");
                        SerializedProperty p_depth = property.FindPropertyRelative("depth");
                        EditorGUI.BeginChangeCheck();
                        bool foldout_deeps = EditorGUILayout.Foldout(p_foldout_deeps.boolValue, EditorGUIUtility.TrTextContent($"Depth[{p_depth.intValue}]"));
                        if (EditorGUI.EndChangeCheck())
                            p_foldout_deeps.boolValue = foldout_deeps;
                        if (foldout_deeps) {
                            SerializedProperty p_objects = property.FindPropertyRelative("objects");
                            for (int L = 0; L < p_objects.arraySize; L++) {
                                property = p_objects.GetArrayElementAtIndex(L);
                                string obj_name = property.objectReferenceValue.name;
                                int id = property.objectReferenceInstanceIDValue;
                                EditorGUILayout.LabelField(EditorGUIUtility.TrTextContent($"[ID:{id}]{obj_name}"), EditorStyles.boldLabel);
                            }
                        }
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

