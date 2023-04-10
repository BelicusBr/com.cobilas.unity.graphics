using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [CustomEditor(typeof(SO_IGUTextSettings))]
    public class SO_IGUTextSettingsEditor : UnityEditor.Editor {
        /*settings
private Color cursorColor; 
private Color selectionColor;
private float cursorFlashSpeed;
private bool doubleClickSelectsWord;
private bool tripleClickSelectsLine;
         */
        private SerializedProperty prop_settingsName;
        private SerializedProperty prop_cursorColor;
        private SerializedProperty prop_selectionColor;
        private SerializedProperty prop_cursorFlashSpeed;
        private SerializedProperty prop_doubleClickSelectsWord;
        private SerializedProperty prop_tripleClickSelectsLine;

        private void OnEnable() {
            SerializedProperty prop_settings = serializedObject.FindProperty("settings");
            prop_settingsName = serializedObject.FindProperty("settingsName");
            prop_cursorColor = prop_settings.FindPropertyRelative("cursorColor");
            prop_selectionColor = prop_settings.FindPropertyRelative("selectionColor");
            prop_cursorFlashSpeed = prop_settings.FindPropertyRelative("cursorFlashSpeed");
            prop_doubleClickSelectsWord = prop_settings.FindPropertyRelative("doubleClickSelectsWord");
            prop_tripleClickSelectsLine = prop_settings.FindPropertyRelative("tripleClickSelectsLine");
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent("Name"), EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            string txt = prop_settingsName.stringValue;
            EditorGUI.BeginChangeCheck();
            txt = EditorGUILayout.TextField(txt);
            if (EditorGUI.EndChangeCheck())
                prop_settingsName.stringValue = txt;
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent("Settings"), EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            EditorGUILayout.PropertyField(prop_doubleClickSelectsWord, EditorGUIUtility.TrTempContent("Double Click Selects Word"));
            EditorGUILayout.PropertyField(prop_tripleClickSelectsLine, EditorGUIUtility.TrTempContent("Triple Click Selects Line"));
            EditorGUILayout.PropertyField(prop_cursorColor, EditorGUIUtility.TrTempContent("Cursor Color"));
            EditorGUILayout.PropertyField(prop_cursorFlashSpeed, EditorGUIUtility.TrTempContent("Cursor Flash Speed"));
            EditorGUILayout.PropertyField(prop_selectionColor, EditorGUIUtility.TrTempContent("Selection Color"));
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }
    }
}