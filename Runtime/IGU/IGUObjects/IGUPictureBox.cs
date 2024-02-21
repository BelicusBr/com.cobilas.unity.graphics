using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUPictureBox : IGUObject {
        [SerializeField] protected Texture texture;
        [SerializeField] protected bool alphaBlend;
        [SerializeField] protected float imageAspect;
        [SerializeField] protected ScaleMode scaleMode;
        [SerializeField] protected Vector4 borderWidths;
        [SerializeField] protected Vector4 borderRadiuses;

        public Texture MainTexture { get => texture; set => texture = value; }
        public bool AlphaBlend { get => alphaBlend; set => alphaBlend = value; }
        public float ImageAspect { get => imageAspect; set => imageAspect = value; }
        public ScaleMode MainScaleMode { get => scaleMode; set => scaleMode = value; }
        public Vector4 BorderWidths { get => borderWidths; set => borderWidths = value; }
        public Vector4 BorderRadiuses { get => borderRadiuses; set => borderRadiuses = value; }
        public float BorderWidth { get => borderWidths.Summation() / 4f; set => borderWidths = Vector4.one * value; }
        public float BorderRadius { get => borderRadiuses.Summation() / 4f; set => borderRadiuses = Vector4.one * value; }

        protected override void Ignition() {
            base.Ignition();
            myConfg = IGUConfig.Default;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            texture = Texture2D.whiteTexture;
            scaleMode = ScaleMode.StretchToFill;
            alphaBlend = true;
            imageAspect = 0f;
            borderWidths = Vector4.zero;
            borderRadiuses = Vector4.zero;
        }

        protected override void LowCallOnIGU() {
            
            GUI.DrawTexture(GetRect(), texture, scaleMode, alphaBlend, imageAspect, myColor.MyColor, borderWidths, borderRadiuses);
        }
    }
}
