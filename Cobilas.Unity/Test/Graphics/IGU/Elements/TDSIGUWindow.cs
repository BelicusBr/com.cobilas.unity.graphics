using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Physics;
using Cobilas.Unity.Test.Graphics.IGU.Interfaces;
using System.Collections.Generic;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public sealed class TDSIGUWindow : IGUObject, IIGUWindow, IIGUPhysics {
        private IGUPhysicsBase physicsBase;
        [SerializeField] private IGUStyle style;
        [SerializeField] private WindowFocusStatus isFocused;

        public IGUPhysicsBase Physics { get => physicsBase; set => physicsBase = value; }

        WindowFocusStatus IIGUWindow.IsFocused { get => isFocused; set => isFocused = value; }

        public void CallPhysicsFeedback(Vector2 mouse, List<IGUPhysicsBase> phys) {
            if (physicsBase.CollisionConfirmed(mouse))
                phys[0] = physicsBase;
        }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultWindow;
            style = (IGUStyle)"Black window border";
        }

        protected override void IGUOnEnable() {
            base.IGUOnEnable();
            physicsBase = new IGUPhysicsTest();
        }

        protected override void LowCallOnIGU() {
            BackEndIGU.SimpleWindow(LocalRect, new Rect(0f ,0f, 130f, 25f), Vector2.zero,
                IGUTextObject.GetIGUContentTemp("TDS Window"), style, physicsBase, GetInstanceID(),
                funcwin, ref isFocused);
        }

        private void funcwin (int id, Vector2 vector) {

        }
    }
}