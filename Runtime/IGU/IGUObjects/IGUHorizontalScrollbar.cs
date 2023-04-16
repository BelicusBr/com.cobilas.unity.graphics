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
        public IGUStyle SliderObjectThumbStyle { get => sliderObjectThumbStyle; set => sliderObjectThumbStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            sliderObjectStyle = IGUSkins.GetIGUStyle("Black horizontal scrollbar border");
            sliderObjectThumbStyle = IGUSkins.GetIGUStyle("Black horizontal scrollbar border thumb");
            onModifiedScrollbar = new IGUOnSliderValueEvent();
            onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            scrollbarThumbSize = 0f;
        }

        protected override void LowCallOnIGU() {

            GUIStyle style = IGUStyle.GetGUIStyleTemp(sliderObjectStyle, 0);
            GUIStyle style2 = IGUStyle.GetGUIStyleTemp(sliderObjectThumbStyle, 1);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            Rect rect = GetRect();

            float valuetemp = GUI.Slider(rect, isInt ? ValueToInt : value, scrollbarThumbSize,
                temp.Min, temp.Max, style, style2, true, GUIUtility.GetControlID(FocusType.Passive, rect));

            if (valuetemp != value) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedScrollbar.Invoke(value);
                    onModifiedScrollbarInt.Invoke((int)value);
                }
            }
        }
    }
}
