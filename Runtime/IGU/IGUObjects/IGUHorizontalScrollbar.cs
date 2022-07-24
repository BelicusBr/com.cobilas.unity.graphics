using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalScrollbar : IGUSliderObject {
        private float oldValue;
        [SerializeField] protected float scrollbarThumbSize;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float ScrollbarThumbSize { get => scrollbarThumbSize; set => scrollbarThumbSize = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.horizontalScrollbar);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            value = GUI.HorizontalScrollbar(rectTemp, isInt ? ValueToInt : value, scrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

            if (oldValue != (oldValue = value)) {
                onModifiedScrollbar.Invoke(value);
                onModifiedScrollbarInt.Invoke((int)value);
            }
        }

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float value, float scrollbarThumbSize, MaxMinSlider maxMin) {
            IGUHorizontalScrollbar horizontalScrollbar = Internal_CreateIGUInstance<IGUHorizontalScrollbar>(name, value, maxMin);
            horizontalScrollbar.myConfg = IGUConfig.Default;
            horizontalScrollbar.myRect = IGURect.DefaultTextArea;
            horizontalScrollbar.myColor = IGUColor.DefaultBoxColor;
            horizontalScrollbar.onModifiedScrollbar = new IGUOnSliderValueEvent();
            horizontalScrollbar.onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            horizontalScrollbar.scrollbarThumbSize = scrollbarThumbSize;
            return horizontalScrollbar;
        }

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float scrollbarThumbSize, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, scrollbarThumbSize, maxMin);

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float scrollbarThumbSize)
            => CreateIGUInstance(name, scrollbarThumbSize, MaxMinSlider.Default);

        public static IGUHorizontalScrollbar CreateIGUInstance(string name)
            => CreateIGUInstance(name, 0);
    }
}
