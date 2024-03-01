using UnityEngine;
using Cobilas.Collections;
using UnityEngine.SceneManagement;
using Cobilas.Unity.Management.Runtime;
using Cobilas.Unity.Management.Container;

namespace Cobilas.Unity.Graphics.Resolutions {
    using UEResolution = UnityEngine.Resolution;
    [AddSceneContainer]
    public class CobilasResolutions : MonoBehaviour, ISerializationCallbackReceiver, ISceneContainerItem {
        [SerializeField] private Resolution[] resolutions;
        [SerializeField] private AspectRatio[] aspectRatios;
        [SerializeField] private int[] frequencys;
        private bool AfterDeserialize = false;

        private static CobilasResolutions cb_resolutions;

        public static Resolution[] Resolutions => cb_resolutions.resolutions;
        public static AspectRatio[] AspectRatios => cb_resolutions.aspectRatios;
        public static int[] Frequencys => cb_resolutions.frequencys;
        public static bool FullScrean { get => Screen.fullScreen; set => Screen.fullScreen = value; }
        public static FullScreenMode fullScreenMode { get => Screen.fullScreenMode; set => Screen.fullScreenMode = value; }

        public static int ResolutionsCount => ArrayManipulation.ArrayLength(cb_resolutions.resolutions);
        public static int AspectRatiosCount => ArrayManipulation.ArrayLength(cb_resolutions.aspectRatios);
        public static int FrequencyCount => ArrayManipulation.ArrayLength(cb_resolutions.frequencys);

        private void Start() {
            if (cb_resolutions != this)
                Destroy(this);
        }

        private void OnEnable() {
            if (AfterDeserialize) {
                AfterDeserialize = false;
                CobilasResolutions temp = FindObjectOfType<CobilasResolutions>();
                if (temp == null) {
                    cb_resolutions = temp;
                    Internal_Refresh();
                } else {
                    if (temp != this)
                        Destroy(this);
                }
            }
        }

        private void Internal_Refresh() {
            ArrayManipulation.ClearArraySafe(ref cb_resolutions.resolutions);
            ArrayManipulation.ClearArraySafe(ref cb_resolutions.aspectRatios);
            ArrayManipulation.ClearArraySafe(ref cb_resolutions.frequencys);
            cb_resolutions.aspectRatios = new AspectRatio[] {
                new AspectRatio(16, 10),
                new AspectRatio(16, 9),
                new AspectRatio(13, 7),
                new AspectRatio(7, 3),
                new AspectRatio(5, 4),
                new AspectRatio(4, 3),
                new AspectRatio(3, 2)
            };

            UEResolution[] resolutionstemp = Screen.resolutions;
            for (int I = 0; I < ArrayManipulation.ArrayLength(resolutionstemp); I++) {
                Resolution resolutionTemp = new Resolution(resolutionstemp[I]);
                if (!ContainsResolution(resolutionTemp))
                    ArrayManipulation.Add(resolutionTemp, ref cb_resolutions.resolutions);
                if (!ContainsFrequency(resolutionstemp[I].refreshRate))
                    ArrayManipulation.Add(resolutionstemp[I].refreshRate, ref cb_resolutions.frequencys);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() => AfterDeserialize = true;
        
        void ISceneContainerItem.sceneUnloaded(Scene scene) {}

        void ISceneContainerItem.sceneLoaded(Scene scene, LoadSceneMode mode) {}

        //[CRIOLM_CallWhen(typeof(ContainerManager), CRIOLMType.AfterSceneLoad)]
        [CallWhenStart(InitializePriority.Low, "#ContainerManager")]
#pragma warning disable IDE0051 // Remover membros privados não utilizados
        private static void Init() {
#pragma warning restore IDE0051 // Remover membros privados não utilizados
            cb_resolutions = FindObjectOfType<CobilasResolutions>();
            cb_resolutions.Internal_Refresh();
        }

        public static void SetResolution(Resolution resolution, bool fullScreen)
            => SetResolution(resolution, fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, 0);

        public static void SetResolution(Resolution resolution, FullScreenMode fullScreenMode)
            => SetResolution(resolution, fullScreenMode, 0);

        public static void SetResolution(Resolution resolution, bool fullScreen, int preferredRefreshRate)
            => SetResolution(resolution, fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, preferredRefreshRate);

        public static void SetResolution(Resolution resolution, FullScreenMode fullScreenMode, int preferredRefreshRate)
            => Screen.SetResolution(resolution.Width, resolution.Height, fullScreenMode, preferredRefreshRate);

        public static Resolution ApplayAspectRatio(Resolution resolution, AspectRatio aspect)
            => ApplayAspectRatioInHeight(ApplayAspectRatioInWidth(resolution, aspect), aspect);

        public static Resolution ApplayAspectRatioInWidth(Resolution resolution, AspectRatio aspect)
            => new Resolution(resolution.Width / aspect.Width * aspect.Height, resolution.Height);

        public static Resolution ApplayAspectRatioInHeight(Resolution resolution, AspectRatio aspect)
            => new Resolution(resolution.Width, resolution.Height / aspect.Width * aspect.Height);

        public static void Refresh() => cb_resolutions.Internal_Refresh();

        private static bool ContainsFrequency(int frequency) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(cb_resolutions.frequencys); I++)
                if (cb_resolutions.frequencys[I] == frequency)
                    return true;
            return false;
        }

        private static bool ContainsResolution(Resolution resolution) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(cb_resolutions.resolutions); I++)
                if (cb_resolutions.resolutions[I] == resolution)
                    return true;
            return false;
        }
    }
}
