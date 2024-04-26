using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUPhysicsClippingContainer : IIGUClippingPhysics {
        [SerializeField] private IGUObject[] objs;
        private IGUBasicPhysics.CallPhysicsFeedback callPhysicsFeedback;

        public long Count => ArrayManipulation.ArrayLongLength(objs);

        IGUConfig IIGUPhysics.LocalConfig => throw new NotImplementedException();
        bool IIGUPhysics.IsPhysicalElement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IGUBasicPhysics IIGUPhysics.Physics { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => callPhysicsFeedback?.Invoke(mouse, ref phys);

        public bool AddOtherPhysics(IGUObject obj) {
            if (Contains(obj)) return false;
            ArrayManipulation.Add(obj, ref objs);
            RefreshEvents();
            return true;
        }

        public bool RemoveOtherPhysics(IGUObject obj) {
            if (!Contains(obj)) return false;
            ArrayManipulation.Remove(obj, ref objs);
            RefreshEvents();
            return true;
        }

        public void RefreshEvents() {
            callPhysicsFeedback = (IGUBasicPhysics.CallPhysicsFeedback) null;
            for (long I = 0; I < Count; I++)
                callPhysicsFeedback += (objs[I] as IIGUPhysics).CallPhysicsFeedback;
        }

        private bool Contains(IGUObject obj) {
            if (ArrayManipulation.EmpytArray(objs)) return false;
            return ArrayManipulation.Exists(obj, objs);
        }
    }
}