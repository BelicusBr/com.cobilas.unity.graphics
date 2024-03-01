using System;
using UnityEngine;
using System.Text;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUCanvas : IEquatable<IGUCanvas>, IDisposable {
        private event Action onIGU;
        private bool disposedValue;
        private event Action onToolTip;
        private event Action onEndOfFrame;
        [SerializeField] private string name;
        [SerializeField] private string guid;
        [SerializeField] private IGUConfig config;
        [SerializeField] private IGUDepthDictionary[] deeps;
        [SerializeField] private bool loadWhenSceneActivates;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool foldout;
        #endif

        public string Name => name;
        public Action OnIGU { get => InternalOnIGU; }
        public Action OnEndOfFrame { get => onEndOfFrame; }
        public Action OnToolTip { get => InternalOnToolTip; }
        public int DepthCount => ArrayManipulation.ArrayLength(deeps);
        public IGUConfig Config { get => config; set => config = value; }
        public bool LoadWhenSceneActivates { get => loadWhenSceneActivates; set => loadWhenSceneActivates = value; }

        public IGUCanvas(string name) {
            this.name = name;
            loadWhenSceneActivates = false;
            this.config = IGUConfig.Default;
            this.guid = Guid.NewGuid().ToString();
        }

        ~IGUCanvas()
            => Dispose(disposing: false);

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void RefreshEvents() {
            onIGU = onEndOfFrame = onToolTip = (Action)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(deeps); I++)
                for (int J = 0; J < deeps[I].Count; J++) {
                    onIGU += deeps[I][J].OnIGU;
                    if (deeps[I][J] is IIGUToolTip toolTip)
                        onToolTip += toolTip.InternalDrawToolTip;
                    if (deeps[I][J] is IIGUEndOfFrame endOfFrame)
                        onEndOfFrame += endOfFrame.EndOfFrame;
                }
        }

        public bool Add(IGUObject @object) {
            IGUDepthDictionary list = GetIGUDepthDictionary(@object.MyConfig.Depth);
            if (list.Contains(@object)) return false;
            list.Add(@object);
            RefreshEvents();
            return true;
        }

        public bool Remove(IGUObject @object) {
            if (ContainsDepth(@object.MyConfig.Depth))
                return false;
            IGUDepthDictionary list = GetIGUDepthDictionary(@object.MyConfig.Depth);
            if (!list.Contains(@object)) return false;
            list.Remove(@object);
            RefreshEvents();
            return true;
        }

        public void ChangeDepth(IGUObject @object, int oldDepth, int newDepth) {
            if (ContainsDepth(oldDepth))
                GetIGUDepthDictionary(oldDepth).Remove(@object);
            GetIGUDepthDictionary(newDepth).Add(@object);
            RefreshEvents();
        }

        public bool Equals(IGUCanvas other)
            => other.name == name && other.guid == guid;

        public override bool Equals(object obj)
            => obj is IGUCanvas cv && Equals(cv);

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        private void InternalOnIGU() {
            if (!config.IsVisible) return;
            bool oldEnabled = GUI.enabled;
            GUI.enabled = config.IsEnabled;
            onIGU?.Invoke();
            GUI.enabled = oldEnabled;
        }

        private void InternalOnToolTip() {
            if (!config.IsVisible) return;
            onToolTip?.Invoke();
        }

        private IGUDepthDictionary GetIGUDepthDictionary(int depth) {
            for (int I = 0; I < DepthCount; I++)
                if (deeps[I].Depth == depth)
                    return deeps[I];
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
                    onIGU = 
                        onEndOfFrame = 
                        onToolTip = (Action)null;
                    for (int I = 0; I < DepthCount; I++)
                        deeps[I].Clear();
                    ArrayManipulation.ClearArraySafe(ref deeps);
                }
                disposedValue = true;
            }
        }

        public static bool operator ==(IGUCanvas A, IGUCanvas B) => object.Equals(A, B);
        public static bool operator !=(IGUCanvas A, IGUCanvas B) => !object.Equals(A, B);
    }
}