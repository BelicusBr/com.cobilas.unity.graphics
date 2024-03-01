using System;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU {
    internal sealed class WindowStatus {
        public int IDFocus;
        public int CurrentID;
        public Vector2 CurrentPosition;
        public Action<int, Vector2> winFunc;
        public BackEndIGU.WindowFocusStatus FocusStatus;

        public void ClippingFunc(Vector2 scrollOffset)
            => winFunc(IDFocus, scrollOffset);
    }
}