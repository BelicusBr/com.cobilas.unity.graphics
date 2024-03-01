using System;
using UnityEngine;
using Cobilas.Collections;
using UnityEngine.SceneManagement;
using Cobilas.Unity.Graphics.IGU.Events;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUCanvasContainer : MonoBehaviour {
        public enum CanvasType : byte {
            All = 0,
            Volatile = 1,
            Permanent = 2
        }

        private DIGUAction<IGUCanvas> onIGU;
        private DIGUAction<IGUCanvas> onToolTip;
        private DIGUAction<IGUCanvas> onEndOfFrame;
        [SerializeField] private IGUCanvas[] VolatileContainer;
        [SerializeField] private IGUCanvas[] PermanentContainer;

        private static IGUCanvasContainer container;

        public Action OnIGU { get => onIGU.Function; }
        public Action OnToolTip { get => onToolTip.Function; }
        public Action OnEndOfFrame { get => onEndOfFrame.Function; }

        private void Awake() {
            VolatileContainer = new IGUCanvas[1];
            PermanentContainer = new IGUCanvas[1];
            VolatileContainer[0] = new IGUCanvas("Generic container");
            PermanentContainer[0] = new IGUCanvas("Permanent generic container");
            PermanentContainer[0].LoadWhenSceneActivates =
                VolatileContainer[0].LoadWhenSceneActivates = true;
        }

        private void OnEnable() {
            if (container == null)
                container = this;
            
            (onIGU = new DIGUAction<IGUCanvas>()).RefreshFunction = (i) => i.OnIGU;
            (onToolTip = new DIGUAction<IGUCanvas>()).RefreshFunction = (i) => i.OnToolTip;
            (onEndOfFrame = new DIGUAction<IGUCanvas>()).RefreshFunction = (i) => i.OnEndOfFrame;

            for (int I = 0; I < ArrayManipulation.ArrayLength(VolatileContainer); I++)
                AddEvents(VolatileContainer[I]);
            for (int I = 0; I < ArrayManipulation.ArrayLength(PermanentContainer); I++)
                AddEvents(PermanentContainer[I]);

            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        private void ActiveSceneChanged(Scene scene1, Scene scene2) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(VolatileContainer); I++)
                if (VolatileContainer[I].LoadWhenSceneActivates)
                    AddEvents(VolatileContainer[I]);
            for (int I = 0; I < ArrayManipulation.ArrayLength(PermanentContainer); I++)
                if (PermanentContainer[I].LoadWhenSceneActivates)
                    AddEvents(PermanentContainer[I]);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(VolatileContainer); I++)
                if (!VolatileContainer[I].LoadWhenSceneActivates)
                    AddEvents(VolatileContainer[I]);
            for (int I = 0; I < ArrayManipulation.ArrayLength(PermanentContainer); I++)
                if (!PermanentContainer[I].LoadWhenSceneActivates)
                    AddEvents(PermanentContainer[I]);
        }

        private void SceneUnloaded(Scene scene) {
            onIGU.Clear();
            onToolTip.Clear();
            onEndOfFrame.Clear();
            for (int I = 0; I < ArrayManipulation.ArrayLength(VolatileContainer); I++)
                VolatileContainer[I].Dispose();

            VolatileContainer = new IGUCanvas[1];
            VolatileContainer[0] = new IGUCanvas("Generic container");
            PermanentContainer[0].LoadWhenSceneActivates =
                VolatileContainer[0].LoadWhenSceneActivates = true;
        }

        private void OnDestroy() {
            onIGU.Clear();
            onToolTip.Clear();
            onEndOfFrame.Clear();
            onIGU = onToolTip = onEndOfFrame = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(VolatileContainer); I++)
                VolatileContainer[I].Dispose();
            for (int I = 0; I < ArrayManipulation.ArrayLength(PermanentContainer); I++)
                PermanentContainer[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref VolatileContainer);
            ArrayManipulation.ClearArraySafe(ref PermanentContainer);
        }

        private void AddEvents(IGUCanvas canvas) {
            canvas.RefreshEvents();
            this.onIGU += canvas;
            this.onToolTip += canvas;
            this.onEndOfFrame += canvas;
        }

        public static IGUCanvas GetOrCreateIGUCanvas(string name, CanvasType type = CanvasType.All) {
            IGUCanvas res = GetGUCanvas(name, CanvasType.All);
            if (res == null) {
                res = new IGUCanvas(name);
                container.AddEvents(res);
                switch (type) {
                    case CanvasType.All:
                    case CanvasType.Volatile:
                        ArrayManipulation.Add(res, ref container.VolatileContainer);
                        break;
                    case CanvasType.Permanent:
                        ArrayManipulation.Add(res, ref container.PermanentContainer);
                        break;
                }
            }
            return res;
        }

        public static IGUCanvas GetGenericContainer()
            => container.VolatileContainer[0];

        public static IGUCanvas GetPermanentGenericContainer()
            => container.PermanentContainer[0];

        public static IGUCanvas GetGUCanvas(string name, CanvasType type = CanvasType.All) {
            foreach (IGUCanvas item in GetAllIGUCanvas(type))
                if (item.Name == name)
                    return item;
            return (IGUCanvas)null;
        }

        public static IGUCanvas[] GetAllIGUCanvas(CanvasType type = CanvasType.All) {
            switch (type) {
                case CanvasType.Volatile:
                    return container.VolatileContainer;
                case CanvasType.Permanent:
                    return container.PermanentContainer;
                default:
                    return ArrayManipulation.Add(container.VolatileContainer, container.PermanentContainer);
            }
            
        }
    }
}