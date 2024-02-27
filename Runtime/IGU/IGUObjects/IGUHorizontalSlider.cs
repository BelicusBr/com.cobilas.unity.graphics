using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalSlider : IGUSliderObject {
        [SerializeField] protected IGUStyle horizontalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public IGUStyle HorizontalSliderThumb { get => horizontalSliderThumb; set => horizontalSliderThumb = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myColor = IGUColor.DefaultBoxColor;
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
            sliderObjectStyle = (IGUStyle)"Black horizontal slider border";
            horizontalSliderThumb = (IGUStyle)"Black horizontal slider border thumb";
        }

        protected override void LowCallOnIGU() {

            sliderObjectStyle = sliderObjectStyle ?? (IGUStyle)"Black horizontal slider border";

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = BackEndIGU.Slider(LocalRect, isInt ? ValueToInt : value, temp, true,
                    sliderObjectStyle, horizontalSliderThumb);

            if (valuetemp != value)
                if (IGUDrawer.Drawer.GetMouseButton(LocalConfig.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
