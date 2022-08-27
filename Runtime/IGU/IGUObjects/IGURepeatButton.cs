using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURepeatButton : IGUButton {
        public const string DefaultContentIGURepeatButton = "IGURepeatButton";
        [SerializeField] protected IGUOnClickEvent onRepeatClick;
        private bool onClicked;

        public override bool Clicked => clicked[0];

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.color = myColor.MyColor;
            GUI.enabled = myConfg.IsEnabled;
            GUI.contentColor = myColor.TextColor;
            GUI.backgroundColor = myColor.BackgroundColor;

            buttonStyle = GetDefaultValue(buttonStyle, GUI.skin.button);
            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            Event current = IGUDrawer.IGUEvent;

            bool restemp = GUI.RepeatButton(rectTemp, GetGUIContent(DefaultContentIGURepeatButton), buttonStyle);

            if (restemp) {
                if (IGUDrawer.Drawer.GetMouseButton(myConfg.MouseType)) RepeatButtonClick();
            } else {
                clicked[0] = false;
                onClicked = true;
            }

            if (useTooltip)
                if (rectTemp.Contains(current.mousePosition))
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

        protected override void Reset() { }

        public new static IGURepeatButton CreateIGUInstance(string name, IGUContent content) {
            IGURepeatButton button = Internal_CreateIGUInstance<IGURepeatButton>(name, content);
            button.clicked = new bool[2];
            button.myConfg = IGUConfig.Default;
            button.myRect = IGURect.DefaultButton;
            button.onClick = new IGUOnClickEvent();
            button.myColor = IGUColor.DefaultBoxColor;
            button.onRepeatClick = new IGUOnClickEvent();
            (button as ISerializationCallbackReceiver).OnAfterDeserialize();
            return button;
        }

        public new static IGURepeatButton CreateIGUInstance(string name, string text)
            => CreateIGUInstance(name, new IGUContent(text));

        public new static IGURepeatButton CreateIGUInstance(string name)
            => CreateIGUInstance(name, DefaultContentIGURepeatButton);
    }
}
