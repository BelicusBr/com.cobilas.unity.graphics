using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public class IGUTextSettings {
        [SerializeField] private Color cursorColor;
        [SerializeField] private Color selectionColor;
        [SerializeField] private float cursorFlashSpeed;
        [SerializeField] private bool doubleClickSelectsWord;
        [SerializeField] private bool tripleClickSelectsLine;

#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public IGUTextSettings() {
            doubleClickSelectsWord = true;
            tripleClickSelectsLine = true;
            cursorColor = Color.white;
            cursorFlashSpeed = -1;
            selectionColor = new Color32(255, 98, 0, 178);
        }

        public Color CursorColor { get => cursorColor; set => cursorColor = value; }
        public Color SelectionColor { get => selectionColor; set => selectionColor = value; }
        public float CursorFlashSpeed { get => cursorFlashSpeed; set => cursorFlashSpeed = value; }
        public bool DoubleClickSelectsWord { get => doubleClickSelectsWord; set => doubleClickSelectsWord = value; }
        public bool TripleClickSelectsLine { get => tripleClickSelectsLine; set => tripleClickSelectsLine = value; }

        public static explicit operator GUISettings(IGUTextSettings T)
            => new GUISettings() {
                doubleClickSelectsWord = T.doubleClickSelectsWord,
                tripleClickSelectsLine = T.tripleClickSelectsLine,
                cursorColor = T.cursorColor,
                cursorFlashSpeed = T.cursorFlashSpeed,
                selectionColor = T.selectionColor
            };

        public static explicit operator IGUTextSettings(GUISettings T)
            => new IGUTextSettings() {
                doubleClickSelectsWord = T.doubleClickSelectsWord,
                tripleClickSelectsLine = T.tripleClickSelectsLine,
                cursorColor = T.cursorColor,
                cursorFlashSpeed = T.cursorFlashSpeed,
                selectionColor = T.selectionColor
            };
    }
}
