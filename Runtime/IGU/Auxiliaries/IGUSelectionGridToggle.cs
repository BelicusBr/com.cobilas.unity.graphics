using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUSelectionGridToggle : IGUObject {
        [SerializeField] private int index;
        [SerializeField] private bool _checked;
        [SerializeField] private bool onclicked;
        [SerializeField] private IGUStyle style;
        [SerializeField] private bool useTooltip;
        [SerializeField] private bool checkedtemp;
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
        public string Text { get => MyContent.Text; set => MyContent.Text = value; }
        public Texture Image { get => MyContent.Image; set => MyContent.Image = value; }
        public string ToolTip { get => MyContent.Tooltip; set => MyContent.Tooltip = value; }
        public IGUStyle Style { get => style; set => style = value ?? IGUSkins.GetIGUStyle("Black button border"); }
        public IGUStyle TooltipStyle { get => tooltipStyle; set => tooltipStyle = value ?? IGUSkins.GetIGUStyle("Black box border"); }
        public bool Checked { 
            get => _checked;
            internal set => onChecked.Invoke(checkedtemp = _checked = value);
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            useTooltip =
                checkedtemp =
                _checked = false;
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

            checkedtemp = BackEndIGU.Toggle(LocalRect, checkedtemp, myContent, style);

            bool isRect = LocalRect.ModifiedRect.Contains(IGUDrawer.Drawer.GetMousePosition());
            if (IGUDrawer.Drawer.GetMouseButton(LocalConfig.MouseType))
                onclicked = true;
            if (Event.current.type == EventType.Repaint)
                if (isRect && _checked != checkedtemp && onclicked) {
                    onclicked = false;
                    onClick.Invoke();
                    Debug.Log($"{_checked}|{checkedtemp}|{Text}");
                    onChecked.Invoke(_checked = checkedtemp);
                }

            if (_checked) checkBoxOn.Invoke();
            else checkBoxOff.Invoke();

            if (useTooltip && isRect)
                DrawTooltip();
        }

        private void DrawTooltip() {
            IGUDrawer.Drawer.SetTootipText(ToolTip);
            IGUDrawer.Drawer.GUIStyleTootip((GUIStyle)TooltipStyle);
            IGUDrawer.Drawer.OpenTooltip();
        }
    }
}