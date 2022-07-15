using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUTextField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField, HideInInspector] 
        protected bool isFocused;
        [SerializeField] protected bool isTextArea;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected GUIStyle textFieldStyle;
        [SerializeField] protected IGUTextFieldKeyCodeEvent onKeyDown;
        [SerializeField] protected IGUTextFieldKeyCharEvent onCharDown;

        /// <summary>Indica sé o <see cref="IGUTextField"/> está focado.</summary>
        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        public IGUTextFieldKeyCharEvent OnCharDown => onCharDown;
        public IGUTextFieldKeyCodeEvent OnKeyDown => onKeyDown;
        /// <summary>Número maximo de caracteres permitidos.(50000 padrão)</summary>
        public int MaxLength { get => maxLength; set => maxLength = value; }
        /// <summary>Permite o <see cref="IGUTextField"/> ser multi-linha.</summary>
        public bool IsTextArea { get => isTextArea; set => isTextArea = value; }
        public GUIStyle TextFieldStyle { get => textFieldStyle; set => textFieldStyle = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            textFieldStyle = GetDefaultValue(textFieldStyle, GUI.skin.textField);

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            if (isTextArea) Text = GUI.TextArea(rectTemp, GetGUIContent("").text, maxLength, textFieldStyle);
            else Text = GUI.TextField(rectTemp, GetGUIContent("").text, maxLength, textFieldStyle);
            
            SetGUISettings(oldSettings);
            Event current = Event.current;

            if (rectTemp.Contains(current.mousePosition)) {
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

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);

        public static IGUTextField CreateIGUInstance(string name, bool isTextArea, int maxLength, IGUContent content) {
            IGUTextField textField = Internal_CreateIGUInstance<IGUTextField>(name, content);
            textField.maxLength = maxLength;
            textField.myConfg = IGUConfig.Default;
            textField.onClick = new IGUOnClickEvent();
            textField.settings = new IGUTextSettings();
            textField.myColor = IGUColor.DefaultBoxColor;
            textField.onKeyDown = new IGUTextFieldKeyCodeEvent();
            textField.onCharDown = new IGUTextFieldKeyCharEvent();
            if (textField.isTextArea = isTextArea) textField.myRect = IGURect.DefaultBox;
            else textField.myRect = IGURect.DefaultButton;
            return textField;
        }

        public static IGUTextField CreateIGUInstance(string name, bool isTextArea, IGUContent content)
            => CreateIGUInstance(name, isTextArea, 50000, content);

        public static IGUTextField CreateIGUInstance(string name, IGUContent content)
            => CreateIGUInstance(name, false, content);

        public static IGUTextField CreateIGUInstance(string name, bool isTextArea, int maxLength, string text)
            => CreateIGUInstance(name, isTextArea, maxLength, new IGUContent(text));

        public static IGUTextField CreateIGUInstance(string name, bool isTextArea, string text)
            => CreateIGUInstance(name, isTextArea, new IGUContent(text));

        public static IGUTextField CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGUTextField CreateIGUInstance(string name)
            => CreateIGUInstance(name, "");
    }
}
