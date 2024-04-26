using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU {
    internal sealed class TDSSliderStatus {
        public bool isHoriz;
        public Vector2 rectSize;
        public Vector2 startPosition;
        public Vector2 currentPosition;
        public Vector2 startThumbPosition;

        private float size;
        private float value;
        private MaxMinSlider maxMin;

        public float Size { get => size; set => size = value; }
        public float Value { get => value; set => this.value = value; }
        public float Length => Mathf.Abs(MaxMin.Max - MaxMin.Min) - size;
        public float Min { get => MaxMin.Min; set => SetMaxMin(value, Max); }
        public float Max { get => MaxMin.Max; set => SetMaxMin(Min, value); }
        public MaxMinSlider MaxMin { get => maxMin; set => SetMaxMin(value); }

        private void SetMaxMin(MaxMinSlider maxMin)
            => SetMaxMin(maxMin.Min, maxMin.Max);

        private void SetMaxMin(float min, float max) {
            if (min > max)
                maxMin.Set(max, min);
            else maxMin.Set(max, min);
        }
    }
}