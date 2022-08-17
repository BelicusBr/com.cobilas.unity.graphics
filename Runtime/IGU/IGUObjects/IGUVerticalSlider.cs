using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalSlider : IGUSliderObject {
        [SerializeField] protected GUIStyle verticalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public GUIStyle VerticalSliderThumb { get => verticalSliderThumb; set => verticalSliderThumb = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.verticalSlider);
            verticalSliderThumb = GetDefaultValue(verticalSliderThumb, GUI.skin.verticalSliderThumb);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.VerticalSlider(rectTemp, isInt ? ValueToInt : value, temp.Min, temp.Max, sliderObjectStyle, verticalSliderThumb);

            if (valuetemp != value) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
            }
        }

        public static IGUVerticalSlider CreateIGUInstance(string name, float value, MaxMinSlider maxMin) {
            IGUVerticalSlider verticalSlider = Internal_CreateIGUInstance<IGUVerticalSlider>(name, value, maxMin);
            verticalSlider.maxMinSlider = maxMin;
            verticalSlider.myConfg = IGUConfig.Default;
            verticalSlider.myColor = IGUColor.DefaultBoxColor;
            verticalSlider.onModifiedSlider = new IGUOnSliderValueEvent();
            verticalSlider.onModifiedSliderInt = new IGUOnSliderIntValueEvent();
            return verticalSlider;
        }

        public static IGUVerticalSlider CreateIGUInstance(string name, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, maxMin);

        public static IGUVerticalSlider CreateIGUInstance(string name)
            => CreateIGUInstance(name, MaxMinSlider.Default);
    }
}
