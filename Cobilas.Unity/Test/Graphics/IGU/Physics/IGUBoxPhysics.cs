using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUBoxPhysics : IGUPhysicsBase {
        private int rectHash;
        private IGUObject target;
        private Triangle[] triangles;
        private Triangle[] bufferTriangles;

        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;
        public override IGUObject Target { get => target; set => SetTarget(value); }

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
            Rect rect = new Rect(Vector2.zero, Vector2.one * 2f);
            Texture2D texture = Texture2D.whiteTexture;
            foreach (Triangle item in bufferTriangles) {
                Color oldColor = GUI.color;
                GUI.color = Color.red;
                rect.position = item.A - Vector2.one;
                GUI.DrawTexture(rect, texture);
                rect.position = item.B - Vector2.one;
                GUI.DrawTexture(rect, texture);
                rect.position = item.C - Vector2.one;
                GUI.DrawTexture(rect, texture);
                GUI.color = oldColor;
            }

            return Triangle.InsideInTriangle(bufferTriangles, mouse);
        }

        public void OnDrawGizmos() {
            for (int I = 0; I < bufferTriangles.Length; I++) {
                Gizmos.color = Color.red;
                Vector2 a = bufferTriangles[I].A;
                Vector2 b = bufferTriangles[I].B;
                Vector2 c = bufferTriangles[I].C;

                a = Camera.current.ScreenToWorldPoint((a - Vector2.up * Screen.height).InvertY());
                b = Camera.current.ScreenToWorldPoint((b - Vector2.up * Screen.height).InvertY());
                c = Camera.current.ScreenToWorldPoint((c - Vector2.up * Screen.height).InvertY());

                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, a);
            }
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