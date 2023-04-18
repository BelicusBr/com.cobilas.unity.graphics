using UnityEngine;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabel : IGUTextObject {
        public const string DefaultIGULabel = "IGU Label";
        [SerializeField] private bool autoSize;
        [SerializeField] private IGUStyle labelStyle;

        public bool AutoSize { get => autoSize; set => autoSize = value; }
        public IGUStyle LabelStyle { get => labelStyle; set => labelStyle = value; }
        public bool RichText { get => labelStyle.RichText; set => labelStyle.RichText = value; }

        protected override void Awake() {
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultLabelColor;
            labelStyle = IGUSkins.GetIGUStyle("Label");
            autoSize = RichText = false;
            content = new IGUContent(DefaultIGULabel);
        }

        protected override void LowCallOnIGU() {

            GUIStyle style = IGUStyle.GetGUIStyleTemp(labelStyle);

            GUIContent mycontent = GetGUIContent(DefaultIGULabel);

            _ = myRect.SetSize(autoSize ? style.CalcSize(mycontent) + Vector2.right * 2f : myRect.Size);

            GUI.Label(GetRect(), mycontent, style);

            if (useTooltip)
                if (GetRect(true).Contains(Event.current.mousePosition))
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
