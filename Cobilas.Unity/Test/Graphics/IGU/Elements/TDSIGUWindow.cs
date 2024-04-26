using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Graphics.IGU.Physics;
using System.Collections.Generic;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public sealed class TDSIGUWindow : IGUObject, IIGUWindow, IIGUClippingPhysics {
        public GUI.WindowFunction windowFunction;
        private IGUBasicPhysics physicsBase;
        [SerializeField] private IGUStyle style;
        [SerializeField] private WindowFocusStatus isFocused;
        [SerializeField] private IGUPhysicsClippingContainer phyContainer;

        public override IGUBasicPhysics Physics { get => physicsBase; set => physicsBase = value; }
        
        WindowFocusStatus IIGUWindow.IsFocused { get => isFocused; }

        public bool IsClipping => true;

        public Rect RectView { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector2 ScrollView { get => LocalRect.Position; set => throw new System.NotImplementedException(); }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultWindow;
            style = (IGUStyle)"Black window border";
            phyContainer = new IGUPhysicsClippingContainer();
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            physicsBase = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
            physicsBase.Target = this;
            phyContainer.RefreshEvents();
        }

        protected override void LowCallOnIGU() {
            myRect = BackEndIGU.SimpleWindow(LocalRect, new Rect(0f ,0f, LocalRect.Width, 25f), LocalRect.Position,
                IGUTextObject.GetIGUContentTemp("TDS Window"), style, physicsBase, GetInstanceID(),
                funcwin, ref isFocused);
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => phyContainer.CallPhysicsFeedback(mouse, ref phys);

        private void funcwin (int id, Vector2 vector)
            => windowFunction?.Invoke(id);

        public bool AddOtherPhysics(IGUObject obj)
            => phyContainer.AddOtherPhysics(obj);

        public bool RemoveOtherPhysics(IGUObject obj)
            => phyContainer.RemoveOtherPhysics(obj);
    }
}