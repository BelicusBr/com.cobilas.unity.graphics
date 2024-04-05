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
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public class TDSIGUPhysicsTemp : IGUObject, IIGUPhysics {
        public IGUBasicPhysics _Physics;
        public IGUBasicPhysics Physics { get => _Physics; set => _Physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            _Physics = new IGUBoxPhysics();
            myRect = IGURect.DefaultButton;
        }

        protected override void IGUOnEnable() {
            _Physics = new IGUBoxPhysics();
        }

        protected override void LowCallOnIGU() {
            //GUI.Button((Rect)LocalRect, "Button");
            // if (TDS_IGUStyle.DrawButton((Rect)LocalRect, (IGUStyle)"Black button border", new IGUContent("Button"), _Physics))
            //     Debug.Log($"[ID:{GetInstanceID()}]{name}");
            if (BackEndIGU.Button(LocalRect, new IGUContent("Button"), (IGUStyle)"Black button border", _Physics, GetInstanceID()))
                Debug.Log($"[ID:{GetInstanceID()}]{name}");
        }

        public void OnDrawGizmos() {
            //(_Physics as IGUBoxPhysics)?.OnDrawGizmos();
        }

        void IIGUPhysics.CallPhysicsFeedback(Vector2 mouse, List<IGUBasicPhysics> phys) {
            if (!LocalConfig.IsVisible) return;
            _Physics.Target = this;
            if (parent is IIGUPhysics phy && parent is IIGUClipping)
                if (!phy.Physics.CollisionConfirmed(mouse))
                    return;
            // if (parent is IIGUClipping clipping)
            //     _Physics.Rect = LocalRect.SetPosition(LocalRect.Position + clipping.ScrollView);
            // else _Physics.Rect = LocalRect;
            _Physics.IsHotPotato = false;
            if (_Physics.CollisionConfirmed(mouse))
                phys[0] = _Physics;
        }
    }
}