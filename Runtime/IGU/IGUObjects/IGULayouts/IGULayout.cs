using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU.Layouts {
    public abstract class IGULayout : IGUObject {

        public abstract int Count { get; }

        public abstract IGUObject this[int index] { get; }

        public abstract bool Add(IGUObject item);
        public abstract bool Remove(IGUObject item);
        public abstract bool Contains(IGUObject item);
    }
}