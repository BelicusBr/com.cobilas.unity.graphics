using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class TDS_IGUSelectionGridToggle : IGUObject {
        private bool oneClick;
        [SerializeField] private int index;
        [SerializeField] private bool _checked;
        [SerializeField] private IGUStyle style;
        [SerializeField] private bool useTooltip;
        [SerializeField] private IGUContent myContent;
        [SerializeField] private IGUStyle tooltipStyle;
        [SerializeField] private IGUOnClickEvent onClick;
        [SerializeField] private IGUOnClickEvent checkBoxOn;
        [SerializeField] private IGUOnClickEvent checkBoxOff;
        [SerializeField] private IGUOnCheckedEvent onChecked;

        public IGUContent MyContent => myContent;
        public IGUOnClickEvent OnClick => onClick;
        public IGUOnClickEvent CheckBoxOn => checkBoxOn;
        public IGUOnCheckedEvent OnChecked => onChecked;
        public IGUOnClickEvent CheckBoxOff => checkBoxOff;
        public int Index { get => index; set => index = value; }
        public bool UseTooltip { get => useTooltip; set => useTooltip = value; }
        public bool Checked { get => _checked; internal set => _checked = value; }
        public string Text { get => MyContent.Text; set => MyContent.Text = value; }
        public Texture Image { get => MyContent.Image; set => MyContent.Image = value; }
        public string ToolTip { get => MyContent.Tooltip; set => MyContent.Tooltip = value; }
        public IGUStyle Style { get => style; set => style = value ?? IGUSkins.GetIGUStyle("Black button border"); }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value ?? IGUSkins.GetIGUStyle("Black box border"); }

        protected override void Ignition() {
            base.Ignition();
            useTooltip = false;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            checkBoxOn = new IGUOnClickEvent();
            checkBoxOff = new IGUOnClickEvent();
            onChecked = new IGUOnCheckedEvent();
            style =  IGUSkins.GetIGUStyle("Black button border");
            tooltipStyle = IGUSkins.GetIGUStyle("Black box border");
            myContent = new IGUContent(IGUCheckBox.DefaultContantIGUCheckBox);
        }

        protected override void LowCallOnIGU() {
            Rect rect = IGURect.rectTemp;
            rect.position = LocalRect.ModifiedPosition;
            rect.size = LocalRect.Size;

            bool checkedtemp = GUI.Toggle(rect, Checked, (GUIContent)myContent, (GUIStyle)style);

            if (checkedtemp) {
                if (oneClick)
                    ModChecked(checkedtemp, false);
            } else {
                if (!oneClick)
                    ModChecked(checkedtemp, true);
            }

            if (Checked) checkBoxOn.Invoke();
            else checkBoxOff.Invoke();

            if (useTooltip)
                if (rect.Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
        }

        private void ModChecked(bool modChecked, bool modOneClick) {
            if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                Checked = modChecked;
                oneClick = modOneClick;
                onClick.Invoke();
                onChecked.Invoke(modChecked);
            }
        }

        private void DrawTooltip() {
            IGUDrawer.Drawer.SetTootipText(ToolTip);
            IGUDrawer.Drawer.GUIStyleTootip((GUIStyle)TooltipStyle);
            IGUDrawer.Drawer.OpenTooltip();
        }
    }
}