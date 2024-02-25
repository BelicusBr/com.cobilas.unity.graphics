using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private IGUStyle boxStyle;

        public IGUStyle BoxStyle { get => boxStyle; set => boxStyle = value ?? (IGUStyle)"Black box border"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            boxStyle = (IGUStyle)"Black box border";
            content = new IGUContent(DefaultIGUBox);
        }

        protected override void LowCallOnIGU() {
            boxStyle.RichText = richText;
            BackEndIGU.Box(LocalRect, MyContent, boxStyle);

            if (useTooltip)
                if (LocalRect.ModifiedRect.Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();
    }
}
