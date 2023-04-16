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

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            sliderObjectStyle = IGUSkins.GetIGUStyle("Black horizontal slider border");
            horizontalSliderThumb = IGUSkins.GetIGUStyle("Black horizontal slider border thumb");
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
        }

        protected override void LowCallOnIGU() {

            GUIStyle style = IGUStyle.GetGUIStyleTemp(sliderObjectStyle, 0);
            GUIStyle style2 = IGUStyle.GetGUIStyleTemp(horizontalSliderThumb, 1);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.HorizontalSlider(GetRect(), isInt ? ValueToInt : value, temp.Min, temp.Max, style, style2);

            if (valuetemp != value)
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
