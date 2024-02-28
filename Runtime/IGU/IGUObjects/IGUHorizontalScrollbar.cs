using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalScrollbar : IGUSliderObject {
        [SerializeField] protected float scrollbarThumbSize;
        [SerializeField] protected IGUStyle sliderObjectThumbStyle;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float ScrollbarThumbSize { get => scrollbarThumbSize; set => scrollbarThumbSize = value; }
        public IGUStyle SliderObjectThumbStyle { 
            get => sliderObjectThumbStyle;
            set => sliderObjectThumbStyle = value ?? (IGUStyle)"Black horizontal scrollbar border thumb";
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            scrollbarThumbSize = 0f;
            myColor = IGUColor.DefaultBoxColor;
            onModifiedScrollbar = new IGUOnSliderValueEvent();
            onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            sliderObjectStyle = (IGUStyle)"Black horizontal scrollbar border";
            sliderObjectThumbStyle = (IGUStyle)"Black horizontal scrollbar border thumb";
        }

        protected override void LowCallOnIGU() {

            sliderObjectStyle = sliderObjectStyle ?? (IGUStyle)"Black horizontal scrollbar border";

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max - scrollbarThumbSize);

            float valuetemp = BackEndIGU.Slider(LocalRect, isInt ? ValueToInt : value, scrollbarThumbSize, temp, true,
                     sliderObjectStyle, sliderObjectThumbStyle);

            if (valuetemp != value) {
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) {
                    value = valuetemp;
                    onModifiedScrollbar.Invoke(value);
                    onModifiedScrollbarInt.Invoke((int)value);
                }
            }
        }
    }
}
