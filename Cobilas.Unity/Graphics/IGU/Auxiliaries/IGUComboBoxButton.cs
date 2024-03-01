using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUComboBoxButton : IGUObject, IIGUToolTip {

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
        public IGUStyle Style { get => style; set => style = value ?? (IGUStyle)"Black button border"; }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value ?? (IGUStyle)"Black box border"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            useTooltip = false;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            style = (IGUStyle)"Black button border";
            tooltipStyle = (IGUStyle)"Black box border";
            myContent = new IGUContent(IGUButton.DefaultContentIGUButton);
        }

        protected override void LowCallOnIGU() {
            if (BackEndIGU.Button(LocalRect, MyContent, style))
                if (IGUDrawer.GetMouseButtonUp(LocalConfig.MouseType))
                    onClick.Invoke();
        }

        void IIGUToolTip.InternalDrawToolTip() {
            if (LocalRect.Contains(IGUDrawer.MousePosition) && UseTooltip &&
                LocalConfig.IsVisible && !string.IsNullOrEmpty(ToolTip))
                    IGUDrawer.DrawTooltip(ToolTip, tooltipStyle);
        }
    }
}