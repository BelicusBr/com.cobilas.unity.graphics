using System;
using UnityEngine;
using Cobilas.Collections;
using UnityEngine.SceneManagement;

namespace Cobilas.Unity.Graphics.IGU {
    public sealed class IGUCanvasContainer : MonoBehaviour {
        private event Action onIGU;
        private event Action onToolTip;
        private event Action onEndOfFrame;

        [SerializeField] private IGUCanvas GenericContainer;
        [SerializeField] private IGUCanvas PermanentGenericContainer;
        [SerializeField] private IGUCanvas[] others;

        private static IGUCanvasContainer container;

        public Action OnIGU { get => onIGU; }
        public Action OnToolTip { get => onToolTip; }
        public Action OnEndOfFrame { get => onEndOfFrame; }

        private void Awake() {
            if (container != null) return;
            container = this;
            others = new IGUCanvas[0];
            GenericContainer = new IGUCanvas("Generic container");
            PermanentGenericContainer = new IGUCanvas("Permanent generic container");
        }

        private void OnEnable() {
            AddEvents(GenericContainer);
            AddEvents(PermanentGenericContainer);
            for (int I = 0; I < ArrayManipulation.ArrayLength(others); I++)
                AddEvents(others[I]);
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode) {
            AddEvents(GenericContainer);
            AddEvents(PermanentGenericContainer);
        }

        private void SceneUnloaded(Scene scene) {
            onIGU = null;
            GenericContainer.Dispose();
            GenericContainer = new IGUCanvas("Generic container");
            for (int I = 0; I < ArrayManipulation.ArrayLength(others); I++)
                others[I].Dispose();
            others = new IGUCanvas[0];
        }

        private void OnDestroy() {
            onIGU = onToolTip = null;
            GenericContainer.Dispose();
            GenericContainer = null;
            PermanentGenericContainer.Dispose();
            PermanentGenericContainer = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(others); I++)
                others[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref others);
        }

        private void AddEvents(IGUCanvas canvas) {
            canvas.RefreshEvents();
            this.onIGU += canvas.OnIGU;
            this.onToolTip += canvas.OnToolTip;
            this.onEndOfFrame += canvas.OnEndOfFrame;
        }

        public static IGUCanvas GetOrCreateIGUCanvas(string name) {
            IGUCanvas res = GetGUCanvas(name);
            if (res == null) {
                res = new IGUCanvas(name);
                container.onIGU += res.OnIGU;
                ArrayManipulation.Add(res, ref container.others);
            }
            return res;
        }

        public static IGUCanvas GetGenericContainer()
            => container.GenericContainer;

        public static IGUCanvas GetPermanentGenericContainer()
            => container.PermanentGenericContainer;

        public static IGUCanvas GetGUCanvas(string name) {
            foreach (IGUCanvas item in GetAllIGUCanvas())
                if (item.Name == name)
                    return item;
            return (IGUCanvas)null;
        }

        public static IGUCanvas[] GetAllIGUCanvas()
            => ArrayManipulation.Add(container.GenericContainer,
                ArrayManipulation.Add(container.PermanentGenericContainer, container.others));
    }
}