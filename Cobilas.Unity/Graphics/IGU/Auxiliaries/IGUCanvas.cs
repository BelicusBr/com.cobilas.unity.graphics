using System;
using UnityEngine;
using System.Text;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using Cobilas.Unity.Graphics.IGU.Elements;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUCanvas : IEquatable<IGUCanvas>, IDisposable {
        private bool disposedValue;
        [SerializeField] private string name;
        [SerializeField] private string guid;
        [SerializeField] private IGUConfig config;
        [SerializeField] private IGUDepthDictionary[] deeps;
        [SerializeField] private IGUCanvasContainer container;
        [SerializeField] private IGUCanvasContainer.CanvasType status;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool foldout;
        #endif

        public string Name => name;
        public IGUDepthDictionary[] Deeps => deeps;
        public IGUCanvasContainer.CanvasType Status => status;
        public int DepthCount => ArrayManipulation.ArrayLength(Deeps);
        public IGUConfig Config { get => config; set => config = value; }
        internal IGUCanvasContainer Container { get => container; set => container = value; }

        public IGUDepthDictionary this[int index] => deeps[index];

        public IGUCanvas(string name, IGUCanvasContainer.CanvasType status) {
            this.name = name;
            this.status = status;
            this.config = IGUConfig.Default;
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
            this.guid = builder.ToString();
        }

        public IGUCanvas(string name) : this(name, IGUCanvasContainer.CanvasType.Volatile) {}

        ~IGUCanvas()
            => Dispose(disposing: false);

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool Add(IGUObject @object) {
            IGUDepthDictionary list = GetIGUDepthDictionary(@object.MyConfig.Depth);
            if (list.Contains(@object)) return false;
            list.Add(@object);
            container.RefreshEvents();
            return true;
        }

        public bool Remove(IGUObject @object) {
            if (ContainsDepth(@object.MyConfig.Depth))
                return false;
            IGUDepthDictionary list = GetIGUDepthDictionary(@object.MyConfig.Depth);
            if (!list.Contains(@object)) return false;
            list.Remove(@object);
            container.RefreshEvents();
            return true;
        }

        public void ChangeDepth(IGUObject @object, int oldDepth, int newDepth) {
            if (ContainsDepth(oldDepth))
                GetIGUDepthDictionary(oldDepth).Remove(@object);
            GetIGUDepthDictionary(newDepth).Add(@object);
            container.RefreshEvents();
        }

        public bool Equals(IGUCanvas other)
            => other.name == name && other.guid == guid;

        public override bool Equals(object obj)
            => obj is IGUCanvas cv && Equals(cv);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Clear() {
            for (int I = 0; I < DepthCount; I++)
                Deeps[I].Clear();
            ArrayManipulation.ClearArraySafe(ref deeps);
            container.RefreshEvents();
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendFormat("\tName:{0}\r\n", name);
            builder.AppendFormat("\tGUID:{0}\r\n", guid);
            builder.AppendFormat("\tDepthCount:{0}\r\n", DepthCount);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private IGUDepthDictionary GetIGUDepthDictionary(int depth) {
            for (int I = 0; I < DepthCount; I++)
                if (Deeps[I].Depth == depth)
                    return Deeps[I];
            IGUDepthDictionary res = new IGUDepthDictionary(depth);
            ArrayManipulation.Add(res, ref deeps);
            deeps = IGUDepthDictionary.ReorderDepthDictionary(deeps);
            return res;
        }

        private bool ContainsDepth(int depth) {
            for (int I = 0; I < DepthCount; I++)
                if (deeps[I].Depth == depth)
                    return true;
            return false;
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

        public static bool operator ==(IGUCanvas A, IGUCanvas B) => object.Equals(A, B);
        public static bool operator !=(IGUCanvas A, IGUCanvas B) => !object.Equals(A, B);
    }
}