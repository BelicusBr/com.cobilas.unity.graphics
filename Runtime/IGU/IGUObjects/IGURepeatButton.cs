using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURepeatButton : IGUButton {
        public const string DefaultContentIGURepeatButton = "IGURepeatButton";
        [SerializeField] protected IGUOnClickEvent onRepeatClick;
        private bool onClicked;

        public override bool Clicked => clicked[0];
        protected override void Awake() {
            base.Awake();
            content = new IGUContent(DefaultContentIGURepeatButton);
            onRepeatClick = new IGUOnClickEvent();
        }

        protected override void LowCallOnIGU() {

            bool restemp = GUI.RepeatButton(GetRect(), GetGUIContent(DefaultContentIGURepeatButton),
                IGUStyle.GetGUIStyleTemp(buttonStyle));

            if (restemp) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) RepeatButtonClick();
            } else {
                clicked[0] = false;
                onClicked = true;
            }

            if (useTooltip)
                if (GetRect(true).Contains(IGUDrawer.Drawer.GetMousePosition()))
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

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);
    }
}
