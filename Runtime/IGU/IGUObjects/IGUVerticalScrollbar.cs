using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalScrollbar : IGUSliderObject {
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

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.verticalScrollbar);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.VerticalScrollbar(rectTemp, isInt ? ValueToInt : value, scrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

            if (valuetemp != value) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedScrollbar.Invoke(value);
                    onModifiedScrollbarInt.Invoke((int)value);
                }
            }
        }

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float value, float scrollbarThumbSize, MaxMinSlider maxMin) {
            IGUVerticalScrollbar verticalScrollbar = Internal_CreateIGUInstance<IGUVerticalScrollbar>(name, value, maxMin);
            verticalScrollbar.maxMinSlider = maxMin;
            verticalScrollbar.myConfg = IGUConfig.Default;
            verticalScrollbar.myColor = IGUColor.DefaultBoxColor;
            verticalScrollbar.onModifiedScrollbar = new IGUOnSliderValueEvent();
            verticalScrollbar.onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            verticalScrollbar.scrollbarThumbSize = scrollbarThumbSize;
            return verticalScrollbar;
        }

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float scrollbarThumbSize, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, scrollbarThumbSize, maxMin);

        public static IGUVerticalScrollbar CreateIGUInstance(string name, float scrollbarThumbSize)
            => CreateIGUInstance(name, scrollbarThumbSize, MaxMinSlider.Default);

        public static IGUVerticalScrollbar CreateIGUInstance(string name)
            => CreateIGUInstance(name, 0);
    }
}
