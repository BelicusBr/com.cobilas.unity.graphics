namespace Cobilas.Unity.Graphics.IGU {
    public sealed class HashCodeCompare {
        private int[] list;

        public HashCodeCompare(int capacity) => list = new int[capacity];

        public bool HashCodeEqual(int index, int ComparisonHashCode)
            => list[index] == (list[index] = ComparisonHashCode);
    }
}
