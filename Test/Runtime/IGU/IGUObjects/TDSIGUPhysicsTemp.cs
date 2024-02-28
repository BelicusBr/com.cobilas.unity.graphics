using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using Cobilas.Unity.Graphics.IGU.Physics;
using UnityEngine;

namespace Test.Runtime.IGU.IGUObjects {
    public class TDSIGUPhysicsTemp : IGUObject, IIGUPhysics {
        public IGUMonoPhysics _Physics;
        public IGUPhysicsBase Physics => _Physics;

        protected override void IGUAwake() {
            base.IGUAwake();
            _Physics = new IGUMonoPhysics();
            myRect = IGURect.DefaultButton;
        }

        protected override void LowCallOnIGU() {
            
            GUI.Button((Rect)LocalRect, "Button");
        }
    }
}