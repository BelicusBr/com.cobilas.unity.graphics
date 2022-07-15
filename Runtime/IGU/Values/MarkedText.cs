using System.Text;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [System.Serializable]
    public struct MarkedText {
        [SerializeField] private string text;
        [SerializeField] private Color textColor;
        [SerializeField] private bool unchangeText;
        [SerializeField] private FontStyle fontStyle;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif
        public string Text => text;
        public Color TextColor => textColor;
        public bool UnchangeText => unchangeText;
        public FontStyle MyFontStyle => fontStyle;

        private MarkedText(string text, FontStyle style, Color color, bool unchangeText) {
            this.unchangeText = unchangeText;
            this.text = text;
            this.fontStyle = style;
            this.textColor = color;
#if UNITY_EDITOR
            foldout = false;
#endif
        }

        public MarkedText(string text, FontStyle style, Color color) : 
            this(text, style, color, false) { }

        public MarkedText(string text, Color color) : this(text, FontStyle.Normal, color) { }

        public MarkedText(string text, FontStyle style) : this(text, style, Color.white) { }

        public MarkedText(string text) : 
            this(text, FontStyle.Normal, Color.white) { }

        public MarkedText(string text, bool unchangeText) :
            this(text, FontStyle.Normal, Color.white, unchangeText) { }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            if (unchangeText) builder.Append(text);
            else {
                builder.AppendFormat("<color=#{0}>", ColorUtility.ToHtmlStringRGBA(textColor));
                switch (fontStyle) {
                    case FontStyle.Bold:
                        builder.AppendFormat("<b>{0}</b>", text);
                        break;
                    case FontStyle.Italic:
                        builder.AppendFormat("<i>{0}</i>", text);
                        break;
                    case FontStyle.BoldAndItalic:
                        builder.AppendFormat("<b><i>{0}</i></b>", text);
                        break;
                }
                builder.Append("</color>");
            }
            return builder.ToString();
        }
    }
}
