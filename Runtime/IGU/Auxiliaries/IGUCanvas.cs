using System;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [Serializable]
    public sealed class IGUCanvas : IDisposable {
        private event Action onIGU;
        private bool disposedValue;
        private event Action onToolTip;
        private event Action onEndOfFrame;
        [SerializeField] private string name;
        [SerializeField] private IGUConfig config;
        [SerializeField] private IGUDepthDictionary[] deeps;

        public string Name => name;
        public Action OnIGU { get => InternalOnIGU; }
        public Action OnEndOfFrame { get => onEndOfFrame; }
        public Action OnToolTip { get => InternalOnToolTip; }
        public int DepthCount => ArrayManipulation.ArrayLength(deeps);
        public IGUConfig Config { get => config; set => config = value; }

        public IGUCanvas(string name)
        {
            this.name = name;
            this.config = IGUConfig.Default;
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
            GetIGUDepthDictionary(@object.MyConfig.Depth).Remove(@object);
            RefreshEvents();
            return true;
        }

        public void ChangeDepth(IGUObject @object, int oldDepth, int newDepth) {
            if (ContainsDepth(oldDepth))
                GetIGUDepthDictionary(oldDepth).Remove(@object);
            GetIGUDepthDictionary(newDepth).Add(@object);
            RefreshEvents();
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
            bool oldEnabled = GUI.enabled;
            GUI.enabled = config.IsEnabled;
            onToolTip?.Invoke();
            GUI.enabled = oldEnabled;
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
    }
}