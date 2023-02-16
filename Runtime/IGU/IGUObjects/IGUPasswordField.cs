using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPasswordField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected GUIStyle passwordFieldStyle;

        /// <summary>Indica sé o <see cref="IGUPasswordField"/> está focado.</summary>
        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        /// <summary>Caracter de ocultação.</summary>
        public char MaskChar { get => maskChar; set => maskChar = value; }
        /// <summary>Número maximo de caracteres permitidos.(50000 padrão)</summary>
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public GUIStyle PasswordFieldStyle { get => passwordFieldStyle; set => passwordFieldStyle = value; }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            passwordFieldStyle = GetDefaultValue(passwordFieldStyle, GUI.skin.textField);

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            Text = GUI.PasswordField(rectTemp, GetGUIContent("").text, maskChar, maxLength, passwordFieldStyle);

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
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);

        protected override void SetGUISettings(GUISettings settings)
            => base.SetGUISettings(settings);

        protected override void SetGUISettings(IGUTextSettings settings)
            => base.SetGUISettings(settings);

        public static IGUPasswordField CreateIGUInstance(string name, char maskChar, int maxLength, IGUContent content) {
            IGUPasswordField textField = Internal_CreateIGUInstance<IGUPasswordField>(name, content);
            textField.maskChar = maskChar;
            textField.maxLength = maxLength;
            textField.onClick = new IGUOnClickEvent();
            textField.myConfg = IGUConfig.Default;
            textField.myRect = IGURect.DefaultButton;
            textField.myColor = IGUColor.DefaultBoxColor;
            return textField;
        }

        public static IGUPasswordField CreateIGUInstance(string name, char maskChar, IGUContent content)
            => CreateIGUInstance(name, maskChar, 50000, content);

        public static IGUPasswordField CreateIGUInstance(string name, IGUContent content)
            => CreateIGUInstance(name, '*', content);

        public static IGUPasswordField CreateIGUInstance(string name, char maskChar, int maxLength, string text)
            => CreateIGUInstance(name, maskChar, maxLength, new IGUContent(text));

        public static IGUPasswordField CreateIGUInstance(string name, char maskChar, string text)
            => CreateIGUInstance(name, maskChar, new IGUContent(text));

        public static IGUPasswordField CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public static IGUPasswordField CreateIGUInstance(string name)
            => CreateIGUInstance(name, "");
    }
}
