using System;
using UnityEngine;
using System.Globalization;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUNumericBox : IGUObject, ISerializationCallbackReceiver {

        private bool afterDeserialize = false;

        [SerializeField] protected float value;
        [SerializeField] protected float additionValue;
        [SerializeField] protected IGUButton buttonLeft;
        [SerializeField] protected IGUButton buttonRight;
        [SerializeField] protected IGUTextField textField;
        [SerializeField] protected MaxMinSlider maxMinSlider;
        [SerializeField] protected string numberOfDecimalPlaces;
        protected HashCodeCompare compare = new HashCodeCompare(2);

        public float Value { get => value; set => this.value = value; }
        public IGUOnClickEvent ButtonLeftOnClick => buttonLeft.OnClick;
        public IGUOnClickEvent ButtonRightOnClick => buttonRight.OnClick;
        /// <summary>-130x130 pareão, <see cref="MaxMinSlider"/>.Zero para ser ilimitado.</summary>
        public MaxMinSlider MaxMin { get => maxMinSlider; set => maxMinSlider = value; }
        /// <summary>Valor de soma ou subtração.(.001f valor padrão)</summary>
        public float AdditionValue { get => additionValue; set => additionValue = value; }
        public bool UseTooltip { get => textField.UseTooltip; set => textField.UseTooltip = value; }
        public GUIStyle TooltipStyle { get => textField.TooltipStyle; set => textField.TooltipStyle = value; }
        public GUIStyle ButtonLeftStyle { get => buttonLeft.ButtonStyle; set => buttonLeft.ButtonStyle = value; }
        /// <summary>Quantas casas decimais para ser exibida.(N3 padrão)</summary>
        public string NumberOfDecimalPlaces { get => numberOfDecimalPlaces; set => numberOfDecimalPlaces = value; }
        public GUIStyle ButtonRightStyle { get => buttonRight.ButtonStyle; set => buttonRight.ButtonStyle = value; }
        public GUIStyle TextFieldStyle { get => textField.TextFieldStyle; set => textField.TextFieldStyle = value; }

        public override void OnIGU() {
            if (!GetModIGUConfig().IsVisible) return;
            IGURect rect = myRect;

            float buttonWidgh = rect.Width * .5f;
            float textFieldHeight = rect.Height - 12;

            textField.MyRect = textField.MyRect.SetSize(rect.Width, textFieldHeight);
            buttonLeft.MyRect = buttonLeft.MyRect.SetPosition(0, textFieldHeight);
            buttonRight.MyRect = buttonRight.MyRect.SetPosition(buttonWidgh, textFieldHeight);

            buttonLeft.MyRect = buttonLeft.MyRect.SetSize(buttonWidgh, 12);
            buttonRight.MyRect = buttonRight.MyRect.SetSize(buttonWidgh, 12);

            buttonLeft.MyColor = buttonRight.MyColor = textField.MyColor = myColor;

            if (maxMinSlider != MaxMinSlider.Zero) {
                value = maxMinSlider.Min < value ? maxMinSlider.Min : value;
                value = maxMinSlider.Max > value ? maxMinSlider.Max : value;
            }

            try {
                if (!compare.HashCodeEqual(0, value.GetHashCode())) {
                    if (numberOfDecimalPlaces != (string)null)
                        textField.Text = value.ToString(numberOfDecimalPlaces, CultureInfo.InvariantCulture);
                    else textField.Text = value.ToString(CultureInfo.InvariantCulture);
                }
                if (!compare.HashCodeEqual(1, textField.Text.GetHashCode()))
                    value = Convert.ToSingle(textField.Text, CultureInfo.InvariantCulture);
            } catch {
                textField.Text = "0";
            }

            if (textField.TextFieldStyle != (GUIStyle)null)
                textField.TextFieldStyle.alignment = TextAnchor.MiddleCenter;

            textField.OnIGU();
            buttonLeft.OnIGU();
            buttonRight.OnIGU();
        }

        private void InitEvents() {
            buttonLeft.OnClick.AddListener(() => value -= additionValue);
            buttonRight.OnClick.AddListener(() => value += additionValue);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() => afterDeserialize = true;

        protected override void OnEnable() {
#if UNITY_EDITOR
            if (afterDeserialize) {
                afterDeserialize = false;
                InitEvents();
            }
#endif
        }

        public static IGUNumericBox CreateIGUInstance(string name, float value, MaxMinSlider maxmin) {
            IGUNumericBox numericBox = Internal_CreateIGUInstance<IGUNumericBox>(name);
            numericBox.value = value;
            numericBox.maxMinSlider = maxmin;
            numericBox.additionValue = .001f;
            numericBox.myConfg = IGUConfig.Default;
            numericBox.numberOfDecimalPlaces = "N3";
            numericBox.myColor = IGUColor.DefaultBoxColor;
            numericBox.myRect = IGURect.DefaultButton.SetSize(50f, 32f);
            numericBox.buttonLeft = IGUButton.CreateIGUInstance($"({name})-ButtonLeft", "<");
            numericBox.textField = IGUTextField.CreateIGUInstance($"({name})-TextField", "0");
            numericBox.buttonRight = IGUButton.CreateIGUInstance($"({name})-ButtonRight", ">");
            numericBox.InitEvents();
            return numericBox;
        }

        public static IGUNumericBox CreateIGUInstance(string name, float value)
            => CreateIGUInstance(name, value, new MaxMinSlider(-130f, 130f));

        public static IGUNumericBox CreateIGUInstance(string name)
            => CreateIGUInstance(name, 0f);
    }
}
