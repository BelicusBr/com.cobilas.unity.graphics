using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUScrollViewDefault : IGUDefaultValue {

        public static IGUDefaultValue DefaultValue => new IGUScrollViewDefault("ele_IGUScrollView");

        public IGUScrollViewDefault(string name, Rect viewRect, bool alwaysShowVertical, bool alwaysShowHorizontal, GUIStyle verticalScrollbarStyle, GUIStyle horizontalScrollbarStyle) 
            : base(name, viewRect, alwaysShowVertical, alwaysShowHorizontal, verticalScrollbarStyle, horizontalScrollbarStyle) { }
        public IGUScrollViewDefault(string name, Rect viewRect, bool alwaysShowVertical, bool alwaysShowHorizontal)
            : base(name, viewRect, alwaysShowVertical, alwaysShowHorizontal, (GUIStyle)null, (GUIStyle)null) { }
        public IGUScrollViewDefault(string name, Rect viewRect) : base(name, viewRect, false, false) { }
        public IGUScrollViewDefault(string name) : base(name, new Rect(0, 0, 250f, 250f)) { }


        public static implicit operator IGUScrollViewDefault((string, Rect, bool, bool, GUIStyle, GUIStyle) A) => new IGUScrollViewDefault(A.Item1, A.Item2, A.Item3, A.Item4, A.Item5, A.Item6);
        public static implicit operator IGUScrollViewDefault((string, Rect, bool, bool) A) => new IGUScrollViewDefault(A.Item1, A.Item2, A.Item3, A.Item4);
        public static implicit operator IGUScrollViewDefault((string, Rect) A) => new IGUScrollViewDefault(A.Item1, A.Item2);
    }
}
