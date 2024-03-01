using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUStyleRectOffSet {
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool foldout;
#endif
        //x = left
        //y = right
        [SerializeField]
        private Vector2Int rectOffSet_xy;
        //x = top
        //y = bottom
        [SerializeField]
        private Vector2Int rectOffSet_zw;

        public int Left { get => rectOffSet_xy.x; set => rectOffSet_xy.x = value; }
        public int Right { get => rectOffSet_xy.y; set => rectOffSet_xy.y = value; }
        public int Top { get => rectOffSet_zw.x; set => rectOffSet_zw.x = value; }
        public int Bottom { get => rectOffSet_zw.y; set => rectOffSet_zw.y = value; }

        public RectInt RectOffset { 
            get => new RectInt(rectOffSet_xy, rectOffSet_zw);
            set {
                rectOffSet_xy = value.position;
                rectOffSet_zw = value.size;
            }
        }

        public IGUStyleRectOffSet(int left, int right, int top, int bottom) {
            rectOffSet_xy.x = left;
            rectOffSet_xy.y = right;
            rectOffSet_zw.x = top;
            rectOffSet_zw.y = bottom;
        }

        public IGUStyleRectOffSet() : this(0, 0, 0, 0) { }

        public static explicit operator RectOffset(IGUStyleRectOffSet A)
            => new RectOffset(
                A.rectOffSet_xy.x, A.rectOffSet_xy.y,
                A.rectOffSet_zw.x, A.rectOffSet_zw.y
                );
    }
}
