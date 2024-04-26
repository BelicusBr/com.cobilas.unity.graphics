using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Physics {
    public abstract class IGUBasicPhysics : ScriptableObject {
        public abstract Triangle[] Triangles { get; }
        public abstract IGUObject Target { get; set; }
        public abstract bool IsHotPotato { get; set; }
        public abstract bool IsFixedCollision { get; set; }
        public abstract IGUBasicPhysics Parent { get; set; }
        public delegate void CallPhysicsFeedback(Vector2 mousePosition, ref IGUBasicPhysics phy);

        protected virtual void Awake() {}

        public abstract bool CollisionConfirmed(Vector2 mouse);

        public static IGUBasicPhysics Create(Type type, IGUObject target, bool isFixedCollision) {
            if (!type.IsSubclassOf(typeof(IGUBasicPhysics)))
                throw new IGUException($"Class {type.Name} does not inherit from class IGUBasicPhysics.");
            else if (type.IsAbstract) 
                throw new IGUException("The target class cannot be abstract.");
            IGUBasicPhysics result = (IGUBasicPhysics)CreateInstance(type);
            result.Target = target;
            result.IsFixedCollision = isFixedCollision;
            return result;
        }

        public static IGUBasicPhysics Create(Type type, IGUObject target)
            => Create(type, target, false);

        public static T Create<T>(IGUObject target, bool isFixedCollision) where T: IGUBasicPhysics
            => (T)Create(typeof(T), target, isFixedCollision);

        public static T Create<T>(IGUObject target) where T: IGUBasicPhysics
            => (T)Create(typeof(T), target, false);

        public static float GetGlobalRotation(IGUObject obj)
            => obj.Parent == null ? obj.MyRect.Rotation : obj.MyRect.Rotation + GetGlobalRotation(obj.Parent);

        public static IGURect GetLocalPosition(IGUObject obj, bool noRotation) {
            if (obj.Parent != null) {
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
            float globalRotation = GetGlobalRotation(obj);
            IGURect local = obj.LocalRect.SetRotation(globalRotation);
            if (rectHash == (rectHash = local.GetHashCode())) return rectHash;

            for (int I = 0; I < triangles.Length; I++) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * globalRotation);
                IGURect mypos = GetLocalPosition(obj, false).ModifiedRect;
                Vector2 a = triangles[I].A * mypos.Size;
                Vector2 b = triangles[I].B * mypos.Size;
                Vector2 c = triangles[I].C * mypos.Size;

                a = quaternion.GenerateDirectionRight() * a.x + quaternion.GenerateDirectionUp() * a.y;
                b = quaternion.GenerateDirectionRight() * b.x + quaternion.GenerateDirectionUp() * b.y;
                c = quaternion.GenerateDirectionRight() * c.x + quaternion.GenerateDirectionUp() * c.y;

                a += mypos.Position;
                b += mypos.Position;
                c += mypos.Position;

                bufferTriangles[I] = new Triangle(a, b, c);
            }
            return rectHash;
        }
    }
}
