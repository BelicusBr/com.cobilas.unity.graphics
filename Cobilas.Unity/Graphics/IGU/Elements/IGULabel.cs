using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabel : IGUTextObject {
        public const string DefaultIGULabel = "IGU Label";
        [SerializeField] protected bool autoSize;
        [SerializeField] protected IGUStyle labelStyle;
        [SerializeField] protected IGUBasicPhysics physics;

        public bool AutoSize { get => autoSize; set => autoSize = value; }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public IGUStyle LabelStyle { get => labelStyle; set => labelStyle = value ?? (IGUStyle)"Label"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoSize = false;
            myRect = IGURect.DefaultButton;
            labelStyle = (IGUStyle)"Label";
            myColor = IGUColor.DefaultLabelColor;
            content = new IGUContent(DefaultIGULabel);
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
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
