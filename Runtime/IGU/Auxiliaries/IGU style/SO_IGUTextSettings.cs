using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [CreateAssetMenu(fileName ="IGUText settings", menuName = "IGUSkin/IGUTextSettings")]
    public class SO_IGUTextSettings : ScriptableObject {
        [SerializeField] private string settingsName;
        [SerializeField] private IGUTextSettings settings;

        public string SettingsName => settingsName;
        public IGUTextSettings Settings => settings;

        private void Awake()
        {
            settingsName = string.Empty;
            settings = new IGUTextSettings();
        }
    }
}