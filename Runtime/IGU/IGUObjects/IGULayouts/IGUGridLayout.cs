using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public sealed class IGUGridLayout : IGULayout, ISerializationCallbackReceiver {

        [SerializeField] 
        private CellIGUObject[] objects;
        [SerializeField] 
        private GridLayoutCellCursor cursor;
        private bool AfterDeserialize = false;
        private event Action<CellCursor> sub_OnIGU;

        public override int Count => ArrayManipulation.ArrayLength(objects);
        public Vector2 Spacing { get => cursor.spacing; set => cursor.spacing = value; }
        public Vector2 CellSize { get => cursor.CellSize; set => cursor.CellSize = value; }
        public int DirectionalCount { get => cursor.directionalCount; set => cursor.directionalCount = value; }
        public DirectionalBreak DirectionalBreak { get => cursor.directionalBreak; set => cursor.directionalBreak = value; }

        public override IGUObject this[int index] => objects[index].@object;

        protected override void Awake() {
            cursor = new GridLayoutCellCursor();
            DirectionalCount = 3;
            cursor.UseCellSize = true;
            Spacing = Vector2.one * 3f;
            myConfg = IGUConfig.Default;
            CellSize = Vector2.one * 100f;
            myRect = IGURect.DefaultButton;
            myColor = IGUColor.DefaultBoxColor;
            DirectionalBreak = DirectionalBreak.VerticalBreak;
        }

        protected override void OnEnable() {
            if (!AfterDeserialize) return;
            AfterDeserialize = false;
            RefreshOnIGUEvent();
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;

            cursor.Reset();
            cursor.elementCount = Count;
            sub_OnIGU?.Invoke(cursor);
            myRect.SetSize(cursor.GridRect);
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

        public override bool Remove(IGUObject item) {
            if (item == this || !Contains(item))
                return false;
            item.Parent = null;
            int index = IndexOf(item);
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
            for (int I = 0; I < Count; I++)
                sub_OnIGU += objects[I].OnIGU;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => AfterDeserialize = true;

        [Serializable]
        private sealed class GridLayoutCellCursor : CellCursor {
            public Vector2 spacing;
            public int elementCount;
            public int directionalCount;
            public DirectionalBreak directionalBreak;
            [SerializeField] private Vector2 cursor;
            [SerializeField] private int cursorCount;
            [SerializeField] private Vector2 cellSize;
            [SerializeField] private bool useCellSize;

            public override Vector2 GridRect => GetGridRect();
            public override Vector2 CellSize { get => cellSize; set => cellSize = value; }
            public override bool UseCellSize { get => useCellSize; set => useCellSize = value; }

            public override void Reset() {
                cursorCount = 0;
                cursor = Vector2.zero;
            }

            public override void MarkCount(IGUObject @object) {
                Vector2 vec1 = Vector2.zero,
                        vec2 = Vector2.zero;
                switch (directionalBreak) {
                    case DirectionalBreak.VerticalBreak:
                        vec1 = Vector2.up;
                        vec2 = Vector2.right;
                        break;
                    case DirectionalBreak.HorizontalBreak:
                        vec1 = Vector2.right;
                        vec2 = Vector2.up;
                        break;
                }

                if (cursorCount >= directionalCount) {
                    cursor = vec2.Multiplication(cursor);
                    cursor += vec2.Multiplication(cellSize) + vec2.Multiplication(spacing);
                    cursorCount = 0;
                }

                @object.MyRect = @object.MyRect.SetPosition(cursor);
                cursor += vec1.Multiplication(@object.MyRect.Size) + spacing.Multiplication(vec1);
                ++cursorCount;
            }

            private Vector2 GetGridRect() {
                Vector2 vec1 = Vector2.zero,
                    vec2 = Vector2.zero;
                switch (directionalBreak) {
                    case DirectionalBreak.VerticalBreak:
                        vec1 = Vector2.up;
                        vec2 = Vector2.right;
                        break;
                    case DirectionalBreak.HorizontalBreak:
                        vec1 = Vector2.right;
                        vec2 = Vector2.up;
                        break;
                }
                vec1 *= directionalCount;
                vec2 *= Mathf.CeilToInt(elementCount / (float)directionalCount);
                return (vec1 + vec2).Multiplication(cellSize) + (vec1 + vec2).Multiplication(spacing);
            }
        }
    }
}
