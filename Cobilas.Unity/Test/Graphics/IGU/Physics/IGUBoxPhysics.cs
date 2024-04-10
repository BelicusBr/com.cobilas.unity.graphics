using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUBoxPhysics : IGUBasicPhysics {
        private int rectHash;
        private IGUObject target;
        private readonly Triangle[] triangles;
        private readonly Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUBasicPhysics Parent { get; set; }
        public override IGUObject Target { get => target; set => target = value; }

        public IGUBoxPhysics() {
            IsHotPotato = false;
            triangles = Triangle.Box;
            bufferTriangles = new Triangle[2];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            rectHash = BuildBufferTriangles(triangles, bufferTriangles, target, rectHash);
            if (Parent != null) {
                //Debug.Log($"[{target.name}][Parent:{Parent.CollisionConfirmed(mouse)}]phy");
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            }
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }
    }
}