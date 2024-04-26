using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public class IGUMeshPhysics : IGUBasicPhysics {
        protected int rectHash;
        [SerializeField] protected IGUObject target;
        [SerializeField] protected bool isHotPotato;
        [SerializeField] protected Triangle[] triangles;
        [SerializeField] protected bool isFixedCollision;
        [SerializeField] protected IGUBasicPhysics parent;
        [SerializeField, HideInInspector] protected Triangle[] bufferTriangles;

        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => parent; set => parent = value; }
        public override bool IsHotPotato { get => isHotPotato; set => isHotPotato = value; }
        public override bool IsFixedCollision { get => isFixedCollision; set => isFixedCollision = value; }

        protected override void Awake() {
            IsHotPotato = false;
            base.Awake();
        }

        public virtual void SetTriangle(Triangle[] triangles) {
            this.triangles = triangles;
            bufferTriangles = new Triangle[triangles.Length];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            if (isFixedCollision) return true;
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }
    }
}