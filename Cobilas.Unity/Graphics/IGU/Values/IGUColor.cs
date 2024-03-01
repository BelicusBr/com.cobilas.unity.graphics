using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct IGUColor : IEquatable<IGUColor> {
        [SerializeField] private Color color;
        [SerializeField] private Color textColor;
        [SerializeField] private Color backgroundColor;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public Color MyColor => color;
        public Color TextColor => textColor;
        public Color BackgroundColor => backgroundColor;
        public static IGUColor DefaultLabelColor => new IGUColor(Color.white, Color.white, Color.clear);
        public static IGUColor DefaultBoxColor => new IGUColor(Color.white, Color.white, Color.white);

        public IGUColor(Color color, Color textColor, Color backgroundColor) {
            this.color = color;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
#if UNITY_EDITOR
            foldout = false;
#endif
        }

#if UNITY_EDITOR
        public IGUColor SetFolDout(bool foldout) {
            this.foldout = foldout;
            return this;
        }
#endif

        public override int GetHashCode()
            => base.GetHashCode() >> color.GetHashCode() ^
            textColor.GetHashCode() << backgroundColor.GetHashCode();

        public override bool Equals(object obj)
            => obj is IGUColor _color && Equals(_color);

        public bool Equals(IGUColor other)
            => other.color == color && other.textColor == textColor &&
            other.backgroundColor == backgroundColor;

        public override string ToString()
            => $"{{(c:{color} txtc:{textColor} bgdc:{backgroundColor})}}";

        public IGUColor SetColor(Color color) {
            this.color = color;
            return this;
        }

        public IGUColor SetTextColor(Color color) {
            this.textColor = color;
            return this;
        }

        public IGUColor SetBackgroundColor(Color color) {
            this.backgroundColor = color;
            return this;
        }

        public static bool operator ==(IGUColor A, IGUColor B) => A.Equals(B);
        public static bool operator !=(IGUColor A, IGUColor B) => !(A == B);
    }
}
