using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalScrollbar : IGUSliderObject {
        private float oldValue;
        [SerializeField] protected float verticalScrollbarThumbSize;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float VerticalScrollbarThumbSize { get => verticalScrollbarThumbSize; set => verticalScrollbarThumbSize = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.verticalScrollbar);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            value = GUI.VerticalScrollbar(rectTemp, isInt ? ValueToInt : value, verticalScrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

            if (oldValue != (oldValue = value)) {
                onModifiedScrollbar.Invoke(value);
                onModifiedScrollbarInt.Invoke((int)value);
            }
        }

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float value, float verticalScrollbarThumbSize, MaxMinSlider maxMin) {
            IGUVerticalScrollbar verticalScrollbar = Internal_CreateIGUInstance<IGUVerticalScrollbar>(name, value, maxMin);
            verticalScrollbar.maxMinSlider = maxMin;
            verticalScrollbar.myConfg = IGUConfig.Default;
            verticalScrollbar.myRect = IGURect.DefaultTextArea;
            verticalScrollbar.myColor = IGUColor.DefaultBoxColor;
            verticalScrollbar.onModifiedScrollbar = new IGUOnSliderValueEvent();
            verticalScrollbar.onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            verticalScrollbar.verticalScrollbarThumbSize = verticalScrollbarThumbSize;
            return verticalScrollbar;
        }

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float verticalScrollbarThumbSize, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, verticalScrollbarThumbSize, maxMin);

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float verticalScrollbarThumbSize)
            => CreateIGUInstance(name, verticalScrollbarThumbSize, MaxMinSlider.Default);

        public static IGUVerticalScrollbar CreateIGUInstance(string name)
            => CreateIGUInstance(name, 0);
    }
}
