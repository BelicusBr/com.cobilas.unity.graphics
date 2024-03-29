using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUSpherePhysics : IGUPhysicsBase {
        private int rectHash;
        private IGUObject target;
        private Triangle[] triangles;
        private Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => SetTarget(value); }

        public IGUSpherePhysics() {
            triangles = Triangle.Circle;
            bufferTriangles = new Triangle[triangles.Length];
        }

        public override bool CollisionConfirmed(Vector2 mouse)
            => Triangle.InsideInTriangle(bufferTriangles, mouse);

        private void SetTarget(IGUObject target) {
            this.target = target;
            if (target == null) return;
            if (this.rectHash == (this.rectHash = this.target.MyRect.GetHashCode())) return;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetGlobalRotation(target));
                Vector2 n_size = target.MyRect.Size * .5f;
                Vector2 a = triangles[I].A * n_size;
                Vector2 b = triangles[I].B * n_size;
                Vector2 c = triangles[I].C * n_size;

                a = quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y;
                b = quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y;
                c = quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y;
                
                a += GetLocalPosition(target, false).Position;
                b += GetLocalPosition(target, false).Position;
                c += GetLocalPosition(target, false).Position;
                
                bufferTriangles[I] = new Triangle(a, b, c);
            }
        }
    }
}