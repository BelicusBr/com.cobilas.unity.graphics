using System;
using UnityEngine;
using System.Globalization;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public readonly struct Triangle : IEquatable<Triangle>, IFormattable {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly Vector2 c;

        public Vector2 A => a;
        public Vector2 B => b;
        public Vector2 C => c;

        public Triangle(Vector2 a, Vector2 b, Vector2 c) {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public bool Equals(Triangle other)
            => other.a == a && other.b == b && other.c == c;

        public override bool Equals(object obj)
            => obj is Triangle tg && Equals(tg);

        public override int GetHashCode()
            => a.GetHashCode() >> b.GetHashCode() ^ c.GetHashCode();

        public override string ToString()
            => ToString("A:{0} B:{1} C:{2}", CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider)
            => string.Format(formatProvider, format, a, b, c);

        public static bool operator ==(Triangle A, Triangle B) => A.Equals(B);
        public static bool operator !=(Triangle A, Triangle B) => !A.Equals(B);
    }
}