using System;
using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using Cobilas.Unity.Management.Container;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;
using UnityEngine.SceneManagement;

namespace Cobilas.Unity.Graphics.IGU {
    [AddSceneContainer]
    public class IGUDrawer : IGUBehaviour, ISerializationCallbackReceiver, ISceneContainerItem {
        private Action onIGU;
        private Coroutine EndOfFrameCoroutine = null;
        private IGUToolTip toolTip = new IGUToolTip();
        [SerializeField] private IGUMouseInput[] mouses;
        [SerializeField] private IGUContainer[] containers;
        [SerializeField] private IGUObject[] reserialization;
#if UNITY_EDITOR
#pragma warning disable IDE0052
        [SerializeField, HideInInspector] private Vector2 editor_ScaleFactor;
        [SerializeField, HideInInspector] private Vector2Int editor_CurrentResolution;
#pragma warning restore IDE0052
#endif

        private static IGUDrawer drawer;
        private static Vector2Int baseCurrentResolution = new Vector2Int(1024, 768);

        public static Vector2Int BaseResolution => new Vector2Int(1024, 768);
        public static Vector2Int BaseResolutionPlatform => GetBaseResolutionPlatform();
        public static Vector2Int MobileBaseResolution_Portrait => new Vector2Int(480, 800);
        public static Vector2Int MobileBaseResolution_Landscape => new Vector2Int(800, 480);
        public static Vector2Int CurrentResolution => new Vector2Int(Screen.width, Screen.height);
        public static Vector2 ScaleFactor => ((Vector2)CurrentResolution).Division(baseCurrentResolution);
        public static Vector2Int BaseCurrentResolution { get => baseCurrentResolution; set => baseCurrentResolution = value; }

        public static IGUDrawer Drawer => drawer;
        public static event Action EventEndOfFrame = (Action)null;

        protected override void Awake() {
            drawer = this;
            mouses = new IGUMouseInput[3];
        }

        private void OnEnable() {
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(reserialization); I++)
                (reserialization[I] as IIGUSerializationCallbackReceiver).Reserialization();
        }

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

        protected override void OnGUI() {
            Vector2 mousePosition = Event.current.mousePosition;
#if PLATFORM_STANDALONE || UNITY_EDITOR
            mouses[0] = mouses[0].SetValues(
                Input.GetKeyDown(KeyCode.Mouse0),
                Input.GetKey(KeyCode.Mouse0),
                Input.GetKeyUp(KeyCode.Mouse0),
                mousePosition
                );
            mouses[1] = mouses[1].SetValues(
                Input.GetKeyDown(KeyCode.Mouse1),
                Input.GetKey(KeyCode.Mouse1),
                Input.GetKeyUp(KeyCode.Mouse1),
                Vector2.zero
                );
            mouses[2] = mouses[2].SetValues(
                Input.GetKeyDown(KeyCode.Mouse2),
                Input.GetKey(KeyCode.Mouse2),
                Input.GetKeyUp(KeyCode.Mouse2),
                Vector2.zero
                );
#else
            mouses[0] = 
                mouses[1] = 
                mouses[2] = mouses[0].SetValues(true, true, true, mousePosition);
#endif
            toolTip.Close();
            onIGU?.Invoke();
            toolTip.Draw(mousePosition, ScaleFactor);
        }

        public void OpenTooltip() => toolTip.Open();

        public void GUIStyleTootip(GUIStyle style) => toolTip.SetGuiStyle(style);

        public void SetTootipText(string txt) => toolTip.SetMSM(txt);

        public bool GetMouseButton(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Press;
        }

        public bool GetMouseButtonDown(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Down;
        }

        public bool GetMouseButtonUp(MouseButtonType type) {
            if (type == MouseButtonType.All) return true;
            return mouses[(int)type].Up;
        }

        public Vector2 GetMousePosition() => mouses[0].MousePosition;

        public bool Contains(IGUContainer container) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                if (containers[I] == container)
                    return true;
            return false;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            drawer = this;
            toolTip = new IGUToolTip();
            onIGU = (Action)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(containers); I++)
                onIGU += (containers[I] as IIGUContainer).OnIGU;
        }

        void ISceneContainerItem.sceneUnloaded(Scene scene) {}

        void ISceneContainerItem.sceneLoaded(Scene scene, LoadSceneMode mode) {}

        internal void Add(IGUContainer container) {
            if (Contains(container)) return;
            onIGU += (container as IIGUContainer).OnIGU;
            ArrayManipulation.Add(container, ref containers);
        }

        internal void Remove(IGUContainer container) {
            if (!Contains(container)) return;
            onIGU -= (container as IIGUContainer).OnIGU;
            ArrayManipulation.Remove(container, ref containers);
        }

        public static Vector2Int GetBaseResolutionPlatform() {
#if PLATFORM_ANDROID
            switch (Screen.orientation) {
                case ScreenOrientation.Portrait:
                case ScreenOrientation.PortraitUpsideDown:
                    return MobileBaseResolution_Portrait;
                case ScreenOrientation.LandscapeLeft:
                case ScreenOrientation.LandscapeRight:
                    return MobileBaseResolution_Landscape;
                //case ScreenOrientation.AutoRotation:
                //    break;
            }
#else
            return BaseResolution;
#endif
        }

        internal static void AddReserialization(IGUObject item)
            => ArrayManipulation.Add(item, ref drawer.reserialization);

        internal static void RemoveReserialization(IGUObject item) {
            if (drawer.reserialization != null && ArrayManipulation.Exists(item, drawer.reserialization))
                ArrayManipulation.Remove(item, ref drawer.reserialization);
        }
    }
}
