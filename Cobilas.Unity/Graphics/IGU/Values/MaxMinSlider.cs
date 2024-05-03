using System;
using UnityEngine;
using System.Globalization;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct MaxMinSlider : IEquatable<MaxMinSlider>, IEquatable<MaxMinSliderInt>, IFormattable {
        [SerializeField] private float min;
        [SerializeField] private float max;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public float Min => min;
        public float Max => max;

        public static MaxMinSlider Zero => new MaxMinSlider(0f, 0f);
        /// <summary>min:0f max:130f</summary>
        public static MaxMinSlider Default => new MaxMinSlider(0f, 130f);

        public MaxMinSlider(float min, float max) {
            this.min = min;
            this.max = max;
#if UNITY_EDITOR
            foldout = false;
#endif
        }

        public MaxMinSliderInt ToMaxMinSliderInt()
#if UNITY_EDITOR
            => new MaxMinSliderInt((int)min, (int)max) {
                foldout = this.foldout
            };
#else
            => new MaxMinSliderInt((int)min, (int)max);
#endif

        public MaxMinSlider Set(Vector2Int minmax)
            => Set(minmax.x, minmax.y);

        public MaxMinSlider Set(Vector2 minmax)
            => Set(minmax.x, minmax.y);

        public MaxMinSlider Set(float min, float max) {
            this.min = min;
            this.max = max;
            return this;
        }

        public override int GetHashCode()
            => base.GetHashCode() >> min.GetHashCode() ^ max.GetHashCode();

        public override bool Equals(object obj)
            => obj is MaxMinSlider mms && Equals(mms);

        public bool Equals(MaxMinSlider other)
            => other.max == max && other.min == min;

        public bool Equals(MaxMinSliderInt other)
            => other.Max == max && other.Min == min;

        public override string ToString()
            => ToString("N", CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) {
            string num1 = min.ToString(format, formatProvider);
            string num2 = max.ToString(format, formatProvider);
            return string.Format("(Min:{0} Max{1})", num1, num2);
        }

        public static bool operator ==(MaxMinSlider A, MaxMinSlider B) => A.Equals(B);
        public static bool operator !=(MaxMinSlider A, MaxMinSlider B) => !(A == B);
        public static bool operator ==(MaxMinSlider A, MaxMinSliderInt B) => A.Equals(B);
        public static bool operator !=(MaxMinSlider A, MaxMinSliderInt B) => !(A == B);
        public static bool operator ==(MaxMinSliderInt A, MaxMinSlider B) => B.Equals(A);
        public static bool operator !=(MaxMinSliderInt A, MaxMinSlider B) => !(B == A);
        public static explicit operator MaxMinSliderInt(MaxMinSlider A) => A.ToMaxMinSliderInt();
    }
}
