using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Test.Graphics.IGU.Physics;

namespace Cobilas.Unity.Test.Graphics.IGU.Interfaces {
    public interface IIGUPhysics {
        IGUPhysicsBase Physics { get; set; }
        void CallPhysicsFeedback(Vector2 mouse, List<IGUPhysicsBase> phys);
    }
}