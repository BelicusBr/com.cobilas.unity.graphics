using UnityEngine;
using System.Globalization;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUNumericBox : IGUObject, IIGUToolTip, IIGUEndOfFrame {

        [SerializeField] protected float value;
        [SerializeField] protected float additionValue;
        [SerializeField] protected IGUTextField textField;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected MaxMinSlider maxMinSlider;
        [SerializeField] protected IGURepeatButton buttonLeft;
        [SerializeField] protected IGURepeatButton buttonRight;
        [SerializeField] protected string numberOfDecimalPlaces;

        public float Value { get => value; set => this.value = value; }
        public IGUOnClickEvent ButtonLeftOnClick => buttonLeft.OnClick;
        public IGUOnClickEvent ButtonRightOnClick => buttonRight.OnClick;
        public MaxMinSlider MaxMin { get => maxMinSlider; set => maxMinSlider = value; }
        public float AdditionValue { get => additionValue; set => additionValue = value; }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public string ToolTip { get => textField.ToolTip; set => textField.ToolTip = value; }
        public bool UseTooltip { get => textField.UseTooltip; set => textField.UseTooltip = value; }
        public IGUStyle TooltipStyle { get => textField.TooltipStyle; set => textField.TooltipStyle = value; }
        public IGUStyle ButtonLeftStyle { get => buttonLeft.ButtonStyle; set => buttonLeft.ButtonStyle = value; }
        public string NumberOfDecimalPlaces { get => numberOfDecimalPlaces; set => numberOfDecimalPlaces = value; }
        public IGUStyle ButtonRightStyle { get => buttonRight.ButtonStyle; set => buttonRight.ButtonStyle = value; }
        public IGUStyle TextFieldStyle { get => textField.TextFieldStyle; set => textField.TextFieldStyle = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            value = 0f;
            additionValue = .001f;
            numberOfDecimalPlaces = "N3";
            myColor = IGUColor.DefaultBoxColor;
            maxMinSlider = new MaxMinSlider(-130f, 130f);
            myRect = IGURect.DefaultButton.SetSize(50f, 32f);
            textField = Create<IGUTextField>($"--[{name}]TextField");
            buttonLeft = Create<IGURepeatButton>($"--[{name}]ButtonLeft");
            buttonRight = Create<IGURepeatButton>($"--[{name}]ButtonRight");
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            buttonLeft.Text = "<";
            textField.Text = "0";
            buttonRight.Text = ">";
            textField.TextFieldStyle.Alignment = TextAnchor.MiddleCenter;
            buttonLeft.Parent =
                buttonRight.Parent =
                textField.Parent = this;
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
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

            if (maxMinSlider != MaxMinSlider.Zero)
                value = Mathf.Clamp(value, maxMinSlider.Min, maxMinSlider.Max);

            textField.OnIGU();
            buttonLeft.OnIGU();
            buttonRight.OnIGU();
        }

        private void InitEvents() {
            textField.OnStringChanged.AddListener(ModText);
            buttonLeft.OnRepeatClick.AddListener(() => ModValue(-additionValue));
            buttonRight.OnRepeatClick.AddListener(() => ModValue(additionValue));
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys) {
            (textField as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
            (buttonLeft as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
            (buttonRight as IIGUPhysics).CallPhysicsFeedback(mouse, ref phys);
        }

        private void ModValue(float value) {
            this.value += value;
            if (!string.IsNullOrEmpty(numberOfDecimalPlaces))
                textField.Text = this.value.ToString(numberOfDecimalPlaces, CultureInfo.CurrentCulture);
            else textField.Text = this.value.ToString(CultureInfo.CurrentCulture);
        }

        private void ModText(string text) {
            if (float.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out float value))
                this.value = value;
        }

        void IIGUToolTip.InternalDrawToolTip() 
            => (textField as IIGUToolTip).InternalDrawToolTip();

        void IIGUEndOfFrame.EndOfFrame() {
            (buttonLeft as IIGUEndOfFrame).EndOfFrame();
            (buttonRight as IIGUEndOfFrame).EndOfFrame();
        }
    }
}
