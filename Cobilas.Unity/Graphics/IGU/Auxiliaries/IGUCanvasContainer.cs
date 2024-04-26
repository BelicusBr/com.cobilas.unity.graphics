using System;
using UnityEngine;
using Cobilas.Collections;
using UnityEngine.SceneManagement;
using Cobilas.Unity.Graphics.IGU.Physics;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUCanvasContainer : MonoBehaviour {
        public enum CanvasType : byte {
            All = 0,
            Volatile = 1,
            Permanent = 2
        }

        private event Action onIGU;
        private event Action onToolTip;
        private event Action onEndOfFrame;
        private event IGUBasicPhysics.CallPhysicsFeedback callPhysics;
        [SerializeField] private long focusedWindowId;
        [SerializeField] private IGUCanvas[] Containers;
        [SerializeField] private MaxMinSliderInt maxMinDepth;

        private static IGUCanvasContainer container;

        public Action OnIGU { get => onIGU; }
        public Action OnToolTip { get => onToolTip; }
        public Action OnEndOfFrame { get => onEndOfFrame; }
        public IGUBasicPhysics.CallPhysicsFeedback CallPhysics { get => callPhysics; }

        public static IGUCanvasContainer CurrentContainer => container;

        private void Awake() {
            focusedWindowId = long.MinValue;
            Containers = new IGUCanvas[] {
                new IGUCanvas("Generic container"),
                new IGUCanvas("Permanent generic container", CanvasType.Permanent)
            };
        }

        private void OnEnable() {
            if (container == null)
                container = this;

            InternalRefreshEvents();

            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        //A ação está sendo duplicada
        private void ActiveSceneChanged(Scene scene1, Scene scene2) {
            InternalRefreshEvents();
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode) {
        }

        private void SceneUnloaded(Scene scene) {
            onIGU = (Action)null;
            onToolTip = (Action)null;
            onEndOfFrame = (Action)null;
            callPhysics = (IGUBasicPhysics.CallPhysicsFeedback)null;
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
            callPhysics = (IGUBasicPhysics.CallPhysicsFeedback)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Containers); I++)
                Containers[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref Containers);
        }

        private void InternalRefreshEvents() {
            onIGU = (Action)null;
            onToolTip = (Action)null;
            onEndOfFrame = (Action)null;
            IGUObject[] elements = IGUCanvas.ReoderDepth(Containers);
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(elements); I++) {
                onIGU += (elements[I] as IIGUObject).InternalOnIGU;
                if (elements[I] is IIGUToolTip tip) onToolTip += tip.InternalDrawToolTip;
                if (elements[I] is IIGUEndOfFrame frame) onEndOfFrame += frame.EndOfFrame;                
            }
            InternalRefreshPhyEvents();
        }

        private void InternalRefreshPhyEvents() {
            callPhysics = (IGUBasicPhysics.CallPhysicsFeedback)null;
            IGUObject[] elements = IGUCanvas.ReoderDepth(Containers);
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(elements); I++) {
                IIGUPhysics phyTemp = elements[I] as IIGUPhysics;
                if (phyTemp.IsPhysicalElement)
                    callPhysics += phyTemp.CallPhysicsFeedback;
            }
        }

        public static IGUCanvas GetOrCreateIGUCanvas(string name, CanvasType type = CanvasType.All) {
            IGUCanvas res = GetGUCanvas(name, CanvasType.All);
            if (res == null) {
                res = new IGUCanvas(name, type == CanvasType.All ? CanvasType.Volatile : type);
                ArrayManipulation.Add(res, ref CurrentContainer.Containers);
                IGUCanvasContainer.RefreshEvents();
            }
            return res;
        }

        public static void RefreshEvents() => IGUCanvasContainer.CurrentContainer.InternalRefreshEvents();

        public static void RefreshPhyEvents() => IGUCanvasContainer.CurrentContainer.InternalRefreshPhyEvents();

        public static IGUCanvas GetGenericContainer() => IGUCanvasContainer.CurrentContainer.Containers[0];

        public static IGUCanvas GetPermanentGenericContainer() => IGUCanvasContainer.CurrentContainer.Containers[1];

        public static IGUCanvas GetGUCanvas(string name, CanvasType type = CanvasType.All) {
            foreach (IGUCanvas item in GetAllIGUCanvas(type))
                if (item.Name == name)
                    return item;
            return (IGUCanvas)null;
        }

        public static IGUCanvas[] GetAllIGUCanvas(CanvasType type = CanvasType.All) {
            if (type == CanvasType.All)
                return CurrentContainer.Containers;
            IGUCanvas[] result = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(CurrentContainer.Containers); I++)
                if (CurrentContainer.Containers[I].Status == type)
                    ArrayManipulation.Add(CurrentContainer.Containers[I], ref result);
            return result;
        }
    }
}