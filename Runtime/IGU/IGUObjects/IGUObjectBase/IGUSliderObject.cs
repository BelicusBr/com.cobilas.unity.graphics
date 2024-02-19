using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUSliderObject : IGUObject {
        [SerializeField] protected bool isInt;
        [SerializeField] protected float value;
        [SerializeField] protected MaxMinSlider maxMinSlider;
        [SerializeField] protected IGUStyle sliderObjectStyle;

        /// <summary>Permite o controle <see cref="IGUSliderObject"/> utilizar valores <see cref="int"/>.</summary>
        public bool IsInt { get => isInt; set => isInt = value; }
        public float Value { get => value; set => this.value = value; }
        public int ValueToInt { get => (int)value; set => this.value = value; }
        public MaxMinSlider MaxMinValue { get => maxMinSlider; set => maxMinSlider = value; }
        public IGUStyle SliderObjectStyle { get => sliderObjectStyle; set => sliderObjectStyle = value; }

        protected override void Ignition() {
            base.Ignition();
            myRect = IGURect.DefaultSlider;
            value = 0f;
            isInt = false;
            maxMinSlider = new MaxMinSlider(0f, 30f);
        }

        protected override void LowCallOnIGU() => base.LowCallOnIGU();
        protected override void IgnitionEnable() => base.IgnitionEnable();
        protected override void IgnitionDisable() => base.IgnitionDisable();
        protected override void DestroyIgnition() => base.DestroyIgnition();
    }
}
