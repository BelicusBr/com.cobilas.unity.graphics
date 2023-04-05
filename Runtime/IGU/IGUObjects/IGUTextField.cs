﻿using UnityEngine;
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

        protected override void Awake() {
            base.Awake();
            maxLength = 50000;
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            onClick = new IGUOnClickEvent();
            myColor = IGUColor.DefaultBoxColor;
            onKeyDown = new IGUTextFieldKeyCodeEvent();
            onCharDown = new IGUTextFieldKeyCharEvent();
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
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
    }
}
