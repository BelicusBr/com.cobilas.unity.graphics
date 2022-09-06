using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct IGUMouseInput {
        [SerializeField] private bool down;
        [SerializeField] private bool up;
        [SerializeField] private bool press;
        [SerializeField] private Vector2 mousePosition;

        public bool Up => up;
        public bool Down => down;
        public bool Press => press;
        public Vector2 MousePosition => mousePosition;

        public IGUMouseInput SetDown(bool down) {
            this.down = down;
            return this;
        }

        public IGUMouseInput SetUp(bool up) {
            this.up = up;
            return this;
        }

        public IGUMouseInput SetPress(bool press) {
            this.press = press;
            return this;
        }

        public IGUMouseInput SetMousePosition(Vector2 position) {
            this.mousePosition = position;
            return this;
        }

        public IGUMouseInput SetValues(bool down, bool press, bool up, Vector2 position)
            => this.SetDown(down).SetPress(press).SetUp(up).SetMousePosition(position);
    }
}
