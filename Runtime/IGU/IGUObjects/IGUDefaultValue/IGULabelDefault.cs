using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGULabelDefault : IGUDefaultValue {
        public static IGUDefaultValue DefaultValue => new IGULabelDefault("ele_IGULabel");

        public IGULabelDefault(string name, bool useTooltip, bool autoSize, bool richText, IGUContent content, GUIStyle style) : base(name, useTooltip, autoSize, richText, content, style) { }
        public IGULabelDefault(string name, bool useTooltip, bool autoSize, bool richText, string content, GUIStyle style) : this(name, useTooltip, autoSize, richText, new IGUContent(content), style) { }
        public IGULabelDefault(string name, bool useTooltip, bool autoSize, bool richText, IGUContent content) : this(name, useTooltip, autoSize, richText, content, (GUIStyle)null) { }
        public IGULabelDefault(string name, bool useTooltip, bool autoSize, bool richText, string content) : this(name, useTooltip, autoSize, richText, new IGUContent(content), (GUIStyle)null) { }
        public IGULabelDefault(string name, bool useTooltip, bool autoSize, bool richText) : this(name, useTooltip, autoSize, richText, IGULabel.DefaultIGULabel) { }
        public IGULabelDefault(string name, bool useTooltip, bool autoSize) : this(name, useTooltip, autoSize, false) { }
        public IGULabelDefault(string name, bool useTooltip) : this(name, useTooltip, false) { }
        public IGULabelDefault(string name) : this(name, false) { }


        public static implicit operator IGULabelDefault((string, bool, bool, bool, IGUContent, GUIStyle) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5, A.Item6);
        public static implicit operator IGULabelDefault((string, bool, bool, bool, string, GUIStyle) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5, A.Item6);
        public static implicit operator IGULabelDefault((string, bool, bool, bool, IGUContent) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5);
        public static implicit operator IGULabelDefault((string, bool, bool, bool, string) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5);
        public static implicit operator IGULabelDefault((string, bool, bool, bool) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGULabelDefault((string, bool, bool) A) => new IGULabelDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGULabelDefault((string, bool) A) => new IGULabelDefault(A.Item1, A.Item2);
    }
}
