using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUTextObject : IGUObject {
        [SerializeField] protected bool useTooltip;
        [SerializeField] protected IGUContent content;
        [SerializeField] protected GUIStyle tooltipStyle;

        private static readonly GUIContent GUIContentTemp = new GUIContent();

        public IGUContent MyContent { get => content; set => content = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public string Text { get => content.Text; set => content.Text = value; }
        public Texture Image { get => content.Image; set => content.Image = value; }
        public string ToolTip { get => content.Tooltip; set => content.Tooltip = value; }
        public GUIStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value; }

        public override void OnIGU() => base.OnIGU();
        protected override void Awake() => base.Awake();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnDestroy() => base.OnDestroy();

        protected virtual GUIContent GetGUIContent(string defaultGUIContent) {
            if (content == null) content = new IGUContent(defaultGUIContent);
            return GetGUIContentTemp(content.Text, content.Tooltip, content.Image);
        }

        protected virtual void DrawTooltip() {
            tooltipStyle = GetDefaultValue(tooltipStyle, GUI.skin.box);
            Rect rectTemp = new Rect(Event.current.mousePosition, tooltipStyle.CalcSize(GetGUIContentTemp(ToolTip)));
            GUI.enabled = true;
            Matrix4x4 oldmatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(0, rectTemp.position);
            GUI.Box(rectTemp, GetGUIContentTemp(ToolTip), tooltipStyle);
            GUI.matrix = oldmatrix;
            GUI.enabled = myConfg.IsEnabled;
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

        protected static T Internal_CreateIGUInstance<T>(string name, bool useTooltip, IGUContent content) where T : IGUTextObject {
            T textObject = IGUObject.Internal_CreateIGUInstance<T>(name);
            textObject.useTooltip = useTooltip;
            textObject.content = content;
            return textObject;
        }

        protected static T Internal_CreateIGUInstance<T>(string name, IGUContent content) where T : IGUTextObject
            => Internal_CreateIGUInstance<T>(name, false, content);
    }
}
