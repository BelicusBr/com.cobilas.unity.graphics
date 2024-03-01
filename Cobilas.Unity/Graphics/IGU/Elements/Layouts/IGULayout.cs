using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public abstract class IGULayout : IGUObject {

        public abstract int Count { get; }

        public abstract IGUObject this[int index] { get; }

        public abstract void Clear();
        public abstract bool Remove(int index);
        public abstract bool Add(IGUObject item);
        public abstract bool Remove(IGUObject item);
        public abstract bool Contains(IGUObject item);
        public abstract void Clear(bool destroyObject);
        public abstract bool Remove(int index, bool destroyObject);
        public abstract bool Remove(IGUObject item, bool destroyObject);
    }
}