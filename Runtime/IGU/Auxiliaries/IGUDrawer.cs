using System;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using Cobilas.Unity.Management.Container;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [AddToPermanentContainer]
    public class IGUDrawer : IGUBehaviour, ISerializationCallbackReceiver {
        private Action onIGU;
        private Coroutine EndOfFrameCoroutine = null;
        [SerializeField] private IGUContainer[] containers;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private Vector2 editor_ScaleFactor;
        [SerializeField, HideInInspector] private Vector2Int editor_CurrentResolution;

#endif

        private static IGUDrawer drawer;
        public static Vector2Int BaseResolution => new Vector2Int(1024, 768);
        public static Vector2Int CurrentResolution => new Vector2Int(Screen.width, Screen.height);
        public static Vector2 ScaleFactor => ((Vector2)CurrentResolution).Division(BaseResolution);

        public static IGUDrawer Drawer => drawer;
        public static event Action EventEndOfFrame = (Action)null;

        protected override void Awake() => drawer = this;

        private void LateUpdate() {
            if (EndOfFrameCoroutine == null)
                EndOfFrameCoroutine = StartCoroutine(EndOfFrame());
#if UNITY_EDITOR
            editor_CurrentResolution = CurrentResolution;
            editor_ScaleFactor = ScaleFactor;
#endif
        }

        private IEnumerator EndOfFrame() {
            while (true) {
                yield return new WaitForEndOfFrame();
                EventEndOfFrame?.Invoke();
            }
        }

        protected override void OnGUI() => onIGU?.Invoke();

        public bool Contains(IGUContainer container) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                if (containers[I] == container)
                    return true;
            return false;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            drawer = this;
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                onIGU += (containers[I] as IIGUContainer).OnIGU;
        }

        internal void Add(IGUContainer container) {
            if (Contains(container)) return;
            onIGU += (container as IIGUContainer).OnIGU;
            ArrayManipulation.Add(container, ref containers);
        }

        internal void Remove(IGUContainer container) {
            if (!Contains(container)) return;
            onIGU += (container as IIGUContainer).OnIGU;
            ArrayManipulation.Remove(container, ref containers);
        }
    }
}
