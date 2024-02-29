using UnityEngine;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Physics;

namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUPhysics {
        IGUPhysicsBase Physics { get; }
        void CallPhysicsFeedback(Vector2 mouse, List<IGUPhysicsBase> phys);
    }
}