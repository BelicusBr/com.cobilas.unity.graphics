using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUSliderObject : IGUObject {
        [SerializeField] protected bool isInt;
        [SerializeField] protected float value;
        [SerializeField] protected MaxMinSlider maxMinSlider;
        [SerializeField] protected IGUStyle sliderObjectStyle;

        public bool IsInt { get => isInt; set => isInt = value; }
        public float Value { get => value; set => this.value = value; }
        public int ValueToInt { get => (int)value; set => this.value = value; }
        public MaxMinSlider MaxMinValue { get => maxMinSlider; set => maxMinSlider = value; }
        public IGUStyle SliderObjectStyle { get => sliderObjectStyle; set => sliderObjectStyle = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultSlider;
            value = 0f;
            isInt = false;
            maxMinSlider = new MaxMinSlider(0f, 30f);
        }

        protected override void IGUStart() => base.IGUStart();
        protected override void PreOnIGU() => base.PreOnIGU();
        protected override void PostOnIGU() => base.PostOnIGU();
        protected override void IGUOnEnable() => base.IGUOnEnable();
        protected override void IGUOnDisable() => base.IGUOnDisable();
        protected override void LowCallOnIGU() => base.LowCallOnIGU();
        protected override void IGUOnDestroy() => base.IGUOnDestroy();
    }
}
