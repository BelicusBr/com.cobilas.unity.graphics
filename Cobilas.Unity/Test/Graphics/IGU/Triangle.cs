using System;
using UnityEngine;
using Cobilas.Collections;
using System.Globalization;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public readonly struct Triangle : IEquatable<Triangle>, IFormattable {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly Vector2 c;

        public Vector2 A => a;
        public Vector2 B => b;
        public Vector2 C => c;

        public static Triangle[] Circle16 => GetCircle(22.5f);
        public static Triangle[] Circle32 => GetCircle(11.25f);
        public static Triangle[] Circle64 => GetCircle();
        public static Triangle[] Circle128 => GetCircle(2.8125f);
        public static Triangle[] Box => new Triangle[2] {
            new Triangle(Vector2.zero, Vector2.up, Vector2.one),
            new Triangle(Vector2.one, Vector2.right, Vector2.zero)
        };

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
            => ToString(null, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider)
            => string.Format(formatProvider, format ?? "A:{0} B:{1} C:{2}", a, b, c);

        public static bool InsideInTriangle(Triangle[] triangles, Vector2 position) {
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(triangles); I++)
                if (InsideInTriangle(triangles[I], position))
                    return true;
            return false;
        }

        public static bool InsideInTriangle(Triangle triangle, Vector2 position)
            => Area(triangle.a, triangle.b, triangle.c, position);

        public static bool InsideInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 position)
            => Area(a, b, c, position);

        private static bool Area(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p) {
            Vector2 center = (p1 + p2 + p3) / 3f;
            if (center == p1 || center == p2 || center == p3)
                return center == p;

            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Sign(p, p1, p2);
            d2 = Sign(p, p2, p3);
            d3 = Sign(p, p3, p1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
            => (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);

        private static Triangle[] GetCircle(float warp = 5.625f) {
            Triangle[] result = new Triangle[1];
            int count = 0;
            float rot = warp * 2f;
            Vector2 center = Vector2.one * .5f;

            Triangle triangle = new Triangle((Vector2)Quaternion.identity.GenerateDirectionUp() * .5f + center, center,
                (Vector2)Quaternion.Euler(Vector3.forward * warp).GenerateDirectionUp() * .5f + center);
            result[0] = triangle;
            while (rot < 360f) {
                Quaternion quaternion = Quaternion.Euler(Vector3.forward * rot);
                rot += warp;
                triangle = new Triangle(result[count].c, center, (Vector2)quaternion.GenerateDirectionUp() * .5f + center);
                ArrayManipulation.Add(triangle, ref result);
                count++;
            }
            triangle = new Triangle(result[result.Length - 1].c, center, result[0].a);
            ArrayManipulation.Add(triangle, ref result);
            return result;
        }

        public static bool operator ==(Triangle A, Triangle B) => A.Equals(B);
        public static bool operator !=(Triangle A, Triangle B) => !A.Equals(B);
    }
}