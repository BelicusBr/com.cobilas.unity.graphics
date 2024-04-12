using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalSlider : IGUSliderObject {
        [SerializeField] protected IGUStyle verticalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public IGUStyle VerticalSliderThumb { 
            get => verticalSliderThumb;
            set => verticalSliderThumb = value ?? (IGUStyle)"Black vertical slider border thumb";
        }
        public override IGUBasicPhysics Physics { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override void IGUAwake() {
            base.IGUAwake();
            myColor = IGUColor.DefaultBoxColor;
            myRect = myRect.SetSize(25f, 130f);
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
            sliderObjectStyle = (IGUStyle)"Black vertical slider border";
            verticalSliderThumb = (IGUStyle)"Black vertical slider border thumb";
        }

        protected override void LowCallOnIGU() {
            sliderObjectStyle = sliderObjectStyle ?? (IGUStyle)"Black vertical slider border";

            MaxMinSlider temp = isInt? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = BackEndIGU.Slider(LocalRect, isInt ? ValueToInt : value, temp, GetInstanceID(),
                    IGUNonePhysics.None, sliderObjectStyle, verticalSliderThumb, false);

            if (valuetemp != value)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
