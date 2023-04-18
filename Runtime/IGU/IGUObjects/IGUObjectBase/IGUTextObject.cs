using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextObject : IGUObject {
        [SerializeField] protected IGUStyle tooltipStyle;
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected IGUContent content;

        private static readonly GUIContent GUIContentTemp = new GUIContent();

        public IGUContent MyContent { get => content; set => content = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public string Text { get => content.Text; set => content.Text = value; }
        public Texture Image { get => content.Image; set => content.Image = value; }
        public string ToolTip { get => content.Tooltip; set => content.Tooltip = value; }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value; }

        protected override void Awake() {
            base.Awake();
            useTooltip = false;
            content = new IGUContent();
            tooltipStyle = IGUSkins.GetIGUStyle("Black box border");
        }

        protected override void LowCallOnIGU() => base.OnIGU();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnIGUDestroy() => base.OnIGUDestroy();

        protected virtual GUIContent GetGUIContent(string defaultGUIContent) {
            if (content == null) content = new IGUContent(defaultGUIContent);
            return GetGUIContentTemp(content.Text, content.Tooltip, content.Image);
        }

        protected virtual void DrawTooltip() {
            IGUDrawer.Drawer.SetTootipText(ToolTip);
            IGUDrawer.Drawer.GUIStyleTootip(IGUStyle.GetGUIStyleTemp(tooltipStyle));
            IGUDrawer.Drawer.OpenTooltip();
        }

        public static GUIContent GetGUIContentTemp(IGUContent content)
            => GetGUIContentTemp(content.Text, content.Tooltip, content.Image);

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

/* vs3.1.1
 * ##fix
 * No IGUObjetc dentro do método internalOnIGU a verificação da prop IGUObject.Pivot tava errada
 * so IGUObject.PivotX era setado corretamente
 * ##fix
 * Com a substitução o GUIStyle por IGUStyle um erro inesperado que ocorria quando a unity fazia o processo de
 * deserialização e serialização que fazia que o campo `GUIStyle tooltipStyle` se tornase `GUIStyle.none` foi corrigido.
 * ##add
 * o metodo [protected]void:IGUObjetc.LowCallOnIGU(); foi add.
 * a calsse IGURectClip foi add
 * ##remove
 * 
 */