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

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myColor = IGUColor.DefaultBoxColor;
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
        }

        public override void OnIGU() {

            sliderObjectStyle = GetDefaultValue(sliderObjectStyle, GUI.skin.horizontalSlider);
            horizontalSliderThumb = GetDefaultValue(horizontalSliderThumb, GUI.skin.horizontalSliderThumb);

            MaxMinSlider temp = isInt ? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = GUI.HorizontalSlider(GetRect(), isInt ? ValueToInt : value, temp.Min, temp.Max, sliderObjectStyle, horizontalSliderThumb);

            if (valuetemp != value)
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
