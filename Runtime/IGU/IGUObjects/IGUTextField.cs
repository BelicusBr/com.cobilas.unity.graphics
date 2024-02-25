using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUTextField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField, HideInInspector] 
        protected bool isFocused;
        [SerializeField] protected bool isTextArea;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle textFieldStyle;
        [SerializeField] protected IGUTextFieldKeyCodeEvent onKeyDown;
        [SerializeField] protected IGUTextFieldKeyCharEvent onCharDown;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUTextFieldKeyCodeEvent OnKeyDown => onKeyDown;
        public IGUTextFieldKeyCharEvent OnCharDown => onCharDown;
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public bool IsTextArea { get => isTextArea; set => isTextArea = value; }
        public IGUStyle TextFieldStyle { get => textFieldStyle; set => textFieldStyle = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            maxLength = 50000;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            onKeyDown = new IGUTextFieldKeyCodeEvent();
            onCharDown = new IGUTextFieldKeyCharEvent();
            textFieldStyle = IGUSkins.GetIGUStyle("Black text field border");
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            if (isTextArea) Text = BackEndIGU.TextArea(LocalRect, Text ?? string.Empty, maxLength, textFieldStyle);
            else Text = BackEndIGU.TextField(LocalRect, Text ?? string.Empty, maxLength, textFieldStyle);
            
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

            if (current.type == EventType.KeyDown && isFocused) {
                onKeyDown.Invoke(current.keyCode);
                onCharDown.Invoke(current.character);
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
