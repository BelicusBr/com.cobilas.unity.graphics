using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public class IGUCollectionPhysics : IGUMeshPhysics {

        [SerializeField] private bool onCollision;
        [SerializeField] private IGUBasicPhysics[] subPhysics;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public IGUBasicPhysics[] SubPhysics => subPhysics;
        public int SubPhysicsCount => ArrayManipulation.ArrayLength(subPhysics);
        public override IGUObject Target { get => target; set => target = value; }
        public bool OnCollision { get => onCollision; set => onCollision = value; }
        public override IGUBasicPhysics Parent { get => parent; set => parent = value; }

        protected override void Awake() {
            base.Awake();
            onCollision = false;
        }

        public override void SetTriangle(Triangle[] triangles)
            => base.SetTriangle(triangles);

        public override bool CollisionConfirmed(Vector2 mouse) {
            if (onCollision) return true;
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        public virtual bool Add(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(subPhysics) &&
                ArrayManipulation.Exists(physics, subPhysics))) return false;
            physics.Parent = this;
            subPhysics = ArrayManipulation.Add(physics, subPhysics);
            return true;
        }

        public virtual bool Remove(IGUBasicPhysics physics) {
            if (physics == null || (!ArrayManipulation.EmpytArray(subPhysics) &&
                !ArrayManipulation.Exists(physics, subPhysics))) return false;
            physics.Parent = null;
            subPhysics = ArrayManipulation.Remove(physics, subPhysics);
            return true;
        }
    }
}