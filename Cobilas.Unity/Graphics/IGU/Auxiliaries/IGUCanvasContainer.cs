using System;
using UnityEngine;
using Cobilas.Collections;
using UnityEngine.SceneManagement;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUCanvasContainer : MonoBehaviour {
        public enum CanvasType : byte {
            All = 0,
            Volatile = 1,
            Permanent = 2
        }

        private Action onIGU;
        private Action onToolTip;
        private Action onEndOfFrame;
        [SerializeField] private long focusedWindowId;
        [SerializeField] private IGUCanvas[] Containers;
        [SerializeField] private MaxMinSliderInt maxMinDepth;

        private static IGUCanvasContainer container;

        public Action OnIGU { get => onIGU; }
        public Action OnToolTip { get => onToolTip; }
        public Action OnEndOfFrame { get => onEndOfFrame; }

        private void Awake() {
            focusedWindowId = long.MinValue;
            Containers = new IGUCanvas[] {
                new IGUCanvas("Generic container"),
                new IGUCanvas("Permanent generic container", CanvasType.Permanent)
            };
            Containers[0].Container = this;
            Containers[1].Container = this;
        }

        private void OnEnable() {
            if (container == null)
                container = this;

            RefreshEvents();

            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        //A ação está sendo duplicada
        private void ActiveSceneChanged(Scene scene1, Scene scene2) {
            RefreshEvents();
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode) {
        }

        private void SceneUnloaded(Scene scene) {
            onIGU = (Action)null;
            onToolTip = (Action)null;
            onEndOfFrame = (Action)null;
            Containers[0].Clear();
            for (int I = 1; I < ArrayManipulation.ArrayLength(Containers); I++)
                if (Containers[I].Status == CanvasType.Volatile)
                    Containers[I].Dispose();
            if (Containers.Length > 2)
                ArrayManipulation.Resize(ref Containers, 2);
        }

        private void OnDestroy() {
            onIGU = (Action)null;
            onToolTip = (Action)null;
            onEndOfFrame = (Action)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Containers); I++)
                Containers[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref Containers);
        }

        internal void RefreshEvents() {
            onIGU = (Action)null;
            onToolTip = (Action)null;
            onEndOfFrame = (Action)null;
            IGUDepthDictionary[] list = ReoderDepth(Containers);
            if (!ArrayManipulation.EmpytArray(list)) maxMinDepth = MaxMinSliderInt.Default;
            else maxMinDepth.Set(list[0].Depth, list[list.Length - 1].Depth);
            foreach (IGUDepthDictionary item1 in list)
                foreach (Elements.IGUObject item2 in item1) {
                    onIGU += item2.OnIGU;
                    if (item2 is IIGUToolTip tip) onToolTip += tip.InternalDrawToolTip;
                    if (item2 is IIGUEndOfFrame frame) onEndOfFrame += frame.EndOfFrame;
                }
        }

        internal void FocusWindow(int id) {
            if (id == focusedWindowId) return;
            focusedWindowId = id;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Containers); I++)
                for (int J = 0; J < Containers[I].DepthCount; J++)
                    for (int L = 0; L < Containers[I][J].Count; L++)
                        if (Containers[I][J][L] is IIGUWindow win) {
                            if (win.GetInstanceID() == id) win.IsFocused = WindowFocusStatus.Focused;
                            else if (win.IsFocused == WindowFocusStatus.Focused)
                                win.IsFocused = WindowFocusStatus.Unfocused;
                        }
        }

        public static IGUCanvas GetOrCreateIGUCanvas(string name, CanvasType type = CanvasType.All) {
            IGUCanvas res = GetGUCanvas(name, CanvasType.All);
            if (res == null) {
                res = new IGUCanvas(name, type == CanvasType.All ? CanvasType.Volatile : type);
                ArrayManipulation.Add(res, ref container.Containers);
                container.RefreshEvents();
            }
            return res;
        }

        public static IGUCanvas GetGenericContainer() => container.Containers[0];

        public static IGUCanvas GetPermanentGenericContainer() => container.Containers[1];

        public static IGUCanvas GetGUCanvas(string name, CanvasType type = CanvasType.All) {
            foreach (IGUCanvas item in GetAllIGUCanvas(type))
                if (item.Name == name)
                    return item;
            return (IGUCanvas)null;
        }

        public static IGUCanvas[] GetAllIGUCanvas(CanvasType type = CanvasType.All) {
            if (type == CanvasType.All)
                return container.Containers;
            IGUCanvas[] result = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(container.Containers); I++)
                if (container.Containers[I].Status == type)
                    ArrayManipulation.Add(container.Containers[I], ref result);
            return result;
        }

        private static IGUDepthDictionary[] ReoderDepth(IGUCanvas[] canvas) {
            IGUDepthDictionary[] res = new IGUDepthDictionary[0];
            for (int I = 0; I < ArrayManipulation.ArrayLength(canvas); I++)
                if (canvas[I].Deeps != null)
                    ArrayManipulation.Add(canvas[I].Deeps, ref res);
            if (res == null)
                return res;
            return IGUDepthDictionary.ReorderDepthDictionary(res);
        }
    }
}