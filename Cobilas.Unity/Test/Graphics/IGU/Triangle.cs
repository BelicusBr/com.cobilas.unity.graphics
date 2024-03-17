using System;
using UnityEngine;
using Cobilas.Collections;
using System.Globalization;
using Cobilas.Unity.Graphics.IGU;

namespace Cobilas.Unity.Test.Graphics.IGU {
    public readonly struct Triangle : IEquatable<Triangle>, IFormattable {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly Vector2 c;

        public Vector2 A => a;
        public Vector2 B => b;
        public Vector2 C => c;

        public static Triangle[] Box => new Triangle[2] {
            new Triangle(Vector2.up, Vector2.zero, Vector2.right),
            new Triangle(Vector2.up, Vector2.one, Vector2.right)
        };

        public static Triangle[] Circle {
            get {
                Triangle[] result = new Triangle[1];
                int count = 0;
                float warp = 5.625f;
                float rot = warp * 2f;
                Vector2 center = Vector2.one;

                Triangle triangle = new Triangle((Vector2)Quaternion.identity.GenerateDirectionUp() + center, center,
                    (Vector2)Quaternion.Euler(Vector3.forward * warp).GenerateDirectionUp() + center);
                result[0] = triangle;
                while (rot < 360f) {
                    Quaternion quaternion = Quaternion.Euler(Vector3.forward * rot);
                    rot += warp;
                    triangle = new Triangle(result[count].c, center, (Vector2)quaternion.GenerateDirectionUp() + center);
                    ArrayManipulation.Add(triangle, ref result);
                    count++;
                }
                triangle = new Triangle(result[result.Length - 1].c, center, result[0].a);
                ArrayManipulation.Add(triangle, ref result);
                return result;
            }
        }

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

        public static bool InsideInTriangle(Triangle triangle, IGURect rect, Vector2 position) {
            Quaternion quaternion = Quaternion.Euler(Vector3.forward * rect.Rotation);
            Vector2 a = quaternion.GenerateDirection(triangle.A) * rect.Size + rect.Position;
            Vector2 b = quaternion.GenerateDirection(triangle.B) * rect.Size + rect.Position;
            Vector2 c = quaternion.GenerateDirection(triangle.C) * rect.Size + rect.Position;
            return Area(b, c, a, position);
        }

        public static bool InsideInTriangle(Triangle[] triangles, Vector2 position) {
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(triangles); I++)
                if (InsideInTriangle(triangles[I], position))
                    return true;
            return false;
        }

        public static bool InsideInTriangle(Triangle triangle, Vector2 position)
            => Area(triangle.b, triangle.c, triangle.a, position);

        public static bool InsideInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 position)
            => Area(b, c, a, position);

        private static bool Area(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p) {
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

        public static bool operator ==(Triangle A, Triangle B) => A.Equals(B);
        public static bool operator !=(Triangle A, Triangle B) => !A.Equals(B);
    }
}