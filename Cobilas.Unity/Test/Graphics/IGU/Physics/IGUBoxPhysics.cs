using System;
using Cobilas.Unity.Graphics.IGU;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public sealed class IGUBoxPhysics : IGUPhysicsBase {
        private Triangle[] triangles;

        public IGURect Rect { get; set; }
        public override bool IsHotPotato { get; set; }
        public override Triangle[] Triangles => triangles;

        public IGUBoxPhysics() {
            IsHotPotato = false;
            triangles = Triangle.Box;
        }

        public override bool CollisionConfirmed(Vector2 mouse) {
            Quaternion quaternion = Quaternion.Euler(Vector3.forward * Rect.Rotation);
            Vector2 a0 = quaternion.GenerateDirection(triangles[0].A) * Rect.Size + Rect.Position;
            Vector2 b0 = quaternion.GenerateDirection(triangles[0].B) * Rect.Size + Rect.Position;
            Vector2 c0 = quaternion.GenerateDirection(triangles[0].C) * Rect.Size + Rect.Position;

            Vector2 a1 = quaternion.GenerateDirection(triangles[1].A) * Rect.Size + Rect.Position;
            Vector2 b1 = quaternion.GenerateDirection(triangles[1].B) * Rect.Size + Rect.Position;
            Vector2 c1 = quaternion.GenerateDirection(triangles[1].C) * Rect.Size + Rect.Position;

            return Triangle.InsideInTriangle(a0, b0, c0, mouse) ||
                Triangle.InsideInTriangle(a1, b1, c1, mouse);
        }
    }
}