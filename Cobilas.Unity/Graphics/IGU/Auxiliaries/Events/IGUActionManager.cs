using System;

namespace Cobilas.Unity.Graphics.IGU.Events {
    [Serializable]
    public abstract class IGUActionManager<A, T> where A : Delegate {
        public abstract A Function { get; }
        public abstract int RegisteredCount { get; }
        public abstract Func<T, A> RefreshFunction { get; set; }

        public abstract T this[int index] { get; }

        public abstract void Clear();
        public abstract bool Contains(T item);
    }
}