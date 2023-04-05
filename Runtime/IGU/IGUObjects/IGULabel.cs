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

        protected override void Awake() {
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultLabelColor;
            autoSize = richText = false;
            content = new IGUContent(DefaultIGULabel);
        }

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
    }
}
