using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class TDS_IGUSelectionGridToggle : IGUObject {
        [SerializeField] private int index;
        [SerializeField] private bool oneClick;
        [SerializeField] private bool _checked;
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

        protected override void Ignition() {
            base.Ignition();
            oneClick = true;
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
            rect.size = LocalRect.ModifiedSize;

            checkedtemp = GUI.Toggle(rect, checkedtemp, (GUIContent)myContent, (GUIStyle)style);
            //rect.size = MyRect.SetScaleFactor(GetParentRoot(this).MyRect.ScaleFactor).ModifiedSize;
            Debug.Log($"{Text}|{rect}");

            bool isRect = rect.Contains(IGUDrawer.Drawer.GetMousePosition());
            if (isRect && IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                //Debug.Log($"TG|{checkedtemp}|{Text}");
                _checked = checkedtemp;
                onClick.Invoke();
                onChecked.Invoke(checkedtemp);
            }

            // if (checkedtemp) {
            //     if (oneClick && rect.Contains(IGUDrawer.Drawer.GetMousePosition()))
            //         ModChecked(checkedtemp, false);
            // } else {
            //     if (!oneClick && rect.Contains(IGUDrawer.Drawer.GetMousePosition()))
            //         ModChecked(checkedtemp, true);
            // }

            if (_checked) checkBoxOn.Invoke();
            else checkBoxOff.Invoke();

            if (useTooltip && isRect)
                DrawTooltip();
        }

        public void Select(bool check) => oneClick = !(_checked = check);

        private void ModChecked(bool modChecked, bool modOneClick) {
            if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                _checked = modChecked;
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