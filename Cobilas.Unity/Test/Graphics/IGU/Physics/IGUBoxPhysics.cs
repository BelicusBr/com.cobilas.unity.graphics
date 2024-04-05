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
        public override IGUObject Target { get => target; set => SetTarget(value); }

        public IGUBoxPhysics() {
            IsHotPotato = false;
            triangles = Triangle.Box;
            bufferTriangles = new Triangle[2];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            if (Parent != null)
                return Triangle.InsideInTriangle(bufferTriangles, mouse) && Parent.CollisionConfirmed(mouse);
            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        private void SetTarget(IGUObject target) {
            this.target = target;
            if (target == null) return;
            if (this.rectHash == (this.rectHash = this.target.MyRect.GetHashCode())) return;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetGlobalRotation(target));
                Vector2 a = triangles[I].A * target.MyRect.Size;
                Vector2 b = triangles[I].B * target.MyRect.Size;
                Vector2 c = triangles[I].C * target.MyRect.Size;

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