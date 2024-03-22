using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Physics;
using Cobilas.Unity.Test.Graphics.IGU.Interfaces;
using System.Collections.Generic;
using Cobilas.Collections;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public sealed class TDSIGUWindow : IGUObject, IIGUWindow, IIGUClipping, IIGURectClipPhysics {
        public GUI.WindowFunction windowFunction;
        private IGUPhysicsBase physicsBase;
        [SerializeField] private IGUStyle style;
        [SerializeField] private WindowFocusStatus isFocused;
        [SerializeField] private IGUObject[] internalPhysicsList;

        public IGUPhysicsBase Physics { get => physicsBase; set => physicsBase = value; }
        WindowFocusStatus IIGUWindow.IsFocused { get => isFocused; set => isFocused = value; }
        public IIGUPhysics[] InternalPhysicsList => ArrayManipulation.ConvertAll(internalPhysicsList, I => (IIGUPhysics)I);

        public bool IsClipping => true;

        public Rect RectView { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Vector2 ScrollView { get => LocalRect.Position; set => throw new System.NotImplementedException(); }

        public void CallPhysicsFeedback(Vector2 mouse, List<IGUPhysicsBase> phys) {
            if (!LocalConfig.IsVisible) return;
            if (parent is IIGUPhysics phy && parent is IIGUClipping)
                if (!phy.Physics.CollisionConfirmed(mouse))
                    return;
            if (parent is IIGUClipping clipping)
                physicsBase.Rect = LocalRect.SetPosition(LocalRect.Position + clipping.ScrollView);
            else physicsBase.Rect = LocalRect;
            physicsBase.IsHotPotato = false;
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
            physicsBase = new IGUBoxPhysics();
        }

        protected override void LowCallOnIGU() {
            myRect = BackEndIGU.SimpleWindow(LocalRect, new Rect(0f ,0f, LocalRect.Width, 25f), LocalRect.Position,
                IGUTextObject.GetIGUContentTemp("TDS Window"), style, physicsBase, GetInstanceID(),
                funcwin, ref isFocused);
        }

        private void funcwin (int id, Vector2 vector)
            => windowFunction?.Invoke(id);

        bool IIGURectClipPhysics.Add(IIGUPhysics physics) {
            if (internalPhysicsList != null && ArrayManipulation.Exists((IGUObject)physics, internalPhysicsList)) 
                return false;
            ArrayManipulation.Add((IGUObject)physics, ref internalPhysicsList);
            return true;
        }

        bool IIGURectClipPhysics.Remove(IIGUPhysics physics) {
            if (internalPhysicsList != null && !ArrayManipulation.Exists((IGUObject)physics, internalPhysicsList))
                return false;
            ArrayManipulation.Remove((IGUObject)physics, ref internalPhysicsList);
            return true;
        }
    }
}