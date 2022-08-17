using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUHorizontalSlider : IGUSliderObject {
        [SerializeField] protected GUIStyle horizontalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public GUIStyle HorizontalSliderThumb { get => horizontalSliderThumb; set => horizontalSliderThumb = value; }

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.horizontalSlider);
            horizontalSliderThumb = GetDefaultValue(horizontalSliderThumb, GUI.skin.horizontalSliderThumb);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.HorizontalSlider(rectTemp, isInt ? ValueToInt : value, temp.Min, temp.Max, sliderObjectStyle, horizontalSliderThumb);

            if (valuetemp != value) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
            }
        }

        public static IGUHorizontalSlider CreateIGUInstance(string name, float value, MaxMinSlider maxMin) {
            IGUHorizontalSlider horizontalSlider = Internal_CreateIGUInstance<IGUHorizontalSlider>(name, value, maxMin);
            horizontalSlider.myConfg = IGUConfig.Default;
            horizontalSlider.myColor = IGUColor.DefaultBoxColor;
            horizontalSlider.onModifiedSlider = new IGUOnSliderValueEvent();
            horizontalSlider.onModifiedSliderInt = new IGUOnSliderIntValueEvent();
            return horizontalSlider;
        }

        public static IGUHorizontalSlider CreateIGUInstance(string name, MaxMinSlider maxMin)
            => CreateIGUInstance(name, 0f, maxMin);

        public static IGUHorizontalSlider CreateIGUInstance(string name)
            => CreateIGUInstance(name, MaxMinSlider.Default);
    }
}
