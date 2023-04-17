using System;
using UnityEngine;
using System.Globalization;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUNumericBox : IGUObject, IIGUSerializationCallbackReceiver {

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
        public string Tooltip { get => textField.ToolTip; set => textField.ToolTip = value; }
        public bool UseTooltip { get => textField.UseTooltip; set => textField.UseTooltip = value; }
        public IGUStyle TooltipStyle { get => textField.TooltipStyle; set => textField.TooltipStyle = value; }
        public IGUStyle ButtonLeftStyle { get => buttonLeft.ButtonStyle; set => buttonLeft.ButtonStyle = value; }
        /// <summary>Quantas casas decimais para ser exibida.(N3 padrão)</summary>
        public string NumberOfDecimalPlaces { get => numberOfDecimalPlaces; set => numberOfDecimalPlaces = value; }
        public IGUStyle ButtonRightStyle { get => buttonRight.ButtonStyle; set => buttonRight.ButtonStyle = value; }
        public IGUStyle TextFieldStyle { get => textField.TextFieldStyle; set => textField.TextFieldStyle = value; }

        protected override void Awake() {
            value = 0f;
            additionValue = .001f;
            myConfg = IGUConfig.Default;
            numberOfDecimalPlaces = "N3";
            myColor = IGUColor.DefaultBoxColor;
            maxMinSlider = new MaxMinSlider(-130f, 130f);
            myRect = IGURect.DefaultButton.SetSize(50f, 32f);
            buttonLeft = CreateIGUInstance<IGUButton>($"--[{name}]ButtonLeft");
            textField = CreateIGUInstance<IGUTextField>($"--[{name}]TextField");
            buttonRight = CreateIGUInstance<IGUButton>($"--[{name}]ButtonRight");
            buttonLeft.Text = "<";
            textField.Text = "0";
            buttonRight.Text = ">";
            textField.TextFieldStyle.Alignment = TextAnchor.MiddleCenter;
            buttonLeft.Parent =
                buttonRight.Parent =
                textField.Parent = this;
            InitEvents();
        }

        protected override void LowCallOnIGU() {
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

            textField.OnIGU();
            buttonLeft.OnIGU();
            buttonRight.OnIGU();
        }

        private void InitEvents() {
            buttonLeft.OnClick.AddListener(() => value -= additionValue);
            buttonRight.OnClick.AddListener(() => value += additionValue);
        }

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR
            InitEvents();
#endif
        }
    }
}
