using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    internal sealed class SliderStatus {
        public bool isHoriz;
        public Vector2 rectSize;
        public Vector2 startPosition;
        public Vector2 startThumbPosition;

        private float size;
        private float value;
        private MaxMinSlider maxMin;

        public float Size { get => size; set => size = value; }
        public MaxMinSlider MaxMin { get => maxMin; set => SetMaxMin(value); }
        public float Min { get => MaxMin.Min; set => SetMaxMin(value, MaxMin.Max); }
        public float Max { get => MaxMin.Max; set => SetMaxMin(MaxMin.Min, value); }
        public float Value { get => Mathf.Clamp(value, MaxMin.Min, MaxMin.Max - size); set => this.value = value; }

        public void SetMaxMin(MaxMinSlider maxMin)
            => SetMaxMin(maxMin.Min, maxMin.Max);

        public void SetMaxMin(float min, float max) {
            if (min > max)
                maxMin.Set(max, min);
            else maxMin.Set(min, max);
        }

        public void CalculatorThumbPosition(Vector2 mousePosition, Vector2 thumbSize, Rect rect) {
            mousePosition = (rect.position + thumbSize * .5f - mousePosition).Invert();
            Vector2 sizeMod = mousePosition / (rect.size - thumbSize);
            float leng = Mathf.Abs(MaxMin.Max - MaxMin.Min) - size;
            value = MaxMin.Min + leng * (isHoriz ? sizeMod.x : sizeMod.y);
        }

        public Vector2 GetThumbPosition() {
            float val = Mathf.Abs(Value - maxMin.Min);
            float leng = Mathf.Abs(MaxMin.Max - MaxMin.Min) - size;
            return startThumbPosition + (isHoriz ? Vector2.right * rectSize.x : Vector2.up * rectSize.y) * (val / leng);
        }
    }
}