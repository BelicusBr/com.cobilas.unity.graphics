using UnityEngine;

namespace Cobilas.Unity.Graphics.IGU {
    public abstract class IGUBehaviour : MonoBehaviour {
        protected virtual void Start() { }
        protected virtual void Awake() { }
        protected virtual void OnGUI() { }
    }
}
