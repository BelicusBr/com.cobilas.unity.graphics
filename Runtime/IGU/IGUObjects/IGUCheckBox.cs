using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUCheckBox : IGUTextObject {
        public const string DefaultContantIGUCheckBox = "IGU check box";

        private bool oneClick;
        [SerializeField] protected bool _checked;
        [SerializeField] protected GUIStyle checkBoxStyle;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUOnClickEvent checkBoxOn;
        [SerializeField] protected IGUOnClickEvent checkBoxOff;
        [SerializeField] protected IGUOnCheckedEvent onChecked;

        public IGUOnClickEvent OnClick => onClick;
        public IGUOnCheckedEvent OnChecked => onChecked;
        public IGUOnClickEvent CheckBoxOn => checkBoxOn;
        public IGUOnClickEvent CheckBoxOff => checkBoxOff;
        public bool Checked { get => _checked; set => _checked = value; }
        public GUIStyle CheckBoxStyle { get => checkBoxStyle; set => checkBoxStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            content = new IGUContent(DefaultContantIGUCheckBox);
            oneClick = !(_checked = false);
            onClick = new IGUOnClickEvent();
            checkBoxOn = new IGUOnClickEvent();
            checkBoxOff = new IGUOnClickEvent();
            onChecked = new IGUOnCheckedEvent();
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();

            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            checkBoxStyle = GetDefaultValue(checkBoxStyle, GUI.skin.toggle);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            bool checkedtemp = GUI.Toggle(rectTemp, _checked, GetGUIContent(DefaultContantIGUCheckBox), checkBoxStyle);

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
                if (rectTemp.Contains(IGUDrawer.Drawer.GetMousePosition()))
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
