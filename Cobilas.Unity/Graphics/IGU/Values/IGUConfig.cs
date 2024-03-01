using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU
{
    [Serializable]
    public struct IGUConfig : IEquatable<IGUConfig> {
        [SerializeField] private bool isVisible;
        [SerializeField] private bool isEnabled;
        [SerializeField] private int depth;
        [SerializeField] private MouseButtonType type;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public int Depth => depth;
        public bool IsVisible => isVisible;
        public bool IsEnabled => isEnabled;
        public MouseButtonType MouseType => type;

        public static IGUConfig Default => new IGUConfig(true, true, 0, MouseButtonType.Left);

        public IGUConfig(bool isVisible, bool isEnabled, int depth, MouseButtonType type) {
            this.isVisible = isVisible;
            this.isEnabled = isEnabled;
            this.depth = depth;
            this.type = type;
#if UNITY_EDITOR
            foldout = false;
#endif
        }
#if UNITY_EDITOR
        public IGUConfig SetFolDout(bool foldout)
        {
            this.foldout = foldout;
            return this;
        }
#endif
        public override int GetHashCode()
            => base.GetHashCode() >> isEnabled.GetHashCode() ^ isVisible.GetHashCode() << type.GetHashCode() ^ depth.GetHashCode();

        public override bool Equals(object obj)
            => obj is IGUConfig config && Equals(config);

        public bool Equals(IGUConfig other)
            => other.isVisible == isVisible && other.isEnabled == isEnabled &&
            other.depth == depth;

        public IGUConfig SetVisible(bool isVisible) {
            this.isVisible = isVisible;
            return this;
        }

        public IGUConfig SetMouseButtonType(MouseButtonType type) {
            this.type = type;
            return this;
        }

        public IGUConfig SetEnabled(bool isEnabled) {
            this.isEnabled = isEnabled;
            return this;
        }

        public IGUConfig SetDepth(int depth) {
            this.depth = depth;
            return this;
        }

        /// <summary>
        /// v:isVisible e:isEnabled d:depth
        /// </summary>
        public override string ToString()
            => $"{{(v:{isVisible} e:{isEnabled} d:{depth})}}";

        public static bool operator ==(IGUConfig A, IGUConfig B) => A.Equals(B);
        public static bool operator !=(IGUConfig A, IGUConfig B) => !(A == B);
    }
}
