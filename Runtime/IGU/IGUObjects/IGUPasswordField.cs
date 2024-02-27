using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPasswordField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle passwordFieldStyle;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUTextFieldStringEvent onStringChanged;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUTextFieldStringEvent OnStringChanged => onStringChanged;
        public char MaskChar { get => maskChar; set => maskChar = value; }
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public IGUStyle PasswordFieldStyle { 
            get => passwordFieldStyle;
            set => passwordFieldStyle = value ?? (IGUStyle)"Black text field border";
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            maskChar = '*';
            maxLength = 50000;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            onStringChanged = new IGUTextFieldStringEvent();
            passwordFieldStyle = (IGUStyle)"Black text field border";
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            string textTemp = BackEndIGU.PasswordField(LocalRect, Text ?? string.Empty, maskChar, maxLength, passwordFieldStyle);

            SetGUISettings(oldSettings);
            Event current = Event.current;

            if (LocalRect.Contains(current.mousePosition)) {
                if (current.clickCount > 0 && GUI.GetNameOfFocusedControl() == name) {
                    isFocused = true;
                    onClick.Invoke();
                }
            } else {
                if (current.clickCount > 0)
                    isFocused = false;
            }

            if (textTemp != Text && isFocused)
                onStringChanged.Invoke(Text = textTemp);
        }

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);
    }
}
