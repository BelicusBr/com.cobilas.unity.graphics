using System;
using UnityEngine;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Graphics.IGU {
    internal sealed class WindowStatus {
        public int IDFocus;
        public int CurrentID;
        public Vector2 CurrentPosition;
        public Action<int, Vector2> winFunc;
        public WindowFocusStatus FocusStatus;

        public void ClippingFunc(Vector2 scrollOffset)
            => winFunc(IDFocus, scrollOffset);
    }
}