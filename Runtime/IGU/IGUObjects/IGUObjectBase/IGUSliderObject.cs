using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public abstract class IGUSliderObject : IGUObject {
        [SerializeField] protected bool isInt;
        [SerializeField] protected float value;
        [SerializeField] protected MaxMinSlider maxMinSlider;
        [SerializeField] protected GUIStyle sliderObjectStyle;

        /// <summary>Permite o controle <see cref="IGUSliderObject"/> utilizar valores <see cref="int"/>.</summary>
        public bool IsInt { get => isInt; set => isInt = value; }
        public float Value { get => value; set => this.value = value; }
        public int ValueToInt { get => (int)value; set => this.value = value; }
        public MaxMinSlider MaxMinValue { get => maxMinSlider; set => maxMinSlider = value; }
        public GUIStyle SliderObjectStyle { get => sliderObjectStyle; set => sliderObjectStyle = value; }

        protected override void Awake() {
            base.Awake();
            myRect = IGURect.DefaultSlider;
            value = 0f;
            isInt = false;
            maxMinSlider = new MaxMinSlider(0f, 30f);
        }

        public override void OnIGU() => base.OnIGU();
        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();
        protected override void OnIGUDestroy() => base.OnIGUDestroy();
    }
}
