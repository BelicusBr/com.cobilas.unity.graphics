using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUSpherePhysics : IGUPhysicsBase {
        private IGURect rect;
        private Triangle[] triangles;
        private Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGURect Rect { get => rect; set => SetRect(value); }

        public IGUSpherePhysics() {
            triangles = Triangle.Circle;
            bufferTriangles = new Triangle[triangles.Length];
        }

        public override bool CollisionConfirmed(Vector2 mouse)
            => Triangle.InsideInTriangle(bufferTriangles, mouse);

        private void SetRect(IGURect rect) {
            if (this.rect == (this.rect = rect)) return;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * rect.Rotation);
                Vector2 n_size = rect.Size * .5f;
                Vector2 a = triangles[I].A * n_size;
                Vector2 b = triangles[I].B * n_size;
                Vector2 c = triangles[I].C * n_size;

                a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + rect.Position;
                b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + rect.Position;
                c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + rect.Position;
                bufferTriangles[I] = new Triangle(a, b, c);
            }
        }
    }
}