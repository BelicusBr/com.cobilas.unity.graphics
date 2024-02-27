using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextObject : IGUObject, IIGUToolTip {
        [SerializeField] protected bool richText;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected IGUContent content;
        [SerializeField] protected IGUStyle tooltipStyle;

        private static readonly GUIContent GUIContentTemp = new GUIContent();
        private static readonly IGUContent IGUContentTemp = new IGUContent();

        public IGUContent MyContent { get => content; set => content = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public string Text { get => content.Text; set => content.Text = value; }
        public Texture Image { get => content.Image; set => content.Image = value; }
        public string ToolTip { get => content.Tooltip; set => content.Tooltip = value; }
        public bool RichText { get => richText; set => richText = content.RichText = value; }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value ?? (IGUStyle)"Black box border"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            useTooltip =
                richText = false;
            content = new IGUContent();
            tooltipStyle = (IGUStyle)"Black box border";
        }

        protected override void IGUStart() => base.IGUStart();
        protected override void PreOnIGU() => base.PreOnIGU();
        protected override void PostOnIGU() => base.PostOnIGU();
        protected override void IGUOnEnable() => base.IGUOnEnable();
        protected override void IGUOnDisable() => base.IGUOnDisable();
        protected override void LowCallOnIGU() => base.LowCallOnIGU();
        protected override void IGUOnDestroy() => base.IGUOnDestroy();

        public void SetMarkedText(params MarkedText[] markeds)
            => content.SetMarkedText(markeds);

        void IIGUToolTip.InternalDrawToolTip() => DrawTooltip();

        protected virtual void DrawTooltip() {
            if (LocalRect.Contains(IGUDrawer.MousePosition) && useTooltip)
                IGUDrawer.DrawTooltip(ToolTip, tooltipStyle);
        }

        public static GUIContent GetGUIContentTemp(string text, string tooltip, Texture image) {
            GUIContentTemp.text = text;
            GUIContentTemp.tooltip = tooltip;
            GUIContentTemp.image = image;
            return GUIContentTemp;
        }

        public static GUIContent GetGUIContentTemp(string text, Texture image)
            => GetGUIContentTemp(text, string.Empty, image);

        public static GUIContent GetGUIContentTemp(string text, string tooltip)
            => GetGUIContentTemp(text, tooltip, (Texture)null);

        public static GUIContent GetGUIContentTemp(Texture image, string tooltip)
            => GetGUIContentTemp(string.Empty, tooltip, image);

        public static GUIContent GetGUIContentTemp(Texture image)
            => GetGUIContentTemp(string.Empty, string.Empty, image);

        public static GUIContent GetGUIContentTemp(string text)
            => GetGUIContentTemp(text, string.Empty, (Texture)null);

        public static GUIContent GetGUIContentTemp(IGUContent content)
            => GetGUIContentTemp(content.Text, content.Tooltip, content.Image);

        public static IGUContent GetIGUContentTemp(string text, Texture image, string tooltip) {
            IGUContentTemp.Text = text;
            IGUContentTemp.Tooltip = tooltip;
            IGUContentTemp.Image = image;
            return IGUContentTemp;
        }

        public static IGUContent GetIGUContentTemp(string text, string tooltip)
            => GetIGUContentTemp(text, (Texture)null, tooltip);

        public static IGUContent GetIGUContentTemp(string text, Texture image)
            => GetIGUContentTemp(text, image, string.Empty);

        public static IGUContent GetIGUContentTemp(Texture image, string tooltip)
            => GetIGUContentTemp(string.Empty, image, tooltip);

        public static IGUContent GetIGUContentTemp(Texture image)
            => GetIGUContentTemp(string.Empty, image, string.Empty);

        public static IGUContent GetIGUContentTemp(string text)
            => GetIGUContentTemp(text, (Texture)null, string.Empty);
    }
}