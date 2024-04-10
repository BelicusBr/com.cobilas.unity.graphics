using UnityEngine;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    public abstract class IGUBasicPhysics {
        public abstract Triangle[] Triangles { get; }
        public abstract IGUObject Target { get; set; }
        public abstract bool IsHotPotato { get; set; }
        public abstract IGUBasicPhysics Parent { get; set; }
        public delegate void CallPhysicsFeedback(Vector2 mousePosition, ref IGUBasicPhysics phy);

        public abstract bool CollisionConfirmed(Vector2 mouse);

        public static float GetGlobalRotation(IGUObject obj)
            => obj.Parent == null ? obj.MyRect.Rotation : obj.MyRect.Rotation + GetGlobalRotation(obj.Parent);

        public static IGURect GetLocalPosition(IGUObject obj, bool noRotation) {
            if (obj.Parent != null) {
                if (obj.Parent is IIGUClipping cli && cli.IsClipping) 
                    return obj.MyRect.SetScaleFactor(IGUDrawer.ScaleFactor);
                IGURect res = obj.MyRect;
                Vector2 position = res.Position;
                if (!noRotation) {
                    Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetGlobalRotation(obj.Parent));
                    position = quaternion.GenerateDirectionRight() * position.x +
                        quaternion.GenerateDirectionUp() * position.y;
                }
                return res.SetScaleFactor(IGUDrawer.ScaleFactor)
                    .SetPosition(position + GetLocalPosition(obj.Parent, noRotation).Position);
            }
            return obj.MyRect;
        }

        public static int BuildBufferTriangles(Triangle[] triangles, Triangle[] bufferTriangles, IGUObject obj, int rectHash) {
            if (obj == null) return rectHash;
            if (rectHash == (rectHash = obj.MyRect.GetHashCode())) return rectHash;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * GetGlobalRotation(obj));
                Vector2 a = triangles[I].A * obj.MyRect.Size;
                Vector2 b = triangles[I].B * obj.MyRect.Size;
                Vector2 c = triangles[I].C * obj.MyRect.Size;

                a = quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y;
                b = quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y;
                c = quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y;

                a += GetLocalPosition(obj, false).Position;
                b += GetLocalPosition(obj, false).Position;
                c += GetLocalPosition(obj, false).Position;

                bufferTriangles[I] = new Triangle(a, b, c);
            }
            return rectHash;
        }
    }
}
