using UnityEngine;
using System.Collections;
using Cobilas.Collections;
using UnityEngine.SceneManagement;
using Cobilas.Unity.Management.Container;
using Cobilas.Unity.Graphics.IGU.Elements;
using Cobilas.Unity.Graphics.IGU.Interfaces;

namespace Cobilas.Unity.Graphics.IGU {
    [AddSceneContainer]
    [RequireComponent(typeof(IGUCanvasContainer))]
    public class IGUDrawer : MonoBehaviour, ISceneContainerItem {
        private Vector2 mousePosition;
        private Coroutine EndOfFrameCoroutine = null;
        [SerializeField] private IGUMouseInput[] mouses;
        [SerializeField] private IGUObject[] reserialization;
        [SerializeField] private IGUCanvasContainer canvasContainer;
#if UNITY_EDITOR
#pragma warning disable IDE0052
        [SerializeField, HideInInspector] private Vector2 editor_ScaleFactor;
        [SerializeField, HideInInspector] private Vector2Int editor_CurrentResolution;
#pragma warning restore IDE0052
#endif

        private static IGUDrawer drawer;
        private static Vector2Int baseCurrentResolution = new Vector2Int(1024, 768);

        public static IGUDrawer Drawer => drawer;
        public static Vector2 MousePosition => drawer.mousePosition;
        public static Vector2Int BaseResolution => new Vector2Int(1024, 768);
        public static Vector2Int BaseResolutionPlatform => GetBaseResolutionPlatform();
        public static Vector2Int MobileBaseResolution_Portrait => new Vector2Int(480, 800);
        public static Vector2Int MobileBaseResolution_Landscape => new Vector2Int(800, 480);
        public static Vector2Int CurrentResolution => new Vector2Int(Screen.width, Screen.height);
        public static Vector2 ScaleFactor => ((Vector2)CurrentResolution).Division(baseCurrentResolution);
        public static Vector2Int BaseCurrentResolution { get => baseCurrentResolution; set => baseCurrentResolution = value; }

        private void Awake() {
            mouses = new IGUMouseInput[3];
            canvasContainer = GetComponent<IGUCanvasContainer>();
        }

        private void OnEnable() {
            drawer = this;
            if (EndOfFrameCoroutine == null)
                EndOfFrameCoroutine = StartCoroutine(EndOfFrame());
            for (long I = 0; I < ArrayManipulation.ArrayLongLength(reserialization); I++)
                (reserialization[I] as IIGUSerializationCallbackReceiver).Reserialization();
        }

#if UNITY_EDITOR
        private void LateUpdate() {
            editor_CurrentResolution = CurrentResolution;
            editor_ScaleFactor = ScaleFactor;
        }
#endif

        private IEnumerator EndOfFrame() {
            while (true) {
                yield return new WaitForEndOfFrame();
                canvasContainer.OnEndOfFrame?.Invoke();
            }
        }

        private void OnGUI() {
            GUIUtility.ScaleAroundPivot(ScaleFactor, Vector2.zero);
            mousePosition = Event.current.mousePosition;
// #if PLATFORM_STANDALONE || UNITY_EDITOR
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
// #else
//             mouses[0] = 
//                 mouses[1] = 
//                 mouses[2] = mouses[0].SetValues(true, true, true, mousePosition);
// #endif
            canvasContainer.OnIGU?.Invoke();
            canvasContainer.OnToolTip?.Invoke();
        }

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

        void ISceneContainerItem.sceneUnloaded(Scene scene) {}

        void ISceneContainerItem.sceneLoaded(Scene scene, LoadSceneMode mode) {}

        public static void DrawTooltip(string text, IGUStyle style) {
            GUIContent content = IGUTextObject.GetGUIContentTemp(text);
            GUIStyle styleTemp = (GUIStyle)style;
            Rect rect = IGURect.rectTemp;
            rect.position = Vector2.one * 5f + MousePosition;
            rect.size = styleTemp.CalcSize(content);
            GUI.Box(rect, content, styleTemp);
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
