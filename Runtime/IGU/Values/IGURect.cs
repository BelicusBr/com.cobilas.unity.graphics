using System;
using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public struct IGURect : IEquatable<IGURect> {
        [SerializeField] private float x;
        [SerializeField] private float y;
        [SerializeField] private float width;
        [SerializeField] private float height;
        [SerializeField] private float pivotX;
        [SerializeField] private float pivotY;
        [SerializeField] private float rotation;
        [SerializeField] private float scaleFactorWidth;
        [SerializeField] private float scaleFactorHeight;
#if UNITY_EDITOR
        [HideInInspector] public bool foldout;
#endif

        public float X => x;
        public float Y => y;
        public float Width => width;
        public float Height => height;
        public float PivotX => pivotX;
        public float PivotY => pivotY;
        public float Rotation => rotation;
        public float ScaleFactorWidth => scaleFactorWidth;
        public float ScaleFactorHeight => scaleFactorHeight;

        public Vector2 Position => new Vector2(x, y);
        public Vector2 Size => new Vector2(width, height);
        public Vector2 Pivot => new Vector2(pivotX, pivotY);
        public Vector2 ScaleFactor => new Vector2(scaleFactorWidth, scaleFactorHeight);

        public Vector2 ModifiedSize => Size.Multiplication(ScaleFactor);
        public Vector2 ModifiedPosition => Position.Multiplication(ScaleFactor) - Size.Multiplication(Pivot);

        public float Up => y;
        public float Donw => y + height;
        public float Right => x;
        public float Left => x + width;
        public Vector2 Center => new Vector2(x + width * .5f, y + height * .5f);

        public float ModifiedUp => (y * scaleFactorHeight) - height * pivotY;
        public float ModifiedDonw => ((y * scaleFactorHeight) + height) - height * pivotY;
        public float ModifiedRight => (x * scaleFactorWidth) - width * pivotX;
        public float ModifiedLeft => ((x * scaleFactorWidth) + width) - width * pivotX;
        public Vector2 ModifiedCenter => new Vector2(
            ((x * scaleFactorWidth) + width * .5f) - width * pivotX,
            ((y * scaleFactorHeight) + height * .5f) - height * pivotY
            );

        public static IGURect DefaultBox => new IGURect(Vector2.zero, new Vector2(50f, 50f), Vector2.zero, Vector2.one);
        public static IGURect DefaultButton => new IGURect(Vector2.zero, new Vector2(130f, 25f), Vector2.zero, Vector2.one);
        public static IGURect DefaultWindow => new IGURect(Vector2.zero, new Vector2(230f, 350f), Vector2.zero, Vector2.one);
        public static IGURect DefaultTextArea => new IGURect(Vector2.zero, new Vector2(250f, 250f), Vector2.zero, Vector2.one);
        public static IGURect DefaultSelectionGrid => new IGURect(Vector2.zero, new Vector2(60f, 25f), Vector2.zero, Vector2.one);

        public IGURect(float x, float y, float width, float height, float pivotX, float pivotY, float SFWidth, float SFHeight) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.scaleFactorWidth = SFWidth;
            this.scaleFactorHeight = SFHeight;
            this.pivotX = pivotX;
            this.pivotY = pivotY;
            rotation = 0;
#if UNITY_EDITOR
            foldout = false;
#endif
        }

        public IGURect(float x, float y, float width, float height, float pivotX, float pivotY)
            : this(x, y, width, height, pivotX, pivotY, 1, 1) { }

        public IGURect(float x, float y, float width, float height)
            : this(x, y, width, height, 0, 0) { }

        public IGURect(Vector2 position, Vector2 size, Vector2 pivot, Vector2 scaleFactor)
            : this(position.x, position.y, size.x, size.y, pivot.x, pivot.y, scaleFactor.x, scaleFactor.y) { }

        public IGURect(Vector2 position, Vector2 size, Vector2 pivot)
            : this(position, size, pivot, Vector2.one) { }

        public IGURect(Vector2 position, Vector2 size)
            : this(position, size, Vector2.zero) { }

        public IGURect(Rect rect, Rect pivot_SF)
            : this(rect.position, rect.size, pivot_SF.position, pivot_SF.size) { }

        public IGURect(Rect rect)
            : this(rect.position, rect.size) { }

        public IGURect(IGURect rect)
            : this(rect.x, rect.y, rect.width, rect.height, rect.pivotX, rect.pivotY, rect.scaleFactorWidth, rect.scaleFactorHeight) { }
#if UNITY_EDITOR
        public IGURect SetFolDout(bool foldout)
        {
            this.foldout = foldout;
            return this;
        }
#endif
        public IGURect SetPosition(float x, float y) {
            this.x = x;
            this.y = y;
            return this;
        }

        public IGURect SetPosition(Vector2 position)
            => SetPosition(position.x, position.y);

        public IGURect SetModifiedPosition(Vector2 position)
            => SetPosition((position + Size.Multiplication(Pivot)).Division(ScaleFactor));

        public IGURect SetSize(float width, float height) {
            this.width = width;
            this.height = height;
            return this;
        }

        public IGURect SetSize(Vector2 size)
            => SetSize(size.x, size.y);

        public IGURect SetModifiedSize(Vector2 size)
            => SetSize(size.Division(ScaleFactor));

        public IGURect SetScaleFactor(float sfWidth, float sfHeight) {
            this.scaleFactorWidth = sfWidth;
            this.scaleFactorHeight = sfHeight;
            return this;
        }

        public IGURect SetScaleFactor(Vector2 scaleFactor) {
            this.scaleFactorWidth = scaleFactor.x;
            this.scaleFactorHeight = scaleFactor.y;
            return this;
        }

        public IGURect SetPivot(float pivotX, float pivotY) {
            this.pivotX = pivotX;
            this.pivotY = pivotY;
            return this;
        }

        public IGURect SetPivot(Vector2 pivot)
            => SetPivot(pivot.x, pivot.y);

        public IGURect SetRotation(float rotation) {
            this.rotation = rotation;
            return this;
        }

        public override int GetHashCode()
            => base.GetHashCode() >> x.GetHashCode() ^ y.GetHashCode() <<
            width.GetHashCode() ^ height.GetHashCode() >> scaleFactorWidth.GetHashCode() ^
            scaleFactorHeight.GetHashCode() << pivotX.GetHashCode() ^ pivotY.GetHashCode() >>
            rotation.GetHashCode();

        public override bool Equals(object obj)
            => obj is IGURect rect && Equals(rect);

        public bool Equals(IGURect other)
            => other.x == x && other.y == y &&
            other.width == width && other.height == height &&
            other.scaleFactorWidth == scaleFactorWidth && other.scaleFactorHeight == scaleFactorHeight &&
            other.pivotX == pivotX && other.pivotY == pivotY &&
            other.rotation == rotation;

        public override string ToString()
            => $"{{(x:{x} y:{y})(w:{width} h:{height})(sfw:{scaleFactorWidth} sfh:{scaleFactorHeight})" +
            $"(px:{pivotX} py:{pivotY})(r:{rotation})}}";

        public static bool operator ==(IGURect A, IGURect B) => A.Equals(B);
        public static bool operator !=(IGURect A, IGURect B) => !(A == B);
    }
}
