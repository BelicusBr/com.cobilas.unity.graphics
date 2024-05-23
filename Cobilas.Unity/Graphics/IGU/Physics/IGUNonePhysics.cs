using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUNonePhysics : IGUBasicPhysics {
        [SerializeField] private IGUObject target;

        private static IGUNonePhysics none;

        public override bool IsFixedCollision { get; set; }
        public override bool IsHotPotato { get => true; set {} }
        public override IGUObject Target { get => target; set => target = value; }
        public override Triangle[] Triangles => throw new NotImplementedException();
        public override IGUBasicPhysics Parent { get => (IGUBasicPhysics)null; set {} }

        public static IGUNonePhysics None => none ?? (none = Create<IGUNonePhysics>(null));

        public override bool CollisionConfirmed(Vector2 mouse)
        {
            throw new NotImplementedException();
        }
    }
}