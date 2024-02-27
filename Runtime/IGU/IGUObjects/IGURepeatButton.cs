using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

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

            bool restemp = BackEndIGU.RepeatButton(LocalRect, MyContent, buttonStyle);

            if (restemp) {
                if (IGUDrawer.Drawer.GetMouseButton(LocalConfig.MouseType))
                    RepeatButtonClick();
            } else {
                clicked[0] = false;
                onClicked = true;
            }
        }

        private void RepeatButtonClick() {
            onRepeatClick.Invoke();
            clicked[0] = true;
            if (onClicked) {
                onClicked = false;
                onClick.Invoke();
            }
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();
    }
}
