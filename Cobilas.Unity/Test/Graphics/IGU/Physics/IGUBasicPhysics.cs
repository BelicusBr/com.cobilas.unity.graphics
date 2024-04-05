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
    }
}
