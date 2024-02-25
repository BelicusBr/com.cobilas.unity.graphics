using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUComboBoxButton : IGUObject {

        [SerializeField] private int index;
        [SerializeField] private IGUStyle style;
        [SerializeField] private bool useTooltip;
        [SerializeField] private IGUContent myContent;
        [SerializeField] private IGUStyle tooltipStyle;
        [SerializeField] private IGUOnClickEvent onClick;

        public IGUContent MyContent => myContent;
        public IGUOnClickEvent OnClick => onClick;
        public int Index { get => index; set => index = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public string Text { get => MyContent.Text; set => MyContent.Text = value; }
        public Texture Image { get => MyContent.Image; set => MyContent.Image = value; }
        public string ToolTip { get => MyContent.Tooltip; set => MyContent.Tooltip = value; }
        public IGUStyle Style { get => style; set => style = value ?? IGUSkins.GetIGUStyle("Black button border"); }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value ?? IGUSkins.GetIGUStyle("Black box border"); }

        protected override void IGUAwake() {
            base.IGUAwake();
            useTooltip = false;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            style = IGUSkins.GetIGUStyle("Black button border");
            tooltipStyle = IGUSkins.GetIGUStyle("Black box border");
            myContent = new IGUContent(IGUButton.DefaultContentIGUButton);
        }

        protected override void LowCallOnIGU() {
            Rect rect = IGURect.rectTemp;
            rect.size = LocalRect.Size;
            rect.position = LocalRect.ModifiedPosition;
            if (GUI.Button(rect, (GUIContent)MyContent, (GUIStyle)style))
                if (IGUDrawer.Drawer.GetMouseButtonUp(LocalConfig.MouseType))
                    onClick.Invoke();

            if (UseTooltip)
                if (rect.Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
        }

        private void DrawTooltip() {
            IGUDrawer.Drawer.SetTootipText(ToolTip);
            IGUDrawer.Drawer.GUIStyleTootip((GUIStyle)TooltipStyle);
            IGUDrawer.Drawer.OpenTooltip();
        }
    }
}