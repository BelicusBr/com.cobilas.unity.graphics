using System;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU.Elements {
    public class IGUComboBoxDefault : IGUDefaultValue {

        public static IGUDefaultValue DefaultValue => new IGUComboBoxDefault("ele_IGUComboBox");
        
        public IGUComboBoxDefault(string name, int index, params IGUContent[] buttons) : base(name, index, buttons) { }
        public IGUComboBoxDefault(string name, int index, params string[] buttons) : 
            base(name, index, ArrayManipulation.EmpytArray(buttons) ? (IGUContent[])null : Array.ConvertAll<string, IGUContent>(buttons, (b) => new IGUContent(b))) { }
        public IGUComboBoxDefault(string name, params IGUContent[] buttons) : this(name, -1, buttons) { }
        public IGUComboBoxDefault(string name, params string[] buttons) : this(name, -1, buttons) { }
        public IGUComboBoxDefault(string name) : this(name, "Item 1", "Item 2", "Item 3") { }

        public static implicit operator IGUComboBoxDefault((string, int, IGUContent[]) A) => new IGUComboBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUComboBoxDefault((string, int, string[]) A) => new IGUComboBoxDefault(A.Item1, A.Item2, A.Item3);
        public static implicit operator IGUComboBoxDefault((string, IGUContent[]) A) => new IGUComboBoxDefault(A.Item1, A.Item2);
        public static implicit operator IGUComboBoxDefault((string, string[]) A) => new IGUComboBoxDefault(A.Item1, A.Item2);
    }
}
