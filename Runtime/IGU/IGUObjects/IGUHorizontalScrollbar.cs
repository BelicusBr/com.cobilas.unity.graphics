using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalScrollbar : IGUSliderObject {
        [SerializeField] protected float scrollbarThumbSize;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedScrollbar;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedScrollbarInt;

        public IGUOnSliderValueEvent OnModifiedScrollbar => onModifiedScrollbar;
        public IGUOnSliderIntValueEvent OnModifiedScrollbarInt => onModifiedScrollbarInt;
        public float ScrollbarThumbSize { get => scrollbarThumbSize; set => scrollbarThumbSize = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            onModifiedScrollbar = new IGUOnSliderValueEvent();
            onModifiedScrollbarInt = new IGUOnSliderIntValueEvent();
            scrollbarThumbSize = 0f;
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = config.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.horizontalScrollbar);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.HorizontalScrollbar(rectTemp, isInt ? ValueToInt : value, scrollbarThumbSize, temp.Min, temp.Max, sliderObjectStyle);

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
