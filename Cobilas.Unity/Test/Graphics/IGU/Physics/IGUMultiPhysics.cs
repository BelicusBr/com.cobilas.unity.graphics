using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUMultiPhysics : IGUPhysicsBase
    {

        public override Triangle[] Triangles => throw new NotImplementedException();

        public override bool IsHotPotato { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IGUObject Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }
    }
}