using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalSlider : IGUSliderObject {
        [SerializeField] protected IGUStyle verticalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public IGUStyle VerticalSliderThumb { get => verticalSliderThumb; set => verticalSliderThumb = value; }

        protected override void Ignition() {
            base.Ignition();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            sliderObjectStyle = IGUSkins.GetIGUStyle("Black vertical slider border");
            verticalSliderThumb = IGUSkins.GetIGUStyle("Black vertical slider border thumb");
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
        }

        protected override void LowCallOnIGU() {

            GUIStyle style = IGUStyle.GetGUIStyleTemp(sliderObjectStyle, 0);
            GUIStyle style2 = IGUStyle.GetGUIStyleTemp(verticalSliderThumb, 1);

            MaxMinSlider temp = isInt? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.VerticalSlider(GetRect(), isInt ? ValueToInt : value, temp.Min, temp.Max, style, style2);

            if (valuetemp != value)
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
