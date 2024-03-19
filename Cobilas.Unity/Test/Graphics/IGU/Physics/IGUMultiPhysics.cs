using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Events;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU.Physics {
    [Serializable]
    public sealed class IGUMultiPhysics : IGUPhysicsBase {
        private Triangle[] triangles;
        private IGUPhysicsBase target;
        [SerializeField] private bool isHotPotato;
        private IGUCollisionConfirmedAction onPhysics;
        private delegate void SubCollisionConfirmed(IGUPhysicsBase ipt, in Vector2 mouse, out IGUPhysicsBase otp);

        public override IGURect Rect { get; set; }
        public override Triangle[] Triangles => triangles;
        public override bool IsHotPotato { get => GetTarget().IsHotPotato; set => GetTarget().IsHotPotato = value; }

        public override bool CollisionConfirmed(Vector2 mouse) {
            onPhysics.Function(target, mouse, out target);
            return target != null;
        }

        public void RefreshEvents(IGUPhysicsBase[] physics) {
            onPhysics = new IGUCollisionConfirmedAction();
            onPhysics.RefreshFunction = p => p.SubCollisionConfirmedFunc;
            for (int I = 0; I < ArrayManipulation.ArrayLength(physics); I++)
                onPhysics += physics[I];
        }

        private IGUPhysicsBase GetTarget() => target is IGUMultiPhysics mp ? mp.GetTarget() : target;

        private sealed class IGUCollisionConfirmedAction : IGUActionManager<SubCollisionConfirmed, PhysicsItem> {
            private PhysicsItem[] physics;
            private event SubCollisionConfirmed function;
            private Func<PhysicsItem, SubCollisionConfirmed> refreshFunction;

            public override PhysicsItem this[int index] => physics[index];

            public override SubCollisionConfirmed Function => function;
            public override int RegisteredCount => ArrayManipulation.ArrayLength(physics);
            public override Func<PhysicsItem, SubCollisionConfirmed> RefreshFunction { get => refreshFunction; set => refreshFunction = value; }

            public override void Clear() {
                function = (SubCollisionConfirmed)null;
                ArrayManipulation.ClearArraySafe(ref physics);
            }

            public override bool Contains(PhysicsItem item) {
                for (int I = 0; I < RegisteredCount; I++)
                    if (physics[I] == item)
                        return true;
                return false;
            }

            public static IGUCollisionConfirmedAction operator +(IGUCollisionConfirmedAction A, IGUPhysicsBase B) {
                PhysicsItem item = (PhysicsItem)B;
                if (!A.Contains(item)) {
                    A.function += A.RefreshFunction(item);
                    ArrayManipulation.Add(item, ref A.physics);
                }
                return A;
            }

            public static IGUCollisionConfirmedAction operator -(IGUCollisionConfirmedAction A, IGUPhysicsBase B) {
                PhysicsItem item = (PhysicsItem)B;
                if (A.Contains(item)) {
                    ArrayManipulation.Remove((PhysicsItem)B, ref A.physics);
                    A.function = null;
                    for (int I = 0; I < A.RegisteredCount; I++)
                        A.function += A.RefreshFunction(A.physics[I]);
                }
                return A;
            }
        }

        private readonly struct PhysicsItem : IEquatable<IGUPhysicsBase> {
            private readonly IGUPhysicsBase item;

            private PhysicsItem(IGUPhysicsBase item) {
                this.item = item;
            }

            public bool Equals(IGUPhysicsBase other) => other == item;
            public override bool Equals(object obj)
                => (obj is PhysicsItem physics && Equals(physics.item)) ||
                (obj is IGUPhysicsBase physics2 && Equals(physics2));
            public override int GetHashCode() => (int)item?.GetHashCode();

            public void SubCollisionConfirmedFunc(IGUPhysicsBase ipt, in Vector2 mouse, out IGUPhysicsBase otp) {
                if (item.CollisionConfirmed(mouse))
                    ipt = item;
                otp = ipt;
            }

            public static explicit operator PhysicsItem(IGUPhysicsBase physicsBase) => new PhysicsItem(physicsBase);
            public static explicit operator IGUPhysicsBase(PhysicsItem physicsBase) => physicsBase.item;

            public static bool operator ==(PhysicsItem A, IGUPhysicsBase B) => A.Equals(B);
            public static bool operator !=(PhysicsItem A, IGUPhysicsBase B) => !A.Equals(B);
            public static bool operator ==(PhysicsItem A, PhysicsItem B) => A.Equals(B);
            public static bool operator !=(PhysicsItem A, PhysicsItem B) => !A.Equals(B);
        }
    }
}