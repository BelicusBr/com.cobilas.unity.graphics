using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUNumericBoxInt : IGUObject, IIGUSerializationCallbackReceiver {

        [SerializeField] protected int value;
        [SerializeField] protected int additionValue;
        [SerializeField] protected IGUButton buttonLeft;
        [SerializeField] protected IGUButton buttonRight;
        [SerializeField] protected IGUTextField textField;
        [SerializeField] protected MaxMinSliderInt maxMinSlider;
        protected HashCodeCompare compare = new HashCodeCompare(2);

        public int Value { get => value; set => this.value = value; }
        public IGUOnClickEvent ButtonLeftOnClick => buttonLeft.OnClick;
        public IGUOnClickEvent ButtonRightOnClick => buttonRight.OnClick;
        /// <summary>Valor de soma ou subtração.(1 valor padrão)</summary>
        public int AdditionValue { get => additionValue; set => additionValue = value; }
        /// <summary>-130x130 pareão, <see cref="MaxMinSliderInt"/>.Zero para ser ilimitado.</summary>
        public MaxMinSliderInt MaxMin { get => maxMinSlider; set => maxMinSlider = value; }
        public bool UseTooltip { get => textField.UseTooltip; set => textField.UseTooltip = value; }
        public GUIStyle TooltipStyle { get => textField.TooltipStyle; set => textField.TooltipStyle = value; }
        public GUIStyle ButtonLeftStyle { get => buttonLeft.ButtonStyle; set => buttonLeft.ButtonStyle = value; }
        public GUIStyle ButtonRightStyle { get => buttonRight.ButtonStyle; set => buttonRight.ButtonStyle = value; }
        public GUIStyle TextFieldStyle { get => textField.TextFieldStyle; set => textField.TextFieldStyle = value; }

        protected override void Awake() {
            value = 0;
            additionValue = 1;
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            maxMinSlider = new MaxMinSliderInt(-130, 130);
            myRect = IGURect.DefaultButton.SetSize(50f, 32f);
            buttonLeft = CreateIGUInstance<IGUButton>();
            textField = CreateIGUInstance<IGUTextField>();
            buttonRight = CreateIGUInstance<IGUButton>();
            buttonLeft.name = "--ButtonLeft";
            textField.name = "--TextField";
            buttonRight.name = "--ButtonRight";
            buttonLeft.Text = "<";
            textField.Text = "0";
            buttonRight.Text = ">";
            buttonLeft.Parent =
                buttonRight.Parent =
                textField.Parent = this;
            InitEvents();
        }

        public override void OnIGU() {
            IGURect rect = myRect;

            float buttonWidgh = rect.Width * .5f;
            float textFieldHeight = rect.Height - 12;

            textField.MyRect = textField.MyRect.SetSize(rect.Width, textFieldHeight);
            buttonLeft.MyRect = buttonLeft.MyRect.SetPosition(0, textFieldHeight);
            buttonRight.MyRect = buttonRight.MyRect.SetPosition(buttonWidgh, textFieldHeight);

            buttonLeft.MyRect = buttonLeft.MyRect.SetSize(buttonWidgh, 12);
            buttonRight.MyRect = buttonRight.MyRect.SetSize(buttonWidgh, 12);

            buttonLeft.MyColor = buttonRight.MyColor = textField.MyColor = myColor;

            if (maxMinSlider != MaxMinSliderInt.Zero) {
                value = maxMinSlider.Min < value ? maxMinSlider.Min : value;
                value = maxMinSlider.Max > value ? maxMinSlider.Max : value;
            }

            try {
                if (!compare.HashCodeEqual(0, value.GetHashCode()))
                    textField.Text = value.ToString();
                if (!compare.HashCodeEqual(1, textField.Text.GetHashCode()))
                    value = Convert.ToInt32(textField.Text);
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

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR
            InitEvents();
#endif
        }
    }
}
