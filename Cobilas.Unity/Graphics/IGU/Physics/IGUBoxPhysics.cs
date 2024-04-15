using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUBoxPhysics : IGUMeshPhysics {

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => parent; set => parent = value; }

        protected override void Awake() {
            base.Awake();
            base.SetTriangle(Triangle.Box);
        }

        public override void SetTriangle(Triangle[] triangles)
            => throw new NotImplementedException();

        public override bool CollisionConfirmed(Vector2 mouse) => base.CollisionConfirmed(mouse);
    }
}