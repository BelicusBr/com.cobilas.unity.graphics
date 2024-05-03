﻿using System;
using UnityEngine;
using System.Globalization;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct MaxMinSliderInt : IEquatable<MaxMinSliderInt>, IFormattable {
        [SerializeField] private int min;
        [SerializeField] private int max;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public int Min => min;
        public int Max => max;

        public static MaxMinSlider Zero => new MaxMinSlider(0f, 0f);
        /// <summary>min:0 max:130</summary>
        public static MaxMinSliderInt Default => new MaxMinSliderInt(0, 130);

        public MaxMinSliderInt(int min, int max) {
            this.min = min;
            this.max = max;
#if UNITY_EDITOR
            foldout = false;
#endif
        }

        public override int GetHashCode()
            => base.GetHashCode() >> min.GetHashCode() ^ max.GetHashCode();

        public override bool Equals(object obj)
            => obj is MaxMinSliderInt mms && Equals(mms);

        public bool Equals(MaxMinSliderInt other)
            => other.max == max && other.min == min;

        public MaxMinSliderInt Set(Vector2Int minmax)
            => Set(minmax.x, minmax.y);

        public MaxMinSliderInt Set(int min, int max) {
            this.min = min;
            this.max = max;
            return this;
        }

        public override string ToString()
            => ToString("N", CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) {
            string num1 = min.ToString(format, formatProvider);
            string num2 = max.ToString(format, formatProvider);
            return string.Format("(Min:{0} Max{1})", num1, num2);
        }

        public static bool operator ==(MaxMinSliderInt A, MaxMinSliderInt B) => A.Equals(B);
        public static bool operator !=(MaxMinSliderInt A, MaxMinSliderInt B) => !(A == B);

        public static implicit operator MaxMinSlider(MaxMinSliderInt M)
#if UNITY_EDITOR
            => new MaxMinSlider(M.min, M.max) {
                foldout = M.foldout
            };
#else
            => new MaxMinSlider(M.min, M.max);
#endif
    }
}
