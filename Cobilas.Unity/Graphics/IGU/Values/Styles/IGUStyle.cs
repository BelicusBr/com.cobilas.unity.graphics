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
        [SerializeField, HideInInspector]
        private GUIStyle gUIStyle;

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

        public static IGUStyle none => new IGUStyle();

        public IGUStyle() {
            gUIStyle = new GUIStyle();
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

        public IGUStyle(IGUStyle other) : this() {
            name =                 other.Name;
            fixedWidth =           other.FixedWidth;
            fixedHeight =          other.FixedHeight;
            stretchWidth =         other.StretchWidth;
            stretchHeight =        other.StretchHeight;
            font =                 other.Font;
            fontSize =             other.FontSize;
            fontStyle =            other.FontStyle;
            wordWrap =             other.WordWrap;
            richText =             other.RichText;
            alignment =            other.Alignment;
            imagePosition =        other.ImagePosition;
            clipping =             other.Clipping;
            contentOffset =        other.ContentOffset;
            normal.Background =    other.normal.Background;
            normal.TextColor =     other.normal.TextColor;
            hover.Background =     other.hover.Background;
            hover.TextColor =      other.hover.TextColor;
            active.Background =    other.active.Background;
            active.TextColor =     other.active.TextColor;
            focused.Background =   other.focused.Background;
            focused.TextColor =    other.focused.TextColor;
            onNormal.Background =  other.onNormal.Background;
            onNormal.TextColor =   other.onNormal.TextColor;
            onHover.Background =   other.onHover.Background;
            onHover.TextColor =    other.onHover.TextColor;
            onActive.Background =  other.onActive.Background;
            onActive.TextColor =   other.onActive.TextColor;
            onFocused.Background = other.onFocused.Background;
            onFocused.TextColor =  other.onFocused.TextColor;
#if UNITY_EDITOR
            normal.ScaledBackgrounds =    other.normal.ScaledBackgrounds;
            hover.ScaledBackgrounds =     other.hover.ScaledBackgrounds;
            active.ScaledBackgrounds =    other.active.ScaledBackgrounds;
            focused.ScaledBackgrounds =   other.focused.ScaledBackgrounds;
            onNormal.ScaledBackgrounds =  other.onNormal.ScaledBackgrounds;
            onHover.ScaledBackgrounds =   other.onHover.ScaledBackgrounds;
            onActive.ScaledBackgrounds =  other.onActive.ScaledBackgrounds;
            onFocused.ScaledBackgrounds = other.onFocused.ScaledBackgrounds;
#endif
            border.Left =     other.border.Left;
            border.Right =    other.border.Right;
            border.Top =      other.border.Top;
            border.Bottom =   other.border.Bottom;
            margin.Left =     other.margin.Left;
            margin.Right =    other.margin.Right;
            margin.Top =      other.margin.Top;
            margin.Bottom =   other.margin.Bottom;
            padding.Left =    other.padding.Left;
            padding.Right =   other.padding.Right;
            padding.Top =     other.padding.Top;
            padding.Bottom =  other.padding.Bottom;
            overflow.Left =   other.overflow.Left;
            overflow.Right =  other.overflow.Right;
            overflow.Top =    other.overflow.Top;
            overflow.Bottom = other.overflow.Bottom;
        }

        public IGUStyle(string styleName) : this(IGUSkins.GetIGUStyle(styleName)) {}

        [Obsolete("Use the explicit (GUIStyle)style conversion.")]
        public static GUIStyle GetGUIStyleTemp(IGUStyle style, int index)
            => InternalGetGUIStyleTemp(style);

        [Obsolete("Use the explicit (GUIStyle)style conversion.")]
        public static GUIStyle GetGUIStyleTemp(IGUStyle style)
            => InternalGetGUIStyleTemp(style);

        private static GUIStyle InternalGetGUIStyleTemp(IGUStyle style) {
            style.gUIStyle.name = style.Name;
            style.gUIStyle.fixedWidth = style.FixedWidth;
            style.gUIStyle.fixedHeight = style.FixedHeight;
            style.gUIStyle.stretchWidth = style.StretchWidth;
            style.gUIStyle.stretchHeight = style.StretchHeight;
            style.gUIStyle.font = style.Font;
            style.gUIStyle.fontSize = style.FontSize;
            style.gUIStyle.fontStyle = style.FontStyle;
            style.gUIStyle.wordWrap = style.WordWrap;
            style.gUIStyle.richText = style.RichText;
            style.gUIStyle.alignment = style.Alignment;
            style.gUIStyle.imagePosition = style.ImagePosition;
            style.gUIStyle.clipping = style.Clipping;
            style.gUIStyle.contentOffset = style.ContentOffset;
            style.gUIStyle.normal.background = style.normal.Background;
            style.gUIStyle.normal.textColor = style.normal.TextColor;
            style.gUIStyle.hover.background = style.hover.Background;
            style.gUIStyle.hover.textColor = style.hover.TextColor;
            style.gUIStyle.active.background = style.active.Background;
            style.gUIStyle.active.textColor = style.active.TextColor;
            style.gUIStyle.focused.background = style.focused.Background;
            style.gUIStyle.focused.textColor = style.focused.TextColor;
            style.gUIStyle.onNormal.background = style.onNormal.Background;
            style.gUIStyle.onNormal.textColor = style.onNormal.TextColor;
            style.gUIStyle.onHover.background = style.onHover.Background;
            style.gUIStyle.onHover.textColor = style.onHover.TextColor;
            style.gUIStyle.onActive.background = style.onActive.Background;
            style.gUIStyle.onActive.textColor = style.onActive.TextColor;
            style.gUIStyle.onFocused.background = style.onFocused.Background;
            style.gUIStyle.onFocused.textColor = style.onFocused.TextColor;
#if UNITY_EDITOR
            style.gUIStyle.normal.scaledBackgrounds = style.normal.ScaledBackgrounds;
            style.gUIStyle.hover.scaledBackgrounds = style.hover.ScaledBackgrounds;
            style.gUIStyle.active.scaledBackgrounds = style.active.ScaledBackgrounds;
            style.gUIStyle.focused.scaledBackgrounds = style.focused.ScaledBackgrounds;
            style.gUIStyle.onNormal.scaledBackgrounds = style.onNormal.ScaledBackgrounds;
            style.gUIStyle.onHover.scaledBackgrounds = style.onHover.ScaledBackgrounds;
            style.gUIStyle.onActive.scaledBackgrounds = style.onActive.ScaledBackgrounds;
            style.gUIStyle.onFocused.scaledBackgrounds = style.onFocused.ScaledBackgrounds;
#endif
            style.gUIStyle.border.left = style.border.Left;
            style.gUIStyle.border.right = style.border.Right;
            style.gUIStyle.border.top = style.border.Top;
            style.gUIStyle.border.bottom = style.border.Bottom;
            style.gUIStyle.margin.left = style.margin.Left;
            style.gUIStyle.margin.right = style.margin.Right;
            style.gUIStyle.margin.top = style.margin.Top;
            style.gUIStyle.margin.bottom = style.margin.Bottom;
            style.gUIStyle.padding.left = style.padding.Left;
            style.gUIStyle.padding.right = style.padding.Right;
            style.gUIStyle.padding.top = style.padding.Top;
            style.gUIStyle.padding.bottom = style.padding.Bottom;
            style.gUIStyle.overflow.left = style.overflow.Left;
            style.gUIStyle.overflow.right = style.overflow.Right;
            style.gUIStyle.overflow.top = style.overflow.Top;
            style.gUIStyle.overflow.bottom = style.overflow.Bottom;
            return style.gUIStyle;
        }

        public static explicit operator GUIStyle(IGUStyle style) 
            => InternalGetGUIStyleTemp(style);
        public static explicit operator IGUStyle(string styleName) 
            => IGUSkins.GetIGUStyle(styleName);
    }
}
