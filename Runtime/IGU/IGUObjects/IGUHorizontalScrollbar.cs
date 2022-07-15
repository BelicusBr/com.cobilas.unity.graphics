using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalScrollbar : IGUSliderObject {
        private float oldValue;
        [SerializeField] protected float horizontalScrollbarThumbSize;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float HorizontalScrollbarThumbSize { get => horizontalScrollbarThumbSize; set => horizontalScrollbarThumbSize = value; }

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

            value = GUI.HorizontalScrollbar(rectTemp, isInt ? ValueToInt : value, horizontalScrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

            if (oldValue != (oldValue = value)) {
                onModifiedScrollbar.Invoke(value);
                onModifiedScrollbarInt.Invoke((int)value);
            }
        }

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float value, float horizontalScrollbarThumbSize, MaxMinSlider maxMin) {
            IGUHorizontalScrollbar horizontalScrollbar = Internal_CreateIGUInstance<IGUHorizontalScrollbar>(name, value, maxMin);
            horizontalScrollbar.myConfg = IGUConfig.Default;
            horizontalScrollbar.myRect = IGURect.DefaultTextArea;
            horizontalScrollbar.myColor = IGUColor.DefaultBoxColor;
            horizontalScrollbar.onModifiedScrollbar = new IGUOnSliderValueEvent();
            horizontalScrollbar.onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            horizontalScrollbar.horizontalScrollbarThumbSize = horizontalScrollbarThumbSize;
            return horizontalScrollbar;
        }

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float horizontalScrollbarThumbSize, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, horizontalScrollbarThumbSize, maxMin);

        public static IGUHorizontalScrollbar CreateIGUInstance(string name, float horizontalScrollbarThumbSize)
            => CreateIGUInstance(name, horizontalScrollbarThumbSize, MaxMinSlider.Default);

        public static IGUHorizontalScrollbar CreateIGUInstance(string name)
            => CreateIGUInstance(name, 0);
    }
}
