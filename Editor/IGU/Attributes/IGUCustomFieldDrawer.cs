using System;

namespace Cobilas.Unity.Editor.Graphics.IGU {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class IGUCustomFieldDrawerAttribute : Attribute {
        private readonly string targetField;

        public string TargetField => targetField;

        public IGUCustomFieldDrawerAttribute(string targetField) 
            => this.targetField = targetField;
    }
}
