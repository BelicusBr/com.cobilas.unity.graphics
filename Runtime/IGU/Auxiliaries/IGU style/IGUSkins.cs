using UnityEngine;
using Cobilas.Collections;

namespace Cobilas.Unity.Graphics.IGU {
    public static class IGUSkins {

        private static IGUStyleCustom[] iGUStyles;
        private static SO_IGUTextSettings[] iGUTextSettings;

        public static int StyleCount => ArrayManipulation.ArrayLength(iGUStyles);
        public static int SettingsCount => ArrayManipulation.ArrayLength(iGUTextSettings);

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            iGUStyles = Resources.LoadAll<IGUStyleCustom>(string.Empty);
            iGUTextSettings = Resources.LoadAll<SO_IGUTextSettings>(string.Empty);
        }

        public static IGUStyle GetIGUStyle(int index) => iGUStyles[index].Style;
        public static IGUTextSettings GetIGUTextSettings(int index) => iGUTextSettings[index].Settings;

        public static IGUStyle GetIGUStyle(string name) {
            for (int I = 0; I < StyleCount; I++)
                if (iGUStyles[I].Style.Name == name)
                    return GetIGUStyle(I);
            return (IGUStyle)null;
        }

        public static IGUTextSettings GetIGUTextSettings(string name) {
            for (int I = 0; I < StyleCount; I++)
                if (iGUTextSettings[I].SettingsName == name)
                    return GetIGUTextSettings(I);
            return (IGUTextSettings)null;
        }
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Refresh() {
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
            Init();
        }
#endif
    }
}
