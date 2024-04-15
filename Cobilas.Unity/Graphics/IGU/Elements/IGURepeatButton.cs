using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURepeatButton : IGUButton {
        public const string DefaultContentIGURepeatButton = "IGURepeatButton";
        
        private bool onClicked;
        [SerializeField] protected IGUOnClickEvent onRepeatClick;

        public override bool Clicked => clicked[0];
        public IGUOnClickEvent OnRepeatClick => onRepeatClick;
        
        protected override void IGUAwake() {
            base.IGUAwake();
            content = new IGUContent(DefaultContentIGURepeatButton);
            onRepeatClick = new IGUOnClickEvent();
        }

        protected override void LowCallOnIGU() {

            bool restemp = BackEndIGU.RepeatButton(LocalRect, MyContent, buttonStyle,
                physics, GetInstanceID(), out bool onCheckedTemp);

            Event @event = Event.current;

            if (restemp) {
                // if (IGUDrawer.GetMouseButtonPress(LocalConfig.MouseType))
                //     RepeatButtonClick();
                if (@event.type == EventType.MouseDown) {
                    if (@event.button == (int)LocalConfig.MouseType) {
                        onRepeatClick.Invoke();
                        if (onClicked = onCheckedTemp)
                            onClick.Invoke();
                    }
                }
            } 
            // else {
            //     clicked[0] = false;
            //     onClicked = true;
            // }
        }

        // private void RepeatButtonClick() {
        //     onRepeatClick.Invoke();
        //     clicked[0] = true;
        //     if (onClicked) {
        //         onClicked = false;
        //         onClick.Invoke();
        //     }
        // }

        protected override void DrawTooltip()
            => base.DrawTooltip();
    }
}
