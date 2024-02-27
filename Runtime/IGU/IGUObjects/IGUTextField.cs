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
        [SerializeField] protected IGUTextFieldStringEvent onStringChanged;

        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUTextFieldKeyCodeEvent OnKeyDown => onKeyDown;
        public IGUTextFieldKeyCharEvent OnCharDown => onCharDown;
        public IGUTextFieldStringEvent OnStringChanged => onStringChanged;
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public bool IsTextArea { get => isTextArea; set => isTextArea = value; }
        public IGUStyle TextFieldStyle { 
            get => textFieldStyle;
            set => textFieldStyle = value ?? (IGUStyle)"Black text field border";
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            maxLength = 50000;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            onKeyDown = new IGUTextFieldKeyCodeEvent();
            onCharDown = new IGUTextFieldKeyCharEvent();
            onStringChanged = new IGUTextFieldStringEvent();
            textFieldStyle = (IGUStyle)"Black text field border";
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);
            string textTemp = string.Empty;

            if (isTextArea) textTemp = BackEndIGU.TextArea(LocalRect, Text ?? string.Empty, maxLength, textFieldStyle);
            else textTemp = BackEndIGU.TextField(LocalRect, Text ?? string.Empty, maxLength, textFieldStyle);
            
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

            if (current.type == EventType.KeyDown && isFocused) {
                onKeyDown.Invoke(current.keyCode);
                onCharDown.Invoke(current.character);
            }
        }

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);
    }
}
