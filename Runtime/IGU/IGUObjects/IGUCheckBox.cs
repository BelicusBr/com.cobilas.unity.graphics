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

        protected override void SetDefaultValue(IGUDefaultValue value) {
            if (value == null) value = IGUCheckBoxDefault.DefaultValue;
            else if (value.GetType() == typeof(IGUCheckBoxDefault))
                throw new IGUException();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            name = value.GetValue<string>(0L);
            useTooltip = value.GetValue<bool>(1L);
            container = value.GetValue<IGUContainer>(2L);
            _checked = value.GetValue<bool>(3L);
            checkBoxStyle = value.GetValue<GUIStyle>(4L);
            oneClick = !_checked;
            onClick = new IGUOnClickEvent();
            checkBoxOn = new IGUOnClickEvent();
            checkBoxOff = new IGUOnClickEvent();
            onChecked = new IGUOnCheckedEvent();
        }

        public static IGUCheckBox CreateIGUInstance(string name, bool _checked, IGUContent content) {
            IGUCheckBox checkBox = Internal_CreateIGUInstance<IGUCheckBox>(name);
            checkBox.content = content;
            checkBox._checked = _checked;
            checkBox.oneClick = !_checked;
            checkBox.onClick = new IGUOnClickEvent();
            checkBox.checkBoxOn = new IGUOnClickEvent();
            checkBox.myConfg = IGUConfig.Default;
            checkBox.checkBoxOff = new IGUOnClickEvent();
            checkBox.myRect = IGURect.DefaultButton;
            checkBox.onChecked = new IGUOnCheckedEvent();
            checkBox.myColor = IGUColor.DefaultBoxColor;
            return checkBox;
        }

        public static IGUCheckBox CreateIGUInstance(string name, IGUContent content)
            => CreateIGUInstance(name, false, content);

        public static IGUCheckBox CreateIGUInstance(string name, bool _checked, string text)
            => CreateIGUInstance(name, _checked, new IGUContent(text));

        public static IGUCheckBox CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, false, text);

        public static IGUCheckBox CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultContantIGUCheckBox);
    }
}
