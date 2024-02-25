using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPasswordField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle passwordFieldStyle;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
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
            passwordFieldStyle = (IGUStyle)"Black text field border";
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            Text = BackEndIGU.PasswordField(LocalRect, Text ?? string.Empty, maskChar, maxLength, passwordFieldStyle);

            SetGUISettings(oldSettings);
            Event current = Event.current;

            if (LocalRect.ModifiedRect.Contains(current.mousePosition)) {
                if (current.clickCount > 0 && GUI.GetNameOfFocusedControl() == name) {
                    isFocused = true;
                    onClick.Invoke();
                }
                if (useTooltip)
                    DrawTooltip();
            } else {
                if (current.clickCount > 0)
                    isFocused = false;
            }
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);
    }
}
