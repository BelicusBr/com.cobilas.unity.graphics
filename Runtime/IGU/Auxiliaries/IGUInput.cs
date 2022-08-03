using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct IGUMouseInput {
        [SerializeField] private bool down;
        [SerializeField] private bool up;
        [SerializeField, HideInInspector] private bool oldDown;

        public bool Down => down;
        public bool Up => up;
        public bool Press => !up && oldDown;

        public static IGUMouseInput MouseUp => new IGUMouseInput(true, false);
        public static IGUMouseInput MouseDown => new IGUMouseInput(false, true);
        public static IGUMouseInput UseMouseDown => new IGUMouseInput(false, false, true);
        public static IGUMouseInput MouseNone => new IGUMouseInput(false, false);

        public IGUMouseInput(bool up, bool down, bool oldDown) {
            this.up = up;
            this.down = down;
            this.oldDown = oldDown;
        }

        public IGUMouseInput(bool up, bool down) : this(up, down, false) { }
    }
}
