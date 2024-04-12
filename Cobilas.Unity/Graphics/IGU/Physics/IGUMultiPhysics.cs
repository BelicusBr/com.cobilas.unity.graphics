using System;
using UnityEngine;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    [Serializable]
    public class IGUMultiPhysics : IGUMeshPhysics {

        [SerializeField] private IGUObject[] subPhysics;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => target = value; }
        public override IGUBasicPhysics Parent { get => (parent as IIGUPhysics).Physics; set => parent = value.Target; }
        public IEnumerable<IGUBasicPhysics> SubPhysics {
            get {
                for (int I = 0; I < ArrayManipulation.ArrayLength(subPhysics); I++)
                    yield return (subPhysics[I] as IIGUPhysics).Physics;
            }
        }

        public IGUMultiPhysics(IGUObject target, Triangle[] triangles) : base(target, triangles) {}

        public override void SetTriangle(Triangle[] triangles)
            => base.SetTriangle(triangles);

        public override bool CollisionConfirmed(Vector2 mouse) {
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        public virtual bool Add(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(subPhysics) &&
                ArrayManipulation.Exists(physics.Target, subPhysics))) return false;
            physics.Parent = this;
            subPhysics = ArrayManipulation.Add(physics.Target, subPhysics);
            return true;
        }

        public virtual bool Remove(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(subPhysics) &&
                !ArrayManipulation.Exists(physics.Target, subPhysics))) return false;
            physics.Parent = null;
            subPhysics = ArrayManipulation.Remove(physics.Target, subPhysics);
            return true;
        }
    }
}