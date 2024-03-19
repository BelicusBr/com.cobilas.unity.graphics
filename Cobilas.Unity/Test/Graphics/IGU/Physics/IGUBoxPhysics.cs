using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUBoxPhysics : IGUPhysicsBase {
        private IGURect rect;
        private Triangle[] triangles;
        private Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGURect Rect { get => rect; set => SetRect(value); }

        public IGUBoxPhysics() {
            IsHotPotato = false;
            triangles = Triangle.Box;
            bufferTriangles = new Triangle[2];
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            // Quaternion quaternion = Quaternion.Euler(Vector3.forward * Rect.Rotation);
            // Vector2 a0 = quaternion.GenerateDirection(triangles[0].A) * Rect.Size + Rect.Position;
            // Vector2 b0 = quaternion.GenerateDirection(triangles[0].B) * Rect.Size + Rect.Position;
            // Vector2 c0 = quaternion.GenerateDirection(triangles[0].C) * Rect.Size + Rect.Position;

            // Vector2 a1 = quaternion.GenerateDirection(triangles[1].A) * Rect.Size + Rect.Position;
            // Vector2 b1 = quaternion.GenerateDirection(triangles[1].B) * Rect.Size + Rect.Position;
            // Vector2 c1 = quaternion.GenerateDirection(triangles[1].C) * Rect.Size + Rect.Position;

            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        private void SetRect(IGURect rect) {
            if (this.rect == (this.rect = rect)) return;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * rect.Rotation);
                Vector2 a = triangles[I].A * rect.Size;
                Vector2 b = triangles[I].B * rect.Size;
                Vector2 c = triangles[I].C * rect.Size;

                a = (Vector2)(quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y) + rect.Position;
                b = (Vector2)(quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y) + rect.Position;
                c = (Vector2)(quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y) + rect.Position;
                bufferTriangles[I] = new Triangle(a, b, c);
            }
        }
    }
}