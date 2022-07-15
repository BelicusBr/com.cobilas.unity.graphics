using System;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class IGUCustomDrawerAttribute : Attribute {
        private Type typeTarget;
        private bool useForChildren;

        public Type TypeTarget => typeTarget;
        public bool UseForChildren => useForChildren;

        public IGUCustomDrawerAttribute(Type typeTarget, bool useForChildren) {
            this.typeTarget = typeTarget;
            this.useForChildren = useForChildren;
        }

        public IGUCustomDrawerAttribute(Type typeTarget) : this(typeTarget, false) { }
    }
}