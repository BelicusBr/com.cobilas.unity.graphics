using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGURectClip : IGUObject, IIGUClippingPhysics {

        public event Action<Vector2> RectClipAction;
        [SerializeField] protected Rect rectView;
        [SerializeField] protected bool autoInvert;
        [SerializeField] private bool renderOffSet;
        [SerializeField] protected Vector2 scrollView;
        [SerializeField] protected IGUBasicPhysics physics;
        [SerializeField] protected IGUPhysicsClippingContainer phyContainer;

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
            myRect = IGURect.DefaultBox;
            myColor = IGUColor.DefaultBoxColor;
            phyContainer = new IGUPhysicsClippingContainer();
            rectView = new Rect(Vector2.zero, myRect.Size * 2f);
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            phyContainer.RefreshEvents();
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.Clipping(LocalRect, renderOffSet ? ScrollView : Vector2.zero, ClipFunc);
        }

        public bool AddOtherPhysics(IGUObject obj)
            => phyContainer.AddOtherPhysics(obj);

        public bool RemoveOtherPhysics(IGUObject obj)
            => phyContainer.RemoveOtherPhysics(obj);

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => phyContainer.CallPhysicsFeedback(mouse, ref phys);

        private void ClipFunc(Vector2 scrollOffset) 
            => RectClipAction?.Invoke(scrollOffset);

        public static IGURectClip operator +(IGURectClip A, IIGUPhysics B) {
            _ = A.AddOtherPhysics(B.Physics.Target);
            return A;
        }

        public static IGURectClip operator -(IGURectClip A, IIGUPhysics B) {
            _ = A.RemoveOtherPhysics(B.Physics.Target);
            return A;
        }
    }
}
