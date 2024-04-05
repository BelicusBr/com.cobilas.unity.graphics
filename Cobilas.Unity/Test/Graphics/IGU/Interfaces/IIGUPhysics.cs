using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Test.Graphics.IGU.Physics;

namespace Cobilas.Unity.Test.Graphics.IGU.Interfaces {
    public interface IIGUPhysics {
        IGUBasicPhysics Physics { get; set; }
        void CallPhysicsFeedback(Vector2 mouse, List<IGUBasicPhysics> phys);
    }
}