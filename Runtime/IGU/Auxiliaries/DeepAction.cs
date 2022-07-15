using System;
using UnityEngine;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {

    [Serializable]
    public sealed class DeepAction : IEquatable<int>, ISerializationCallbackReceiver {
        [SerializeField] private int depth;
        [SerializeField] private IGUObject[] objects;
        private Action onIGU;
        private Action<List<IIGUObject>, int> alteredDepth;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public int Depth => depth;
        public int Count => ArrayManipulation.ArrayLength(objects);

        public DeepAction(int depth) => this.depth = depth;

        public bool Add(IGUObject item) {
            if (Contains(item)) return false;

            onIGU += (item as IIGUObject).InternalOnIGU;
            alteredDepth += (item as IIGUObject).AlteredDepth;

            ArrayManipulation.Add(item, ref objects);
            return true;
        }

        public bool Remove(IGUObject item) {
            if (!Contains(item)) return false;
            onIGU -= (item as IIGUObject).InternalOnIGU;
            alteredDepth -= (item as IIGUObject).AlteredDepth;
            ArrayManipulation.Remove(item, ref objects);
            return true;
        }

        public bool Contains(IGUObject item) {
            for (int I = 0; I < Count; I++)
                if (item.GetInstanceID() == objects[I].GetInstanceID())
                    return true;
            return false;
        }

        public void OnIGU() => onIGU?.Invoke();
        public void AlteredDepth(List<IIGUObject> changed) => alteredDepth?.Invoke(changed, depth);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
            => (obj is int value && Equals(value)) ||
            (obj is DeepAction deep && Equals(deep.depth));

        public bool Equals(int other)
            => other == depth;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            onIGU = (Action)null;
            alteredDepth = (Action<List<IIGUObject>, int>)null;

            for (int I = 0; I < Count; I++) {
                onIGU += (objects[I] as IIGUObject).InternalOnIGU;
                alteredDepth += (objects[I] as IIGUObject).AlteredDepth;
            }
        }

        public static bool operator ==(DeepAction A, int B) => A.Equals(B);
        public static bool operator !=(DeepAction A, int B) => !(A == B);
        public static bool operator ==(int A, DeepAction B) => B.Equals(A);
        public static bool operator !=(int A, DeepAction B) => !(B == A);
    }
}
