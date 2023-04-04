using UnityEngine;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabel : IGUTextObject {
        public const string DefaultIGULabel = "IGU Label";
        [SerializeField] private bool autoSize;
        [SerializeField] private bool richText;
        [SerializeField] private GUIStyle labelStyle;

        public bool AutoSize { get => autoSize; set => autoSize = value; }
        public bool RichText { get => richText; set => richText = value; }
        public GUIStyle LabelStyle { get => labelStyle; set => labelStyle = value; }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            labelStyle = GetDefaultValue(labelStyle, GUI.skin.label);
            labelStyle.richText = richText;
            GUIContent mycontent = GetGUIContent(DefaultIGULabel);

            _ = myRect.SetSize(autoSize ? labelStyle.CalcSize(mycontent) + Vector2.right * 2f : myRect.Size);

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            GUI.Label(rectTemp, mycontent, labelStyle);

            if (useTooltip)
                if (rectTemp.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        public void SetMarkedText(params MarkedText[] markeds) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < ArrayManipulation.ArrayLength(markeds); I++)
                builder.Append(markeds[I]);
            Text = builder.ToString();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void SetDefaultValue(IGUDefaultValue value) {
            if (value == null) value = IGULabelDefault.DefaultValue;
            else if (value.GetType() == typeof(IGULabelDefault))
                throw new IGUException();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultLabelColor;
            name = value.GetValue<string>(0L);
            useTooltip = value.GetValue<bool>(1L);
            autoSize = value.GetValue<bool>(2L);
            richText = value.GetValue<bool>(3L);
            container = value.GetValue<IGUContainer>(4L);
            labelStyle = value.GetValue<GUIStyle>(5L);
        }

        public static IGULabel CreateIGUInstance(string name, bool autoSize, bool richText, IGUContent content) {
            IGULabel label = Internal_CreateIGUInstance<IGULabel>(name, content);
            label.myConfg = IGUConfig.Default;
            label.myRect = IGURect.DefaultButton;
            label.myColor = IGUColor.DefaultLabelColor;
            label.autoSize = autoSize;
            label.richText = richText;
            return label;
        }

        public static IGULabel CreateIGUInstance(string name, bool autoSize, IGUContent content)
            => CreateIGUInstance(name, autoSize, false, content);

        public static IGULabel CreateIGUInstance(string name, IGUContent content)
            => CreateIGUInstance(name, false, content);

        public static IGULabel CreateIGUInstance(string name, bool autoSize, bool richText, string text)
            => CreateIGUInstance(name, autoSize, richText, new IGUContent(text));

        public static IGULabel CreateIGUInstance(string name, bool autoSize, string text)
            => CreateIGUInstance(name, autoSize, new IGUContent(text));

        public static IGULabel CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGULabel CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultIGULabel);
    }
}
