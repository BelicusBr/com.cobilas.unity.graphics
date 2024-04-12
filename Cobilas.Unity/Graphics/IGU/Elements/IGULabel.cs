using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabel : IGUTextObject {
        public const string DefaultIGULabel = "IGU Label";
        [SerializeField] private bool autoSize;
        [SerializeField] private IGUStyle labelStyle;

        public bool AutoSize { get => autoSize; set => autoSize = value; }
        public IGUStyle LabelStyle { get => labelStyle; set => labelStyle = value ?? (IGUStyle)"Label"; }
        public override IGUBasicPhysics Physics { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoSize = false;
            myRect = IGURect.DefaultButton;
            labelStyle = (IGUStyle)"Label";
            myColor = IGUColor.DefaultLabelColor;
            content = new IGUContent(DefaultIGULabel);
        }

        protected override void LowCallOnIGU() {
            labelStyle.RichText = richText;
            GUIStyle style = (GUIStyle)labelStyle;
            GUIContent content = GetGUIContentTemp(MyContent);

            myRect = myRect.SetSize(autoSize ? style.CalcSize(content) + Vector2.right * 2f : myRect.Size);

            BackEndIGU.Label(LocalRect, MyContent, labelStyle, GetInstanceID());
        }
    }
}
