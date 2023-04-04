using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUCheckBoxDefault : IGUDefaultValue {
        public static IGUDefaultValue DefaultValue => new IGUCheckBoxDefault("ele_IGUCheckBox");

        public IGUCheckBoxDefault(string name, bool useTooltip, IGUContent content, bool _checked, GUIStyle style) : base(name, useTooltip, content, _checked, style) { }
        public IGUCheckBoxDefault(string name, bool useTooltip, string content, bool _checked, GUIStyle style) : this(name, useTooltip, new IGUContent(content), _checked, style) { }
        public IGUCheckBoxDefault(string name, bool useTooltip, IGUContent content, bool _checked) : this(name, useTooltip, content, _checked, (GUIStyle)null) { }
        public IGUCheckBoxDefault(string name, bool useTooltip, string content, bool _checked) : this(name, useTooltip, new IGUContent(content), _checked, (GUIStyle)null) { }
        public IGUCheckBoxDefault(string name, bool useTooltip, IGUContent content) : this(name, useTooltip, content, false) { }
        public IGUCheckBoxDefault(string name, bool useTooltip, string content) : this(name, useTooltip, new IGUContent(content), false) { }
        public IGUCheckBoxDefault(string name, bool useTooltip) : this(name, useTooltip, IGUCheckBox.DefaultContantIGUCheckBox) { }
        public IGUCheckBoxDefault(string name) : this(name, false) { }


        public static implicit operator IGUCheckBoxDefault((string, bool, IGUContent, bool, GUIStyle) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5);
        public static implicit operator IGUCheckBoxDefault((string, bool, string, bool, GUIStyle) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5);
        public static implicit operator IGUCheckBoxDefault((string, bool, IGUContent, bool) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGUCheckBoxDefault((string, bool, string, bool) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGUCheckBoxDefault((string, bool, IGUContent) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUCheckBoxDefault((string, bool, string) A) => new IGUCheckBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUCheckBoxDefault((string, bool) A) => new IGUCheckBoxDefault(A.Item1, A.Item2);
    }
}
