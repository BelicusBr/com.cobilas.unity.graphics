using System;
using System.Text;
using Cobilas.Collections;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUContent {
        [SerializeField] private string text;
        [SerializeField] private Texture image;
        [SerializeField] private bool richText;
        [SerializeField] private string tooltip;
        [SerializeField] private MarkedText[] markeds;

#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
        [HideInInspector] public bool hide_Tooltip;
#endif

        public Texture Image { get => image; set => image = value; }
        public string Tooltip { get => tooltip; set => tooltip = value; }
        public bool RichText { get => richText; set => richText = value; }
        public string Text { 
            get {
                if (!richText) return text;
                StringBuilder builder = new StringBuilder(text);
                for (int I = 0; I < ArrayManipulation.ArrayLength(markeds); I++)
                    builder.Replace(markeds[I].Text, markeds[I].ToString());
                return builder.ToString();
            }
            set => text = value;
        }

        public IGUContent() : this((string)null, (string)null, (Texture)null) { }

        public IGUContent(string text, string tooltip, Texture image) {
            this.text = text;
            this.image = image;
            this.tooltip = tooltip;
#if UNITY_EDITOR
            hide_Tooltip = true;
#endif
        }

        public IGUContent(IGUContent src) : this(src.text, src.tooltip, src.image) { }
        public IGUContent(GUIContent src) : this(src.text, src.tooltip, src.image) { }

        public IGUContent(string text) : this(text, (string)null, (Texture)null) { }
        public IGUContent(Texture image) : this((string)null, (string)null, image) { }
        public IGUContent(string text, Texture image) : this(text, (string)null, image) { }
        public IGUContent(Texture image, string tooltip) : this((string)null, tooltip, image) { }
        public IGUContent(string text, string tooltip) : this(text, tooltip, (Texture)null) { }

        public void SetMarkedText(params MarkedText[] markeds) => this.markeds = markeds;

        public static explicit operator GUIContent(IGUContent A) => new GUIContent(A.text, A.image, A.tooltip);
        public static explicit operator IGUContent(GUIContent A) => new IGUContent(A);
    }
}
