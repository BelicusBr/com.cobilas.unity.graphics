using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextObject : IGUObject {
        protected GUIStyle tooltipStyle;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected IGUContent content;

        private static readonly GUIContent GUIContentTemp = new GUIContent();

        public IGUContent MyContent { get => content; set => content = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public string Text { get => content.Text; set => content.Text = value; }
        public Texture Image { get => content.Image; set => content.Image = value; }
        public string ToolTip { get => content.Tooltip; set => content.Tooltip = value; }
        public GUIStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value; }

        protected override void Awake() {
            base.Awake();
            useTooltip = false;
            content = new IGUContent();
        }

        public override void OnIGU() => base.OnIGU();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnIGUDestroy() => base.OnIGUDestroy();

        protected virtual GUIContent GetGUIContent(string defaultGUIContent) {
            if (content == null) content = new IGUContent(defaultGUIContent);
            return GetGUIContentTemp(content.Text, content.Tooltip, content.Image);
        }

        protected virtual void DrawTooltip() {
            IGUDrawer.Drawer.SetTootipText(ToolTip);
            IGUDrawer.Drawer.GUIStyleTootip(tooltipStyle);
            IGUDrawer.Drawer.SetTootipPosition(Event.current.mousePosition);
            IGUDrawer.Drawer.OpenTooltip();
        }

        public static GUIContent GetGUIContentTemp(string text, string tooltip, Texture image) {
            GUIContentTemp.text = text;
            GUIContentTemp.tooltip = tooltip;
            GUIContentTemp.image = image;
            return GUIContentTemp;
        }

        public static GUIContent GetGUIContentTemp(string text, Texture image)
            => GetGUIContentTemp(text, "", image);

        public static GUIContent GetGUIContentTemp(string text, string tooltip)
            => GetGUIContentTemp(text, tooltip, (Texture)null);

        public static GUIContent GetGUIContentTemp(string text)
            => GetGUIContentTemp(text, "", (Texture)null);
    }
}
