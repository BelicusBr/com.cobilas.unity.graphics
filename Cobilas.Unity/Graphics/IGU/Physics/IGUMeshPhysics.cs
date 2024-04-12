using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    [Serializable]
    public class IGUMeshPhysics : IGUBasicPhysics {
        protected int rectHash;
        [SerializeField] protected IGUObject target;
        [SerializeField] protected IGUObject parent;
        [SerializeField] protected Triangle[] triangles;
        [SerializeField, HideInInspector] protected Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => (parent as IIGUPhysics).Physics; set => parent = value.Target; }

        public IGUMeshPhysics(IGUObject target, Triangle[] triangles) {
            this.target = target;
            SetTriangle(triangles);
        }

        public virtual void SetTriangle(Triangle[] triangles) {
            this.triangles = triangles;
            bufferTriangles = new Triangle[triangles.Length];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }
    }
}