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

        public IGUTextSettings(Color cursorColor, Color selectionColor, 
            float cursorFlashSpeed, bool doubleClickSelectsWord, bool tripleClickSelectsLine) {
            this.cursorColor = cursorColor;
            this.selectionColor = selectionColor;
            this.cursorFlashSpeed = cursorFlashSpeed;
            this.doubleClickSelectsWord = doubleClickSelectsWord;
            this.tripleClickSelectsLine = tripleClickSelectsLine;
        }

        public IGUTextSettings(IGUTextSettings textSettings) :
            this(textSettings.cursorColor, textSettings.selectionColor, textSettings.cursorFlashSpeed,
                textSettings.doubleClickSelectsWord, textSettings.tripleClickSelectsLine) { }

        public IGUTextSettings(GUISettings settings) :
            this(settings.cursorColor, settings.selectionColor, settings.cursorFlashSpeed,
                settings.doubleClickSelectsWord, settings.tripleClickSelectsLine) { }

        public IGUTextSettings() :
            this(Color.white, new Color32(255, 98, 0, 178), -1, true, true) { }

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
            => new IGUTextSettings(
                T.cursorColor,
                T.selectionColor,
                T.cursorFlashSpeed,
                T.doubleClickSelectsWord,
                T.tripleClickSelectsLine
            );
    }
}
