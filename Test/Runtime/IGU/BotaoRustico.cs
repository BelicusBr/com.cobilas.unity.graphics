using UnityEngine;
using System.Collections;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public class BotaoRustico : MonoBehaviour
    {

        public Rect position = new Rect(Vector2.up * 37.5f, Vector2.right * 130f + Vector2.up * 25f);
        public GUIContent content = new GUIContent();
        public bool isHover,
            isActive, 
            on, 
            hasKeyboardFocus, 
            _enabled = true;

        // Use this for initialization
        void Start()
        {

        }

        private void OnGUI() {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            _ = GUI.Button(position, content, style);
            Rect pos = position;
            pos.y += position.height;
            isHover = pos.Contains(Event.current.mousePosition);
            GUI.enabled = _enabled;
            if (isHover && Event.current.type == EventType.MouseDown) {
                isActive = true;
                Event.current.Use();
            } else if (isHover && Event.current.type == EventType.MouseUp) {
                isActive = false;
            } else if (Event.current.type == EventType.Repaint) {
                style.Draw(pos, content, isHover, isActive, on, hasKeyboardFocus);
            }
        }
    }
}