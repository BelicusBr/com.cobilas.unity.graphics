using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUNonePhysics : IGUBasicPhysics {
        [SerializeField] private IGUObject target;

        private readonly static IGUNonePhysics none = Create<IGUNonePhysics>(null);

        public override IGUObject Target { get => target; set => target = value; }
        public override Triangle[] Triangles => throw new NotImplementedException();
        public override bool IsHotPotato { get => true; set => throw new NotImplementedException(); }
        public override bool IsFixedCollision { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IGUBasicPhysics Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static IGUNonePhysics None => none;

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }
    }
}