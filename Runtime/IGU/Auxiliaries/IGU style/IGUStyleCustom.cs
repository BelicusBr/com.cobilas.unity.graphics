using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    [CreateAssetMenu(fileName = "IGUStyle custom", menuName = "IGUSkin/IGUStyleCustom")]
    public class IGUStyleCustom : ScriptableObject {
        [SerializeField] private IGUStyle style;

        public IGUStyle Style => style;
    }
}