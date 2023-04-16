using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalScrollbar : IGUSliderObject {
        [SerializeField] protected float scrollbarThumbSize;
        [SerializeField] protected GUIStyle sliderObjectThumbStyle;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float ScrollbarThumbSize { get => scrollbarThumbSize; set => scrollbarThumbSize = value; }
        public GUIStyle SliderObjectThumbStyle { get => sliderObjectThumbStyle; set => sliderObjectThumbStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            onModifiedScrollbar = new IGUOnSliderValueEvent();
            onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            scrollbarThumbSize = 0f;
        }

        public override void OnIGU() {

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.horizontalScrollbar);
            sliderObjectThumbStyle = GetDefaultValue(sliderObjectThumbStyle, GUI.skin.horizontalScrollbarThumb);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            Rect rect = GetRect();

            float valuetemp = GUI.Slider(rect, isInt ? ValueToInt : value, scrollbarThumbSize,
                temp.Min, temp.Max, sliderObjectStyle, sliderObjectThumbStyle, true,
                GUIUtility.GetControlID(FocusType.Passive, rect));

            //float valuetemp = GUI.HorizontalScrollbar(GetRect(), isInt ? ValueToInt : value, scrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

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
