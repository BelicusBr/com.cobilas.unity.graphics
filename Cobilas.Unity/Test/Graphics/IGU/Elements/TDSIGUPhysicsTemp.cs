using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Test.Graphics.IGU.Interfaces;
using Cobilas.Unity.Test.Graphics.IGU.Physics;
using Cobilas.Unity.Test.Graphics.IGU;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public class TDSIGUPhysicsTemp : IGUObject, IIGUPhysics {
        public IGUPhysicsBase _Physics;
        public IGUPhysicsBase Physics { get => _Physics; set => _Physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            _Physics = new IGUBoxPhysics();
            myRect = IGURect.DefaultButton;
        }

        protected override void LowCallOnIGU() {
            //GUI.Button((Rect)LocalRect, "Button");
            if (TDS_IGUStyle.DrawButton((Rect)LocalRect, (IGUStyle)"Black button border", new IGUContent("Button"), _Physics))
                Debug.Log($"[ID:{GetInstanceID()}]{name}");
        }

        void IIGUPhysics.CallPhysicsFeedback(Vector2 mouse, List<IGUPhysicsBase> phys) {
            if (!LocalConfig.IsVisible) return;
            _Physics.Rect = LocalRect;
            _Physics.IsHotPotato = false;
            if (_Physics.CollisionConfirmed(mouse))
                phys[0] = _Physics;
        }
    }
}