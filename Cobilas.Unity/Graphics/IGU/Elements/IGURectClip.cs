using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject, IIGUClipping {

        public event Action<Vector2> RectClipAction;
        [SerializeField] protected Rect rectView;
        [SerializeField] protected bool isClipping;
        [SerializeField] protected bool autoInvert;
        [SerializeField] protected Vector2 scrollView;
        [SerializeField] protected IGUBasicPhysics physics;

        public bool IsClipping => isClipping;
        public bool AutoInvert { get => autoInvert; set => autoInvert = value; }
        public Rect RectView { get => rectView; set => rectView = value; }
        public Vector2 ScrollView { 
            get => rectView.position = scrollView.Invert(autoInvert, autoInvert);
            set => rectView.position = scrollView = value.Invert(autoInvert, autoInvert);
        }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            autoInvert = true;
            isPhysicalElement = false;
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            rectView = new Rect(Vector2.zero, myRect.Size * 2f);
            physics = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            (physics as IGUCollectionPhysics).SetTriangle(Triangle.Box);
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.Clipping(LocalRect, scrollView, ClipFunc);
        }

        private void ClipFunc(Vector2 scrollOffset) {
            isClipping = true;
            RectClipAction?.Invoke(scrollOffset);
            isClipping = false;
        }
    }
}
