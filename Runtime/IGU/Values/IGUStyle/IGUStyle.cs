using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUStyle {
        [SerializeField] private string name;
        [SerializeField] private float fixedWidth;
        [SerializeField] private float fixedHeight;
        [SerializeField] private bool stretchWidth;
        [SerializeField] private bool stretchHeight;
        [SerializeField] private Font font;
        [SerializeField] private int fontSize;
        [SerializeField] private FontStyle fontStyle;
        [SerializeField] private bool wordWrap;
        [SerializeField] private bool richText;
        [SerializeField] private TextAnchor alignment;
        [SerializeField] private ImagePosition imagePosition;
        [SerializeField] private TextClipping clipping;
        [SerializeField] private Vector2 contentOffset;
        [SerializeField] private IGUStyleStatus normal;
        [SerializeField] private IGUStyleStatus hover;
        [SerializeField] private IGUStyleStatus active;
        [SerializeField] private IGUStyleStatus focused;
        [SerializeField] private IGUStyleStatus onNormal;
        [SerializeField] private IGUStyleStatus onHover;
        [SerializeField] private IGUStyleStatus onActive;
        [SerializeField] private IGUStyleStatus onFocused;
        [SerializeField] private IGUStyleRectOffSet border;
        [SerializeField] private IGUStyleRectOffSet margin;
        [SerializeField] private IGUStyleRectOffSet padding;
        [SerializeField] private IGUStyleRectOffSet overflow;

        public string Name { get => name; set => name = value; }
        public float FixedWidth { get => fixedWidth; set => fixedWidth = value; }
        public float FixedHeight { get => fixedHeight; set => fixedHeight = value; }
        public bool StretchWidth { get => stretchWidth; set => stretchWidth = value; }
        public bool StretchHeight { get => stretchHeight; set => stretchHeight = value; }
        public Font Font { get => font; set => font = value; }
        public int FontSize { get => fontSize; set => fontSize = value; }
        public FontStyle FontStyle { get => fontStyle; set => fontStyle = value; }
        public bool WordWrap { get => wordWrap; set => wordWrap = value; }
        public bool RichText { get => richText; set => richText = value; }
        public TextAnchor Alignment { get => alignment; set => alignment = value; }
        public ImagePosition ImagePosition { get => imagePosition; set => imagePosition = value; }
        public TextClipping Clipping { get => clipping; set => clipping = value; }
        public Vector2 ContentOffset { get => contentOffset; set => contentOffset = value; }
        public IGUStyleStatus Normal { get => normal; set => normal = value; }
        public IGUStyleStatus Hover { get => hover; set => hover = value; }
        public IGUStyleStatus Active { get => active; set => active = value; }
        public IGUStyleStatus Focused { get => focused; set => focused = value; }
        public IGUStyleStatus OnNormal { get => onNormal; set => onNormal = value; }
        public IGUStyleStatus OnHover { get => onHover; set => onHover = value; }
        public IGUStyleStatus OnActive { get => onActive; set => onActive = value; }
        public IGUStyleStatus OnFocused { get => onFocused; set => onFocused = value; }
        public IGUStyleRectOffSet Border { get => border; set => border = value; }
        public IGUStyleRectOffSet Margin { get => margin; set => margin = value; }
        public IGUStyleRectOffSet Padding { get => padding; set => padding = value; }
        public IGUStyleRectOffSet Overflow { get => overflow; set => overflow = value; }

        private static GUIStyle[] styletemp = new GUIStyle[1];
        public static IGUStyle none => new IGUStyle();

        public IGUStyle() {
            Normal = new IGUStyleStatus();
            Hover = new IGUStyleStatus();
            Active = new IGUStyleStatus();
            Focused = new IGUStyleStatus();
            OnNormal = new IGUStyleStatus();
            OnHover = new IGUStyleStatus();
            OnActive = new IGUStyleStatus();
            OnFocused = new IGUStyleStatus();
            Border = new IGUStyleRectOffSet();
            Margin = new IGUStyleRectOffSet();
            Padding = new IGUStyleRectOffSet();
            Overflow = new IGUStyleRectOffSet();
        }

        public static GUIStyle GetGUIStyleTemp(IGUStyle style, int index) {
            index = index < 0 ? 0 : index;
            if (index + 1 > styletemp.Length)
                Array.Resize(ref styletemp, index + 1);
            if (styletemp[index] == null)
                styletemp[index] = new GUIStyle();

            styletemp[index].name = style.Name;
            styletemp[index].fixedWidth = style.FixedWidth;
            styletemp[index].fixedHeight = style.FixedHeight;
            styletemp[index].stretchWidth = style.StretchWidth;
            styletemp[index].stretchHeight = style.StretchHeight;
            styletemp[index].font = style.Font;
            styletemp[index].fontSize = style.FontSize;
            styletemp[index].fontStyle = style.FontStyle;
            styletemp[index].wordWrap = style.WordWrap;
            styletemp[index].richText = style.RichText;
            styletemp[index].alignment = style.Alignment;
            styletemp[index].imagePosition = style.ImagePosition;
            styletemp[index].clipping = style.Clipping;
            styletemp[index].contentOffset = style.ContentOffset;
            styletemp[index].normal.background = style.normal.Background;
            styletemp[index].normal.textColor = style.normal.TextColor;
            styletemp[index].normal.scaledBackgrounds = style.normal.ScaledBackgrounds;
            styletemp[index].hover.background = style.hover.Background;
            styletemp[index].hover.textColor = style.hover.TextColor;
            styletemp[index].hover.scaledBackgrounds = style.hover.ScaledBackgrounds;
            styletemp[index].active.background = style.active.Background;
            styletemp[index].active.textColor = style.active.TextColor;
            styletemp[index].active.scaledBackgrounds = style.active.ScaledBackgrounds;
            styletemp[index].focused.background = style.focused.Background;
            styletemp[index].focused.textColor = style.focused.TextColor;
            styletemp[index].focused.scaledBackgrounds = style.focused.ScaledBackgrounds;
            styletemp[index].onNormal.background = style.onNormal.Background;
            styletemp[index].onNormal.textColor = style.onNormal.TextColor;
            styletemp[index].onNormal.scaledBackgrounds = style.onNormal.ScaledBackgrounds;
            styletemp[index].onHover.background = style.onHover.Background;
            styletemp[index].onHover.textColor = style.onHover.TextColor;
            styletemp[index].onHover.scaledBackgrounds = style.onHover.ScaledBackgrounds;
            styletemp[index].onActive.background = style.onActive.Background;
            styletemp[index].onActive.textColor = style.onActive.TextColor;
            styletemp[index].onActive.scaledBackgrounds = style.onActive.ScaledBackgrounds;
            styletemp[index].onFocused.background = style.onFocused.Background;
            styletemp[index].onFocused.textColor = style.onFocused.TextColor;
            styletemp[index].onFocused.scaledBackgrounds = style.onFocused.ScaledBackgrounds;
            styletemp[index].border.left = style.border.Left;
            styletemp[index].border.right = style.border.Right;
            styletemp[index].border.top = style.border.Top;
            styletemp[index].border.bottom = style.border.Bottom;
            styletemp[index].margin.left = style.margin.Left;
            styletemp[index].margin.right = style.margin.Right;
            styletemp[index].margin.top = style.margin.Top;
            styletemp[index].margin.bottom = style.margin.Bottom;
            styletemp[index].padding.left = style.padding.Left;
            styletemp[index].padding.right = style.padding.Right;
            styletemp[index].padding.top = style.padding.Top;
            styletemp[index].padding.bottom = style.padding.Bottom;
            styletemp[index].overflow.left = style.overflow.Left;
            styletemp[index].overflow.right = style.overflow.Right;
            styletemp[index].overflow.top = style.overflow.Top;
            styletemp[index].overflow.bottom = style.overflow.Bottom;
            return styletemp[index];
        }

        public static GUIStyle GetGUIStyleTemp(IGUStyle style)
            => GetGUIStyleTemp(style, 0);

        public static explicit operator GUIStyle(IGUStyle style)
            => new GUIStyle(GetGUIStyleTemp(style));
    }
}
