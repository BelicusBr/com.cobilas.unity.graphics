using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Graphics.IGU {
    internal sealed class SliderStatus {
            public bool isHoriz;
            public Vector2 rectSize;
            public Vector2 startedPosition;
            public Vector2 currentPosition;
            public Vector2 startedThumbPosition;
            private float min;
            private float max;
            private float size;
            private float value;
            private float normalSize;
            private Vector2 thumbPosX;
            private Vector2 thumbPosY;
            private Vector2 posCompare;

            public float Min => min;
            public float Max => max - size;
            public float NormalSize => normalSize;
            public float MinMaxLength => (float)Math.Abs(max - min);
            public float Size { get => size; set => SetSize(value); }
            public float Value { get => value; set => SetValue(value); }

            public void SetMinMax(float min, float max) {
                if (min > max) {
                    this.max = min;
                    this.min = max;
                } else {
                    this.min = min;
                    this.max = max;
                }
            }

            public void SetMinMax(MaxMinSlider maxMin)
                => SetMinMax(maxMin.Min, maxMin.Max);

            public void PlaceThumbOnMouse(Vector2 mousePosition)
                => CalculatorThumbPosition(isHoriz ? mousePosition.Multiplication(Vector2.right) : mousePosition.Multiplication(Vector2.up));

            public void CalculatorThumbPosition()
                => CalculatorThumbPosition(startedThumbPosition + (currentPosition - startedPosition));

            public Vector2 GetThumbPosition() => isHoriz ? thumbPosX : thumbPosY;

            private void CalculatorThumbPosition(Vector2 position) {
                if (posCompare == (posCompare = position)) return;
                Vector2 length = (Vector2.one * (MinMaxLength - size)).Division(rectSize);
                posCompare = posCompare.Multiplication(length).Division(rectSize.Multiplication(length));
                posCompare = posCompare.Multiplication(Vector2.one * (MinMaxLength - size)) + Vector2.one * min;
                Value = isHoriz ? posCompare.x : posCompare.y;
            }

            private void SetValue(float value) {
                if (this.value == (this.value = value)) return;
                thumbPosX = rectSize.Multiplication(Vector2.right) * (Mathf.Abs(this.value - min) / (MinMaxLength - size));
                thumbPosY = rectSize.Multiplication(Vector2.up) * (Mathf.Abs(this.value - min) / (MinMaxLength - size));
            }

            private void SetSize(float size) {
                if (this.size == size) return;
                this.size = Mathf.Clamp(size, 0f, MinMaxLength);
                normalSize = this.size / MinMaxLength;
            }
    }
}