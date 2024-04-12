using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    [Serializable]
    public sealed class IGUBoxPhysics : IGUBasicPhysics {
        private int rectHash;
        [SerializeField] private IGUObject target;
        [SerializeField] private IGUObject parent;
        [SerializeField] private Triangle[] triangles;
        [SerializeField, HideInInspector] private Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => (parent as IIGUPhysics).Physics; set => parent = value.Target; }

        public IGUBoxPhysics(IGUObject target) {
            IsHotPotato = false;
            this.target = target;
            triangles = Triangle.Box;
            bufferTriangles = new Triangle[2];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }
    }
}