using System;
using System.Text;
using UnityEngine;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUContent : IEquatable<IGUContent>, IEquatable<Texture> {
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

        public readonly static IGUContent none = new IGUContent();

        public IGUContent() : this(string.Empty, string.Empty, (Texture)null) { }

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

        public IGUContent(string text) : this(text, string.Empty, (Texture)null) { }
        public IGUContent(Texture image) : this(string.Empty, string.Empty, image) { }
        public IGUContent(string text, Texture image) : this(text, string.Empty, image) { }
        public IGUContent(Texture image, string tooltip) : this(string.Empty, tooltip, image) { }
        public IGUContent(string text, string tooltip) : this(text, tooltip, (Texture)null) { }

        public void SetMarkedText(params MarkedText[] markeds) => this.markeds = markeds;

        public bool Equals(Texture other) => other == image;
        public bool Equals(IGUContent other)
            => other.text == text && other.tooltip == tooltip && Equals(other.image);
        public override bool Equals(object obj)
            => (obj is IGUContent other && Equals(other)) ||
                (obj is Texture img && Equals(img));
        public override int GetHashCode() => base.GetHashCode();

        public static explicit operator GUIContent(IGUContent A) => new GUIContent(A.text, A.image, A.tooltip);
        public static explicit operator IGUContent(GUIContent A) => new IGUContent(A);

        public static bool operator ==(IGUContent A, IGUContent B) {
            if (A is null && B is null) return true;
            else if (A is null || B is null) return false;
            return A.Equals(B);
        }
        public static bool operator !=(IGUContent A, IGUContent B) => !(A == B);
        public static bool operator ==(IGUContent A, Texture B) {
            if (A is null && B == null) return true;
            else if (A is null || B == null) return false;
            return A.Equals(B);
        }
        public static bool operator !=(IGUContent A, Texture B) => !(A == B);
        public static bool operator ==(Texture A, IGUContent B) => B == A;
        public static bool operator !=(Texture A, IGUContent B) => B != A;
    }
}
