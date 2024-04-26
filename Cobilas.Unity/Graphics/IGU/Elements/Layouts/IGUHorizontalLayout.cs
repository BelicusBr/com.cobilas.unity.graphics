using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public sealed class IGUHorizontalLayout : IGULayout, IIGUSerializationCallbackReceiver {
        private event Action<CellCursor> sub_OnIGU;
        [SerializeField] private CellIGUObject[] objects;
        [SerializeField] private IGUBasicPhysics physics;
        [SerializeField] private HorizontalLayoutCellCursor cursor;
        private IGUBasicPhysics.CallPhysicsFeedback callPhysicsFeedback;

        public Vector2 GridRect => cursor.GridRect;
        public override int Count => ArrayManipulation.ArrayLength(objects);
        public float Spacing { get => cursor.spacing; set => cursor.spacing = value; }
        public Vector2 CellSize { get => cursor.CellSize; set => cursor.CellSize = value; }
        public override IGUBasicPhysics Physics { get => physics; set => physics = value; }
        public bool UseCellSize { get => cursor.UseCellSize; set => cursor.UseCellSize = value; }

        public override IGUObject this[int index] => objects[index].@object;

        protected override void IGUAwake() {
            base.IGUAwake();
            //isPhysicalElement = false;
            physics = IGUBasicPhysics.Create<IGUBoxPhysics>(this, true);
            cursor = new HorizontalLayoutCellCursor();
            Spacing = 3f;
            UseCellSize = false;
            CellSize = Vector2.one * 100f;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
        }

        protected override void LowCallOnIGU() {
            cursor.Reset();
            sub_OnIGU?.Invoke(cursor);
            myRect = myRect.SetSize(cursor.GridRect);
        }

        public override bool Contains(IGUObject item)
            => IndexOf(item) >= 0;

        public override bool Add(IGUObject item) {
            if (item == this || Contains(item))
                return false;
            item.Parent = this;
            ArrayManipulation.Add(new CellIGUObject(item), ref objects);
            RefreshOnIGUEvent();
            return true;
        }

        public override bool Remove(IGUObject item)
            => Remove(item, false);

        public override bool Remove(IGUObject item, bool destroyObject)
            => Remove(IndexOf(item), destroyObject);

        public override bool Remove(int index)
            => Remove(index, false);

        public override bool Remove(int index, bool destroyObject) {
            if (index < 0 || index >= Count) return false;
            objects[index].@object.Parent = null;
            if (destroyObject)
                Destroy(objects[index].@object);
            objects[index].Dispose();
            ArrayManipulation.Remove(index, ref objects);
            RefreshOnIGUEvent();
            return true;
        }

        public override void Clear(bool destroyObject) {
            for (int I = 0; I < Count; I++) {
                objects[I].@object.Parent = null;
                objects[I].Dispose();
                if (destroyObject)
                    Destroy(objects[I].@object);
            }
            ArrayManipulation.ClearArraySafe(ref objects);
            RefreshOnIGUEvent();
        }

        public override void Clear()
            => Clear(false);

        private int IndexOf(IGUObject item) {
            for (int I = 0; I < Count; I++)
                if (objects[I] == item)
                    return I;
            return -1;
        }

        private void RefreshOnIGUEvent() {
            sub_OnIGU = (Action<CellCursor>)null;
            callPhysicsFeedback = (IGUBasicPhysics.CallPhysicsFeedback)null;
            for (int I = 0; I < Count; I++) {
                sub_OnIGU += objects[I].OnIGU;
                callPhysicsFeedback += (objects[I].@object as IIGUPhysics).CallPhysicsFeedback;
            }
        }

        protected override void InternalCallPhysicsFeedback(Vector2 mouse, ref IGUBasicPhysics phys)
            => callPhysicsFeedback?.Invoke(mouse, ref phys);

        void IIGUSerializationCallbackReceiver.Reserialization() {
#if UNITY_EDITOR
            RefreshOnIGUEvent();
#endif
        }

        [Serializable]
        private sealed class HorizontalLayoutCellCursor : CellCursor {
            public float spacing;
            [SerializeField] private float posW;
            [SerializeField] private bool useCellSize;
            [SerializeField] private Vector2 cellSize;
            [SerializeField] private Vector2 gridRect;

            public override Vector2 GridRect => gridRect;
            public override Vector2 CellSize { get => cellSize; set => cellSize = value; }
            public override bool UseCellSize { get => useCellSize; set => useCellSize = value; }

            public override void MarkCount(IGUObject @object) {
                @object.MyRect = @object.LocalRect.SetPosition(posW, 0f);
                posW += spacing + @object.MyRect.Width;
                gridRect.x = posW;
                gridRect.y = @object.MyRect.Height > gridRect.y ? @object.MyRect.Height : gridRect.y;
            }

            public override void Reset()
                => posW = 0;
        }
    }
}
