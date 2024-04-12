using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBox : IGUTextObject {
        public const string DefaultIGUBox = "IGU Box";
        [SerializeField] private IGUStyle boxStyle;
        [SerializeField] private IGUBoxPhysics boxPhysics;

        public IGUStyle BoxStyle { get => boxStyle; set => boxStyle = value ?? (IGUStyle)"Black box border"; }
        public override IGUBasicPhysics Physics { get => boxPhysics; set => boxPhysics = (IGUBoxPhysics)value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            boxPhysics = new IGUBoxPhysics(this);
            boxStyle = (IGUStyle)"Black box border";
            content = new IGUContent(DefaultIGUBox);
        }

        protected override void LowCallOnIGU() {
            boxStyle.RichText = richText;
            BackEndIGU.Label(LocalRect, MyContent, boxStyle, GetInstanceID());
        }
    }
}
