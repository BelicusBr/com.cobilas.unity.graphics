using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private IGUStyle boxStyle;

        public IGUStyle BoxStyle { get => boxStyle; set => boxStyle = value; }

        protected override void Ignition() {
            base.Ignition();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            boxStyle = IGUSkins.GetIGUStyle("Black box border");
            content = new IGUContent(DefaultIGUBox);
        }

        protected override void LowCallOnIGU() {

            GUI.Box(GetRect(), GetGUIContent(DefaultIGUBox), IGUStyle.GetGUIStyleTemp(boxStyle));

            if (useTooltip)
                if (GetRect(true).Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);
    }
}
