using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU;
using Cobilas.Unity.Graphics.IGU.Elements;
using System.Collections.Generic;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public sealed class IGUGridLayout : IGULayout, ISerializationCallbackReceiver {

        private Vector2 spacing;
        private Vector2 cellSize;
        private bool useCellSize;
        private IGUObject[] objects;
        private Vector2 cellSizeTemp;
        private int directionalCount;
        private event Action sub_OnIGU;
        private bool AfterDeserialize = false;
        private DirectionalBreak directionalBreak;

        public Vector2 Spacing { get => spacing; set => spacing = value; }
        public override int Count => ArrayManipulation.ArrayLength(objects);
        public Vector2 CellSize { get => cellSize; set => cellSize = value; }
        public bool UseCellSize { get => useCellSize; set => useCellSize = value; }
        public int DirectionalCount { get => directionalCount; set => directionalCount = value; }
        public DirectionalBreak DirectionalBreak { get => directionalBreak; set => directionalBreak = value; }

        public override IGUObject this[int index] => objects[index];

        protected override void OnEnable() {
            if (!AfterDeserialize) return;
            AfterDeserialize = false;
            RefreshOnIGUEvent();
        }

        public override void OnIGU() {
            
        }

        public override bool Contains(IGUObject item)
            => ArrayManipulation.Exists(item, objects);

        public override bool Add(IGUObject item) {
            if (item == this || Contains(item))
                return false;
            item.Parent = this;
            ArrayManipulation.Add(item, ref objects);
            RefreshOnIGUEvent();
            return true;
        }

        public override bool Remove(IGUObject item) {
            if (item == this || !Contains(item))
                return false;
            item.Parent = null;
            ArrayManipulation.Remove(item, ref objects);
            RefreshOnIGUEvent();
            return true;
        }

        private void RefreshOnIGUEvent() {
            sub_OnIGU = (Action)null;
            for (int I = 0; I < Count; I++)
                sub_OnIGU += objects[I].OnIGU;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => AfterDeserialize = true;

        public static IGUGridLayout CreateIGUInstance(string name)
            => Internal_CreateIGUInstance<IGUGridLayout>(name);

        private sealed class CellCursor {
            private int dirCount;
            private Vector2 cursor;


        }

        private sealed class CellIGUObject : IEquatable<IGUObject> {
            public IGUObject @object;
            private Vector2 mySize;

            public CellIGUObject(IGUObject @object) {
                this.@object = @object;
                this.mySize = @object.MyRect.Size;
            }

            public override bool Equals(object obj)
                => obj is IGUObject iguobj && Equals(iguobj);

            public bool Equals(IGUObject other)
                => this.@object == other;

            public override int GetHashCode()
                => @object.GetHashCode();

            public static bool operator ==(CellIGUObject A, IGUObject B)
                => (object)A != null && (object)B != null && A.Equals(B);

            public static bool operator !=(CellIGUObject A, IGUObject B)
                => !(A == B);

            public static bool operator ==(IGUObject A, CellIGUObject B)
                => (object)A != null && (object)B != null && B.Equals(A);

            public static bool operator !=(IGUObject A, CellIGUObject B)
                => !(A == B);
        }
    }
}
