using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUCheckBox : IGUTextObject {
        public const string DefaultContantIGUCheckBox = "IGU check box";

        [SerializeField] private bool oneClick;
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
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            checkBoxStyle = GetDefaultValue(checkBoxStyle, GUI.skin.toggle);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            if (_checked = GUI.Toggle(rectTemp, _checked, GetGUIContent(DefaultContantIGUCheckBox), checkBoxStyle)) {
                checkBoxOn.Invoke();
                if (oneClick) {
                    oneClick = false;
                    onClick.Invoke();
                    onChecked.Invoke(_checked);
                }
            } else {
                checkBoxOff.Invoke();
                if (!oneClick) {
                    oneClick = true;
                    onClick.Invoke();
                    onChecked.Invoke(_checked);
                }
            }

            Event current = Event.current;

            if (useTooltip)
                if (rectTemp.Contains(current.mousePosition))
                    DrawTooltip();
        }

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void DrawTooltip()
            => base.DrawTooltip();

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
