using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUDefaultValue {
        private object[] values;

        public long Count => ArrayManipulation.ArrayLongLength(values);

        public IGUDefaultValue(params object[] values)
            => this.values = values;

        public IGUDefaultValue(IGUDefaultValue defaultValue) :
            this(defaultValue.values) { }

        public virtual object GetValue(long index) => values[index];
        public virtual T GetValue<T>(long index) => (T)values[index];

        public static IGUDefaultValue Merge(params IGUDefaultValue[] A) {
            IGUDefaultValue res = new IGUDefaultValue((object[])null);
            foreach (var item in A)
                ArrayManipulation.Add(item.values, ref res.values);
            return res;
        }

        public static IGUDefaultValue Merge(IGUDefaultValue A, IGUDefaultValue B)
            => Merge(A, B);
    }
}
