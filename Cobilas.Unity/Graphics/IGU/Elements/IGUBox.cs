using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] protected IGUStyle boxStyle;
        [SerializeField] protected IGUBasicPhysics physics;

        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public IGUStyle BoxStyle { get => boxStyle; set => boxStyle = value ?? (IGUStyle)"Black box border"; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            boxStyle = (IGUStyle)"Black box border";
            content = new IGUContent(DefaultIGUBox);
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
        }

        protected override void LowCallOnIGU() {
            boxStyle.RichText = richText;
            BackEndIGU.Label(LocalRect, MyContent, boxStyle, GetInstanceID());
        }
    }
}
