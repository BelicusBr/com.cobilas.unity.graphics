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

        private static readonly GUIStyle styletemp = new GUIStyle();
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

        public static GUIStyle GetGUIStyleTemp(IGUStyle style) {
            styletemp.name = style.Name;
            styletemp.fixedWidth = style.FixedWidth;
            styletemp.fixedHeight = style.FixedHeight;
            styletemp.stretchWidth = style.StretchWidth;
            styletemp.stretchHeight = style.StretchHeight;
            styletemp.font = style.Font;
            styletemp.fontSize = style.FontSize;
            styletemp.fontStyle = style.FontStyle;
            styletemp.wordWrap = style.WordWrap;
            styletemp.richText = style.RichText;
            styletemp.alignment = style.Alignment;
            styletemp.imagePosition = style.ImagePosition;
            styletemp.clipping = style.Clipping;
            styletemp.contentOffset = style.ContentOffset;
            
            styletemp.normal.background = style.normal.Background;
            styletemp.normal.textColor = style.normal.TextColor;
            styletemp.normal.scaledBackgrounds = style.normal.ScaledBackgrounds;
            styletemp.hover.background = style.hover.Background;
            styletemp.hover.textColor = style.hover.TextColor;
            styletemp.hover.scaledBackgrounds = style.hover.ScaledBackgrounds;
            styletemp.active.background = style.active.Background;
            styletemp.active.textColor = style.active.TextColor;
            styletemp.active.scaledBackgrounds = style.active.ScaledBackgrounds;
            styletemp.focused.background = style.focused.Background;
            styletemp.focused.textColor = style.focused.TextColor;
            styletemp.focused.scaledBackgrounds = style.focused.ScaledBackgrounds;

            styletemp.onNormal.background = style.onNormal.Background;
            styletemp.onNormal.textColor = style.onNormal.TextColor;
            styletemp.onNormal.scaledBackgrounds = style.onNormal.ScaledBackgrounds;
            styletemp.onHover.background = style.onHover.Background;
            styletemp.onHover.textColor = style.onHover.TextColor;
            styletemp.onHover.scaledBackgrounds = style.onHover.ScaledBackgrounds;
            styletemp.onActive.background = style.onActive.Background;
            styletemp.onActive.textColor = style.onActive.TextColor;
            styletemp.onActive.scaledBackgrounds = style.onActive.ScaledBackgrounds;
            styletemp.onFocused.background = style.onFocused.Background;
            styletemp.onFocused.textColor = style.onFocused.TextColor;
            styletemp.onFocused.scaledBackgrounds = style.onFocused.ScaledBackgrounds;

            styletemp.border.left = style.border.Left;
            styletemp.border.right = style.border.Right;
            styletemp.border.top = style.border.Top;
            styletemp.border.bottom = style.border.Bottom;

            styletemp.margin.left = style.margin.Left;
            styletemp.margin.right = style.margin.Right;
            styletemp.margin.top = style.margin.Top;
            styletemp.margin.bottom = style.margin.Bottom;

            styletemp.padding.left = style.padding.Left;
            styletemp.padding.right = style.padding.Right;
            styletemp.padding.top = style.padding.Top;
            styletemp.padding.bottom = style.padding.Bottom;

            styletemp.overflow.left = style.overflow.Left;
            styletemp.overflow.right = style.overflow.Right;
            styletemp.overflow.top = style.overflow.Top;
            styletemp.overflow.bottom = style.overflow.Bottom;
            return styletemp;
        }

        public static explicit operator GUIStyle(IGUStyle style)
            => new GUIStyle(GetGUIStyleTemp(style));
    }
}
