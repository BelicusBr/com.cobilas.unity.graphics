using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.Resolutions {
    [Serializable]
    public struct AspectRatio : IEquatable<AspectRatio> {
        [SerializeField] private int width;
        [SerializeField] private int height;

        public int Width => width;
        public int Height => height;

        public static AspectRatio Zero => new AspectRatio(0, 0);

        public AspectRatio(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public AspectRatio(AspectRatio aspect) : this(aspect.width, aspect.height) { }

        public override int GetHashCode()
            => base.GetHashCode() >> width.GetHashCode() ^ height.GetHashCode();

        public override bool Equals(object obj)
            => obj is AspectRatio res && Equals(res);

        public bool Equals(AspectRatio other)
            => other.width == width && other.height == height;

        public override string ToString()
            => $"{width}:{height}";

        public static bool operator ==(AspectRatio A, AspectRatio B)
            => A.Equals(B);

        public static bool operator !=(AspectRatio A, AspectRatio B)
            => !(A == B);
    }
}
