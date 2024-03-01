using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.Resolutions {
    using UEResolution = UnityEngine.Resolution;
    [Serializable]
    public struct Resolution : IEquatable<Resolution> {
        [SerializeField] private int width;
        [SerializeField] private int height;

        public int Width => width;
        public int Height => height;

        public Resolution(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public Resolution(Resolution resolution) : this(resolution.width, resolution.height) { }

        public Resolution(UEResolution resolution) : this(resolution.width, resolution.height) { }

        public override int GetHashCode()
            => base.GetHashCode() >> width.GetHashCode() ^ height.GetHashCode();

        public override bool Equals(object obj)
            => obj is Resolution res && Equals(res);

        public bool Equals(Resolution other)
            => other.width == width && other.height == height;

        public override string ToString()
            => $"{width}X{height}";

        public static bool operator ==(Resolution A, Resolution B)
            => A.Equals(B);

        public static bool operator !=(Resolution A, Resolution B)
            => !(A == B);
    }
}
