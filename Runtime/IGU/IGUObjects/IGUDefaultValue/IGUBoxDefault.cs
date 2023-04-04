using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUBoxDefault : IGUDefaultValue {

        public static IGUDefaultValue DefaultValue => new IGUBoxDefault("ele_IGUBox");
        public static IGUDefaultValue ButtonDefaultValue => new IGUBoxDefault("ele_IGUButton", false, IGUButton.DefaultContentIGUButton);
        public static IGUDefaultValue RepeatButtonDefaultValue => new IGUBoxDefault("ele_IGURepeatButton", false, IGURepeatButton.DefaultContentIGURepeatButton);

        public IGUBoxDefault(string name, bool useTooltip, IGUContent content, GUIStyle style) : base(name, useTooltip, content, style) { }
        public IGUBoxDefault(string name, bool useTooltip, string content, GUIStyle style) : this(name, useTooltip, new IGUContent(content), style) { }
        public IGUBoxDefault(string name, bool useTooltip, IGUContent content) : this(name, useTooltip, content, (GUIStyle)null) { }
        public IGUBoxDefault(string name, bool useTooltip, string content) : this(name, useTooltip, new IGUContent(content), (GUIStyle)null) { }
        public IGUBoxDefault(string name, bool useTooltip) : this(name, useTooltip, IGUBox.DefaultIGUBox) { }
        public IGUBoxDefault(string name) : this(name, false) { }


        public static implicit operator IGUBoxDefault((string, bool, IGUContent, GUIStyle) A) => new IGUBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGUBoxDefault((string, bool, string, GUIStyle) A) => new IGUBoxDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGUBoxDefault((string, bool, IGUContent) A) => new IGUBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUBoxDefault((string, bool, string) A) => new IGUBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUBoxDefault((string, bool) A) => new IGUBoxDefault(A.Item1, A.Item2);
    }
}
