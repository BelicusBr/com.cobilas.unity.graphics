using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Test.Graphics.IGU;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU.Elements {
    public class TDSIGUPhysicsTemp : IGUObject, IIGUPhysics, ISerializationCallbackReceiver {
        public IGUBasicPhysics _Physics;
        public override IGUBasicPhysics Physics { get => _Physics; set => _Physics = value; }

        protected override void IGUAwake() {
            base.IGUAwake();
            myRect = IGURect.DefaultButton;
        }

        protected override void IGUOnEnable() {
            _Physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this);
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

        void IIGUPhysics.CallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys) {
            if (!LocalConfig.IsVisible) return;
            _Physics.IsHotPotato = false;
            // if (parent is IIGUPhysics phy && parent is IIGUClipping)
            //     if (!phy.Physics.CollisionConfirmed(mouse))
            //         return;
            if (_Physics.CollisionConfirmed(mouse))
                phys = _Physics;
        }

        public void OnBeforeSerialize()
        {
            Debug.Log("OnBeforeSerialize");
        }

        public void OnAfterDeserialize()
        {
            Debug.Log("OnAfterDeserialize");
        }
    }
}