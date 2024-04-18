using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Graphics.IGU.Physics;
using System.Collections.Generic;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public sealed class TDSIGUWindow : IGUObject, IIGUWindow, IIGUPhysics {
        public GUI.WindowFunction windowFunction;
        private IGUBasicPhysics physicsBase;
        [SerializeField] private IGUStyle style;
        [SerializeField] private WindowFocusStatus isFocused;

        public override IGUBasicPhysics Physics { get => physicsBase; set => physicsBase = value; }
        WindowFocusStatus IIGUWindow.IsFocused { get => isFocused; set => isFocused = value; }

        public bool IsClipping => true;

        public Rect RectView { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector2 ScrollView { get => LocalRect.Position; set => throw new System.NotImplementedException(); }

        void IIGUPhysics.CallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys) {
            if (!LocalConfig.IsVisible) return;
            physicsBase.IsHotPotato = false;
            // if (parent is IIGUPhysics phy && parent is IIGUClipping)
            //     if (!phy.Physics.CollisionConfirmed(mouse))
            //         return;
            if (physicsBase.CollisionConfirmed(mouse))
                phys = physicsBase;
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultWindow;
            style = (IGUStyle)"Black window border";
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            physicsBase = IGUBasicPhysics.Create<IGUCollectionPhysics>(this);
            physicsBase.Target = this;
        }

        protected override void LowCallOnIGU() {
            myRect = BackEndIGU.SimpleWindow(LocalRect, new Rect(0f ,0f, LocalRect.Width, 25f), LocalRect.Position,
                IGUTextObject.GetIGUContentTemp("TDS Window"), style, physicsBase, GetInstanceID(),
                funcwin, ref isFocused);
        }

        private void funcwin (int id, Vector2 vector)
            => windowFunction?.Invoke(id);
    }
}