using UnityEngine;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.Resolutions {
    using UEResolution = UnityEngine.Resolution;
    public static class CobilasResolutions {
        private static Resolution[] resolutions;
        private static AspectRatio[] aspectRatios;
        private static int[] frequencys;

        public static Resolution[] Resolutions => resolutions;
        public static AspectRatio[] AspectRatios => aspectRatios;
        public static int[] Frequencys => frequencys;
        public static bool FullScrean { get => Screen.fullScreen; set => Screen.fullScreen = value; }
        public static FullScreenMode fullScreenMode { get => Screen.fullScreenMode; set => Screen.fullScreenMode = value; }

        public static int ResolutionsCount => ArrayManipulation.ArrayLength(resolutions);
        public static int AspectRatiosCount => ArrayManipulation.ArrayLength(aspectRatios);
        public static int FrequencyCount => ArrayManipulation.ArrayLength(frequencys);

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

        public static void Refresh() {
            ArrayManipulation.ClearArraySafe(ref resolutions);
            ArrayManipulation.ClearArraySafe(ref aspectRatios);
            ArrayManipulation.ClearArraySafe(ref frequencys);
            aspectRatios = new AspectRatio[] {
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
                    ArrayManipulation.Add(resolutionTemp, ref resolutions);
                if (!ContainsFrequency(resolutionstemp[I].refreshRate))
                    ArrayManipulation.Add(resolutionstemp[I].refreshRate, ref frequencys);
            }
        }

        private static bool ContainsFrequency(int frequency) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(frequencys); I++)
                if (frequencys[I] == frequency)
                    return true;
            return false;
        }

        private static bool ContainsAspectRatio(AspectRatio aspectRatio) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(aspectRatios); I++)
                if (aspectRatios[I] == aspectRatio)
                    return true;
            return false;
        }

        private static bool ContainsResolution(Resolution resolution) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(resolutions); I++)
                if (resolutions[I] == resolution)
                    return true;
            return false;
        }
    }
}
