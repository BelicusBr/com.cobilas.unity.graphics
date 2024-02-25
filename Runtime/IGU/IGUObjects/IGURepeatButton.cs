using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURepeatButton : IGUButton {
        public const string DefaultContentIGURepeatButton = "IGURepeatButton";
        [SerializeField] protected IGUOnClickEvent onRepeatClick;
        private bool onClicked;

        public override bool Clicked => clicked[0];
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

            if (useTooltip)
                if (LocalRect.ModifiedRect.Contains(IGUDrawer.Drawer.GetMousePosition()))
                    DrawTooltip();
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
