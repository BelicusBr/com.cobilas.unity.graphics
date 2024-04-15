using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPictureBox : IGUObject {
        [SerializeField] protected Texture texture;
        [SerializeField] protected bool alphaBlend;
        [SerializeField] protected float imageAspect;
        [SerializeField] protected ScaleMode scaleMode;
        [SerializeField] protected Vector4 borderWidths;
        [SerializeField] protected Vector4 borderRadiuses;
        [SerializeField] protected IGUBasicPhysics physics;

        public Texture MainTexture { get => texture; set => texture = value; }
        public bool AlphaBlend { get => alphaBlend; set => alphaBlend = value; }
        public float ImageAspect { get => imageAspect; set => imageAspect = value; }
        public ScaleMode MainScaleMode { get => scaleMode; set => scaleMode = value; }
        public Vector4 BorderWidths { get => borderWidths; set => borderWidths = value; }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public Vector4 BorderRadiuses { get => borderRadiuses; set => borderRadiuses = value; }
        public float BorderWidth { get => borderWidths.Summation() / 4f; set => borderWidths = Vector4.one * value; }
        public float BorderRadius { get => borderRadiuses.Summation() / 4f; set => borderRadiuses = Vector4.one * value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            texture = Texture2D.whiteTexture;
            scaleMode = ScaleMode.StretchToFill;
            alphaBlend = true;
            imageAspect = 0f;
            borderWidths = Vector4.zero;
            borderRadiuses = Vector4.zero;
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.TextureBox(LocalRect, texture, scaleMode, AlphaBlend, imageAspect, myColor.MyColor,
                    borderWidths, borderRadiuses);
        }
    }
}
