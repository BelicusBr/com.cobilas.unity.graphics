using System;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    [Serializable]
    public abstract class IGUPhysicsBase {
        public abstract bool IsHotPotato { get; set; }

        public abstract bool CollisionConfirmed(Vector2 mouse);
    }
}
