using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public class IGUMultiPhysics : IGUBasicPhysics {

        private int rectHash;
        private IGUObject target;
        private readonly Triangle[] triangles;
        private readonly Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUBasicPhysics Parent { get; set; }
        public IGUBasicPhysics[] SubPhysics { get; private set; }
        public override IGUObject Target { get => target; set => target = value; }

        public IGUMultiPhysics(Triangle[] triangles) {
            this.triangles = triangles;
            bufferTriangles = new Triangle[triangles.Length];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        public virtual bool Add(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(SubPhysics) &&
                ArrayManipulation.Exists(physics, SubPhysics))) return false;
            physics.Parent = this;
            SubPhysics = ArrayManipulation.Add(physics, SubPhysics);
            return true;
        }

        public virtual bool Remove(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(SubPhysics) &&
                !ArrayManipulation.Exists(physics, SubPhysics))) return false;
            physics.Parent = null;
            SubPhysics = ArrayManipulation.Remove(physics, SubPhysics);
            return true;
        }
    }
}