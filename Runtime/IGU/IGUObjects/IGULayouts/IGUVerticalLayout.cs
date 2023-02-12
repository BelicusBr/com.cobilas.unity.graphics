using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public sealed class IGUVerticalLayout : IGULayout, ISerializationCallbackReceiver {

        private bool AfterDeserialize = false;

        public override int Count => throw new NotImplementedException();

        public override IGUObject this[int index] => throw new NotImplementedException();

        protected override void OnEnable()
        {
            
        }

        public override void OnIGU() {
            
        }

        public override bool Add(IGUObject item)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(IGUObject item)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(IGUObject item)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void Clear(bool destroyObject)
        {
            throw new NotImplementedException();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
            => AfterDeserialize = true;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        [Serializable]
        private sealed class VerticalLayoutCellCursor : CellCursor {
            public float spacing;
            [SerializeField] private float posY;
            [SerializeField] private bool useCellSize;
            [SerializeField] private Vector2 cellSize;

            public override Vector2 GridRect => throw new NotImplementedException();
            public override Vector2 CellSize { get => cellSize; set => cellSize = value; }
            public override bool UseCellSize { get => useCellSize; set => useCellSize = value; }

            public override void MarkCount(IGUObject @object) {
                @object.MyRect = @object.MyRect.SetPosition(0f, posY);
                posY += spacing + @object.MyRect.Height;
            }

            public override void Reset() {
                posY = 0;
            }
        }
    }
}
