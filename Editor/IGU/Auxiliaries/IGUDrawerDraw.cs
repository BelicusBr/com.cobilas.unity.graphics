using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomEditor(typeof(IGUDrawer))]
    public class IGUDrawerDraw : UEEditor {

        SerializedProperty prop_listDeep;

        private void OnEnable() {
            prop_listDeep = serializedObject.FindProperty("containers");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            SerializedProperty prop_editor_ScaleFactor = serializedObject.FindProperty("editor_ScaleFactor");
            SerializedProperty prop_editor_CurrentResolution = serializedObject.FindProperty("editor_CurrentResolution");

            EditorGUILayout.LabelField("Base resolution", IGUDrawer.BaseResolution.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Current resolution", prop_editor_CurrentResolution.vector2IntValue.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Scale factor", prop_editor_ScaleFactor.vector2Value.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Containers", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            for (int I = 0; I < prop_listDeep.arraySize; I++) {
                SerializedProperty prop_listDeep_Item = prop_listDeep.GetArrayElementAtIndex(I);
                IGUContainer temp = prop_listDeep_Item.objectReferenceValue as IGUContainer;
                EditorGUILayout.LabelField(IGUTextObject.GetGUIContentTemp($"(ID:{temp.GetInstanceID()})-{temp.name}"));
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
