using System;
using UnityEngine;
using System.Text;
using System.Linq;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUCanvas : IEquatable<IGUCanvas>, IDisposable {
        private bool disposedValue;
        [SerializeField] private string name;
        [SerializeField] private string guid;
        [SerializeField] private IGUConfig config;
        [SerializeField] private IGUObject[] elements;
        [SerializeField] private IGUCanvasContainer.CanvasType status;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool foldout;
        #endif

        public string Name => name;
        public IGUCanvasContainer.CanvasType Status => status;
        public IGUConfig Config { get => config; set => config = value; }
        public int ElementsCount => ArrayManipulation.ArrayLength(elements);

        public IGUObject this[int index] => elements[index];

        public IGUCanvas(string name, IGUCanvasContainer.CanvasType status) {
            this.name = name;
            this.status = status;
            this.config = IGUConfig.Default;
            this.guid = IGUCanvas.CreateGUID();
        }

        public IGUCanvas(string name) : this(name, IGUCanvasContainer.CanvasType.Volatile) {}

        ~IGUCanvas()
            => Dispose(disposing: false);

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool Add(IGUObject @object) {
            if (Contains(@object)) return false;
            ArrayManipulation.Add(@object, ref elements);
            return true;
        }

        public bool Remove(IGUObject @object) {
            if (!Contains(@object)) return false;
            ArrayManipulation.Remove(@object, ref elements);
            return true;
        }

        public bool Equals(IGUCanvas other)
            => other.name == name && other.guid == guid;

        public override bool Equals(object obj)
            => obj is IGUCanvas cv && Equals(cv);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Contains(IGUObject element)
            => IndexOf(element) > -1;

        public int IndexOf(IGUObject element) {
            for (int I = 0; I < ElementsCount; I++)
                if (elements[I] == element)
                    return I;
            return -1;
        }

        public void Clear() {
            ArrayManipulation.ClearArraySafe(ref elements);
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendFormat("\tName:{0}\r\n", name);
            builder.AppendFormat("\tGUID:{0}\r\n", guid);
            builder.AppendFormat("\tElementsCount:{0}\r\n", ElementsCount);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    name = string.Empty;
                    Clear();
                }
                disposedValue = true;
            }
        }

        public static string CreateGUID() {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < 15; I++) {
                if (Randomico.BooleanRandom) { 
                    switch (Randomico.ByteRange(0, 5)) {
                        case 0: _ = builder.Append('A'); break;
                        case 1: _ = builder.Append('B'); break;
                        case 2: _ = builder.Append('C'); break;
                        case 3: _ = builder.Append('D'); break;
                        case 4: _ = builder.Append('E'); break;
                        case 5: _ = builder.Append('F'); break;
                    }
                } else {
                    _ = builder.Append(Randomico.ByteRange(0, 9)); 
                }
            }
            return builder.ToString();
        }
        
        public static IGUObject[] ReoderDepth(IGUCanvas[] canvas) {
            IGUObject[] wins = new IGUObject[0];
            IGUObject[] result = new IGUObject[0];
            IGUObject focused = null;
            Dictionary<int, IGUObject[]> deeps = new Dictionary<int, IGUObject[]>();
            for (int I = 0; I < ArrayManipulation.ArrayLength(canvas); I++)
                for (int J = 0; J < canvas[I].ElementsCount; J++) {
                    IGUObject temp = canvas[I][J];
                    if (temp is IIGUWindow window) {
                        switch (window.IsFocused) {
                            case WindowFocusStatus.Focused:
                                focused = temp;
                                continue;
                            case WindowFocusStatus.Unfocused:
                                ArrayManipulation.Add(temp, ref wins);
                                continue;
                        }
                    } else if (deeps.ContainsKey(temp.MyConfig.Depth)) {
                        deeps[temp.MyConfig.Depth] = ArrayManipulation.Add(temp, deeps[temp.MyConfig.Depth]);
                        continue;
                    }
                    deeps.Add(temp.MyConfig.Depth, new IGUObject[] { temp });
                }
            foreach (KeyValuePair<int, IGUObject[]> item in deeps.OrderBy((k) => k.Key))
                ArrayManipulation.Add(item.Value, ref result);
            if (!ArrayManipulation.EmpytArray(wins))
                ArrayManipulation.Add(wins, ref result);
            if (focused != null)
                ArrayManipulation.Add(focused, ref result);
            return result;
        }

        public static bool operator ==(IGUCanvas A, IGUCanvas B) => object.Equals(A, B);
        public static bool operator !=(IGUCanvas A, IGUCanvas B) => !object.Equals(A, B);
    }
}