using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private GUIStyle boxStyle;

        public GUIStyle BoxStyle { get => boxStyle; set => boxStyle = value; }

        protected override void Awake() {
            base.Awake();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            content = new IGUContent(DefaultIGUBox);
        }

        public override void OnIGU() {

            boxStyle = GetDefaultValue(boxStyle, GUI.skin.box);
            GUIContent mycontent = GetGUIContent(DefaultIGUBox);

            GUI.Box(GetRect(), mycontent, boxStyle);

            if (useTooltip)
                if (GetRect(true).Contains(Event.current.mousePosition))
                    DrawTooltip();
        }

        protected override void DrawTooltip()
            => base.DrawTooltip();

        protected override GUIContent GetGUIContent(string defaultGUIContent)
            => base.GetGUIContent(defaultGUIContent);
    }
}
