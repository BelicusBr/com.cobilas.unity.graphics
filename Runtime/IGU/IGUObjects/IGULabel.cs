using UnityEngine;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabel : IGUTextObject {
        public const string DefaultIGULabel = "IGU Label";
        [SerializeField] private bool autoSize;
        [SerializeField] private IGUStyle labelStyle;

        public bool AutoSize { get => autoSize; set => autoSize = value; }
        public IGUStyle LabelStyle { get => labelStyle; set => labelStyle = value ?? (IGUStyle)"Label"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoSize = false;
            myRect = IGURect.DefaultButton;
            labelStyle = (IGUStyle)"Label";
            myColor = IGUColor.DefaultLabelColor;
            content = new IGUContent(DefaultIGULabel);
        }

        protected override void LowCallOnIGU() {
            labelStyle.RichText = richText;
            GUIStyle style = (GUIStyle)labelStyle;
            GUIContent content = GetGUIContentTemp(MyContent);

            myRect = myRect.SetSize(autoSize ? style.CalcSize(content) + Vector2.right * 2f : myRect.Size);

            BackEndIGU.Label(LocalRect, MyContent, labelStyle);

            if (useTooltip)
                if (LocalRect.ModifiedRect.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();
    }
}
