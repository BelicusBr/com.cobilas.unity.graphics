using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUStyleStatus {
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool foldout;
#endif
        [SerializeField]
        private Texture2D background;
        [SerializeField]
        private Color textColor;
#if UNITY_EDITOR
        [SerializeField]
        private Texture2D[] scaledBackgrounds;
#endif

        public Color TextColor { get => textColor; set => textColor = value; }
        public Texture2D Background { get => background; set => background = value; }
#if UNITY_EDITOR
        public Texture2D[] ScaledBackgrounds { get => scaledBackgrounds; set => scaledBackgrounds = value; }
#endif

        public static explicit operator GUIStyleState(IGUStyleStatus A)
            => new GUIStyleState() {
                background = A.Background,
                textColor = A.TextColor
#if UNITY_EDITOR
                ,
                scaledBackgrounds = A.ScaledBackgrounds
#endif
            };
    }
}
