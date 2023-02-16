using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public sealed class IGUVerticalLayout : IGULayout, ISerializationCallbackReceiver {

        [SerializeField]
        private CellIGUObject[] objects;
        [SerializeField]
        private bool AfterDeserialize = false;
        private VerticalLayoutCellCursor cursor;
        private event Action<CellCursor> sub_OnIGU;

        public Vector2 GridRect => cursor.GridRect;
        public override int Count => ArrayManipulation.ArrayLength(objects);
        public float Spacing { get => cursor.spacing; set => cursor.spacing = value; }
        public Vector2 CellSize { get => cursor.CellSize; set => cursor.CellSize = value; }
        public bool UseCellSize { get => cursor.UseCellSize; set => cursor.UseCellSize = value; }

        public override IGUObject this[int index] => objects[index].@object;

        protected override void OnEnable() {
            if (!AfterDeserialize) return;
            AfterDeserialize = false;
            RefreshOnIGUEvent();
        }

        public override void OnIGU() {
            IGUConfig config = GetModIGUConfig();
            if (!config.IsVisible) return;

            cursor.Reset();
            BeginDoNotModifyRect(false);
            sub_OnIGU?.Invoke(cursor);
            EndDoNotModifyRect();
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

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => AfterDeserialize = true;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        public static IGUVerticalLayout CreateIGUInstance(string name, float spacing, Vector2 cellSize, bool useCellSize) {
            IGUVerticalLayout res = Internal_CreateIGUInstance<IGUVerticalLayout>(name);
            res.cursor = new VerticalLayoutCellCursor();
            res.Spacing = spacing;
            res.CellSize = cellSize;
            res.UseCellSize = useCellSize;
            res.myConfg = IGUConfig.Default;
            res.myRect = IGURect.DefaultButton;
            res.myColor = IGUColor.DefaultBoxColor;
            return res;
        }

        public static IGUVerticalLayout CreateIGUInstance(string name, float spacing, Vector2 cellSize)
            => CreateIGUInstance(name, spacing, cellSize, false);

        public static IGUVerticalLayout CreateIGUInstance(string name, float spacing)
            => CreateIGUInstance(name, spacing, Vector2.one * 100f);

        public static IGUVerticalLayout CreateIGUInstance(string name)
            => CreateIGUInstance(name, 3f);

        [Serializable]
        private sealed class VerticalLayoutCellCursor : CellCursor {
            public float spacing;
            [SerializeField] private float posY;
            [SerializeField] private bool useCellSize;
            [SerializeField] private Vector2 cellSize;
            [SerializeField] private Vector2 gridRect;

            public override Vector2 GridRect => gridRect;
            public override Vector2 CellSize { get => cellSize; set => cellSize = value; }
            public override bool UseCellSize { get => useCellSize; set => useCellSize = value; }

            public override void MarkCount(IGUObject @object) {
                @object.MyRect = @object.MyRect.SetPosition(0f, posY);
                posY += spacing + @object.MyRect.Height;
                gridRect.x = @object.MyRect.Width > gridRect.x ? @object.MyRect.Width : gridRect.x;
                gridRect.y = posY;
            }

            public override void Reset() {
                posY = 0;
            }
        }
    }
}
