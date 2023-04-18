﻿using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPasswordField : IGUTextFieldObject {

        [SerializeField] protected int maxLength;
        [SerializeField] protected char maskChar;
        [SerializeField, HideInInspector] protected bool isFocused;
        [SerializeField] protected IGUOnClickEvent onClick;
        [SerializeField] protected IGUStyle passwordFieldStyle;

        /// <summary>Indica sé o <see cref="IGUPasswordField"/> está focado.</summary>
        public bool IsFocused => isFocused;
        public IGUOnClickEvent OnClick => onClick;
        /// <summary>Caracter de ocultação.</summary>
        public char MaskChar { get => maskChar; set => maskChar = value; }
        /// <summary>Número maximo de caracteres permitidos.(50000 padrão)</summary>
        public int MaxLength { get => maxLength; set => maxLength = value; }
        public IGUStyle PasswordFieldStyle { get => passwordFieldStyle; set => passwordFieldStyle = value; }

        protected override void Awake() {
            base.Awake();
            maskChar = '*';
            maxLength = 50000;
            onClick = new IGUOnClickEvent();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            passwordFieldStyle = IGUSkins.GetIGUStyle("Black text field border");
        }

        protected override void LowCallOnIGU() {

            GUISettings oldSettings = GUI.skin.settings;
            SetGUISettings(settings);

            Text = GUI.PasswordField(GetRect(), GetGUIContent("").text, maskChar, maxLength, IGUStyle.GetGUIStyleTemp(passwordFieldStyle));

            SetGUISettings(oldSettings);
            Event current = Event.current;

            if (GetRect(true).Contains(current.mousePosition)) {
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
    }
}
