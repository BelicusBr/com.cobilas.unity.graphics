using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public class IGUMultiPhysics : IGUBasicPhysics {

        public IGUBasicPhysics[] SubPhysics { get; private set; }
        public override Triangle[] Triangles => throw new NotImplementedException();
        public override IGUObject Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsHotPotato { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IGUBasicPhysics Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }

        public bool Add(IGUBasicPhysics physics) {
            throw new NotImplementedException();
        }

        public bool Remove(IGUBasicPhysics physics) {
            throw new NotImplementedException();
        }
    }
}