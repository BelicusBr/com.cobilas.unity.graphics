using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    [Serializable]
    public abstract class CellCursor {
        public abstract Vector2 GridRect { get; }
        public abstract Vector2 CellSize { get; set; }
        public abstract bool UseCellSize { get; set; }

        public abstract void Reset();
        public abstract void MarkCount(IGUObject @object);
    }
}
