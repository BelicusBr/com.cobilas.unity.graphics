using UnityEditor;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomEditor(typeof(IGUContainer))]
    public class IGUContainerDraw : UEEditor {

        SerializedProperty prop_listDeep;

        private void OnEnable() {
            prop_listDeep = serializedObject.FindProperty("deepActions");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.LabelField("Elements", EditorStyles.boldLabel);
            for (int I = 0; I < prop_listDeep.arraySize; I++) {
                SerializedProperty prop_listDeep_Item = prop_listDeep.GetArrayElementAtIndex(I);
                SerializedProperty prop_listDeep_Item_depth = prop_listDeep_Item.FindPropertyRelative("depth");
                SerializedProperty prop_listDeep_Item_foldout = prop_listDeep_Item.FindPropertyRelative("foldout");
                SerializedProperty prop_listDeep_Item_objects = prop_listDeep_Item.FindPropertyRelative("objects");

                int depth = prop_listDeep_Item_depth.intValue;
                bool foldout = prop_listDeep_Item_foldout.boolValue;
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                if (foldout = EditorGUILayout.Foldout(foldout, IGUTextObject.GetGUIContentTemp($"depth item[index:{I} depth:{depth}]"))) {
                    for (int J = 0; J < prop_listDeep_Item_objects.arraySize; J++) {
                        SerializedProperty prop_objects_Item = prop_listDeep_Item_objects.GetArrayElementAtIndex(J);
                        IGUObject temp = prop_objects_Item.objectReferenceValue as IGUObject;
                        EditorGUILayout.LabelField(IGUTextObject.GetGUIContentTemp($"(ID:{temp.GetInstanceID()})-{temp.name}"));
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
                prop_listDeep_Item_foldout.boolValue = foldout;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
