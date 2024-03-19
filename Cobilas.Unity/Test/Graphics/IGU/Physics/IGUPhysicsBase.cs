using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    [Serializable]
    public abstract class IGUPhysicsBase {
        public abstract IGURect Rect { get; set; }
        public abstract Triangle[] Triangles { get; }
        public abstract bool IsHotPotato { get; set; }

        public abstract bool CollisionConfirmed(Vector2 mouse);
    }
}
