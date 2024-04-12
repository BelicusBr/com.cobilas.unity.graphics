using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUNonePhysics : IGUBasicPhysics {
        private readonly static IGUNonePhysics none = new IGUNonePhysics();

        public override Triangle[] Triangles => throw new NotImplementedException();
        public override bool IsHotPotato { get => true; set => throw new NotImplementedException(); }
        public override IGUObject Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IGUBasicPhysics Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static IGUNonePhysics None => none;

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }
    }
}