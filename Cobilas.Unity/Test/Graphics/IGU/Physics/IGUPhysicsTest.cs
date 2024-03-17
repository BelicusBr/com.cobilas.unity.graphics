using System;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUPhysicsTest : IGUPhysicsBase {
        public override Triangle[] Triangles => throw new NotImplementedException();
        public override bool IsHotPotato { get => true; set => throw new NotImplementedException(); }

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }
    }
}