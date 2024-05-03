using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public static class TDSBackEndIGU {
        public static float Slider(IGURect rect, float value, float size, MaxMinSlider maxMin, int id, bool useScrollWheel, bool isHoriz, GUIStyle slider, GUIStyle thumb) {
            Rect rect_slider = (Rect)rect;
            Event current = Event.current;
            Rect rect_slider_thumb = Rect.zero;
            TDSSliderStatus sliderStatus = (TDSSliderStatus)GUIUtility.GetStateObject(typeof(TDSSliderStatus), id);

            sliderStatus.Size = size;
            sliderStatus.Value = value;
            sliderStatus.MaxMin = maxMin;
            sliderStatus.isHoriz = isHoriz;

            float sizeClamp = (isHoriz ? rect_slider.width : rect_slider.height) -
                ((isHoriz ? slider.padding.horizontal : slider.padding.vertical) +
                (isHoriz ? thumb.padding.left : thumb.padding.top));

            sliderStatus.Size = sliderStatus.Size > sizeClamp ? sizeClamp : sliderStatus.Size;

            Vector2 snum1 = (isHoriz ? Vector2.right * (size - thumb.padding.left) : Vector2.up * (size - thumb.padding.top)).Clamp(0f, size);
            Vector2 snum2 = Vector2.right * thumb.padding.horizontal + Vector2.up * thumb.padding.vertical;

            rect_slider_thumb.position = rect_slider.position +
                Vector2.right * (isHoriz ? thumb.margin.right + slider.padding.right : thumb.margin.left + slider.padding.left) + 
                Vector2.up * (isHoriz ? thumb.margin.bottom + slider.padding.bottom : thumb.margin.top + slider.padding.top);
            rect_slider_thumb.size = snum1 + snum2;

            sliderStatus.rectSize = rect_slider.size - rect_slider_thumb.size -
                (isHoriz ? Vector2.right * slider.padding.horizontal : Vector2.up * slider.padding.vertical);
            sliderStatus.startThumbPosition = rect_slider_thumb.position;

            bool isHover = rect.Contains(current.mousePosition);

            switch (current.type) {
                case EventType.MouseDown:
                    if (isHover) {
                        GUIUtility.hotControl = id;
                        sliderStatus.startPosition = current.mousePosition;
                        sliderStatus.CalculatorThumbPosition(current.mousePosition, rect_slider_thumb.size, rect_slider);
                        current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id) {
                        GUIUtility.hotControl = 0;
                        current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != id) break;
                    sliderStatus.CalculatorThumbPosition(current.mousePosition, rect_slider_thumb.size, rect_slider);
                    current.Use();
                    break;
                case EventType.ScrollWheel:
                    if (isHover && useScrollWheel) {
                        sliderStatus.Value += current.delta.y;
                        current.Use();
                    }
                    break;
                case EventType.Repaint:
                    rect_slider_thumb.position = sliderStatus.GetThumbPosition();
                    slider.Draw(rect_slider, GUIContent.none, id);
                    thumb.Draw(rect_slider_thumb, rect_slider_thumb.Contains(current.mousePosition), 
                        GUIUtility.hotControl == id, false, false);
                    break;
            }

            return sliderStatus.Value;
        }
    }

    internal sealed class TDSSliderStatus {
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
