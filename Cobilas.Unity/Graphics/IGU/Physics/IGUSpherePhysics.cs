using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public sealed class IGUSpherePhysics : IGUMeshPhysics {

        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => parent; set => parent = value; }
        public override bool IsHotPotato { get => base.IsHotPotato; set => base.IsHotPotato = value; }

        protected override void Awake() {
            base.Awake();
            base.SetTriangle(Triangle.Circle64);
        }

        public override void SetTriangle(Triangle[] triangles)
            => throw new NotImplementedException();

        public override bool CollisionConfirmed(Vector2 mouse)
            => base.CollisionConfirmed(mouse);
    }
}