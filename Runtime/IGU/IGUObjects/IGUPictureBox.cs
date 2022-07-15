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

        public override void OnIGU() {
            if (!myConfg.IsVisible) return;
            GUI.enabled = myConfg.IsEnabled;

            Rect rectTemp = new Rect(GetPosition(), myRect.Size);
            
            GUI.DrawTexture(rectTemp, texture, scaleMode, alphaBlend, imageAspect, myColor.MyColor, borderWidths, borderRadiuses);
        }

        public static IGUPictureBox CreateIGUInstance(
            string name, Texture texture, 
            ScaleMode scaleMode, bool alphaBlend,
            float imageAspect, Vector4 borderWidths, 
            Vector4 borderRadiuses) {
            IGUPictureBox pictureBox = Internal_CreateIGUInstance<IGUPictureBox>(name);
            pictureBox.myConfg = IGUConfig.Default;
            pictureBox.myRect = IGURect.DefaultBox;
            pictureBox.myColor = IGUColor.DefaultBoxColor;
            pictureBox.texture = texture;
            pictureBox.scaleMode = scaleMode;
            pictureBox.alphaBlend = alphaBlend;
            pictureBox.imageAspect = imageAspect;
            pictureBox.borderWidths = borderWidths;
            pictureBox.borderRadiuses = borderRadiuses;
            return pictureBox;
        }

        public static IGUPictureBox CreateIGUInstance(
            string name, Texture texture,
            ScaleMode scaleMode, bool alphaBlend,
            float imageAspect, float BorderWidth, float BorderRadius)
            => CreateIGUInstance(name, texture, scaleMode, alphaBlend, imageAspect,
                Vector4.one * BorderWidth, Vector4.one * BorderRadius);

        public static IGUPictureBox CreateIGUInstance(
            string name, Texture texture,
            ScaleMode scaleMode, bool alphaBlend,
            float imageAspect, float borderWidth)
            => CreateIGUInstance(name, texture, scaleMode, alphaBlend, imageAspect, borderWidth, 0f);

        public static IGUPictureBox CreateIGUInstance(
            string name, Texture texture,
            ScaleMode scaleMode, bool alphaBlend,
            float imageAspect)
            => CreateIGUInstance(name, texture, scaleMode, alphaBlend, imageAspect, 0f);

        public static IGUPictureBox CreateIGUInstance(
            string name, Texture texture,
            ScaleMode scaleMode, bool alphaBlend)
            => CreateIGUInstance(name, texture, scaleMode, alphaBlend, 0f);

        public static IGUPictureBox CreateIGUInstance(string name, Texture texture, ScaleMode scaleMode)
            => CreateIGUInstance(name, texture, scaleMode, true);

        public static IGUPictureBox CreateIGUInstance(string name, Texture texture)
            => CreateIGUInstance(name, texture, ScaleMode.StretchToFill);

        public static IGUPictureBox CreateIGUInstance(string name)
            => CreateIGUInstance(name, Texture2D.whiteTexture);
    }
}
