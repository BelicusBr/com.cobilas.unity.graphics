using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUVerticalSlider : IGUSliderObject {
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUStyle verticalSliderThumb;
        [SerializeField] protected IGUOnSliderValueEvent onModifiedSlider;
        [SerializeField] protected IGUOnSliderIntValueEvent onModifiedSliderInt;

        public IGUOnSliderValueEvent OnModifiedSlider => onModifiedSlider;
        public IGUOnSliderIntValueEvent OnModifiedSliderInt => onModifiedSliderInt;
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public IGUStyle VerticalSliderThumb { 
            get => verticalSliderThumb;
            set => verticalSliderThumb = value ?? (IGUStyle)"Black vertical slider border thumb";
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            myColor = IGUColor.DefaultBoxColor;
            myRect = myRect.SetSize(25f, 130f);
            onModifiedSlider = new IGUOnSliderValueEvent();
            onModifiedSliderInt = new IGUOnSliderIntValueEvent();
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            sliderObjectStyle = (IGUStyle)"Black vertical slider border";
            verticalSliderThumb = (IGUStyle)"Black vertical slider border thumb";
        }

        protected override void LowCallOnIGU() {
            sliderObjectStyle = sliderObjectStyle ?? (IGUStyle)"Black vertical slider border";

            MaxMinSlider temp = isInt? maxMinSlider.ToMaxMinSliderInt() : maxMinSlider;
            value = Mathf.Clamp(value, temp.Min, temp.Max);

            float valuetemp = BackEndIGU.Slider(LocalRect, isInt ? ValueToInt : value, temp, GetInstanceID(),
                    physics, sliderObjectStyle, verticalSliderThumb, false);

            if (valuetemp != value)
                if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType)) {
                    value = valuetemp;
                    onModifiedSlider.Invoke(value);
                    onModifiedSliderInt.Invoke((int)value);
                }
        }
    }
}
