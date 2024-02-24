using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUCheckBox : IGUTextObject {
        public const string DefaultContantIGUCheckBox = "IGU check box";

        private bool oneClick;
        [SerializeField] protected bool _checked;
        [SerializeField] protected bool checkedtemp;
        [SerializeField] protected IGUStyle checkBoxStyle;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUOnClickEvent checkBoxOn;
        [SerializeField] protected IGUOnClickEvent checkBoxOff;
        [SerializeField] protected IGUOnCheckedEvent onChecked;

        public IGUOnClickEvent OnClick => onClick;
        public IGUOnCheckedEvent OnChecked => onChecked;
        public IGUOnClickEvent CheckBoxOn => checkBoxOn;
        public IGUOnClickEvent CheckBoxOff => checkBoxOff;
        public IGUStyle CheckBoxStyle { get => checkBoxStyle; set => checkBoxStyle = value; }
        public bool Checked { 
            get => _checked;
            set => onChecked.Invoke(_checked = checkedtemp = value);
        }

        protected override void Ignition() {
            base.Ignition();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            content = new IGUContent(DefaultContantIGUCheckBox);
            checkBoxStyle = IGUSkins.GetIGUStyle("Black toggle border");
            oneClick = !(_checked = false);
            onClick = new IGUOnClickEvent();
            checkBoxOn = new IGUOnClickEvent();
            checkBoxOff = new IGUOnClickEvent();
            onChecked = new IGUOnCheckedEvent();
        }

        protected override void LowCallOnIGU() {

            checkedtemp = GUI.Toggle(GetRect(), checkedtemp, GetGUIContent(DefaultContantIGUCheckBox),
                IGUStyle.GetGUIStyleTemp(checkBoxStyle));

            if (checkedtemp) {
                if (oneClick)
                    ModChecked(checkedtemp, false);
            } else {
                if (!oneClick)
                    ModChecked(checkedtemp, true);
            }

            if (_checked) checkBoxOn.Invoke();
            else checkBoxOff.Invoke();

            if (useTooltip)
                if (GetRect(true).Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
        }

        private void ModChecked(bool modChecked, bool modOneClick) {
            if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                _checked = modChecked;
                oneClick = modOneClick;
                onClick.Invoke();
                onChecked.Invoke(modChecked);
            }
        }

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void DrawTooltip()
            => base.DrawTooltip();
    }
}
