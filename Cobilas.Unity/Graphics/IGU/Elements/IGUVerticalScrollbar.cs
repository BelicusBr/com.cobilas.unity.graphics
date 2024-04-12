using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalScrollbar : IGUSliderObject {
        [SerializeField] protected float scrollbarThumbSize;
        [SerializeField] protected IGUStyle sliderObjectThumbStyle;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float ScrollbarThumbSize { get => scrollbarThumbSize; set => scrollbarThumbSize = value; }
        public IGUStyle SliderObjectThumbStyle { 
            get => sliderObjectThumbStyle;
            set => sliderObjectThumbStyle = value ?? (IGUStyle)"Black vertical scrollbar border thumb";
        }
        public override IGUBasicPhysics Physics { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override void IGUAwake() {
            base.IGUAwake();
            scrollbarThumbSize = 0f;
            myColor = IGUColor.DefaultBoxColor;
            myRect = myRect.SetSize(25f, 130f);
            onModifiedScrollbar = new IGUOnSliderValueEvent();
            onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            sliderObjectStyle = (IGUStyle)"Black vertical scrollbar border";
            sliderObjectThumbStyle = (IGUStyle)"Black vertical scrollbar border thumb";
        }

        protected override void LowCallOnIGU() {

            sliderObjectStyle = sliderObjectStyle ?? (IGUStyle)"Black vertical scrollbar border";

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max - scrollbarThumbSize);

            float valuetemp = BackEndIGU.Slider(LocalRect, isInt ? ValueToInt : value, scrollbarThumbSize, temp, 
                GetInstanceID(), IGUNonePhysics.None, sliderObjectStyle, sliderObjectThumbStyle, false);

            if (valuetemp != value)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) {
                    value = valuetemp;
                    onModifiedScrollbar.Invoke(value);
                    onModifiedScrollbarInt.Invoke((int)value);
                }
        }
    }
}
