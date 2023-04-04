using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private GUIStyle boxStyle;

        public GUIStyle BoxStyle { get => boxStyle; set => boxStyle = value; }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();

            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            boxStyle = GetDefaultValue(boxStyle, GUI.skin.box);
            GUIContent mycontent = GetGUIContent(DefaultIGUBox);

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            GUI.Box(rectTemp, mycontent, boxStyle);

            if (useTooltip)
                if (rectTemp.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void SetDefaultValue(IGUDefaultValue value) {
            if (value == null) value = IGUBoxDefault.DefaultValue;
            else if (value.GetType() == typeof(IGUBoxDefault))
                throw new IGUException();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            name = value.GetValue<string>(0L);
            useTooltip = value.GetValue<bool>(1L);
            container = value.GetValue<IGUContainer>(2L);
            boxStyle = value.GetValue<GUIStyle>(3L);
        }

        public static IGUBox CreateIGUInstance(string name, IGUContent content) {
            IGUBox box = Internal_CreateIGUInstance<IGUBox>(name);
            box.content = content;
            box.myConfg = IGUConfig.Default;
            box.myRect = IGURect.DefaultBox;
            box.myColor = IGUColor.DefaultBoxColor;
            return box;
        }

        public static IGUBox CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGUBox CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultIGUBox);
    }
}
