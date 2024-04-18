using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject {

        public event Action<Vector2> RectClipAction;
        [SerializeField] protected Rect rectView;
        [SerializeField] protected bool autoInvert;
        [SerializeField] private bool renderOffSet;
        [SerializeField] protected Vector2 scrollView;
        [SerializeField] protected IGUBasicPhysics physics;

        // public bool IsClipping => isClipping;
        public Rect RectView { get => rectView; set => rectView = value; }
        public bool AutoInvert { get => autoInvert; set => autoInvert = value; }
        public bool RenderOffSet { get => renderOffSet; set => renderOffSet = value; }
        public Vector2 ScrollView { 
            get => rectView.position = scrollView.Invert(autoInvert, autoInvert);
            set => rectView.position = scrollView = value.Invert(autoInvert, autoInvert);
        }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoInvert =
            renderOffSet = true;
            isPhysicalElement = false;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            rectView = new Rect(Vector2.zero, myRect.Size * 2f);
            physics = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            (physics as IGUCollectionPhysics).SetTriangle(Triangle.Box);
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.Clipping(LocalRect, renderOffSet ? ScrollView : Vector2.zero, ClipFunc);
        }

        private void ClipFunc(Vector2 scrollOffset) 
            => RectClipAction?.Invoke(scrollOffset);
    }
}
